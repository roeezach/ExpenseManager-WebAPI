using ExpensesManager.DB;
using ExpensesManager.DB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExpensesManager.Automation
{
    public class AppDbContextAutomation : DbContext
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
        public bool InDocker { get; set; }

        #endregion

        #region Ctor

        private readonly IConfiguration _configuration;

        public AppDbContextAutomation(DbContextOptions<AppDbContext> options, IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AppDbContextAutomation(DbContextOptions<AppDbContext> options)
        { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Development"));
            }
        }

        #endregion
    }
}