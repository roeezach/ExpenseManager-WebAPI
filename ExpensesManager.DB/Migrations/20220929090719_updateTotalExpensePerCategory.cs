using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesManager.DB.Migrations
{
    public partial class updateTotalExpensePerCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TotalExpensesPerCategory",
                table: "TotalExpensesPerCategory");

            migrationBuilder.AlterColumn<int>(
                name: "SW_UserID",
                table: "TotalExpensesPerCategory",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "ItemID",
                table: "TotalExpensesPerCategory",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TotalExpensesPerCategory",
                table: "TotalExpensesPerCategory",
                column: "ItemID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TotalExpensesPerCategory",
                table: "TotalExpensesPerCategory");

            migrationBuilder.DropColumn(
                name: "ItemID",
                table: "TotalExpensesPerCategory");

            migrationBuilder.AlterColumn<int>(
                name: "SW_UserID",
                table: "TotalExpensesPerCategory",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TotalExpensesPerCategory",
                table: "TotalExpensesPerCategory",
                column: "SW_UserID");
        }
    }
}
