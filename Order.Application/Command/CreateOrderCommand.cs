using MediatR;
using Order.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Command
{
    public record CreateOrderCommand(Guid UserId, List<CreateOrderItemDto> Items) : IRequest<Guid>;
}
