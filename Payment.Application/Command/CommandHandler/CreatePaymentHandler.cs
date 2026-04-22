using MediatR;
using Payment.Application.DTOs;
using Payment.Application.Interface;
using Payment.Domain.Entities;
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
        private readonly TimeZoneInfo _indiaTimeZone;

        public CreatePaymentHandler(IPaymentUnitOfWork context, IStripeService stripe)
        {
            _context = context;
            _stripe = stripe;
            _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<StripePaymentResponseDto> Handle(CreatePaymentCommand request, CancellationToken ct)
        {
            DateTime nowIst = DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _indiaTimeZone),
              DateTimeKind.Unspecified);

            var payment = new PaymentSystem
            {
                Id = Guid.NewGuid(),
                OrderId = request.Dto.OrderId,
                PaymentMethod = request.Dto.PaymentMethod ?? "card",
                Amount = request.Dto.Amount,
                PaymentStatus = request.Dto.PaymentStatus ?? "Paid",
                TransactionId = request.Dto.TransactionId,
                CreatedAt = nowIst,
            };

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync(CancellationToken.None);

            return new StripePaymentResponseDto
            {
                PaymentIntentId = payment.TransactionId
            };
        }
    }
}
