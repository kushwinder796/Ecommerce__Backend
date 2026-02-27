using Order.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Interface
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId);
        Task AddAsync(OrderItem orderItem);
        Task DeleteAsync(Guid id);
    }
}
