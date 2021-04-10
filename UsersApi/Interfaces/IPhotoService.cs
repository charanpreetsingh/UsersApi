using System.Collections.Generic;
using System.Threading.Tasks;
using UsersApi.Models;

namespace UsersApi.Interfaces
{
    public interface IPhotoService
    {
        Task<List<Photo>> GetPhotosByAlbumId(int albumId);
    }
}
