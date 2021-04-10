using System.Collections.Generic;
using System.Threading.Tasks;
using UsersApi.Models;

namespace UsersApi.Interfaces
{
    public interface ICommentService
    {
        Task<List<Comment>> GetCommentsByPostId(int postId);
    }
}
