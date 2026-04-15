using Conversation.Application.Interface;
using Conversation.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversation.Application.Command.CommandHandler
{
    public  class CloseConversationHandler:IRequestHandler<CloseConversationCommand ,Unit>
    {
        private readonly IConversationRepository _conversationRepo;
        private readonly TimeZoneInfo _indiaTimeZone;
        public CloseConversationHandler(IConversationRepository conversationRepo)
        {
            _conversationRepo = conversationRepo;
            _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<Unit> Handle(CloseConversationCommand request, CancellationToken cancellationToken)
        {
            var conversation=await _conversationRepo.GetByIdAsync(request.ConversationId);
            if (conversation == null) throw new Exception("Conversation not found");
            DateTime nowIst = DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _indiaTimeZone), DateTimeKind.Unspecified);

            //conversation.Status = MessageStatus.Closed;
            conversation.UpdatedAt = nowIst;
            await _conversationRepo.UpdateAsync(conversation);
            return Unit.Value;
        }
    }
}
