using Identity.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Command
{
    public record RegisterUserCommand(string FirstName, string LastName,string Email,string Password,string Role) 
        : IRequest<AuthResponseDto>;

}
