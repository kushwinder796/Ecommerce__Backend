using Payment.Application.Interface;
using Payment.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Repositories
{
    public class PaymentUnitOfWork : IPaymentUnitOfWork
    {
        private readonly PaymentDbContext _context;

        private IPaymentRepository? _payments;

        public PaymentUnitOfWork(PaymentDbContext context)
        {
            _context = context;
        }

        public IPaymentRepository Payments
            => _payments ??= new PaymentRepository(_context);

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);

        public void Dispose() => _context.Dispose();
    }
}
