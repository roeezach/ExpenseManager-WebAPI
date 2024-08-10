using ExpenseManager.Automation.Utils;
using ExpensesManager.Automation.Context;
using ExpensesManager.Automation.Repositories.Users;
using ExpensesManager.DB;
using TechTalk.SpecFlow;
using ExpensesManager.Automation.Services;
using ExpensesManager.Automation.Services.UsersService;
using ExpensesManager.Automation.Factories;
using System.Threading.Tasks; // For asynchronous programming
using System.Net.Http; // Ensure this is imported for HttpClient usage
using ExpensesManager.Automation.Utilities;

namespace ExpensesManager.Automation.StepDefinitions.Users
{
    [Binding]
    public class GivenStepsUsers
    {
        private ScenarioContext _scenarioContext;
        //private AppDbContext _appDbContextAutomation;
        //private UsersContext _usersContext;
        private IUsersRepository _usersRepository;
        private IHttpClient _httpClient;

        public GivenStepsUsers(ScenarioContext scenarioContext, AppDbContext appDbContext, IHttpClient httpClient)
        {
            _scenarioContext = scenarioContext;
            _usersRepository = new UsersRepository(appDbContext);
            _httpClient = httpClient;

            _scenarioContext.Add("Database", _usersRepository);
            _scenarioContext.Add("HttpClient", _httpClient);
        }

        [Given(@"I register to the app with '([^']*)' and '([^']*)' and '([^']*)' and '([^']*)' and '([^']*)'")]
        public void GivenIRegisterToTheAppWithAndAndAndAnd(string username, string password, string firstname, string lastName, string email)
        {
            SignUpRequest signUpRequest = RequestBuilderFactory.CreateRegistrationRequest(username, password, firstname, lastName, email);

            HttpRequestMessage requestBuilder = new RequestBuilder<SignUpRequest>
              (HttpMethod.Post, UrlFactory.CreateUrl("/Users/SignUp/SignUp"))
             .AddJsonContent(signUpRequest)
             .Build();

            _scenarioContext.Add("SignUpRequest", requestBuilder);
        }

        [Given(@"I build the  GetUser Requrst with '([^']*)'")]
        public void GivenIBuildTheGetUserRequrstWith(string username)
        {
            HttpRequestMessage requestBuilder = new RequestBuilder<GetUserRequest>
              (HttpMethod.Get, UrlFactory.CreateGetUserUrl(username, "/Users/GetUser/GetUser"))
             .Build();

            _scenarioContext.Add("GetUserRequest", requestBuilder);
            _scenarioContext.Add("Username", username);
        }

        [Given(@"I build the delete request")]
        public void GivenIBuildTheDeleteRequest()
        {
            string username = _scenarioContext.Get<string>("Username");

            HttpRequestMessage deleteRequestBuilder = new RequestBuilder<GetUserRequest>
              (HttpMethod.Delete, UrlFactory.CreateGetUserUrl(username, "/Users/DeleteUser"))
             .Build();

            _scenarioContext.Add("DeleteUserRequest", deleteRequestBuilder);
        }

    }
}
