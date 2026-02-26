using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Command
{
    public record UpdateProductCommand(
          Guid Id,
          string Name,
          string Description,
          decimal Price,
          int Stock,
           IFormFile Image,
          int CategoryId
          ) : IRequest<bool>;
}
