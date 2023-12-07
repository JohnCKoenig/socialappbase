namespace MobileAppAPI.ControllerModels.Chat.Response
{
    public class ChatSessionModel
    {
       public Guid ?ChatId { get; set; }
       public bool IsGroupChat { get; set; }
       public string ?ChatSessionName { get; set; }
       public ChatSessionStatus SessionStatus { get; set; }
    }
}
