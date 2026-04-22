using Microsoft.EntityFrameworkCore;
using Payment.Application.Interface;
using Payment.Infrastructure.Persistence;
using Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _context;

        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentSystem>> GetAllAsync()
            => await _context.Payments.ToListAsync();

        public async Task<PaymentSystem?> GetByIdAsync(Guid id)
            => await _context.Payments.FindAsync(id);

        public async Task<PaymentSystem?> GetByTransactionIdAsync(string transactionId)
            => await _context.Payments
                             .FirstOrDefaultAsync(p => p.TransactionId == transactionId);

        public async Task AddAsync(PaymentSystem payment)
        {
            payment.Id = Guid.NewGuid();
            payment.CreatedAt = DateTime.UtcNow;
            payment.PaymentStatus = "pending";
            await _context.Payments.AddAsync(payment);
        }

        public async Task UpdateAsync(PaymentSystem payment)
        {
            _context.Payments.Update(payment);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var payment = await GetByIdAsync(id);
            if (payment is not null)
                _context.Payments.Remove(payment);
        }
    }
}
