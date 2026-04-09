using Conversation.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversation.Application.Interface
{
    public  interface IConversationRepository
    {
        Task AddAsync(ConversationSystem conversation);

        Task<ConversationSystem?> GetByIdAsync(Guid id);

        Task<List<ConversationSystem>> GetByUserIdAsync(Guid userId);

        Task<List<ConversationSystem>> GetAllAsync();

        Task UpdateAsync(ConversationSystem conversation);
    }
}
