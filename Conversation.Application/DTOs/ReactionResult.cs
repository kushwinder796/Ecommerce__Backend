using System;

namespace Conversation.Application.DTOs
{
    public class ReactionResult
    {
        public bool Success { get; set; }
        public Guid MessageId { get; set; }
        public Guid ConversationId { get; set; }
        public string Reactions { get; set; }
    }
}
