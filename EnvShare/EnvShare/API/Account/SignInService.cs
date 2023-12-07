using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.Models.Account;

namespace MobileApp.API.Account
{
    internal class SignInService : ISignInService
    {
        private readonly HttpClient _httpClient;

        public SignInService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> SignInAsync(string username, string password)
        {
            try
            {
                var signInModel = new SignInModel { Username = username, Password = password }; 
                var json = JsonConvert.SerializeObject(signInModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(URLS.SignInEndpoint, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<TokenResponseModel>(responseContent);

                return token.AccessToken;
            }
            catch (Exception ex)
            {
                // Handle error
                return null;
            }
        }
        public async Task<string> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var refreshModel = new RefreshTokenModel { Token = refreshToken }; 
                var json = JsonConvert.SerializeObject(refreshModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(URLS.GetFullUrl("refresh"), content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<TokenResponseModel>(responseContent);

                return token.AccessToken;
            }
            catch (Exception ex)
            {
                // Handle error
                return null;
            }
        }
    }
}
