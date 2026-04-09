using Conversation.Application.DTOs;
using Conversation.Application.Interface;
using MediatR;

namespace Conversation.Application.Queries.QueriesHandler
{
    public class GetAllConversationsHandler : IRequestHandler<GetAllConversationsQuery, List<ConversationDto>>
    {
        private readonly IConversationRepository _conversationRepository;

        public GetAllConversationsHandler(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        public async Task<List<ConversationDto>> Handle(GetAllConversationsQuery request, CancellationToken cancellationToken)
        {
            var conversations = await _conversationRepository.GetAllAsync();

            return conversations.Select(c => new ConversationDto
            {
                Id = c.Id,
                Status = c.Status.ToString(),
                CreatedAt = c.CreatedAt
            }).ToList();
        }
    }
}
