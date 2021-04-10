using Serilog;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using UsersApi.Helpers;
using UsersApi.Interfaces;
using UsersApi.Models;

namespace UsersApi.Services
{
    public class CommentService : ICommentService
    {
        private readonly ILogger _logger;
        private readonly WebAPIClient _client;
        public CommentService(ILogger logger, WebAPIClient client)
        {
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
            _client = client;
        }

        public async Task<List<Comment>> GetCommentsByPostId(int postId)
        {
            _logger.Information("Request GetCommentsByPostId - " + postId);
            var response = await _client.Request(RestSharp.Method.GET, "/comments?postId=" + postId);
            var comments = JsonSerializer.Deserialize<List<Comment>>(response, _client.SerializerOptions());
            return comments;
        }
    }
}
