using Conversation.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversation.Application.DTOs
{
    public class MessageDto
    {
        public string Message { get; set; }
        public SenderType SenderType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
