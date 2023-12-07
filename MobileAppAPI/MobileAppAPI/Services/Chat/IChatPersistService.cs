namespace MobileAppAPI.Services.Chat
{
    public interface IChatPersistService
    {

        public Task<bool> WriteMessage(string chat_id, string message_text, string sender_id);
    }
}
