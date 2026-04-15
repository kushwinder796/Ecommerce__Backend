using Conversation.Application.Command;
using Conversation.Application.Queries;
using Conversation.Domain.Enum;
using Ecommerce_13.Hubs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ConversationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly TimeZoneInfo _indiaTimeZone;

    public ConversationController(IMediator mediator,IHubContext<ChatHub> hubContext)
    {
        _mediator = mediator;
        _hubContext = hubContext;
        _indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
    }


    private Guid GetUserIdFromToken()
    {
        // Try multiple standard claim types to find the User ID
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? User.FindFirst("userId")?.Value
                       ?? User.FindFirst("uid")?.Value
                       ?? User.FindFirst("sub")?.Value
                       ?? User.FindFirst("id")?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            // Logging the failure to help debug session issues
            Console.WriteLine("Auth Error: User ID claim not found in token.");
            throw new UnauthorizedAccessException(
                "User ID not found in JWT token. Please log in again.");
        }

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            Console.WriteLine($"Auth Error: Invalid Guid format for user ID claim: {userIdClaim}");
            throw new UnauthorizedAccessException(
                "Invalid session session format. Please log in again.");
        }

        return userId;
    }

  
    private string GetUserRoleFromToken()
    {
        return User.FindFirst(ClaimTypes.Role)?.Value
            ?? User.FindFirst("role")?.Value
            ?? "User";
    }

    private string GetUserNameFromToken()
    {
        return User.FindFirst(ClaimTypes.GivenName)?.Value
            ?? User.FindFirst("firstName")?.Value
            ?? User.FindFirst(ClaimTypes.Name)?.Value
            ?? "Unknown User";
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost("start")]
    public async Task<IActionResult> StartConversation([FromBody] StartConversationCommand command)
    {
        try
        {
            var userId = GetUserIdFromToken();
            command.UserId = userId;

            var conversationId = await _mediator.Send(command);

            return Ok(new
            {
                conversationId = conversationId,
                message = "Conversation started successfully"
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Failed to start conversation",
                details = ex.Message
            });
        }
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand command)
    {
        try
        {
            //  Validation  ConversationId is not empty
            if (command.ConversationId == Guid.Empty)
            {
                return BadRequest(new
                {
                    error = "ConversationId is required and cannot be empty."
                });
            }

            //  Validation MessageText is not empty
            if (string.IsNullOrWhiteSpace(command.MessageText))
            {
                return BadRequest(new
                {
                    error = "Message text cannot be empty."
                });
            }

            //  Extract userId from JWT token (this overwrites whatever the frontend sent)
            var userId = GetUserIdFromToken();
            command.SenderId = userId;  

            //  Get user role and name from JWT
            var role = GetUserRoleFromToken();
            var userName = GetUserNameFromToken();

            // Set sender type based on role (Case-insensitive check)
            command.SenderType = string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase) 
                ? SenderType.Admin 
                : SenderType.User;
            command.SenderName = userName;

            //  Send message via MediatR
            await _mediator.Send(command);

            DateTime nowIst = DateTime.SpecifyKind( TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _indiaTimeZone),
             DateTimeKind.Unspecified
            );

            // Broadcast to conversation group via SignalR
            await _hubContext.Clients
                .Group(command.ConversationId.ToString())
                .SendAsync("ReceiveMessage", new
                {
                    id = Guid.NewGuid(),
                    conversationId = command.ConversationId,
                    messageText = command.MessageText,
                    text = command.MessageText,              
                    senderId = userId,
                    senderName = userName,
                    senderType = command.SenderType.ToString(),
                    status = "Sent",
                    createdAt = nowIst,
                    targetUserId = command.TargetUserId
                });

            return Ok(new
            {
                success = true,
                message = "Message sent successfully",
                conversationId = command.ConversationId,
                timestamp = nowIst
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex}");
            return StatusCode(500, new
            {
                error = "Failed to send message",
                details = ex.Message
            });
        }
    }

    [Authorize]
    [HttpGet("messages/{conversationId}")]
    public async Task<IActionResult> GetMessages(Guid conversationId)
    {
        try
        {
            var result = await _mediator.Send(new GetMessagesQuery
            {
                ConversationId = conversationId
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Failed to fetch messages",
                details = ex.Message
            });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllConversations()
    {
        try
        {
            var result = await _mediator.Send(new GetAllConversationsQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Failed to fetch conversations",
                details = ex.Message
            });
        }
    }

    [Authorize(Roles = "User")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyConversations()
    {
        try
        {
            var userId = GetUserIdFromToken();

            var result = await _mediator.Send(new GetUserConversationsQuery
            {
                UserId = userId
            });

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Failed to fetch your conversations",
                details = ex.Message
            });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("close")]
    public async Task<IActionResult> CloseConversation([FromBody] CloseConversationCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Ok(new { message = "Conversation closed successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Failed to close conversation",
                details = ex.Message
            });
        }
    }
}