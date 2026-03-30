using Microsoft.AspNetCore.Mvc;
using YourApp.Services;

namespace YourApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Message))
                {
                    return BadRequest(new { error = "Message is required" });
                }

                if (string.IsNullOrEmpty(request.SessionId))
                {
                    return BadRequest(new { error = "SessionId is required" });
                }

                var response = await _chatService.GetResponseAsync(request.Message, request.SessionId);
                return Ok(new { response = response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in chat endpoint");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; }
        public string SessionId { get; set; }
    }
}