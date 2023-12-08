using MobileAppAPI.ControllerModels;
using MobileAppAPI.ControllerModels.Accounts.Response;
using MobileAppAPI.ControllerModels.Chat;
using MobileAppAPI.ControllerModels.GeneralResponses;
using MobileAppAPI.DBModels;
using MobileAppAPI.DBModels.Chat;
using MobileAppAPI.Services.Accounts;

//Todo: Add checks if users are friends once friend services are implemented
//Todo: Add check to see if the chat type is a group chat before allowing the add of more than one participant
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
        /// <summary>
        /// Creates a new direct message session if one does not already exist
        /// </summary>
        /// <param name="sender">The person intitating the chat</param>
        /// <param name="recipient">The account recieving the message</param>
        /// <returns></returns>
        public async Task<GeneralResponseModel<object>> CreateDirectSession(Guid sender, Guid recipient)
        {
            //Make sure the sender and recipient aren't the same person
            if (sender == recipient)
            {
                return new GeneralResponseModel<object>(ResponseCode.ReflectiveChat, "Cannot create a chat with yourself");
            }
            //Get the username from account service so we can name the chat appropriately
            AccountService actsrv = new AccountService(_context, _configuration);
            var recpient = await actsrv.GetUser(recipient);
            if (recpient.Data.username == null)
            {
                return new GeneralResponseModel<object>(ResponseCode.RecipientDoesNotExist, "The specified recipient does not exist");
            }
            var chatSession = new ChatSessionModel
            {
                ChatId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ChatSessionName = recpient.Data.username,
                IsGroupChat = false,
                SessionStatus = ChatSessionStatus.Active

            };

            //Create a new chat session
            _context.ChatSessions.Add(chatSession);
            await _context.SaveChangesAsync();

            //Add both users to the chat with the same permissions, since this is a direct chat
            await AddParticipantToChat(chatSession.ChatId, sender, ParticipantRole.User, true);
            await AddParticipantToChat(chatSession.ChatId, recipient, ParticipantRole.User, true);
            return GeneralResponseModel<object>.SuccessResponse();

        }
        /// <summary>
        /// Adds a new participant to a chat
        /// </summary>
        /// <param name="chatId">The ID Of the chat session to add to</param>
        /// <param name="userId">The ID of the user to add</param>
        /// <param name="role">The role to assign the user</param>
        /// <param name="notificationsEnabled">Whether or not the user has notifications on, true by default</param>
        /// <returns></returns>
        private async Task<GeneralResponseModel<object>> AddParticipantToChat(Guid chatId, Guid userId, ParticipantRole role, bool notificationsEnabled)
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

            return GeneralResponseModel<object>.SuccessResponse(participant);
        }
    }
}
