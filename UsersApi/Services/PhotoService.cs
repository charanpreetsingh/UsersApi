using Serilog;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using UsersApi.Helpers;
using UsersApi.Interfaces;
using UsersApi.Models;

namespace UsersApi.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly ILogger _logger;
        private readonly WebAPIClient _client;
        public PhotoService(ILogger logger, WebAPIClient client)
        {
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
            _client = client;
        }

        public async Task<List<Photo>> GetPhotosByAlbumId(int albumId)
        {
            _logger.Information("Request GetPhotosByAlbumId - " + albumId);
            var response = await _client.Request(RestSharp.Method.GET, "/photos?albumId=" + albumId);
            var photos = JsonSerializer.Deserialize<List<Photo>>(response, _client.SerializerOptions());
            return photos;
        }
    }
}
