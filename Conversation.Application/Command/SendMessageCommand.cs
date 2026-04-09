using Conversation.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversation.Application.Command
{
    public class SendMessageCommand:IRequest<Unit>
    {
        public Guid ConversationId { get; set; }
        public string  Message { get; set; }
        public SenderType  SenderType { get; set; }
    }
}
