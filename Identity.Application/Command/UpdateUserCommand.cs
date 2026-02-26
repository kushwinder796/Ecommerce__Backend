using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Command
{
    public record UpdateUserCommand(
         Guid Id,
         string FirstName,
         string LastName,
         string Email,
         string Role
     ) : IRequest<bool>;
}
