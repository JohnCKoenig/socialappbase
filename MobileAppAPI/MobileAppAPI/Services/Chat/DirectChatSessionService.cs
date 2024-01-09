using Microsoft.EntityFrameworkCore;
using MobileAppAPI.ControllerModels;
using MobileAppAPI.ControllerModels.Accounts.Response;
using MobileAppAPI.ControllerModels.Chat;
using MobileAppAPI.ControllerModels.Chat.Response;
using MobileAppAPI.ControllerModels.GeneralResponses;
using MobileAppAPI.DBModels;
using MobileAppAPI.DBModels.Chat;
using MobileAppAPI.Services.Accounts;
using System.Runtime.Intrinsics.X86;

//TODO: Add checks if users are friends once friend services are implemented
namespace MobileAppAPI.Services.Chat
{
    public class DirectChatSessionService : IChatSessionService
    {
        private readonly ApplicationDbContext _context;
        private static IConfiguration _configuration;

        public DirectChatSessionService(ApplicationDbContext context, IConfiguration configuration)
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
        public async Task<GeneralResponseModel<ChatSessionResponseModel>> CreateSession(Guid sender, Guid recipient)
        {
            //Make sure the sender and recipient aren't the same person
            if (sender == recipient)
            {
                return new GeneralResponseModel<ChatSessionResponseModel>(ResponseCode.ReflectiveChat, "Cannot create a chat with yourself");
            }
            //Get the username from account service so we can name the chat appropriately
            AccountService actsrv = new AccountService(_context, _configuration);
            var recpient = await actsrv.GetUser(recipient);
            if (recpient.Data.username == null)
            {
                return new GeneralResponseModel<ChatSessionResponseModel>(ResponseCode.RecipientDoesNotExist, "The specified recipient does not exist");
            }
            //If we have an existing ession, get that instead of creating a new one
            var existingChatSession = await GetExistingSession(sender, recipient);
            if (existingChatSession != null)
            {
                return GeneralResponseModel<ChatSessionResponseModel>.SuccessResponse(existingChatSession);
            }
            var chatSession = new ChatSessionModel
            {
                ChatId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ChatSessionName = recpient.Data.username,
                IsGroupChat = false,

            };

            //Create a new chat session
            _context.ChatSessions.Add(chatSession);
            await _context.SaveChangesAsync();

            //Add both users to the chat with the same permissions, since this is a direct chat
            await AddParticipantToChat(chatSession.ChatId, sender, ParticipantRole.User, true, ChatSessionStatus.Active);
            await AddParticipantToChat(chatSession.ChatId, recipient, ParticipantRole.User, true, ChatSessionStatus.Active);
            return GeneralResponseModel<ChatSessionResponseModel>.SuccessResponse(new ChatSessionResponseModel { ChatId=chatSession.ChatId,ChatSessionName=chatSession.ChatSessionName,IsGroupChat=chatSession.IsGroupChat});

        }
       
