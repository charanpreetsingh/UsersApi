using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Threading.Tasks;
using UsersApi.Contracts;

namespace UsersApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public UserController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<UserDetailsResponse>> UserById(int userId)
        {
            try
            {
                UserDetailsRequest request = new UserDetailsRequest { UserId = userId };
                _logger.Information("Request Received - {@request}", JsonConvert.SerializeObject(request));

                var result = await _mediator.Send(request);

                _logger.Information("Response Sent");
                return Ok(result);
            }
            catch (Exception ex)
            {
                string exceptionMessage = string.Format("{0}", (ex.InnerException == null) ? ex.Message : string.Format("{0} - {1}", ex.Message, ex.InnerException.Message));
                _logger.Error("Error: " + exceptionMessage);
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("search/{text}")]
        public async Task<ActionResult<UserSearchResponse>> SearchUser(string text)
        {
            try
            {
                UserSearchRequest request = new UserSearchRequest { SearchText = text };
                _logger.Information("Request Received - {@request}", JsonConvert.SerializeObject(request));

                var result = await _mediator.Send(request);

                _logger.Information("Response Sent");
                return Ok(result);
            }
            catch (Exception ex)
            {
                string exceptionMessage = string.Format("{0}", (ex.InnerException == null) ? ex.Message : string.Format("{0} - {1}", ex.Message, ex.InnerException.Message));
                _logger.Error("Error: " + exceptionMessage);
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("searchbygeo")]
        public async Task<ActionResult<UserSearchByGeoResponse>> SearchUserbyGeoLocation([FromQuery] string lat, string lon)
        {
            try
            {
                UserSearchByGeoRequest request = new UserSearchByGeoRequest { Lat = lat, Lon = lon };
                _logger.Information("Request Received - {@request}", JsonConvert.SerializeObject(request));

                var result = await _mediator.Send(request);

                _logger.Information("Response Sent");
                return Ok(result);
            }
            catch (Exception ex)
            {
                string exceptionMessage = string.Format("{0}", (ex.InnerException == null) ? ex.Message : string.Format("{0} - {1}", ex.Message, ex.InnerException.Message));
                _logger.Error("Error: " + exceptionMessage);
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("/summary")]
        public async Task<ActionResult<SummaryResponse>> SummaryReport()
        {
            try
            {
                SummaryRequest request = new SummaryRequest { };
                _logger.Information("Request Received - {@request}", JsonConvert.SerializeObject(request));

                var result = await _mediator.Send(request);

                _logger.Information("Response Sent");
                return Ok(result);
            }
            catch (Exception ex)
            {
                string exceptionMessage = string.Format("{0}", (ex.InnerException == null) ? ex.Message : string.Format("{0} - {1}", ex.Message, ex.InnerException.Message));
                _logger.Error("Error: " + exceptionMessage);
                return BadRequest();
            }
        }
    }
}
