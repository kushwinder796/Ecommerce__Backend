using Conversation.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversation.Application.DTOs
{
    public class MessageResponse
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid? SenderId { get; set; }
        public string? SenderName { get; set; }
        public string MessageText { get; set; }       
        public string Text { get; set; }               
        public int SenderType { get; set; }
        public string Status { get; set; }
        public Guid? TargetUserId { get; set; }
        public Guid? ParentMessageId { get; set; }
        public string? Reactions { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
