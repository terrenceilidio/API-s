using Microsoft.EntityFrameworkCore.Migrations;

namespace housemon_API.Migrations
{
    public partial class Changedsalarytoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "salary",
                table: "managers",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "salary",
                table: "managers",
                type: "float",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
