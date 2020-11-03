using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace housemon_API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "complaints",
                columns: table => new
                {
                    complaintId = table.Column<string>(nullable: false),
                    userId = table.Column<string>(nullable: true),
                    propertyId = table.Column<string>(nullable: true),
                    dateMade = table.Column<DateTime>(nullable: false),
                    isResolved = table.Column<bool>(nullable: false),
                    dateResolved = table.Column<DateTime>(nullable: false),
                    data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_complaints", x => x.complaintId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    userId = table.Column<string>(nullable: false),
                    names = table.Column<string>(maxLength: 100, nullable: true),
                    gender = table.Column<string>(maxLength: 6, nullable: true),
                    idNumber = table.Column<string>(maxLength: 13, nullable: true),
                    username = table.Column<string>(maxLength: 50, nullable: true),
                    cellNumber = table.Column<string>(maxLength: 20, nullable: true),
                    password = table.Column<string>(maxLength: 16, nullable: true),
                    role = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: false),
                    updatedAt = table.Column<DateTime>(nullable: true),
                    profilePicture = table.Column<string>(nullable: true),
                    propertyId = table.Column<string>(nullable: true),
                    salary = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "landlords",
                columns: table => new
                {
                    userId = table.Column<string>(nullable: false),
                    names = table.Column<string>(maxLength: 100, nullable: true),
                    gender = table.Column<string>(maxLength: 6, nullable: true),
                    idNumber = table.Column<string>(maxLength: 13, nullable: true),
                    username = table.Column<string>(maxLength: 50, nullable: true),
                    cellNumber = table.Column<string>(maxLength: 20, nullable: true),
                    password = table.Column<string>(maxLength: 16, nullable: true),
                    role = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: false),
                    updatedAt = table.Column<DateTime>(nullable: true),
                    profilePicture = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_landlords", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "leases",
                columns: table => new
                {
                    leaseId = table.Column<string>(nullable: false),
                    userId = table.Column<string>(nullable: true),
                    propertyId = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: false),
                    updatedAt = table.Column<DateTime>(nullable: false),
                    signature = table.Column<byte[]>(nullable: true),
                    startDate = table.Column<DateTime>(nullable: false),
                    houseId = table.Column<string>(nullable: true),
                    data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leases", x => x.leaseId);
                });

            migrationBuilder.CreateTable(
                name: "notices",
                columns: table => new
                {
                    noticeId = table.Column<string>(nullable: false),
                    userId = table.Column<string>(nullable: true),
                    propertyId = table.Column<string>(nullable: true),
                    endDate = table.Column<DateTime>(nullable: false),
                    data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notices", x => x.noticeId);
                });

            migrationBuilder.CreateTable(
                name: "properties",
                columns: table => new
                {
                    propertyId = table.Column<string>(nullable: false),
                    rooms = table.Column<int>(nullable: false),
                    address = table.Column<string>(maxLength: 200, nullable: true),
                    country = table.Column<string>(nullable: true),
                    rules = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: false),
                    updatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_properties", x => x.propertyId);
                });

            migrationBuilder.CreateTable(
                name: "landlordProperties",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    LandLorduserId = table.Column<string>(nullable: true),
                    propertyId = table.Column<string>(nullable: true),
                    LastVisit = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_landlordProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_landlordProperties_landlords_LandLorduserId",
                        column: x => x.LandLorduserId,
                        principalTable: "landlords",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_landlordProperties_properties_propertyId",
                        column: x => x.propertyId,
                        principalTable: "properties",
                        principalColumn: "propertyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "managers",
                columns: table => new
                {
                    userId = table.Column<string>(nullable: false),
                    names = table.Column<string>(maxLength: 100, nullable: true),
                    gender = table.Column<string>(maxLength: 6, nullable: true),
                    idNumber = table.Column<string>(maxLength: 13, nullable: true),
                    username = table.Column<string>(maxLength: 50, nullable: true),
                    cellNumber = table.Column<string>(maxLength: 20, nullable: true),
                    password = table.Column<string>(maxLength: 16, nullable: true),
                    role = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: false),
                    updatedAt = table.Column<DateTime>(nullable: true),
                    profilePicture = table.Column<string>(nullable: true),
                    salary = table.Column<int>(nullable: false),
                    propertyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_managers", x => x.userId);
                    table.ForeignKey(
                        name: "FK_managers_properties_propertyId",
                        column: x => x.propertyId,
                        principalTable: "properties",
                        principalColumn: "propertyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tenants",
                columns: table => new
                {
                    userId = table.Column<string>(nullable: false),
                    names = table.Column<string>(maxLength: 100, nullable: true),
                    gender = table.Column<string>(maxLength: 6, nullable: true),
                    idNumber = table.Column<string>(maxLength: 13, nullable: true),
                    username = table.Column<string>(maxLength: 50, nullable: true),
                    cellNumber = table.Column<string>(maxLength: 20, nullable: true),
                    password = table.Column<string>(maxLength: 16, nullable: true),
                    role = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: false),
                    updatedAt = table.Column<DateTime>(nullable: true),
                    profilePicture = table.Column<string>(nullable: true),
                    leaseId = table.Column<string>(nullable: true),
                    propertyId = table.Column<string>(nullable: true),
                    rentAmount = table.Column<int>(nullable: false),
                    deposit = table.Column<int>(nullable: false),
                    rentDueDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenants", x => x.userId);
                    table.ForeignKey(
                        name: "FK_tenants_properties_propertyId",
                        column: x => x.propertyId,
                        principalTable: "properties",
                        principalColumn: "propertyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_landlordProperties_LandLorduserId",
                table: "landlordProperties",
                column: "LandLorduserId");

            migrationBuilder.CreateIndex(
                name: "IX_landlordProperties_propertyId",
                table: "landlordProperties",
                column: "propertyId");

            migrationBuilder.CreateIndex(
                name: "IX_managers_propertyId",
                table: "managers",
                column: "propertyId");

            migrationBuilder.CreateIndex(
                name: "IX_tenants_propertyId",
                table: "tenants",
                column: "propertyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "complaints");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "landlordProperties");

            migrationBuilder.DropTable(
                name: "leases");

            migrationBuilder.DropTable(
                name: "managers");

            migrationBuilder.DropTable(
                name: "notices");

            migrationBuilder.DropTable(
                name: "tenants");

            migrationBuilder.DropTable(
                name: "landlords");

            migrationBuilder.DropTable(
                name: "properties");
        }
    }
}
