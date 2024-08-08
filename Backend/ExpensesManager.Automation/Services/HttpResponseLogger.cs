using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Infrastructure;

namespace ExpensesManager.Automation.Utilities
{
    public class HttpResponseLogger
    {
        private readonly ISpecFlowOutputHelper _outputHelper;

        public HttpResponseLogger(ISpecFlowOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        public async Task LogResponseAsync(HttpResponseMessage response, string responseBody)
        {
            _outputHelper.WriteLine($"Status Code: {response.StatusCode}");
            _outputHelper.WriteLine($"Response Body: {responseBody}");
        }

        public async Task LogResponseAsyncStatusCode(HttpResponseMessage response)
        {
            _outputHelper.WriteLine($"Status Code: {response.StatusCode}");
        }
    }
}
