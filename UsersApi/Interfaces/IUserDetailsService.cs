using System.Threading.Tasks;
using UsersApi.Contracts;
using UsersApi.Models;

namespace UsersApi.Interfaces
{
    public interface IUserDetailsService
    {   
        public Task<UserDetailsResponse> GetUserDetailsById(int userId, User user);
        //public Task<UserDetailsResponse> GetUserDetailsById(int userId, bool fetchUser);
    }
}
