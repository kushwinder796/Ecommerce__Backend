using MediatR;
using Offers.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offers.Application.Queries
{
    public record GetAllOffersQuery : IRequest<IEnumerable<OfferDto>>;
}
