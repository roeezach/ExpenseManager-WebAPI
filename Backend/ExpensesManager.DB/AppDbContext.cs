using ExpensesManager.DB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExpensesManager.DB
{
    public class AppDbContext : DbContext
    {

        #region Properties
        public DbSet<ExpenseRecord> Expenses { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<SwRecords> SpliteWise { get; set; }
        public DbSet<TotalExpensePerCategory> TotalExpensesPerCategory { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<RecalculatedExpenseRecord> RecalculatedExpenseRecords { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public string DbPath { get; }

        #endregion

        #region Ctor

        public AppDbContext()
        {
            Environment.SpecialFolder folder = Environment.SpecialFolder.ApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "ExpensesManagerApp.db");
        }

        #endregion

        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            // Check if running in a Docker container
            bool inDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

            // Configure the database path based on the environment
            if (inDocker)
            {
                // Use the specific path within the container for the SQLite database
                optionsBuilder.UseSqlite($"Data Source=/app/data/ExpensesManagerApp.db");
            }
            else
            {
                // Use the local development path for the SQLite database
                optionsBuilder.UseSqlite($"Data Source={DbPath}");
            }
        }

        #endregion

    }
}