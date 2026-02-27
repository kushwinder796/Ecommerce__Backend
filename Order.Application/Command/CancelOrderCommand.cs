using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Command
{
    public record CancelOrderCommand(Guid Id) : IRequest<bool>;
}
