using Catalog.Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Command.CommandHandler
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request,CancellationToken cancellationToken)
        {

            var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);

            if (category == null) return false;

            var hasProducts = await _unitOfWork.Products.AnyAsync(p => p.CategoryId == request.Id);

            if (hasProducts)
                throw new InvalidOperationException(
                    "Cannot delete! This category has products. Delete products first.");


            await _unitOfWork.Categories.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
