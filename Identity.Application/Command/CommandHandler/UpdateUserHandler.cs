using Identity.Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Command.CommandHandler
{

    public class UpdateUserHandler: IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IidentityUnitOfWork _unitOfWork;
        private readonly TimeZoneInfo _indiaTimeZone;

        public UpdateUserHandler(IidentityUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<bool> Handle(UpdateUserCommand request,CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);

            DateTime nowIst = DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _indiaTimeZone),
             DateTimeKind.Unspecified);

            if (user == null) return false;

        
            if (user.Isactive == false) throw new KeyNotFoundException("User is deleted or inactive");
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Role = request.Role;
            user.UpdatedAt = nowIst;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
