using System.ComponentModel.DataAnnotations;

namespace MobileAppAPI.ControllerModels.Accounts.Input
{
    public class UpdateUserModel
    {

        public string username { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
    }
}
