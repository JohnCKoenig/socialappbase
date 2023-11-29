using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvShare.API.Account
{
    internal interface ISignInService
    {
        /// <summary>
        /// Uses the refresh token to refresh the users current session
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public Task<string> RefreshTokenAsync(string refreshToken);
        /// <summary>
        /// Authenticates a user, returning them a token and refresh token
        /// </summary>
        /// <param name="username">username or email</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        public Task<string> SignInAsync(string username, string password);
    }
}
