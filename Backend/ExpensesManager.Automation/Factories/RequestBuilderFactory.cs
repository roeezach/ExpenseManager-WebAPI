using ExpensesManager.Automation.AutomationUtils;
using ExpensesManager.Automation.Services;
using ExpensesManager.Automation.Services.UsersService;
using System.Text;
using System.Net.Http;


namespace ExpensesManager.Automation.Factories
{
    public static class RequestBuilderFactory
    {
        public static HttpClient CreateHttpClient() 
        {
            return new HttpClient ();
        }
        
        public static SignUpRequest CreateRegistrationRequest(string username ,string password, string firstName, string lastName, string email)
        {
            return new SignUpRequest
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };
        }

        public static GetUserRequest CreateGetUserRequest(string username)
        {
            return new GetUserRequest { Username = username };
        }
    }
}
