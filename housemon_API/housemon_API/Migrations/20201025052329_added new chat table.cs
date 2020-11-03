using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace housemon_API.Migrations
{
    public partial class addednewchattable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         
            migrationBuilder.CreateTable(
                name: "chats",
                columns: table => new
                {
                    chartId = table.Column<string>(nullable: false),
                    recepientId = table.Column<string>(nullable: true),
                    receiverId = table.Column<string>(nullable: true),
                    message = table.Column<string>(nullable: true),
                    timeSent = table.Column<DateTimeOffset>(nullable: false),
                    timeReceived = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chats", x => x.chartId);
                });

   
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
         

            migrationBuilder.DropTable(
                name: "chats");

     
        }
    }
}
