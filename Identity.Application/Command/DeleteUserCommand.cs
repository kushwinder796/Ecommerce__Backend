using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Command
{
    public record  DeleteUserCommand(Guid id):IRequest<bool>;
    
}
