using Identity.Application.DTOs;
using Identity.Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Command.CommandHandler
{
    public class LoginUserHandler: IRequestHandler<LoginUserCommand, AuthResponseDto>
    {
        private readonly IidentityUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public LoginUserHandler(IidentityUnitOfWork unitOfWork,IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> Handle(LoginUserCommand request,CancellationToken cancellationToken)
        {
          
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email)
                ?? throw new KeyNotFoundException(
                    "Invalid email or password");

           
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new KeyNotFoundException(
                    "Invalid email or password");

            
            if (user.Isactive == false)throw new UnauthorizedAccessException(
                "Account is disabled");

            
            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token
            };
        }
    }

}
