using Identity.Application.Interface;
using Identity.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Repositories
{
    public class IdentityUnitOfWork : IidentityUnitOfWork
    {
        private readonly IdentityDbContext _context;
        public IUserRepository Users { get; }

        public IdentityUnitOfWork(IdentityDbContext context)
        {
            _context = context;
            Users = new UserRepository(context);
        }

        public async Task<int> SaveChangesAsync() =>
            await _context.SaveChangesAsync();

        public void Dispose() =>
            _context.Dispose();
    }
}
