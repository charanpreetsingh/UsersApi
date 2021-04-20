using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;
using UsersApi.Contracts;
using UsersApi.Interfaces;

namespace UsersApi.Handlers
{
    public class UserDetailsHandler : IRequestHandler<UserDetailsRequest, UserDetailsResponse>
    {
        private readonly ILogger _logger;
        private readonly IUserDetailsService _userDetailsService;

        public UserDetailsHandler(ILogger logger, IUserService userService, IUserDetailsService userDetailsService)
        {
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
            _userDetailsService = userDetailsService;
        }
        public async Task<UserDetailsResponse> Handle(UserDetailsRequest request, CancellationToken cancellationToken)
        {
            _logger.Information("Handle User Details Request");

            //var userDetailsResponse = await _userDetailsService.GetUserDetailsById(request.UserId, false);
            var userDetailsResponse = await _userDetailsService.GetUserDetailsById(request.UserId, null);

            _logger.Information("Return User Details Response");

            return userDetailsResponse;
        }
    }
}