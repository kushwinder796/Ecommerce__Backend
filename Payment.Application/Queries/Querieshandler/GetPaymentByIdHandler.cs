using MediatR;
using Payment.Application.DTOs;
using Payment.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Queries.Querieshandler
{
    public class GetPaymentByIdHandler : IRequestHandler<GetPaymentByIdQuery, PaymentDto?>
    {
        private readonly IPaymentUnitOfWork _context;
        private readonly TimeZoneInfo _indiaTimeZone;

        public GetPaymentByIdHandler(IPaymentUnitOfWork context)
        {
            _context = context;
            _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<PaymentDto?> Handle(GetPaymentByIdQuery request, CancellationToken ct)
        {
            var payment = await _context.Payments.GetByIdAsync(request.Id);
            if (payment is null) return null;

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
