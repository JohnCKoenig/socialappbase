using System.ComponentModel.DataAnnotations;

namespace MobileAppAPI.DBModels.Accounts
{
    public class RefreshTokenModel
    {
        [Key]
        public Guid refreshtokenid { get; set; }
        public Guid userid { get; set; }
        public string token { get; set; }
        public DateTime expirydate { get; set; }

    }
}
