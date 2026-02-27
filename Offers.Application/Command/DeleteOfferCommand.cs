using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offers.Application.Command
{
    public record DeleteOfferCommand(Guid Id) : IRequest<bool>;
}
