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

        #endregion

        #region Ctor

        public AppDbContextAutomation()
        {
            Environment.SpecialFolder folder = Environment.SpecialFolder.ApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "ExpensesManagerApp.db");
        }

        #endregion

        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
              => optionsBuilder.UseSqlite($"Data Source={DbPath}");
        #endregion

    }
}