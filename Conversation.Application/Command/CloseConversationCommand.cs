using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversation.Application.Command
{
    public  class CloseConversationCommand:IRequest<Unit>
    {
        public Guid ConversationId { get; set; }
    }
}
