using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Command
{
    public record DeleteProductCommand(Guid Id) : IRequest<bool>;
}
