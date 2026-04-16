using Conversation.Application.DTOs;
using MediatR;
using System;

namespace Conversation.Application.Command
{
    public class AddReactionCommand : IRequest<ReactionResult>
    {
        public Guid MessageId { get; set; }
        public string Emoji { get; set; }
        public Guid UserId { get; set; }
    }
}
