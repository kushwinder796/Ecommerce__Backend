using Conversation.Application.DTOs;
using Conversation.Application.Queries;
using Conversation.Application.Interface;
using Conversation.Domain.Entities;
using Conversation.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


public class GetAllConversationsQueryHandler : IRequestHandler<GetAllConversationsQuery, List<ConversationResponse>>
{
    private readonly IConversationRepository _conversationRepository;

    public GetAllConversationsQueryHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async Task<List<ConversationResponse>> Handle( GetAllConversationsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            
            var conversations = await _conversationRepository.GetAllAsQueryable()
                .Include(c => c.Messages)  
                .Where(c => c.Status == (int)ConversationStatus.Open)  
                .OrderByDescending(c => c.UpdatedAt ?? c.CreatedAt)  
                .Select(c => new ConversationResponse
                {
                    ConversationId = c.Id,
                    Id = c.Id,
                    UserId = c.UserId,
                    ProductId = c.ProductId,
                    Status = ((ConversationStatus)c.Status).ToString(),
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,

                    Messages = c.Messages
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => new MessageResponse
                        {
                            Id = m.Id,
                            ConversationId = m.ConversationId,
                            SenderId = m.SenderId,                    //  Real user ID
                            SenderName = m.SenderName ??
                                (m.SenderType == (int)SenderType.Admin
                                    ? "Support Admin"
                                    : "Customer"),                    //  Real name
                            MessageText = m.messagetext,              //  Your property name
                            Text = m.messagetext,                     //  Alias for frontend compatibility
                            SenderType = m.SenderType,
                            Status = m.Status == null ? "Sent" : m.Status.ToString(),
                            TargetUserId = m.TargetUserId,
                            CreatedAt = m.CreatedAt
                        })
                        .ToList()
                })
                .ToListAsync(cancellationToken);

            return conversations;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching conversations: {ex.Message}");
            throw;
        }
    }
}