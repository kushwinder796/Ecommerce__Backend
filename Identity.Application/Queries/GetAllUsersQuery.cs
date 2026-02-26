using Identity.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Queries
{
    public record GetAllUsersQuery( bool ActiveOnly = true) : IRequest<List<UserDto>>;
}
