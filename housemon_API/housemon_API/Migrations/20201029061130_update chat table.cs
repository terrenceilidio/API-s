using Microsoft.EntityFrameworkCore.Migrations;

namespace housemon_API.Migrations
{
    public partial class updatechattable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "recepientId",
                table: "chats");

            migrationBuilder.AddColumn<string>(
                name: "senderId",
                table: "chats",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "senderId",
                table: "chats");

            migrationBuilder.AddColumn<string>(
                name: "recepientId",
                table: "chats",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
