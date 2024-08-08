using global::ExpensesManager.Automation.Repositories.Users;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace ExpensesManager.Automation.Services.UsersService
{
    public class SignUpValidator
    {
        public string UserId { get; private set; }
        public string Username { get; private set; }
        public string Token { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
       
        public void SetPostResponseBody(string responseBody)
        {
            JObject responseJson = JObject.Parse(responseBody);

            SetToken((string)responseJson["token"]);
            SetUsername((string)responseJson["username"]);
            SetUserId((string)responseJson["userID"]);

        }

        public void SetGetResponseBody(string responseBody)
        {
            JObject responseJson = JObject.Parse(responseBody);

            SetUsername((string)responseJson["username"]);
            SetUserId((string)responseJson["userID"]);
            SetFirstname((string)responseJson["firstName"]);
            SetLastname((string)responseJson["lastName"]);
            SetEmail((string)responseJson["email"]);
            SetPasswordHash((string)responseJson["password"]);
        }

        public void ValidatePostResponseBody(string expectedUsername)
        {
            Assert.IsNotNull(UserId, "User ID did not generated.");
            Assert.AreEqual(Username, expectedUsername, "Username does not match.");
            Assert.IsNotNull(Token, "Token did not generated.");
        }

        public void ValidateGetResponseBody(string expectedUsername, string expectedFirstname, string expectedLastname, string expectedEmail)
        {
            Assert.IsNotNull(UserId, "User ID did not generated.");
            Assert.IsNotNull(PasswordHash, "There is no password hash for the user.");
            Assert.AreEqual(Username, expectedUsername, "Username does not match.");
            Assert.AreEqual(FirstName, expectedFirstname, "First name mismatch.");
            Assert.AreEqual(LastName, expectedLastname, "Last name mismatch.");
            Assert.AreEqual(Email, expectedEmail, "Email mismatch.");
        }

        public void ValidateGetResponseBodyIsNull()
        {
            Assert.IsNull(UserId, "User ID did not generated.");
        }

        public void ValidatePostResponse(HttpResponseMessage response)
        {
            Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode, "Status Code Mismatch");
        }

        public void ValidateGetResponse(HttpResponseMessage response)
        {
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode, "Status Code Mismatch");
        }

        public void ValidateGetResponseBody(string expectedUsername)
        {
            Assert.IsNotNull(UserId, "User ID did not generated.");
            Assert.AreEqual(Username, expectedUsername, "Username does not match.");
            Assert.IsNotNull(Token, "Token did not generated.");
        }

        public void ValidateDeleteResponse(HttpResponseMessage response)
        {
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode, "Status Code Mismatch");
        }

        public void ValidateUserNoLongerExistOnDb(string username, IUsersRepository userRepository)
        {
            var user = userRepository.GetUserRecords(username);
            Assert.IsNull(user, " User still exist on the DB.");
        }

        public string GetUserId()
        {
            return UserId;
        }

        private void SetToken(string token)
        {
            Token = token;
        }

        private void SetUsername(string username)
        {
            Username = username;
        }

        private void SetUserId(string userId)
        {
            UserId = userId;
        }

        private void SetFirstname(string firstname)
        {
            FirstName = firstname;
        }

        private void SetLastname(string lastname)
        {
            LastName = lastname;
        }

        private void SetEmail(string email)
        {
            Email = email;
        }

        private void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }
    }
}
