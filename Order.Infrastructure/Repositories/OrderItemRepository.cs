using Microsoft.EntityFrameworkCore;
using Order.Application.Interface;
using Order.Infrastructure.Persistence.Entities;
using Orders.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly OrderDbContext _context;

        public OrderItemRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId) =>
            await _context.OrderItems.Where(i => i.OrderId == orderId).ToListAsync();

        public async Task AddAsync(OrderItem orderItem) =>
            await _context.OrderItems.AddAsync(orderItem);

        public async Task DeleteAsync(Guid id)
        {
            var item = await _context.OrderItems.FindAsync(id);
            if (item != null)
                _context.OrderItems.Remove(item);
        }
    }
}
