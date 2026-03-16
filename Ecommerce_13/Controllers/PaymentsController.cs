using Ecommerce_13.Comman;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.Command;
using Payment.Application.DTOs;
using Payment.Application.Queries;
using Stripe;
using Stripe.Checkout;

namespace Ecommerce_13.Controllers
{
    [Route("api/Payment")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;


        public PaymentsController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PaymentDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetAllPaymentsQuery(), ct);

            return Ok(ApiResponse<IEnumerable<PaymentDto>>.SuccessResult( result,message: "Payments retrieved successfully",
               statusCode: 200));
        }


        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetPaymentByIdQuery(id), ct);

            if (result is null)return NotFound(ApiResponse<string>.FailResult(message: "Payment not found", statusCode: 404));

            return Ok(ApiResponse<PaymentDto>.SuccessResult( result,message: "Payment retrieved successfully",statusCode: 200));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<StripePaymentResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto, CancellationToken ct)
        {
            var result = await _mediator.Send(new CreatePaymentCommand(dto), ct);

            return StatusCode(201, ApiResponse<StripePaymentResponseDto>.SuccessResult( result,message: 
                "Payment intent created successfully",statusCode: 201));
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePaymentDto dto, CancellationToken ct)
        {
            var result = await _mediator.Send(new UpdatePaymentCommand(id, dto), ct);

            if (result is null)return NotFound(ApiResponse<string>.FailResult( message: "Payment not found",statusCode: 404));

            return Ok(ApiResponse<PaymentDto>.SuccessResult(result,message: "Payment updated successfully",statusCode: 200));
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var deleted = await _mediator.Send(new DeletePaymentCommand(id), ct);

            if (!deleted) return NotFound(ApiResponse<string>.FailResult(  message: "Payment not found",  statusCode: 404));

            return Ok(ApiResponse<string>.SuccessResult(data: null, message: "Payment deleted successfully",statusCode: 200));
        }

        [HttpPost("create-session")]
        public async Task<IActionResult> CreateSession([FromBody] CreateCheckoutSessionCommand command)
        {
            try
            {
                var lineItems = command.Items.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = command.Currency ?? "inr",
                        UnitAmount = (long)(item.Price * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Name,
                            Images = item.ImageUrl != null
                                     ? new List<string> { item.ImageUrl }
                                     : null,
                        },
                    },
                    Quantity = item.Quantity,
                }).ToList();

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = lineItems,
                    Mode = "payment",
                    CustomerEmail = command.CustomerEmail,
                    SuccessUrl = command.SuccessUrl
                                         + "?orderId=" + command.OrderId
                                         + "&session_id={CHECKOUT_SESSION_ID}",
                    CancelUrl = command.CancelUrl,
                    Metadata = new Dictionary<string, string>
                    {
                        { "orderId", command.OrderId }
                    }
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                return Ok(ApiResponse<object>.SuccessResult(new
                {
                    url = session.Url,      
                    sessionId = session.Id,
                }, "Checkout session created"));
            }
            catch (StripeException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    statusCode = 400,
                    errors = (object?)null
                });
            }
        }

        [HttpGet("publishable-key")]
        public IActionResult GetPublishableKey()
        {
            return Ok(ApiResponse<string>.SuccessResult(
                _configuration["Stripe:PublishableKey"]!,"Publishable key retrieved"
            ));
        }
        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var webhookSecret = _configuration["Stripe:WebhookSecret"];

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    webhookSecret
                );

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Session;

                    if (!session.Metadata.ContainsKey("orderId"))
                    {
                        Console.WriteLine("No orderId in metadata!");
                        return Ok();
                    }

                    var orderIdStr = session.Metadata["orderId"];
                    Console.WriteLine($"OrderId received: {orderIdStr}");

                    if (!Guid.TryParse(orderIdStr, out Guid orderId))
                    {
                        Console.WriteLine($"Invalid Guid: {orderIdStr}");
                        return Ok();
                    }

                    var dto = new CreatePaymentDto
                    {
                        OrderId = orderId,
                        PaymentMethod = "card",
                        Amount = session.AmountTotal!.Value / 100m,
                        PaymentStatus = "Paid",
                        TransactionId = session.PaymentIntentId
                    };

                    await _mediator.Send(new CreatePaymentCommand(dto), CancellationToken.None);
                }

                return Ok(); 
            }
            catch (StripeException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
