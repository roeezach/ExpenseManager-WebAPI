using Microsoft.EntityFrameworkCore;
using ExpensesManager.UsersService.Models;

namespace ExpensesManager.UsersService.Data
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
    }
}
