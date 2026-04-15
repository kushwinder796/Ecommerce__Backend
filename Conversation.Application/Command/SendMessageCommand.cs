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
        public Guid ConversationId { get; set; }      //  Which conversation
        public Guid? SenderId { get; set; }            // Who sent it (from JWT) (now nullable)
        public string MessageText { get; set; }       // Your property name
        public string? SenderName { get; set; }       // Sender's display name
        public SenderType SenderType { get; set; }    //  USER or ADMIN
        public Guid? TargetUserId { get; set; }       //  For admin replies (who to notif)

    }
}
