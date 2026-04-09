using Conversation.Application.Interface;
using Conversation.Infrastructure.Persistence;
using Conversation.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversation.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ConversationDbContext _context;

        public MessageRepository(ConversationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Message>> GetByConversationIdAsync(Guid conversationId)
        {
            return await _context.Messages
                .Where(x => x.ConversationId == conversationId)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}
