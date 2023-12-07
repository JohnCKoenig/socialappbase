using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Models.Account
{
    internal class TokenResponseModel
    {
        public string AccessToken { get; set; }
        public string AccessTokenExpiry { get; set; }
        public string RefreshToken { get; set; }
        public string RefreshTokenExpiryExpiry { get; set; }
    }
}
