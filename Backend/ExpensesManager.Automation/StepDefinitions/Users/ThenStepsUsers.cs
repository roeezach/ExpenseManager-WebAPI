using ExpensesManager.Automation.Services.UsersService;
using ExpensesManager.Automation.Utilities;
using TechTalk.SpecFlow;
using ExpensesManager.Automation.Repositories.Users;
using ExpensesManager.Automation.Repositories.Mapper;

namespace ExpensesManager.Automation.StepDefinitions.Users
{

    [Binding]
    public class ThenStepsUsers
    {
        private ScenarioContext _context;
        private readonly HttpResponseLogger _responseLogger;
        private SignUpValidator _validator;
        private IUsersRepository _usersRepository;

        public ThenStepsUsers(ScenarioContext context, HttpResponseLogger logger, SignUpValidator validator)
        {
            _context = context;
            _responseLogger = logger;
            _usersRepository = _context.Get<IUsersRepository>("Database"); ;
            _validator = validator;            
        }

        [Then(@"The registration request is succesfull")]
        public void ThenTheRegistrationRequestIsSuccesfull()
        {
            HttpResponseMessage response = _context.Get<HttpResponseMessage>("Response");            
            _validator.ValidatePostResponse(response);
        }

        [Then(@"The response parameters are valid with '([^']*)'")]
        public void ThenTheResponseParametersAreValidWith(string expectedUserName)
        {
            string responseBody = _context.Get<string>("ResponseBody");

            _validator.SetPostResponseBody(responseBody);

            _validator.ValidatePostResponseBody(expectedUserName);
        }

        [Then(@"I found the user")]
        public void ThenIFoundTheUser()
        {
            HttpResponseMessage getUserResponse = _context.Get<HttpResponseMessage>("GetUserResponse");
            _validator.ValidateGetResponse(getUserResponse);
        }

        [Then(@"the User is Valid with the paramaters and '([^']*)' '([^']*)' and '([^']*)' and '([^']*)'")]
        public void ThenTheUserIsValidWithTheParamatersAndAndAnd(string username, string firstname, string lastname, string email)
        {
            string getUserResponseBody = _context.Get<string>("GetUserResponseBody");

            _validator.SetGetResponseBody(getUserResponseBody);
            _validator.ValidateGetResponseBody(username,firstname, lastname, email);            
        }

        [Then(@"The delete Request is Valid")]
        public void ThenTheDeleteRequestIsValid()
        {
            HttpResponseMessage getUserResponse = _context.Get<HttpResponseMessage>("DeleteUserResponse");
            _validator.ValidateDeleteResponse(getUserResponse);
        }

        [Then(@"The user no longer exist")]
        public void ThenTheUserNoLongerExist()
        {
            string getUserResponseBody = _context.Get<string>("GetUserResponseBody");
            
            _validator.SetGetResponseBody(getUserResponseBody);
            _validator.ValidateGetResponseBodyIsNull();
        }

        [Then(@"The user with no longer exist on the DB")]
        public void ThenTheUserWithNoLongerExistOnTheDB()
        {
            string username = _context.Get<string>("Username");
            _validator.ValidateUserNoLongerExistOnDb(username,_usersRepository);
        }
    }
}
