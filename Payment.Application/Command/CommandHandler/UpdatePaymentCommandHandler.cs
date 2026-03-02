using MediatR;
using Payment.Application.DTOs;
using Payment.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Command.CommandHandler
{
    public class UpdatePaymentHandler : IRequestHandler<UpdatePaymentCommand, PaymentDto?>
    {
        private readonly IPaymentUnitOfWork _context;
        private readonly TimeZoneInfo _indiaTimeZone;

        public UpdatePaymentHandler(IPaymentUnitOfWork context)
        {
            _context = context;
            _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<PaymentDto?> Handle(UpdatePaymentCommand request, CancellationToken ct)
        {
            var payment = await _context.Payments.GetByIdAsync(request.Id);
            if (payment is null) return null;

            if (request.Dto.PaymentStatus is not null)
                payment.PaymentStatus = request.Dto.PaymentStatus;

            if (request.Dto.TransactionId is not null)
                payment.TransactionId = request.Dto.TransactionId;

            await _context.Payments.UpdateAsync(payment);
            await _context.SaveChangesAsync(ct);

            return new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                PaymentMethod = payment.PaymentMethod,
                Amount = payment.Amount,
                PaymentStatus = payment.PaymentStatus,
                TransactionId = payment.TransactionId,
                CreatedAt = payment.CreatedAt.HasValue ? DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(payment.CreatedAt.Value, _indiaTimeZone), DateTimeKind.Unspecified) : null,
            };
        }
    }
}
