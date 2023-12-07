using MobileAppAPI.ControllerModels.Accounts.Response;
using MobileAppAPI.ControllerModels.Chat;
using MobileAppAPI.ControllerModels.GeneralResponses;
using MobileAppAPI.DBModels;
using MobileAppAPI.DBModels.Chat;
using MobileAppAPI.Services.Accounts;

namespace MobileAppAPI.Services.Chat
{
    public class ChatSessionService
    {
        private readonly ApplicationDbContext _context;
        private static IConfiguration _configuration;

        public ChatSessionService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async  Task<GeneralResponseModel> CreateDirectSession(Guid sender, Guid recipient)
        {
            if (sender == recipient)
            {
                return new GeneralResponseModel(ResponseCode.ReflectiveChat, "Cannot create a chat session with yourself");
            }
            AccountService actsrv = new AccountService(_context, _configuration);
            var recpient = await actsrv.GetUser(recipient);
            if (recpient.username == null)
            {
                return new GeneralResponseModel(ResponseCode.RecipientDoesNotExist, "Recipient does not exist");
            }
            var chatSession = new ChatSessionModel
            {
                ChatId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ChatSessionName = recpient.username,
                IsGroupChat = false,
                SessionStatus = ChatSessionStatus.Active

            };
            _context.ChatSessions.Add(chatSession);
            await _context.SaveChangesAsync();
            await AddParticipantToChat(chatSession.ChatId, sender, ParticipantRole.User, true);
            await AddParticipantToChat(chatSession.ChatId, recipient, ParticipantRole.User, true);
            return new GeneralResponseModel(ResponseCode.Success, "New chat session created");

        }
        private async Task<GeneralResponseModel> AddParticipantToChat(Guid chatId, Guid userId, ParticipantRole role, bool notificationsEnabled)
        {
            var participant = new ParticipantModel
            {
                ChatId = chatId,
                UserId = userId,
                JoinedAt = DateTime.UtcNow,
                Role = role,
                NotificationsEnabled = notificationsEnabled
            };

            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();

            return new GeneralResponseModel(ResponseCode.Success, "User has been added to the chat");
        }
    }
}
