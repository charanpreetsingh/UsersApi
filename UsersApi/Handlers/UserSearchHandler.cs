using MediatR;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UsersApi.Contracts;
using UsersApi.Interfaces;

namespace UsersApi.Handlers
{
    public class UserSearchHandler : IRequestHandler<UserSearchRequest, UserSearchResponse>
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly IUserDetailsService _userDetailsService;
        public UserSearchHandler(ILogger logger, IUserService userService, IUserDetailsService userDetailsService)
        {
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
            _userService = userService;
            _userDetailsService = userDetailsService;
        }
        public async Task<UserSearchResponse> Handle(UserSearchRequest request, CancellationToken cancellationToken)
        {
            _logger.Information("Handle User Search Request");
            UserDetailsResponse userDetailsResponse;
            List<UserDetailsResponse> userResponse = new List<UserDetailsResponse>();
            var allUsers = await _userService.GetUsers();
            
            var users = allUsers.Where(x => x.Name.Contains(request.SearchText)).ToList();
            
            //List<User> listOfUsers = new List<User>();
            //Dictionary<int, User> dictUsers = new Dictionary<int, User>();

            ////SearchText - 10k
            ////allUsers - 50 mil
            //foreach(int id in request.SearchText) //- 10000 times
            //{
            //    dictUsers.TryGetValue(id, out User user);

            //    User u = allUsers.Where(x => x.Id == id).FirstOrDefault(); //50 mil - 50 mil x 10000

            //    if (u != null)
            //        listOfUsers.Add(u);
            //}

            foreach (var user in users)
            {
                userDetailsResponse = await _userDetailsService.GetUserDetailsById(user.Id, false);
                userDetailsResponse.User = user;
                userResponse.Add(userDetailsResponse);
            }

            _logger.Information("Return User Search Response");
            return new UserSearchResponse { users = userResponse };
        }
    }
}
