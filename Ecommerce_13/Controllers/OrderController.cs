using Ecommerce_13.Comman;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Order.Application.Command;
using Order.Application.DTOs;
using Order.Application.Queries;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Microsoft.Extensions.Configuration;
using MailKit.Security;
using MailKit.Net.Smtp;

namespace Ecommerce_13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;

        public OrderController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _config = configuration;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllOrdersQuery());
            return Ok(ApiResponse<List<OrderDto>>.SuccessResult(result, "Orders retrieved successfully"));
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var result = await _mediator.Send(new GetOrdersByUserIdQuery(userId));
            return Ok(ApiResponse<List<OrderDto>>.SuccessResult(result, "User orders retrieved successfully"));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User not authenticated");

            var userId = Guid.Parse(userIdClaim);
            var command = new CreateOrderCommand(userId, request.Items);
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetByUserId), new { userId },
                ApiResponse<Guid>.SuccessResult(id, "Order created successfully", 201));
        }

        [HttpPut("{id:guid}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusCommand command)
        {
            if (id != command.Id) throw new ArgumentException("ID mismatch");

            var result = await _mediator.Send(command);

            if (!result) throw new KeyNotFoundException($"Order with id {id} not found");

            return Ok(ApiResponse<string>.SuccessResult("Updated", "Order status updated successfully"));
        }

        [HttpPut("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _mediator.Send(new CancelOrderCommand(id));

            if (!result) throw new KeyNotFoundException($"Order with id {id} not found");

            return Ok(ApiResponse<string>.SuccessResult("Cancelled", "Order cancelled successfully"));
        }

        [HttpPost("confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmOrder([FromBody] OrderConfirmationDto order)
        {
            Console.WriteLine("API HIT");

            if (order == null)
                return BadRequest(new { success = false, message = "Invalid order data" });

            try
            {
          
                if (string.IsNullOrEmpty(order.Email) && string.IsNullOrEmpty(order.Phone))
                {
                    return BadRequest(new { success = false, message = "Email or Phone required" });
                }

                if (!string.IsNullOrEmpty(order.Email))
                {
                    try
                    {
                        await SendEmail(order);
                        Console.WriteLine(" Email sent");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(" Email failed: " + ex.Message);
                    }
                }

                if (!string.IsNullOrEmpty(order.Phone))
                {
                    try
                    {
                        await SendSms(order);
                        Console.WriteLine(" SMS sent");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(" SMS failed: " + ex.Message);
                    }
                }

                return Ok(new
                {
                    success = true,
                    message = "Order confirmed (Email/SMS attempted)"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(" CRITICAL ERROR: " + ex.Message);

                return StatusCode(500, new
                {
                    success = false,
                    message = "Server error",
                    error = ex.Message
                });
            }
        }

        private async Task SendSms(OrderConfirmationDto order)
        {
            try
            {
                TwilioClient.Init(
                    _config["Twilio:AccountSid"],
                    _config["Twilio:AuthToken"]
                );

                var message = await MessageResource.CreateAsync(
                    body: $"Hi {order.CustomerName}, your order #{order.Id} is confirmed.",
                    from: new PhoneNumber(_config["Twilio:FromNumber"]),
                    to: new PhoneNumber(order.Phone)
                );

                Console.WriteLine(" SMS SENT");
                Console.WriteLine("SID: " + message.Sid);
                Console.WriteLine("STATUS: " + message.Status);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SMS ERROR: " + ex.Message);
            }
        }

        private async Task SendEmail(OrderConfirmationDto order)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Your Store", _config["Email:From"]));
            email.To.Add(new MailboxAddress("", order.Email));
            email.Subject = $"Order Confirmation #{order.Id}";

            email.Body = new TextPart("html")
            {
                Text = $@"
            <h2>Order Confirmed!</h2>
            <p>Hi {order.CustomerName}</p>
            <p>Order ID: {order.Id}</p>
            <p>Total: ₹{order.TotalAmount}</p>
             
        "
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            
            await smtp.AuthenticateAsync(
                _config["Email:From"],
                _config["Email:Password"]
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}