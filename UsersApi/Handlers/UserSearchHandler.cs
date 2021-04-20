using MediatR;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UsersApi.Contracts;
using UsersApi.Interfaces;
using UsersApi.Models;

namespace UsersApi.Handlers
{
    public class UserSearchHandler : IRequestHandler<UserSearchRequest, UserSearchResponse>
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly IUserDetailsService _userDetailsService;
        private List<UserDetailsResponse> userResponse;
        public UserSearchHandler(ILogger logger, IUserService userService, IUserDetailsService userDetailsService)
        {
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
            _userService = userService;
            _userDetailsService = userDetailsService;
            userResponse = new List<UserDetailsResponse>();
        }
        public async Task<UserSearchResponse> Handle(UserSearchRequest request, CancellationToken cancellationToken)
        {   
            _logger.Information("Handle User Search Request");
            var allUsers = await _userService.GetUsers();
            Dictionary<int, User> allUserDictionary = allUsers.ToDictionary(m => m.Id);
            List<Task<UserDetailsResponse>> listOfTasks = new List<Task<UserDetailsResponse>>();
            foreach (var userid in request.SearchUserIds)
            {
                allUserDictionary.TryGetValue(userid, out User user);
                if (user != null)
                    listOfTasks.Add(_userDetailsService.GetUserDetailsById(user.Id, user));
            }
            userResponse = (await Task.WhenAll(listOfTasks)).ToList();
            
            _logger.Information("Return User Search Response");
            return new UserSearchResponse { users = userResponse };
        }
    }
}
