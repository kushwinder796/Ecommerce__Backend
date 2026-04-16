using Conversation.Application.Command;
using Conversation.Application.DTOs;
using Conversation.Application.Interface;

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Conversation.Application.Command.CommandHandler
{
    public class AddReactionHandler : IRequestHandler<AddReactionCommand, ReactionResult>
    {
        private readonly IMessageRepository _messageRepository;

        public AddReactionHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<ReactionResult> Handle(AddReactionCommand request, CancellationToken cancellationToken)
        {
            var message = await _messageRepository.GetByIdAsync(request.MessageId);
            if (message == null) return new ReactionResult { Success = false };

            var reactions = new Dictionary<string, List<Guid>>();
            if (!string.IsNullOrEmpty(message.Reactions))
            {
                try
                {
                    reactions = JsonSerializer.Deserialize<Dictionary<string, List<Guid>>>(message.Reactions) 
                                ?? new Dictionary<string, List<Guid>>();
                }
                catch { }
            }

            if (!reactions.ContainsKey(request.Emoji))
            {
                reactions[request.Emoji] = new List<Guid>();
            }

            if (!reactions[request.Emoji].Contains(request.UserId))
            {
                reactions[request.Emoji].Add(request.UserId);
            }
            else
            {
                // Remove if already exists (toggle behavior)
                reactions[request.Emoji].Remove(request.UserId);
                if (reactions[request.Emoji].Count == 0) reactions.Remove(request.Emoji);
            }

            message.Reactions = JsonSerializer.Serialize(reactions);
            await _messageRepository.UpdateAsync(message);

            return new ReactionResult
            {
                Success = true,
                MessageId = message.Id,
                ConversationId = message.ConversationId,
                Reactions = message.Reactions
            };
        }
    }
}
