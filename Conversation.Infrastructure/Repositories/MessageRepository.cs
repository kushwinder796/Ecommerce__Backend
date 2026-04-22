using Conversation.Application.Interface;
using Conversation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conversation.Domain.Entities;

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

            if (message.SenderId == Guid.Empty)
                throw new InvalidOperationException("SenderId cannot be empty");

            if (string.IsNullOrWhiteSpace(message.MessageText))
                throw new InvalidOperationException("MessageText cannot be empty");

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Message>> GetByConversationIdAsync(Guid conversationId)
        {
            return await _context.Messages
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<Message?> GetByIdAsync(Guid id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task UpdateAsync(Message message)
        {
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
        }
    }
}
