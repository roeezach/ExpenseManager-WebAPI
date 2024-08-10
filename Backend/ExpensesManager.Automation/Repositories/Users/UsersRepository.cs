using ExpensesManager.DB.Models;
using ExpensesManager.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesManager.Automation.Repositories.Users
{
    public class UsersRepository : IUsersRepository
    {

        private readonly AppDbContext m_AppDbContext;

        public List<DB.Models.Users> MappedRecords { get; set; }

        public UsersRepository(AppDbContext context)
        {
            m_AppDbContext = context;
        }

        public void SetUserRecords()
        {
            MappedRecords = m_AppDbContext.Users.ToList();
        }

        public List<DB.Models.Users> GetAllUsersRecords()
        {
            return MappedRecords;
        }
        
        public DB.Models.Users? GetUserRecords(string username)
        {
            return m_AppDbContext.Users.FirstOrDefault(u => u.Username == username);
        }

    }
}
