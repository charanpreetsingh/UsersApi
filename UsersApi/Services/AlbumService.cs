using Serilog;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using UsersApi.Helpers;
using UsersApi.Interfaces;
using UsersApi.Models;

namespace UsersApi.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly ILogger _logger;
        private readonly WebAPIClient _client;
        public AlbumService(ILogger logger, WebAPIClient client)
        {
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
            _client = client;
        }

        public async Task<List<Album>> GetAlbumsByUserId(int userId)
        {
            _logger.Information("Request GetAlbumsByUserId - " + userId);
            var response = await _client.Request(RestSharp.Method.GET, "/albums?userId=" + userId);
            var albums = JsonSerializer.Deserialize<List<Album>>(response, _client.SerializerOptions());
            return albums;
        }
    }
}
