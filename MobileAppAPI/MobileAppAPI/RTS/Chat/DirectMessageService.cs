using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MobileAppAPI.RTS.Chat
{
    [Authorize]
    public class DirectMessageService : Hub
    {

        public async Task SendMessageToUser(string receiverUserId, string message)
        {
            await Clients.User(receiverUserId).SendAsync("ReceiveMessage", Context.User?.FindFirst(System.Security.Claims.ClaimTypes.Name).Value, message);
        }
        public override async Task OnConnectedAsync()
        {

            var userIdClaim = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var userid = userIdClaim?.Value;


            if (userid != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userid);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User?.Identity?.Name;
            if (userId != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
