using System.Threading.Tasks;
using ExpensesManager.UsersService.Data;
using ExpensesManager.UsersService.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UsersService.Tests
{
    public class UsersDbContextTests
    {
        private UsersDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<UsersDbContext>()
                .UseInMemoryDatabase(databaseName: "test_users_db")
                .Options;
            return new UsersDbContext(options);
        }

        [Fact]
        public async Task Can_add_and_read_user()
        {
            using var ctx = CreateInMemoryContext();
            var user = new Users { UserID = 1, Username = "bob", Password = "p", CreditCardChargeDay = 1 };
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();

            var fetched = await ctx.Users.FirstOrDefaultAsync(u => u.UserID == 1);
            Assert.NotNull(fetched);
            Assert.Equal("bob", fetched.Username);
        }
    }
}
