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
    public class GetMessagesHandler: IRequestHandler<GetMessagesQuery, List<MessageResponse>>
    {
        private readonly IMessageRepository _messageRepository;

        public GetMessagesHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<List<MessageResponse>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await _messageRepository.GetByConversationIdAsync(request.ConversationId);

            return messages.Select(m => new MessageResponse
            {
                MessageText = m.ToString(),
                SenderType = m.SenderType,
                CreatedAt = m.CreatedAt
            }).ToList();
        }
    }
}
