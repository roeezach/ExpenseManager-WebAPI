using Microsoft.EntityFrameworkCore;
using ExpensesManager.ReaderService.Models;

namespace ExpensesManager.ReaderService.Data
{
    public class ReaderDbContext : DbContext
    {
        public ReaderDbContext(DbContextOptions<ReaderDbContext> options) : base(options)
        {
        }

        public DbSet<UploadedFile> UploadedFiles { get; set; }
    }
}
