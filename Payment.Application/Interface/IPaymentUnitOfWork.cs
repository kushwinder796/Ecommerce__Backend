using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Interface
{
    public interface  IPaymentUnitOfWork : IDisposable
    {
        public IPaymentRepository Payments { get; }
        public Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
