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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CatalogDbContext _context;

        public CategoryRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync() =>
            await _context.Categories.ToListAsync();

        public async Task<Category?> GetByIdAsync(int id) =>
            await _context.Categories.FindAsync(id);

        public async Task AddAsync(Category category) =>
            await _context.Categories.AddAsync(category);


        public async Task UpdateAsync(Category category) =>
           _context.Categories.Update(category);

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
                _context.Categories.Remove(category);
        }

    }
}
