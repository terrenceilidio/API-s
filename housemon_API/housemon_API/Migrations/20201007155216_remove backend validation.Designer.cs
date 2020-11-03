﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using housemon_API.Models;

namespace housemon_API.Migrations
{
    [DbContext(typeof(PropertyMonitorDbContext))]
    [Migration("20201007155216_remove backend validation")]
    partial class removebackendvalidation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("housemon_API.Models.Complaint", b =>
                {
                    b.Property<string>("complaintId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("dateMade")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("dateResolved")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("isResolved")
                        .HasColumnType("bit");

                    b.Property<string>("propertyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("userId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("complaintId");

                    b.HasIndex("propertyId");

                    b.ToTable("complaints");
                });

            modelBuilder.Entity("housemon_API.Models.Employee", b =>
                {
                    b.Property<string>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("cellNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("createdAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("idNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("names")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("profilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("propertyId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("salary")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("updatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("userId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("housemon_API.Models.LandLordProperty", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LandLorduserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("LastVisit")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("propertyId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("LandLorduserId");

                    b.HasIndex("propertyId");

                    b.ToTable("landlordProperties");
                });

            modelBuilder.Entity("housemon_API.Models.Landlord", b =>
                {
                    b.Property<string>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("cellNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("createdAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("idNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("names")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("profilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("updatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("userId");

                    b.ToTable("landlords");
                });

            modelBuilder.Entity("housemon_API.Models.Lease", b =>
                {
                    b.Property<string>("leaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("createdAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("houseId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("propertyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("signature")
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTimeOffset>("startDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("updatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("leaseId");

                    b.HasIndex("propertyId");

                    b.ToTable("leases");
                });

            modelBuilder.Entity("housemon_API.Models.Manager", b =>
                {
                    b.Property<string>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("cellNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("createdAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("idNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("names")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("profilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("propertyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("salary")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("updatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("userId");

                    b.HasIndex("propertyId");

                    b.ToTable("managers");
                });

            modelBuilder.Entity("housemon_API.Models.Notice", b =>
                {
                    b.Property<string>("noticeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("endDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("propertyId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("noticeId");

                    b.ToTable("notices");
                });

            modelBuilder.Entity("housemon_API.Models.Property", b =>
                {
                    b.Property<string>("propertyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("address")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("createdAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("rooms")
                        .HasColumnType("int");

                    b.Property<string>("rules")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("updatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("propertyId");

                    b.ToTable("properties");
                });

            modelBuilder.Entity("housemon_API.Models.Tenant", b =>
                {
                    b.Property<string>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("cellNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("createdAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("deposit")
                        .HasColumnType("int");

                    b.Property<string>("gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("idNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("leaseId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("names")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("profilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("propertyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("rentAmount")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("rentDueDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("updatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("userId");

                    b.HasIndex("propertyId");

                    b.ToTable("tenants");
                });

            modelBuilder.Entity("housemon_API.Models.Complaint", b =>
                {
                    b.HasOne("housemon_API.Models.Property", null)
                        .WithMany("Complaints")
                        .HasForeignKey("propertyId");
                });

            modelBuilder.Entity("housemon_API.Models.LandLordProperty", b =>
                {
                    b.HasOne("housemon_API.Models.Landlord", "LandLord")
                        .WithMany("landlordProperties")
                        .HasForeignKey("LandLorduserId");

                    b.HasOne("housemon_API.Models.Property", "Property")
                        .WithMany("landLordProperties")
                        .HasForeignKey("propertyId");
                });

            modelBuilder.Entity("housemon_API.Models.Lease", b =>
                {
                    b.HasOne("housemon_API.Models.Property", null)
                        .WithMany("Leases")
                        .HasForeignKey("propertyId");
                });

            modelBuilder.Entity("housemon_API.Models.Manager", b =>
                {
                    b.HasOne("housemon_API.Models.Property", null)
                        .WithMany("managers")
                        .HasForeignKey("propertyId");
                });

            modelBuilder.Entity("housemon_API.Models.Tenant", b =>
                {
                    b.HasOne("housemon_API.Models.Property", null)
                        .WithMany("tenants")
                        .HasForeignKey("propertyId");
                });
#pragma warning restore 612, 618
        }
    }
}