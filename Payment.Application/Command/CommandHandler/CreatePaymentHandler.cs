using MediatR;
using Payment.Application.DTOs;
using Payment.Application.Interface;
using Payment.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Command.CommandHandler
{
    public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, StripePaymentResponseDto>
    {
        private readonly IPaymentUnitOfWork _context;
        private readonly IStripeService _stripe;

        public CreatePaymentHandler(IPaymentUnitOfWork context, IStripeService stripe)
        {
            _context = context;
            _stripe = stripe;
        }

        public async Task<StripePaymentResponseDto> Handle(CreatePaymentCommand request, CancellationToken ct)
        {
            
            var stripeResponse = await _stripe.CreatePaymentIntentAsync(new StripePaymentRequestDto
            {
                OrderId = request.Dto.OrderId ?? Guid.Empty,
                Amount = request.Dto.Amount ?? 0,
                Currency = "usd",
                Description = $"Payment for Order {request.Dto.OrderId}"
            });

            var payment = new PaymentSystem
            {
                OrderId = request.Dto.OrderId,
                PaymentMethod = request.Dto.PaymentMethod,
                Amount = request.Dto.Amount,
                TransactionId = stripeResponse.PaymentIntentId
            };

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync(ct);

            return stripeResponse;
        }
    }
}
