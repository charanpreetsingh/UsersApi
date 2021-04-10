using System.Collections.Generic;
using System.Threading.Tasks;
using UsersApi.Models;

namespace UsersApi.Interfaces
{
    public interface ITodoService
    {
        Task<List<Todo>> GetTodoByUserId(int userId);
    }
}
