using MobileAppAPI.ControllerModels.Chat;
using System.ComponentModel.DataAnnotations;

namespace MobileAppAPI.DBModels.Chat
{
    public class ChatSessionModel
    {
        [Key]
        public Guid ChatId {  get; set; }
        public DateTime CreatedAt {  get; set; }
        public string ChatSessionName { get; set; }
        public bool IsGroupChat {  get; set; }
    }
}
