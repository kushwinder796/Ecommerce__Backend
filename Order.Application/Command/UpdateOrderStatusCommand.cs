using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Command
{
    public record UpdateOrderStatusCommand( Guid Id,string Status) : IRequest<bool>;
}
