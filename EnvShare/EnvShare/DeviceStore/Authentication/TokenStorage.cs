using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;


namespace EnvShare.DeviceStore.Authentication
{
    internal class TokenStorage
    {
        //TokenStorage() { }
        public static async Task SaveTokenAsync(string token)
        {
            try
            {
                await SecureStorage.SetAsync("AuthToken", token);
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }

        public static async Task<string> GetTokenAsync()
        {
            try
            {
                string token = await SecureStorage.GetAsync("AuthToken");
                return token;
            }
            catch (Exception ex)
            {
                // Handle error
                return null;
            }
        }

        public static async Task<string> GetRefreshTokenAsync()
        {
            try
            {
                string refreshToken = await SecureStorage.GetAsync("RefreshToken");
                return refreshToken;
            }
            catch (Exception ex)
            {
                // Handle error
                return null;
            }
        }
        public static async Task SaveRefreshTokenAsync(string refreshToken)
        {
            try
            {
                await SecureStorage.SetAsync("RefreshToken", refreshToken);
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }
    }
}
