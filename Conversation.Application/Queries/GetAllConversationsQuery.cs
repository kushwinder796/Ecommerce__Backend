using Conversation.Application.DTOs;
using MediatR;

namespace Conversation.Application.Queries
{
    public class GetAllConversationsQuery : IRequest<List<ConversationResponse>>
    {

    }
}
