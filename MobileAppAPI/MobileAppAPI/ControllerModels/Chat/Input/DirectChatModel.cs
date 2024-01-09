namespace MobileAppAPI.ControllerModels.Chat.Input
{
    public class DirectChatModel
    {
        public Guid SessionID { get; set; } 
        public Guid RecipientID { get; set; }
    }
}
