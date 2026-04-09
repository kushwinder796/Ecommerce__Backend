using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversation.Application.Command
{
    public  class StartConversationCommand:IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public Guid? ProductId { get; set; }
    }
}
