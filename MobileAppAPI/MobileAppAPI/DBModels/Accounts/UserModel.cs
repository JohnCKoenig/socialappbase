using System.ComponentModel.DataAnnotations;

namespace MobileAppAPI.DBModels.Accounts
{
    public class UserModel
    {
        [Key]
        public Guid userid { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string password_hash { get; set; }
    }
}
