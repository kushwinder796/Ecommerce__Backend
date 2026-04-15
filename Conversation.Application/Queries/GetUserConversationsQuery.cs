using Conversation.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversation.Application.Queries
{
    public class GetUserConversationsQuery : IRequest<List<ConversationResponse>>
    {
        public Guid UserId { get; set; }
    }
}
