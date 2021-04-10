using Serilog;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using UsersApi.Helpers;
using UsersApi.Interfaces;
using UsersApi.Models;

namespace UsersApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly ILogger _logger;
        private readonly WebAPIClient _client;
        public TodoService(ILogger logger, WebAPIClient client)
        {
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
            _client = client;
        }

        public async Task<List<Todo>> GetTodoByUserId(int userId)
        {
            _logger.Information("Request GetTodoByUserId - " + userId);
            var response = await _client.Request(RestSharp.Method.GET, "/todos?userId=" + userId);
            var todos = JsonSerializer.Deserialize<List<Todo>>(response, _client.SerializerOptions());
            return todos;
        }
    }
}
