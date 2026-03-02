using Payment.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Interface
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<PaymentSystem>> GetAllAsync();
        Task<PaymentSystem?> GetByIdAsync(Guid id);
        Task<PaymentSystem?> GetByTransactionIdAsync(string transactionId);
        Task AddAsync(PaymentSystem payment);
        Task UpdateAsync(PaymentSystem payment);
        Task DeleteAsync(Guid id);
    }
}
