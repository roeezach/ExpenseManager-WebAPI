using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesManager.DB.Migrations
{
    public partial class editCategoriesMigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Categories",
                newName: "MappedCategoriesJson");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "Categories",
                newName: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MappedCategoriesJson",
                table: "Categories",
                newName: "CategoryName");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Categories",
                newName: "CategoryID");
        }
    }
}
