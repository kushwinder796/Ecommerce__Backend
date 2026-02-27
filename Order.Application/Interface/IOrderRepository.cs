using Order.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Interface
{
    public  interface IOrderRepository
    {
        Task<IEnumerable<OrderEntity>> GetAllAsync();
        Task<IEnumerable<OrderEntity>> GetByUserIdAsync(Guid userId);
        Task<OrderEntity?> GetByIdAsync(Guid id);
        Task AddAsync(OrderEntity order);
        Task UpdateAsync(OrderEntity order);
        Task DeleteAsync(Guid id);
    }
}
