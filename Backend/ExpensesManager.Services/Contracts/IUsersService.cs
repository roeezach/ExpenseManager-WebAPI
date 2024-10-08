
using ExpensesManager.DB.Models;
using ExpensesManager.Services;

namespace ExpensesManager.Services.Contracts
{
  public interface IUsersService
  {
    AuthenticatedUsers SignUp(Users users);
    AuthenticatedUsers SignIn(Users users);
    Users? GetUserByUsername(string username);
    void DeleteUserByUserId(int userId);
  }
}