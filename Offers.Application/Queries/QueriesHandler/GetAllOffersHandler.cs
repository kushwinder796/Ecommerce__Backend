using MediatR;
using Offers.Application.DTOs;
using Offers.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offers.Application.Queries.QueriesHandler
{
    public class GetAllOffersHandler : IRequestHandler<GetAllOffersQuery, IEnumerable<OfferDto>>
    {
        private readonly IOfferUnitOfWork  _context;
        private readonly TimeZoneInfo _timeZoneInfo;

        public GetAllOffersHandler(IOfferUnitOfWork context)
        {
            _context  = context;
            _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<IEnumerable<OfferDto>> Handle(GetAllOffersQuery request, CancellationToken ct)
        {
            var offers = await _context.Offers.GetAllAsync();

            return offers.Select(o => new OfferDto
            {
                Id = o.Id,
                OrderId = o.OrderId,
                OfferedPrice = o.OfferedPrice,
                Status = o.Status,
                CreatedAt = o.CreatedAt.HasValue ? DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(o.CreatedAt.Value, _timeZoneInfo), DateTimeKind.Unspecified) : null,
            });
        }
    }
}
