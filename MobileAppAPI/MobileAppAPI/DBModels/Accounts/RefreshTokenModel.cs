using System.ComponentModel.DataAnnotations;

namespace MobileAppAPI.DBModels.Accounts
{
    public class RefreshTokenModel
    {
        [Key]
        public int refreshtokenid { get; set; }
        public int userid { get; set; }
        public string token { get; set; }
        public DateTime expirydate { get; set; }

    }
}
