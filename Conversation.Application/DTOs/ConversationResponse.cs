using Catalog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversation.Application.DTOs
{
    public class ConversationResponse
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ProductId { get; set; }
        public string Status { get; set; }
        public List<MessageResponse> Messages { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? SenderId { get; set; }
        public string? SenderName { get; set; }
        public Guid? TargetUserId { get; set; }
    }
}
