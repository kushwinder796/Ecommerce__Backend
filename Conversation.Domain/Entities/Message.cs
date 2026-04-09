using Conversation.Domain.Enum;
using System;
using System.Collections.Generic;

namespace Conversation.Infrastructure.Persistence.Entities;

public partial class Message
{
    public Guid Id { get; set; }

    public Guid ConversationId { get; set; }

    public SenderType SenderType { get; set; }

    public string MessageText { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ConversationSystem Conversation { get; set; } = null!;
}
