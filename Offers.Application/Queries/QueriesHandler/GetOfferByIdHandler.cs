using MediatR;
using Offers.Application.DTOs;
using Offers.Application.Interface;
using Offers.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offers.Application.Queries.QueriesHandler
{
    public class GetOfferByIdHandler : IRequestHandler<GetOfferByIdQuery, OfferDto?>
    {
        private readonly IOfferUnitOfWork _context;
        private readonly TimeZoneInfo _timeZoneInfo;

        public GetOfferByIdHandler(IOfferUnitOfWork context)
        {
            _context   = context;
            _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<OfferDto?> Handle(GetOfferByIdQuery request, CancellationToken ct)
        {
            var offer = await _context.Offers.GetByIdAsync(request.Id);
            if (offer is null) return null;

            return new OfferDto
            {
                Id = offer.Id,
                OrderId = offer.OrderId,
                OfferedPrice = offer.OfferedPrice,
                Status = offer.Status,
                CreatedAt = offer.CreatedAt.HasValue ? DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(offer.CreatedAt.Value, _timeZoneInfo), DateTimeKind.Unspecified) : null,
            };
        }
    }
}