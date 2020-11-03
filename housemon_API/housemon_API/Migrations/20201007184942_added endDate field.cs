using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace housemon_API.Migrations
{
    public partial class addedendDatefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "houseId",
                table: "leases");

            migrationBuilder.DropColumn(
                name: "signature",
                table: "leases");

            migrationBuilder.DropColumn(
                name: "updatedAt",
                table: "leases");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "endDate",
                table: "leases",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "endDate",
                table: "leases");

            migrationBuilder.AddColumn<string>(
                name: "houseId",
                table: "leases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "signature",
                table: "leases",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updatedAt",
                table: "leases",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
