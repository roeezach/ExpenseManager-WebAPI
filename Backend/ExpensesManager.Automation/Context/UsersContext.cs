using ExpensesManager.Automation.Repositories.Mapper;
using ExpensesManager.Automation.Repositories.Users;

namespace ExpensesManager.Automation.Context
{
    public class UsersContext
    {
        public IUsersRepository MapperRepository { get; set; }

        public UsersContext(IUsersRepository usersRepository)
        {
            this.MapperRepository = usersRepository;
        }
    }
}