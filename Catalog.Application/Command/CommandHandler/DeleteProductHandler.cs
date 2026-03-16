using Catalog.Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Command.CommandHandler
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderUnitOfWork _order;

        public DeleteProductHandler(IUnitOfWork unitOfWork,IOrderUnitOfWork order     )
        {
            _unitOfWork = unitOfWork;
            _order = order;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
            if (product == null) return false;

            var orderItems = await _order.OrderItems.GetByProductIdAsync(request.Id);

            if (orderItems.Any())
            {
                await _order.OrderItems.DeleteRangeAsync(orderItems);
                await _order.SaveChangesAsync(); 
            }

            await _unitOfWork.Products.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}