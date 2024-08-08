using System.Collections;
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

        private readonly IConfiguration _configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString;
                string isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
                // Check if running in Docker
                if (isDocker == "true")
                {
                    connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
                }
                else
                {
                    // Use configuration when running locally
                    connectionString = _configuration.GetConnectionString("Development");
                }

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string is null or empty.");
                }

                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        #endregion

    }
}