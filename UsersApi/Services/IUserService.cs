using UsersApi.Models;

namespace UsersApi.Services
{
    public interface IUserService
    {
        User SearchUserById(int UserId);
    }
}
