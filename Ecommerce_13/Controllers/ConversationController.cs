using Conversation.Application.Command;
using Conversation.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ConversationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly Microsoft.AspNetCore.SignalR.IHubContext<Ecommerce_13.Hubs.ChatHub> _hubContext;
    private readonly TimeZoneInfo _indiaTimeZone;

    public ConversationController(IMediator mediator, Microsoft.AspNetCore.SignalR.IHubContext<Ecommerce_13.Hubs.ChatHub> hubContext)
    {
        _mediator = mediator;
        _hubContext = hubContext;
        _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
    }

    [Authorize(Roles = "User")]
    [HttpPost("start")]
    public async Task<IActionResult> StartConversation(StartConversationCommand command)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                  ?? User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID claim not found in the token.");
        }

        command.UserId = Guid.Parse(userId);

        var id = await _mediator.Send(command);

        return Ok(id);
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage(SendMessageCommand command)
    {
    
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
                ?? User.FindFirst("role")?.Value;

        command.SenderType = role == "Admin" 
            ? Conversation.Domain.Enum.SenderType.ADMIN 
            : Conversation.Domain.Enum.SenderType.USER;

        await _mediator.Send(command);

        DateTime nowIst = DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _indiaTimeZone), DateTimeKind.Unspecified);
        await _hubContext.Clients.Group(command.ConversationId.ToString())
            .SendAsync("ReceiveMessage", new
            {
                command.ConversationId,
                command.Message,
                SenderType = command.SenderType.ToString(),
                CreatedAt = nowIst
            });

        return Ok("Message Sent !!");
    }

    [HttpGet("messages/{conversationId}")]
    public async Task<IActionResult> GetMessages(Guid conversationId)
    {
        var result = await _mediator.Send(new GetMessagesQuery
        {
            ConversationId = conversationId
        });

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllConversations()
    {
        var result = await _mediator.Send(new GetAllConversationsQuery());
        return Ok(result);
    }

    [Authorize(Roles = "User")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyConversations()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID claim not found in the token.");
        }

        var result = await _mediator.Send(new GetUserConversationsQuery
        {
            UserId = Guid.Parse(userId)
        });
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("close")]
    public async Task<IActionResult> CloseConversation(CloseConversationCommand command)
    {
        await _mediator.Send(command);
        return Ok("Conversation Closed !!");
    }
}