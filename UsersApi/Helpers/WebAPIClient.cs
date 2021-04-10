using Polly;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace UsersApi.Helpers
{
    public class WebAPIClient
    {
        private readonly ILogger _logger;
        private string baseURL = "https://jsonplaceholder.typicode.com";
        public WebAPIClient(ILogger logger)
        {
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
        }
        public async Task<string> Request(RestSharp.Method method, string endPoint)
        {
            _logger.Information("Requesting EndPoint - " + endPoint);
            RestClient client = new RestClient(baseURL);
            RestRequest request = new RestRequest(endPoint, method);
            IRestResponse response = new RestResponse();
            //Implement retry Policy
            var maxRetryAttempts = 3;
            var retryCount = 0;
            var pauseBetweenFailures = TimeSpan.FromSeconds(2);
            var retryPolicy = Policy
                .HandleResult<IRestResponse>(r => r.StatusCode != System.Net.HttpStatusCode.OK)
                .WaitAndRetryAsync(maxRetryAttempts, i => pauseBetweenFailures);
            await retryPolicy.ExecuteAsync(async () =>
            {
                retryCount++;
                _logger.Information("Requesting EndPoint - " + endPoint + ", Attempt:" + retryCount);
                response = await client.ExecuteAsync(request);
                return response;
            });
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _logger.Information("Successful response received from EndPoint - " + endPoint);
                return response.Content;
            }
            _logger.Error("Unsuccessful response received from EndPoint - " + endPoint + ", Error : " + response.ErrorMessage);
            throw new Exception("Request to EndPoint - " + endPoint + " failed, Error : " + response.ErrorMessage);
        }
        public JsonSerializerOptions SerializerOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return options;
        }
    }
}
