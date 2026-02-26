using Catalog.Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Command.CommandHandler
{
    public  class UpdateCategoryHandler:IRequestHandler<UpdateCategoryCommand,bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCategoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork =  unitOfWork;
        }
      
        public async Task<bool> Handle(UpdateCategoryCommand request,CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);

            if (category == null) return false;

            category.Name = request.Name;
            category.Description = request.Description;

            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return true;

        }
    }
}