        /// <summary>
        /// Gets a direct chat session between two users if one exists
        /// </summary>
        /// <param name="user1">First user in the session</param>
        /// <param name="user2">Second user in the session</param>
        /// <returns>A response containing a chat between the two users</returns>
        private async Task<ChatSessionResponseModel> GetExistingSession(Guid user1, Guid user2)
        {
            // Query the ChatParticipants table for sessions involving either user
            var chatIdsForUser1 = _context.ChatParticipants
                .Where(cp => cp.UserId == user1)
                .Select(cp => cp.ChatId);

            var chatIdsForUser2 = _context.ChatParticipants
                .Where(cp => cp.UserId == user2)
                .Select(cp => cp.ChatId);

            // Find common sessions between both users
            var commonChatIds = chatIdsForUser1.Intersect(chatIdsForUser2);

            // Query the ChatSessions table to find a direct chat session
            var directChatSession = await _context.ChatSessions
                .Where(cs => commonChatIds.Contains(cs.ChatId) && !cs.IsGroupChat)
                .FirstOrDefaultAsync();
            if(directChatSession !=null)
            {
                ChatSessionResponseModel existingSession = new ChatSessionResponseModel() { ChatId = directChatSession.ChatId, ChatSessionName = directChatSession.ChatSessionName, IsGroupChat = directChatSession.IsGroupChat };
                return existingSession;
            }

            return null;
        }
        /// <summary>
        /// Deletes a direct chat session. 
        /// </summary>
        /// <param name="userId">The user deleting the chat session</param>
        /// <param name="chatId">The chat ID to delete</param>
        /// <returns>A status code indicating whether deletion was succesful</returns>
        public async Task<GeneralResponseModel<ChatSessionResponseModel>> DeleteSession(Guid userId, Guid chatId)
        {
            /*
             * This method is incomplete. One user should not be able to delete the contents of a chat for everyone
             * In the future, there will be a separate table to keep track of when a user last ran the delete operation.
             * This means messages will only be loaded back until the point that the individual user last deleted.
             */
            var chatSession = await (from cs in _context.ChatSessions
                                     join cp in _context.ChatParticipants on cs.ChatId equals cp.ChatId
                                     where cs.ChatId == chatId && cp.UserId == userId
                                     select cs).FirstOrDefaultAsync();

            if (chatSession == null)
            {
                return new GeneralResponseModel<ChatSessionResponseModel>(ResponseCode.ChatSessionDoesNotExist, "The specified chat does not exist");
            }
            _context.ChatSessions.Remove(chatSession);
            await _context.SaveChangesAsync();
            return GeneralResponseModel<ChatSessionResponseModel>.SuccessResponse(null);
        }
        /// <summary>
        /// Lets a participant archive a chat session
        /// </summary>
        /// <param name="userId">The participant who is archiving the chat</param>
        /// <param name="chatId">The ID of the chat being archived</param>
        /// <returns></returns>
        public async Task<GeneralResponseModel<ParticipantModel>> ArchiveSession(Guid userId, Guid chatId)
        {
            //Select a chat participant because a chat is only archived for the participant
            var chatParticipant = await _context.ChatParticipants
                .Where(cp => cp.UserId == userId && cp.ChatId == chatId)
                .FirstOrDefaultAsync();

            if (chatParticipant == null)
            {
                return GeneralResponseModel<ParticipantModel>.ErrorResponse("Chat participant not found");
            }
            //Set the chat status to archived for the particpant
            chatParticipant.SessionStatus = ChatSessionStatus.Archived;

            _context.ChatParticipants.Update(chatParticipant);
            await _context.SaveChangesAsync();

            return GeneralResponseModel<ParticipantModel>.SuccessResponse(chatParticipant);
        }

        /// <summary>
        /// Loads a list of all current direct chat sessions for a user
        /// </summary>
        /// <param name="userId">The user ID to load the sessions for</param>
        /// <returns></returns>
        public async Task<GeneralResponseModel<List<ChatSessionResponseModel>>> LoadSessions(Guid userId)
        {
            //Load all sessions for a given participant that are not currently marked as archived
            var chatSessions = await (from cp in _context.ChatParticipants
                                      join cs in _context.ChatSessions on cp.ChatId equals cs.ChatId
                                      where cp.UserId == userId && cp.SessionStatus != ChatSessionStatus.Archived
                                      select new ChatSessionResponseModel { ChatId = cs.ChatId, ChatSessionName = cs.ChatSessionName, IsGroupChat = cs.IsGroupChat })
                              .ToListAsync();

            return GeneralResponseModel<List<ChatSessionResponseModel>>.SuccessResponse(chatSessions);
        }

        /// <summary>
        /// Loads metadata for a chat session
        /// </summary>
        /// <param name="userId">The user making the request</param>
        /// <param name="chatId">The chat to load metadata for</param>
        /// <returns></returns>
        public async Task<GeneralResponseModel<ChatSessionResponseModel>> LoadSessionMetadata(Guid userId, Guid chatId)
        {
            var chatSession = await _context.ChatSessions.FirstOrDefaultAsync(cs => cs.ChatId == chatId);
            if (chatSession == null)
            {
                return new GeneralResponseModel<ChatSessionResponseModel>(ResponseCode.ChatSessionDoesNotExist, "The specified chat session does not exist");
            }
            var chatSessionResponseModel = new ChatSessionResponseModel { ChatId = chatSession.ChatId, ChatSessionName = chatSession.ChatSessionName, IsGroupChat = chatSession.IsGroupChat };
            return GeneralResponseModel<ChatSessionResponseModel>.SuccessResponse(chatSessionResponseModel);
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
            //Though this method is common to the group and direct messages, we only allow one participant to be added to a direct message
            var chatSession = await _context.ChatSessions.FirstOrDefaultAsync(cs => cs.ChatId == chatId);
            if (chatSession == null)
            {
                return new GeneralResponseModel<object>(ResponseCode.ChatSessionDoesNotExist, "The specified chat session does not exist");
            }
            //Check if the type is a group chat, if so, we definitely messed up
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
    }
}
