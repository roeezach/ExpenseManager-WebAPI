
namespace ExpensesManager.Automation.Repositories.Users
{
    public interface IUsersRepository
    {
        List<DB.Models.Users> GetAllUsersRecords();
        public DB.Models.Users? GetUserRecords(string username);
        void SetUserRecords();
    }
}
