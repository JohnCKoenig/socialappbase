namespace MobileAppAPI.ControllerModels.Chat.Response
{
    public class ChatSessionResponseModel
    {
       public Guid ?ChatId { get; set; }
       public bool IsGroupChat { get; set; }
       public string ?ChatSessionName { get; set; }
    }
}
