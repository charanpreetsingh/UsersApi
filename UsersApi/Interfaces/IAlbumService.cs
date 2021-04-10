using System.Collections.Generic;
using System.Threading.Tasks;
using UsersApi.Models;

namespace UsersApi.Interfaces
{
    public interface IAlbumService
    {
        Task<List<Album>> GetAlbumsByUserId(int userId);
    }
}
