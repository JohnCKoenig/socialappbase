using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MobileAppAPI.DBModels;
using MobileAppAPI.Services.Chat;

namespace MobileAppAPI.RTS.Chat
{
    [Authorize]
    public class DirectMessageService : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public DirectMessageService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task SendMessageToUser(string receiverUserId, string message)
        {
            var sender = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
            if (sender != null && receiverUserId!=null)
            {
                await Clients.User(receiverUserId).SendAsync("ReceiveMessage", sender, message);
                DirectChatSessionService chatsrv = new DirectChatSessionService(_context, _configuration);
                await chatsrv.CreateSession(Guid.Parse(sender), Guid.Parse(receiverUserId));
            }
          
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
