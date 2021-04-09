using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UsersApi.Contracts;
using UsersApi.Services;

namespace UsersApi.Handlers
{
    public class UserSearchHandler : IRequestHandler<UserSearchRequest, UserSearchResponse>
    {
        private readonly IUserService _userService;
        public UserSearchHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<UserSearchResponse> Handle(UserSearchRequest request, CancellationToken cancellationToken)
        {
            var user = _userService.SearchUserById(request.UserId);
            return new UserSearchResponse { User = user };
        }
    }
}
