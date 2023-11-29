using EnvShare.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EnvShare.API.Posts
{
    internal class PostService
    {
        private readonly HttpClient _httpClient;

        public PostService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<PostResponseModel>> LoadPostsAsync(string jwtToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                var response = await _httpClient.GetAsync(URLS.PostsEndpoint);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var posts = JsonConvert.DeserializeObject<List<PostResponseModel>>(content);

                return posts;
            }
            catch (Exception ex)
            {
                // Handle error
                return new List<PostResponseModel>();
            }
        }
    }
}
