using MediatR;
using Offers.Application.DTOs;
using Offers.Application.Interface;
using Offers.Domain.Entities;
using Catalog.Application.Interface;
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
        private readonly IUnitOfWork _catalog;
        private readonly TimeZoneInfo _indiaTimeZone;

        public CreateOfferHandler(IOfferUnitOfWork context, IUnitOfWork catalog)
        {
            _context = context;
            _catalog = catalog;
            _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<OfferDto> Handle(CreateOfferCommand request, CancellationToken ct)
        {
            if (request.Dto.ProductId == Guid.Empty)
                throw new ArgumentException("ProductId is required and cannot be empty.");

            if (request.Dto.OfferedPrice <= 0)
                throw new ArgumentException("OfferedPrice must be greater than 0.");

            var product = await _catalog.Products.GetByIdAsync(request.Dto.ProductId);
            if (product is null)
                throw new KeyNotFoundException($"Product with id {request.Dto.ProductId} not found.");

            if (request.Dto.OfferedPrice >= product.Price)
                throw new ArgumentException($"OfferedPrice ({request.Dto.OfferedPrice}) must be less than the product price ({product.Price}).");

            var offer = new Offer
            {
                ProductId = request.Dto.ProductId,
                OrderId = request.Dto.OrderId,
                OfferedPrice = request.Dto.OfferedPrice
            };

            await _context.Offers.AddAsync(offer);
            await _context.SaveChangesAsync(ct);

            return new OfferDto
            {
                Id = offer.Id,
                ProductId = offer.ProductId,
                ProductName = product.Name,
                OrderId = offer.OrderId,
                OfferedPrice = offer.OfferedPrice,
                Status = offer.Status,
                CreatedAt = offer.CreatedAt.HasValue
                    ? DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(offer.CreatedAt.Value, _indiaTimeZone), DateTimeKind.Unspecified)
                    : null,
            };
        }
    }
}
