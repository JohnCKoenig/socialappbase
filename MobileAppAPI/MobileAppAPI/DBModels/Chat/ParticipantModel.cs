using Microsoft.EntityFrameworkCore;
using MobileAppAPI.ControllerModels.Chat;
using System.ComponentModel.DataAnnotations;

namespace MobileAppAPI.DBModels.Chat
{
    
    public class ParticipantModel
    {
        public Guid ChatId { get; set; }
        public Guid UserId { get; set; }
        public DateTime JoinedAt { get; set; }
        public ParticipantRole Role { get; set; }
        public bool NotificationsEnabled { get; set; }
        public ChatSessionStatus SessionStatus { get; set; }
    }
}
