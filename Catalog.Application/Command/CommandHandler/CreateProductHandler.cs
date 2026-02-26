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
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly TimeZoneInfo _indiaTimeZone;

        public CreateProductHandler(
            IUnitOfWork unitOfWork,
            IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<Guid> Handle(CreateProductCommand request,CancellationToken cancellationToken)
        {
           
            var categoryExists = await _unitOfWork.Products
                .CategoryExistsAsync(request.CategoryId);

            if (!categoryExists)
                throw new KeyNotFoundException( $"Category with id {request.CategoryId} not found");

           
            var imageUrl = await _imageService.UploadImageAsync(request.Image);

            DateTime nowIst = DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _indiaTimeZone),DateTimeKind.Unspecified);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                CategoryId = request.CategoryId,
                ImageUrl = imageUrl,
                CreatedAt = nowIst
            };
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return product.Id;
        }
    }
}
