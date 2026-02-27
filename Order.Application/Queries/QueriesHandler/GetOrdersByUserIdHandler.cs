using MediatR;
using Order.Application.DTOs;
using Order.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Queries.QueriesHandler
{
    public class GetOrdersByUserIdHandler: IRequestHandler<GetOrdersByUserIdQuery, List<OrderDto>>
    {
        private readonly IOrderUnitOfWork _unitOfWork;
        private readonly TimeZoneInfo _timeZoneInfo;

        public GetOrdersByUserIdHandler(IOrderUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<List<OrderDto>> Handle( GetOrdersByUserIdQuery request,CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.Orders .GetByUserIdAsync(request.UserId);
            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                CreatedAt = o.CreatedAt.HasValue? DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(o.CreatedAt.Value,_timeZoneInfo),DateTimeKind.Unspecified): null,
                OrderItems = o.OrderItems.Select(i =>
                    new OrderItemDto
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList()
            }).ToList();
        }

    }
}