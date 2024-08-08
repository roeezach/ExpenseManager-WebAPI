using ExpensesManager.Automation.Factories;
using ExpensesManager.Automation.Repositories.Users;
using ExpensesManager.Automation.Services;
using ExpensesManager.Automation.Services.UsersService;
using ExpensesManager.Automation.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace ExpensesManager.Automation.StepDefinitions.Users
{
    [Binding]
    public class WhenStepsUsers
    {
        private ScenarioContext _context;
        private IHttpClient _httpClient;
        private ISpecFlowOutputHelper _specFlowOutputHelper;
        private readonly HttpResponseLogger _responseLogger;


        public WhenStepsUsers(ISpecFlowOutputHelper specFlowOutputHelper,ScenarioContext context, HttpResponseLogger logger)
        {
            _context = context;
            _specFlowOutputHelper = specFlowOutputHelper;
            _httpClient = _context.Get<IHttpClient>("HttpClient");
            _responseLogger = logger;
        }

        [When(@"I send the Request")]
        public async Task WhenISendTheRequest()
        {            
            var signUpRequest = _context.Get<HttpRequestMessage>("SignUpRequest");

            HttpResponseMessage response = await _httpClient.SendAsync(signUpRequest);
            string responseBody = await response.Content.ReadAsStringAsync();

            await _responseLogger.LogResponseAsync(response, responseBody);

            _context.Add("Response", response);
            _context.Add("ResponseBody", responseBody);
        }

        [When(@"I send the GetUser request")]
        public async Task WhenISendTheGetUserRequest()
        {
            var getUserRequest = _context.Get<HttpRequestMessage>("GetUserRequest");
            
            HttpResponseMessage response = await _httpClient.SendAsync(getUserRequest);
            string responseBody = await response.Content.ReadAsStringAsync();

            await _responseLogger.LogResponseAsync(response, responseBody);

            _context.Add("GetUserResponse", response);
            _context.Add("GetUserResponseBody", responseBody);
        }

        [When(@"I send the delete request")]
        public async Task WhenISendTheDeleteRequest()
        {
            var DeleteUserRequest = _context.Get<HttpRequestMessage>("DeleteUserRequest");
            
            HttpResponseMessage response = await _httpClient.SendAsync(DeleteUserRequest);

            await _responseLogger.LogResponseAsyncStatusCode(response);

            _context.Add("DeleteUserResponse", response);
        }
    }
}
