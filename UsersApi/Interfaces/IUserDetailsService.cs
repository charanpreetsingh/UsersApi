using System.Threading.Tasks;
using UsersApi.Contracts;

namespace UsersApi.Interfaces
{
    public interface IUserDetailsService
    {   
        public Task<UserDetailsResponse> GetUserDetailsById(int userId, bool fetchUser);
    }
}
