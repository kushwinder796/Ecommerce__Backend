using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offers.Application.Interface
{
    public  interface IOfferUnitOfWork:IDisposable
    {
        IOfferRepository Offers { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
