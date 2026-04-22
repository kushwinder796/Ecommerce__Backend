using MediatR;
using Offers.Application.DTOs;
using Offers.Application.Interface;

namespace Offers.Application.Queries.QueriesHandler
{
    public class GetOffersByProductIdHandler : IRequestHandler<GetOffersByProductIdQuery, IEnumerable<OfferDto>>
    {
        private readonly IOfferUnitOfWork _context;
        private readonly TimeZoneInfo _indiaTimeZone;

        public GetOffersByProductIdHandler(IOfferUnitOfWork context)
        {
            _context = context;
            _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<IEnumerable<OfferDto>> Handle(GetOffersByProductIdQuery request, CancellationToken ct)
        {
            var offers = await _context.Offers.GetByProductIdAsync(request.ProductId);

            return offers.Select(o => new OfferDto
            {
                Id = o.Id,
                ProductId = o.ProductId,
                OrderId = o.OrderId,
                OfferedPrice = o.OfferedPrice,
                Status = o.Status,
                CreatedAt = o.CreatedAt.HasValue
                    ? DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(o.CreatedAt.Value, _indiaTimeZone), DateTimeKind.Unspecified)
                    : null,
            });
        }
    }
}
