using Identity.Application.Interface;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _context;
        private readonly TimeZoneInfo _indiaTimeZone;

        public UserRepository(IdentityDbContext context)
        {
            _context = context;
            _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        }

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _context.Users.ToListAsync();

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _context.Users.FindAsync(id);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

        public async Task<bool> ExistsAsync(string email) =>
            await _context.Users
                .AnyAsync(u => u.Email == email);

        public async Task AddAsync(User user) =>
            await _context.Users.AddAsync(user);

        public async Task UpdateAsync(User user) =>
            _context.Users.Update(user);

        public  async Task<IEnumerable<User>> GetAllActiveAsync()=>
           await _context.Users.Where(u => u.Isactive == true).ToListAsync();


        public  async Task SoftDeleteAsync(Guid id)

        {
            var user = await _context.Users.FindAsync(id);

            DateTime nowIst = DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _indiaTimeZone), DateTimeKind.Unspecified);

            if (user !=null)
            {
                user.Isactive = false;
                user.UpdatedAt = nowIst;
            }
        }
            
    }
}

