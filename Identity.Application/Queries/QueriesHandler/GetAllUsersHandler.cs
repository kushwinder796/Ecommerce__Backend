using Identity.Application.DTOs;
using Identity.Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Queries.QueriesHandler
{
    public class GetAllUsersHandler: IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly IidentityUnitOfWork _unitOfWork;

        public GetAllUsersHandler(IidentityUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request,CancellationToken cancellationToken)
        {

            var users = request.ActiveOnly
                ? await _unitOfWork.Users.GetAllActiveAsync()
                : await _unitOfWork.Users.GetAllAsync();

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role,
                IsActive = u.Isactive
            }).ToList();
        }
    }
}