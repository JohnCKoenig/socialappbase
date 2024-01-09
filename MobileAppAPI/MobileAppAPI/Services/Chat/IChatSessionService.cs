using MobileAppAPI.ControllerModels.Chat.Response;
using MobileAppAPI.ControllerModels.GeneralResponses;
using MobileAppAPI.DBModels.Chat;

namespace MobileAppAPI.Services.Chat
{
    public interface IChatSessionService
    {

        Task<GeneralResponseModel<ChatSessionResponseModel>> CreateSession(Guid sender, Guid recipient);
        Task<GeneralResponseModel<ChatSessionResponseModel>> DeleteSession(Guid userId, Guid chatId);
        Task<GeneralResponseModel<ParticipantModel>> ArchiveSession(Guid userId, Guid chatId);
        Task<GeneralResponseModel<List<ChatSessionResponseModel>>> LoadSessions(Guid userId);
        Task<GeneralResponseModel<ChatSessionResponseModel>> LoadSessionMetadata(Guid userId, Guid chatId);
    }
}
