using MediatR;
using Offers.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offers.Application.Command
{
    public record UpdateOfferCommand(Guid Id, UpdateOfferDto Dto) : IRequest<OfferDto?>;
}
