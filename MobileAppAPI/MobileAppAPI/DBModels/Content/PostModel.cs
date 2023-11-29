using Microsoft.AspNetCore.Mvc;
using MobileAppAPI.DBModels.Accounts;
using System.ComponentModel.DataAnnotations;

namespace MobileAppAPI.DBModels.Content
{
    public class PostModel
    {
        [Key]
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string PostLocation { get; set; }
        public DateTime PostDateTime { get; set; }
        public string PostText { get; set; }
        public byte[] PostImage { get; set; }

        // Navigation property for User
        public UserModel User { get; set; }
    }
}
