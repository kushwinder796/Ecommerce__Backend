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
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderEntity>> GetAllAsync() =>
            await _context.Orders.Include(o => o.OrderItems).ToListAsync();

        public async Task<IEnumerable<OrderEntity>> GetByUserIdAsync(Guid userId) =>
            await _context.Orders.Include(o => o.OrderItems).Where(o => o.UserId == userId).ToListAsync();

        public async Task<OrderEntity?> GetByIdAsync(Guid id) =>
            await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);

        public async Task AddAsync(OrderEntity order) =>
            await _context.Orders.AddAsync(order);

        public async Task UpdateAsync(OrderEntity order) =>
            _context.Orders.Update(order);

        public async Task DeleteAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null) _context.Orders.Remove(order);
        }
    }
}
