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
    public class GetAllPaymentsQueryHandler : IRequestHandler<GetAllPaymentsQuery, IEnumerable<PaymentDto>>
    {
        private readonly IPaymentUnitOfWork _context;
        private readonly TimeZoneInfo _indiaTimeZone;

        public GetAllPaymentsQueryHandler(IPaymentUnitOfWork context)
        {
            _context = context;
            _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<IEnumerable<PaymentDto>> Handle(GetAllPaymentsQuery request, CancellationToken ct)
        {
            var payments = await _context.Payments.GetAllAsync();

            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                OrderId = p.OrderId,
                PaymentMethod = p.PaymentMethod,
                Amount = p.Amount,
                PaymentStatus = p.PaymentStatus,
                TransactionId = p.TransactionId,
                CreatedAt = p.CreatedAt.HasValue ? DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(p.CreatedAt.Value, _indiaTimeZone), DateTimeKind.Unspecified) : null,
            });
        }
    }

}
