using Serilog;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using UsersApi.Helpers;
using UsersApi.Interfaces;
using UsersApi.Models;

namespace UsersApi.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger _logger;
        private readonly WebAPIClient _client;
        public UserService(ILogger logger, WebAPIClient client)
        {
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
            _client = client;
        }


        public async Task<User> GetUserById(int userId)
        {
            _logger.Information("Request GetUserById - " + userId);
            var response = await _client.Request(RestSharp.Method.GET, "/users/" + userId);
            var user = JsonSerializer.Deserialize<User>(response, _client.SerializerOptions());
            return user;
        }
        public async Task<List<User>> GetUsers()
        {
            _logger.Information("Request GetUsers");
            var response = await _client.Request(RestSharp.Method.GET, "/users");
            var users = JsonSerializer.Deserialize<List<User>>(response, _client.SerializerOptions());
            return users;
        }
    }
}
