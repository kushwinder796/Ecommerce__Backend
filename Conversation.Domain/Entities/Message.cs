using System;
using System.Collections.Generic;

namespace Conversation.Infrastructure.Persistence.Entities;

public partial class Message
{
    public Guid Id { get; set; }

    public Guid ConversationId { get; set; }

    public int SenderType { get; set; }

    public string MessageText { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public Guid SenderId { get; set; }

    public string? SenderName { get; set; }

    public int? Status { get; set; }

    public Guid? TargetUserId { get; set; }

    public Guid? ParentMessageId { get; set; }

    public string? Reactions { get; set; }

    public virtual ConversationSystem Conversation { get; set; } = null!;
}
