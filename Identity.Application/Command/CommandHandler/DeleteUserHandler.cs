using Identity.Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Command.CommandHandler
{
    public class DeleteUserHandler: IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IidentityUnitOfWork _unitOfWork;

        public DeleteUserHandler(IidentityUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users .GetByIdAsync(request.id);

            if (user == null) return false;

            await _unitOfWork.Users.SoftDeleteAsync(request.id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
