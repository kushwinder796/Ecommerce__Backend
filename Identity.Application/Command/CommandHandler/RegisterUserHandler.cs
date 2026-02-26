using BCrypt.Net;
using Identity.Application.DTOs;
using Identity.Application.Interface;
using Identity.Infrastructure.Persistence.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Command.CommandHandler
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthResponseDto>
    {
        private readonly IidentityUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly TimeZoneInfo _indiaTimeZone;

        public RegisterUserHandler(IidentityUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            if (await _unitOfWork.Users.ExistsAsync(request.Email)) throw new ArgumentException(
                    "User already exists with this email");


            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            DateTime nowIst = DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _indiaTimeZone),
            DateTimeKind.Unspecified);

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = request.Role,
                Isactive = true,
                CreatedAt = nowIst,
                UpdatedAt = nowIst,
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();


            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
            };
        }
    }
}
