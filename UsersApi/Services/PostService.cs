using Serilog;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using UsersApi.Helpers;
using UsersApi.Interfaces;
using UsersApi.Models;

namespace UsersApi.Services
{
    public class PostService : IPostService
    {
        private readonly ILogger _logger;
        private readonly WebAPIClient _client;
        public PostService(ILogger logger, WebAPIClient client)
        {
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
            _client = client;
        }

        public async Task<List<Post>> GetPostsByUserId(int userId)
        {
            _logger.Information("Request GetPostsByUserId - " + userId);
            var response = await _client.Request(RestSharp.Method.GET, "/posts?userId=" + userId);
            var posts = JsonSerializer.Deserialize<List<Post>>(response, _client.SerializerOptions());
            return posts;
        }
    }
}
