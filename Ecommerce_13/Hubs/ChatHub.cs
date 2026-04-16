using Microsoft.AspNetCore.SignalR;

namespace Ecommerce_13.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinConversation(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        }

        public async Task JoinAdminGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
        }

        public async Task LeaveAdminGroup()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admins");
        }

        public async Task LeaveConversation(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        }
        public async Task MarkAsRead (string conversationId)
        {
            await Clients.Users(conversationId).SendAsync("MessagesRead", conversationId);
        }
        public async Task  SendTyping (string conversationId ,string userName)
        {
            await Clients.Groups(conversationId).SendAsync("UserTyping", conversationId, userName);
        }
    }
}
