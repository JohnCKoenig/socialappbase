using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.API
{
    internal class URLS
    {
        private const string BaseUrl = "http://192.168.0.124:26046"; 

        public static string GetFullUrl(string endpoint)
        {
            return $"{BaseUrl}/{endpoint}";
        }

        public static string PostsEndpoint => GetFullUrl("content/posts");
        public static string SignInEndpoint => GetFullUrl("accounts/signin");
        public static string RefreshTokenEndPoint => GetFullUrl("accounts/refreshtoken");
        public static string DMEndPoint => GetFullUrl("rts/dm");

    }
}
