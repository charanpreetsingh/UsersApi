using UsersApi.Models;

namespace UsersApi.Services
{
    public class UserService : IUserService
    {
        public UserService()
        { }

        public User SearchUserById(int UserId)
        {
            return new User { UserId = UserId, FirstName = "TestUser1", LastName = "TestLastName" };
        }
    }
}
