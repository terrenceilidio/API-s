using Microsoft.EntityFrameworkCore.Migrations;

namespace housemon_API.Migrations
{
    public partial class Changedsalarytodouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "salary",
                table: "managers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "salary",
                table: "managers",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
