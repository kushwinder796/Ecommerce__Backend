using MediatR;
using Order.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Command.CommandHandler
{
    public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, bool>
    {
        private readonly IOrderUnitOfWork _unitOfWork;
        public CancelOrderHandler(IOrderUnitOfWork  unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
           var order= await _unitOfWork.Orders.GetByIdAsync( request.Id);

            if (order== null ) return false;
            if (order.Status == "Delivered") throw new ArgumentException("Cannot cancel Delivered order");
            order.Status = "Cancelled";
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();   
            return true;
        }
    }
}
