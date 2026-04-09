using Conversation.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversation.Application.Interface
{
    public interface IMessageRepository
    {
        Task AddAsync(Message message);

        Task<List<Message>> GetByConversationIdAsync(Guid conversationId);
    }
}
