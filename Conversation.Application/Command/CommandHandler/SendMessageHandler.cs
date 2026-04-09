using Conversation.Application.Interface;
using Conversation.Infrastructure.Persistence.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversation.Application.Command.CommandHandler
{
    public class SendMessageHandler : IRequestHandler<SendMessageCommand, Unit>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly TimeZoneInfo _indiaTimeZone;

        public SendMessageHandler(IMessageRepository messageRepository, 
            IConversationRepository conversationRepository)
        {
            _messageRepository = messageRepository;
            _conversationRepository = conversationRepository;
            _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<Unit> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId);
            if (conversation == null)
            {
                throw new KeyNotFoundException($"Conversation with ID {request.ConversationId} not found.");
            }

            DateTime nowIst = DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _indiaTimeZone), DateTimeKind.Unspecified);

            var message = new Message
            {
                Id = Guid.NewGuid(),
                ConversationId = request.ConversationId,
                MessageText = request.Message,
                SenderType = request.SenderType,
                CreatedAt = nowIst
            };

            await _messageRepository.AddAsync(message);

            return Unit.Value;
        }

      
    }
}
