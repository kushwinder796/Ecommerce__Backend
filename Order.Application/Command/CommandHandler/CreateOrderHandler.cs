using MediatR;
using Order.Application.Interface;
using Order.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Command.CommandHandler
{
    public class CreateOrderHandler: IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderUnitOfWork _unitOfWork;
        private readonly TimeZoneInfo  _timeZoneInfo;

        public CreateOrderHandler(IOrderUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<Guid> Handle(CreateOrderCommand request,CancellationToken cancellationToken)
        {
            var totalAmount = request.Items.Sum(i => i.Price * i.Quantity);
            DateTime nowIst = DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZoneInfo), DateTimeKind.Unspecified);

            var order = new OrderEntity
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Status = "Pending",
                TotalAmount = totalAmount,
                CreatedAt =nowIst,
            };

            await _unitOfWork.Orders.AddAsync(order);

            foreach (var item in request.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                await _unitOfWork.OrderItems.AddAsync(orderItem);
            }

            await _unitOfWork.SaveChangesAsync();
            return order.Id;
        }

    }
}
