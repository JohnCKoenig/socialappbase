using Microsoft.EntityFrameworkCore;
using MobileAppAPI.ControllerModels.Chat;
using MobileAppAPI.ControllerModels.Chat.Response;
using MobileAppAPI.ControllerModels.GeneralResponses;
using MobileAppAPI.DBModels;
using MobileAppAPI.DBModels.Chat;

namespace MobileAppAPI.Services.Chat
{
    public class GroupChatSessionService
    {
        private readonly ApplicationDbContext _context;
        private static IConfiguration _configuration;

        public GroupChatSessionService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        /// <summary>
        /// Adds a new participant to a chat
        /// </summary>
        /// <param name="chatId">The ID Of the chat session to add to</param>
        /// <param name="userId">The ID of the user to add</param>
        /// <param name="role">The role to assign the user</param>
        /// <param name="notificationsEnabled">Whether or not the user has notifications on, true by default</param>
        /// <returns></returns>
        private async Task<GeneralResponseModel<object>> AddParticipantToChat(Guid chatId, Guid userId, ParticipantRole role, bool notificationsEnabled, ChatSessionStatus sessionStatus)
        {
            var chatSession = await _context.ChatSessions.FirstOrDefaultAsync(cs => cs.ChatId == chatId);
            if (chatSession == null)
            {
                return new GeneralResponseModel<object>(ResponseCode.ChatSessionDoesNotExist, "The specified chat session does not exist");
            }
            if (chatSession.IsGroupChat)
            {
                return new GeneralResponseModel<object>(ResponseCode.Failure, "Direct sessions cannot be group chats");
            }
            var participant = new ParticipantModel
            {
                ChatId = chatId,
                UserId = userId,
                JoinedAt = DateTime.UtcNow,
                Role = role,
                NotificationsEnabled = notificationsEnabled,
                SessionStatus = sessionStatus

            };

            _context.ChatParticipants.Add(participant);
            await _context.SaveChangesAsync();

            return GeneralResponseModel<object>.SuccessResponse(participant);
        }

        /// <summary>
        /// Removes a user from a chat session
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public async Task<GeneralResponseModel<ChatSessionResponseModel>> LeaveSession(Guid userId, Guid chatId)
        {
            var participant = await _context.ChatParticipants.FirstOrDefaultAsync(cp => cp.ChatId == chatId && cp.UserId == userId);
            if (participant == null)
            {
                return new GeneralResponseModel<ChatSessionResponseModel>(ResponseCode.ParticipantDoesNotExist, "The specified participant does not exist");
            }
            _context.ChatParticipants.Remove(participant);
            await _context.SaveChangesAsync();
            return GeneralResponseModel<ChatSessionResponseModel>.SuccessResponse(null);
        }
    }

}
