using MobileAppAPI.ControllerModels.GeneralResponses;

namespace MobileAppAPI.Services.Chat
{
    public interface IChatSessionService
    {
       
        public Task<GeneralResponseModel> CreateDirectSession();
        public Task<GeneralResponseModel> UpdateDirectSession();
        public Task<GeneralResponseModel> DeleteDirectSession();
        public Task<GeneralResponseModel> CreateGroupSession();
        public Task<GeneralResponseModel> UpdateGroupSession();
        public Task<GeneralResponseModel> DeleteGroupSession();
        public void GetSession();
        /// <summary>
        /// Get a list of all chat sessions a user has
        /// </summary>
        public void GetSessions();
        /// <summary>
        /// Get a list of participants in a chat session
        /// </summary>
        public void GetParticipants();
    }
}
