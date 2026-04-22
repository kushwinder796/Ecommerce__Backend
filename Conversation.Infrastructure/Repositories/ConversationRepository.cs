using Conversation.Application.Interface;
using Conversation.Infrastructure.Persistence;
using Conversation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversation.Infrastructure.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly ConversationDbContext _context;
        public ConversationRepository(ConversationDbContext context)
        {
            _context = context; 
        }
        public async Task AddAsync(ConversationSystem conversation)
        {
            await _context.ConversationSystems.AddAsync(conversation);
            await _context.SaveChangesAsync();
        }

        public IQueryable<ConversationSystem> GetAllAsQueryable()
        {
            return _context.ConversationSystems;
        }

        public async Task<List<ConversationSystem>> GetAllAsync()
        {
            return await _context.ConversationSystems.ToListAsync();
        }

        public async Task<ConversationSystem?> GetByIdAsync(Guid id)
        {
            return await _context.ConversationSystems.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<ConversationSystem>> GetByUserIdAsync(Guid userId)
        {
           return await _context.ConversationSystems.Where(x=>x.UserId ==userId).ToListAsync();
        }

        public async Task UpdateAsync(ConversationSystem conversation)
        {
            _context.ConversationSystems.Update(conversation);
            await _context.SaveChangesAsync();
        }
    }
}
