using System.Threading.Tasks;
using ExpensesManager.ReaderService.Data;
using ExpensesManager.ReaderService.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ReaderService.Tests
{
    public class ReaderDbContextTests
    {
        private ReaderDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ReaderDbContext>()
                .UseInMemoryDatabase(databaseName: "test_reader_db")
                .Options;
            return new ReaderDbContext(options);
        }

        [Fact]
        public async Task Can_add_and_read_uploadedfile()
        {
            using var ctx = CreateInMemoryContext();
            var file = new UploadedFile { Id = 1, UserID = 2, FileName = "f.csv", UploadDate = System.DateTime.UtcNow, FileType = "csv", LinkedMonth = 1, LinkedYear = 2025 };
            ctx.UploadedFiles.Add(file);
            await ctx.SaveChangesAsync();

            var fetched = await ctx.UploadedFiles.FirstOrDefaultAsync(u => u.Id == 1);
            Assert.NotNull(fetched);
            Assert.Equal("f.csv", fetched.FileName);
        }
    }
}
