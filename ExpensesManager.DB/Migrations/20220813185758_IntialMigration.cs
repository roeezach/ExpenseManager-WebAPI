using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesManager.DB.Migrations
{
    public partial class IntialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    TransactionID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Card_Details = table.Column<string>(type: "TEXT", nullable: false),
                    Transaction_Date = table.Column<string>(type: "TEXT", nullable: false),
                    Record_Create_Date = table.Column<string>(type: "TEXT", nullable: false),
                    Expense_Description = table.Column<string>(type: "TEXT", nullable: false),
                    Price_Amount = table.Column<double>(type: "REAL", nullable: false),
                    Debit_Amount = table.Column<double>(type: "REAL", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    Exchange_Rate = table.Column<double>(type: "REAL", nullable: false),
                    Exchange_Description = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    User_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    Linked_Month = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.TransactionID);
                });

            migrationBuilder.CreateTable(
                name: "SpliteWise",
                columns: table => new
                {
                    Internal_TransactionID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SW_TransactionID = table.Column<int>(type: "INTEGER", nullable: false),
                    SW_User_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    Total_Cost = table.Column<double>(type: "REAL", nullable: false),
                    Creation_Method = table.Column<string>(type: "TEXT", nullable: false),
                    Owed_Share = table.Column<double>(type: "REAL", nullable: false),
                    Paid_Share = table.Column<double>(type: "REAL", nullable: false),
                    Expense_Creation_Date = table.Column<string>(type: "TEXT", nullable: false),
                    Record_Creation_Date = table.Column<string>(type: "TEXT", nullable: false),
                    Expense_Description = table.Column<string>(type: "TEXT", nullable: false),
                    Linked_Month = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpliteWise", x => x.Internal_TransactionID);
                });

            migrationBuilder.CreateTable(
                name: "TotalExpensesPerCategory",
                columns: table => new
                {
                    SW_UserID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Total_Amount = table.Column<double>(type: "REAL", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Month = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalExpensesPerCategory", x => x.SW_UserID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    SplitwisePassword = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "SpliteWise");

            migrationBuilder.DropTable(
                name: "TotalExpensesPerCategory");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
