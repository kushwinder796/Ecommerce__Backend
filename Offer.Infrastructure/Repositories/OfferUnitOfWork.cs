using Offers.Application.Interface;
using Offers.Infrastructure.Persistence;
using Order.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offers.Infrastructure.Repositories
{
    public class OfferUnitOfWork : IOfferUnitOfWork
    {
        private readonly OffersDbContext _context;
        private IOfferRepository? _offers;

        public OfferUnitOfWork(OffersDbContext context)
        {
            _context = context;
        }

        public IOfferRepository Offers
            => _offers ??= new OfferRepository(_context);

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);

        public void Dispose() => _context.Dispose();
    }
}
