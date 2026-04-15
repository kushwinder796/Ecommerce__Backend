using Conversation.Application.DTOs;
using Conversation.Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversation.Application.Queries.QueriesHandler
{
    public class GetUserConversationsHandler : IRequestHandler<GetUserConversationsQuery, List<ConversationResponse>>
    {
        private readonly IConversationRepository _conversationRepository;

        public GetUserConversationsHandler(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        public async Task<List<ConversationResponse>> Handle(GetUserConversationsQuery request, CancellationToken cancellationToken)
        {
            var conversations = await _conversationRepository.GetByUserIdAsync(request.UserId);

            return conversations.Select(c => new ConversationResponse
            {
                Id = c.Id,
                Status = c.Status.ToString(),
                CreatedAt = c.CreatedAt
            }).ToList();
        }
    }
}
