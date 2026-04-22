using Conversation.Application.Interface;

using Conversation.Domain.Enum;
using Conversation.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversation.Application.Command.CommandHandler
{
    public class StartConversationHandler : IRequestHandler<StartConversationCommand, Guid>
    {
        private readonly IConversationRepository _conversationRepo;
        private readonly TimeZoneInfo _indiaTimeZone;
        public StartConversationHandler(IConversationRepository conversationRepository)
        {
            _conversationRepo = conversationRepository;
            _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        }

        public async Task<Guid> Handle(StartConversationCommand request, CancellationToken cancellationToken)
        {
            DateTime nowIst = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _indiaTimeZone);

            var conversation = new ConversationSystem
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ProductId = request.ProductId,
                Status = (int)ConversationStatus.Open,
                CreatedAt = nowIst
            };

            await _conversationRepo.AddAsync(conversation);

            return conversation.Id;
        }
    }
}
