using Catalog.Application.Interface;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogDbContext _context;

        public ProductRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await _context.Products.ToListAsync();

        public async Task<Product?> GetByIdAsync(Guid id) =>
            await _context.Products.FindAsync(id);

        public async Task AddAsync(Product product) =>
            await _context.Products.AddAsync(product);

        public async Task UpdateAsync(Product product) =>
            _context.Products.Update(product);

        public async Task DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
                _context.Products.Remove(product);
        }

        public async Task<bool> CategoryExistsAsync(int categoryId) =>
            await _context.Categories
                .AnyAsync(c => c.Id == categoryId);
    }
}