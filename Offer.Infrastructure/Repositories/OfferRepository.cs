using Microsoft.EntityFrameworkCore;
using Offers.Application.Interface;
using Offers.Infrastructure.Persistence;
using Offers.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offers.Infrastructure.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly OffersDbContext _context;

        public OfferRepository(OffersDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Offer>> GetAllAsync()
            => await _context.Offers.ToListAsync();

        public async Task<Offer?> GetByIdAsync(Guid id)
            => await _context.Offers.FindAsync(id);

        public async Task AddAsync(Offer offer)
        {
            offer.Id = Guid.NewGuid();
            offer.CreatedAt = DateTime.UtcNow;
            offer.Status = "pending";
            await _context.Offers.AddAsync(offer);
        }

        public async Task UpdateAsync(Offer offer)
        {
            _context.Offers.Update(offer);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var offer = await GetByIdAsync(id);
            if (offer is not null)
                _context.Offers.Remove(offer);
        }
    }
}
