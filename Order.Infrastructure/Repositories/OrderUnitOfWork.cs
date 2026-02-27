using Order.Application.Interface;
using Orders.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Repositories
{
    public class OrderUnitOfWork : IOrderUnitOfWork
    {
        private readonly OrderDbContext _context;
      
        public IOrderRepository Orders { get; }
        public IOrderItemRepository OrderItems {  get; }
        public OrderUnitOfWork(OrderDbContext context)
        {
            _context = context;
            Orders= new OrderRepository(context);
            OrderItems= new OrderItemRepository(context);
        }
        public  void Dispose()=>
            _context.Dispose(); 
        

        public async Task<int> SaveChangesAsync()=>
        
            await _context.SaveChangesAsync();
        
    }
}
