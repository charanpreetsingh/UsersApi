using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System.Threading.Tasks;
using UsersApi.Contracts;

namespace UsersApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public UserController(IConfiguration configuration, IMediator mediator, ILogger logger)
        {
            _configuration = configuration;
            _mediator = mediator;
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<UserSearchResponse>> UserById(int userId)
        {
            UserSearchRequest request = new UserSearchRequest { UserId = userId };
            _logger.Information("Request Received - {@request}", JsonConvert.SerializeObject(request));
            
            var result = await _mediator.Send(request);

            _logger.Information("Response Sent - {@response}", JsonConvert.SerializeObject(result));
            return Ok(result);
        }
    }
}
