using System.Collections.Generic;
using System.Threading.Tasks;
using UsersApi.Models;

namespace UsersApi.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsByUserId(int userId);
    }
}
