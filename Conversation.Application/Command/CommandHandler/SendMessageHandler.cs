using Conversation.Application.Command;
using Conversation.Application.Interface;
using Conversation.Domain.Enum;
using Conversation.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Unit>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IMessageRepository _messageRepository;

    public SendMessageCommandHandler(IConversationRepository conversationRepository, IMessageRepository messageRepository)
    {
        _conversationRepository = conversationRepository;
        _messageRepository = messageRepository;
    }

    public async Task<Unit> Handle(SendMessageCommand command, CancellationToken cancellationToken)
    {
        try
        {
            //  Validation  ConversationId is not empty
            if (command.ConversationId == Guid.Empty)
            {
                throw new InvalidOperationException("ConversationId cannot be empty");
            }

            //  Validation SenderId is not empty
            if (!command.SenderId.HasValue || command.SenderId == Guid.Empty)
            {
                throw new InvalidOperationException("SenderId cannot be empty. Must extract from JWT token.");
            }

            //  Validation  Message text is not empty
            if (string.IsNullOrWhiteSpace(command.MessageText))
            {
                throw new InvalidOperationException("Message text cannot be empty");
            }

            // Validation  Conversation exists
            var conversation = await _conversationRepository.GetByIdAsync(command.ConversationId);

            if (conversation == null)
            {
                throw new InvalidOperationException(
                    $"Conversation with ID {command.ConversationId} not found");
            }

            //  Create the message entity - using your actual property names
            var message = new Message
            {
                Id = Guid.NewGuid(),
                ConversationId = command.ConversationId,
                SenderId = command.SenderId.Value,              //  Real user ID from JWT (Now using .Value)
                MessageText = command.MessageText,         //  Your property name
                SenderName = command.SenderName,           //  Cache sender name
                SenderType = (int)command.SenderType,           //  USER or ADMIN
                TargetUserId =command.TargetUserId,       //  For admin replies
                ParentMessageId = command.ParentMessageId, // For replies
                Status = (int?)MessageStatus.Sent,             
                CreatedAt = DateTime.UtcNow
            };

            //  Add to database
            await _messageRepository.AddAsync(message);
            
            conversation.UpdatedAt = DateTime.UtcNow;
            await _conversationRepository.UpdateAsync(conversation);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving message: {ex.Message}");
            throw;
        }
    }
}