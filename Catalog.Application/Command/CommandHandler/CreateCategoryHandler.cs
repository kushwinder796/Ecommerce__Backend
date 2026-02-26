using Catalog.Application.Interface;
using Catalog.Infrastructure.Persistence.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Command.CommandHandler
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TimeZoneInfo _indiaTimeZone;

        public CreateCategoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<int> Handle(CreateCategoryCommand request,CancellationToken cancellationToken)
        {

            DateTime nowIst = DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _indiaTimeZone), DateTimeKind.Unspecified);

            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = nowIst,
            };

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return category.Id;
        }
    }
}
