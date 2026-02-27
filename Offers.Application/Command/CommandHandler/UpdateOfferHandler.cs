using MediatR;
using Offers.Application.DTOs;
using Offers.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offers.Application.Command.CommandHandler
{
    public class UpdateOfferHandler : IRequestHandler<UpdateOfferCommand, OfferDto?>
    {
        private readonly IOfferUnitOfWork _context;
        private readonly TimeZoneInfo _indiaTimeZone;

        public UpdateOfferHandler(IOfferUnitOfWork context)
        {
            _context = context;
            _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<OfferDto?> Handle(UpdateOfferCommand request, CancellationToken ct)
        {
            var offer = await _context.Offers.GetByIdAsync(request.Id);
            if (offer is null) return null;

            if (request.Dto.OfferedPrice.HasValue)
                offer.OfferedPrice = request.Dto.OfferedPrice.Value;

            if (request.Dto.Status is not null)
                offer.Status = request.Dto.Status;

            await _context.Offers.UpdateAsync(offer);
            await _context.SaveChangesAsync(ct);

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
