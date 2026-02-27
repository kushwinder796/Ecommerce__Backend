using MediatR;
using Offers.Application.DTOs;
using Offers.Application.Interface;
using Offers.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offers.Application.Command.CommandHandler
{
    public class CreateOfferHandler : IRequestHandler<CreateOfferCommand, OfferDto>
    {
        private readonly IOfferUnitOfWork _context;
        private readonly TimeZoneInfo _indiaTimeZone;


        public CreateOfferHandler(IOfferUnitOfWork context)
        {
            _context=context;
            _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<OfferDto> Handle(CreateOfferCommand request, CancellationToken ct)
        {
            var offer = new Offer
            {
                OrderId = request.Dto.OrderId,
                OfferedPrice = request.Dto.OfferedPrice
            };

            await   _context.Offers.AddAsync(offer);
            await   _context.SaveChangesAsync(ct);

            return new OfferDto
            {
                Id = offer.Id,
                OrderId = offer.OrderId,
                OfferedPrice = offer.OfferedPrice,
                Status = offer.Status,
                CreatedAt = offer.CreatedAt.HasValue ? DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(offer.CreatedAt.Value, _indiaTimeZone), DateTimeKind.Unspecified) : null,
            };
        }
    }
}
