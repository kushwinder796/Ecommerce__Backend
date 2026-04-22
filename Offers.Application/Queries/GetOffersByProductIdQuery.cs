using MediatR;
using Offers.Application.DTOs;

namespace Offers.Application.Queries
{
    public record GetOffersByProductIdQuery(Guid ProductId) : IRequest<IEnumerable<OfferDto>>;
}
