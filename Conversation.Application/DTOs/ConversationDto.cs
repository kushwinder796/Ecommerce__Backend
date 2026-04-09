using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversation.Application.DTOs
{
    public class ConversationDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime?  CreatedAt { get; set; }
    }
}
