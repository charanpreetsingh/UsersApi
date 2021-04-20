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
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            _logger.Information("Handle User Search Request");
            var allUsers = await _userService.GetUsers();
            Dictionary<int, User> allUserDictionary = allUsers.ToDictionary(m => m.Id);
            var users = allUserDictionary.Where(i => request.SearchUserIds.Contains(i.Key)).ToDictionary(i => i.Key, i => i.Value);
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

            List<Task<UserDetailsResponse>> listOfTasks = new List<Task<UserDetailsResponse>>();

            foreach (var user in users)
            {
                listOfTasks.Add(_userDetailsService.GetUserDetailsById(user.Key, user.Value));
            }
            userResponse = (await Task.WhenAll<UserDetailsResponse>(listOfTasks)).ToList();

            //foreach (var user in users)
            //{
            //    var userDetailsResponse = await _userDetailsService.GetUserDetailsById(user.Key, false);
            //    userDetailsResponse.User = user.Value;
            //    userResponse.Add(userDetailsResponse);
            //}
            watch.Stop();
            _logger.Information($"Execution Time: {watch.ElapsedMilliseconds} ms");
            _logger.Information("Return User Search Response");
            return new UserSearchResponse { users = userResponse };
        }
    }
}
