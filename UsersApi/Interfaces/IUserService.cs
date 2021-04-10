using System.Collections.Generic;
using System.Threading.Tasks;
using UsersApi.Models;

namespace UsersApi.Interfaces
{
    public interface IUserService
    {
        public Task<User> GetUserById(int userId);
        public Task<List<User>> GetUsers();
    }
}
