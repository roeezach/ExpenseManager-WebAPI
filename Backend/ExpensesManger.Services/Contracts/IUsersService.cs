
using ExpensesManager.DB.Models;
using ExpensesManager.Services;

namespace ExpensesManger.Services.Contracts
{
    public interface IUsersService
    {
      AuthenticatedUsers SignUp(Users users);
      AuthenticatedUsers SignIn(Users users);
    }
}