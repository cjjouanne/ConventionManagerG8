using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConventionManager.Data.Migrations
{
    public partial class AddedAplicationUserAndRoleSeeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Curriculum",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<List<int>>(
                name: "SuscribedConferences",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<List<int>>(
                name: "SuscribedEvents",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "83da72ed-83ef-4516-9a03-822aff944e4f", "3e5e6aba-15bd-498a-94d2-74ac317b9a50", "Organizer", "ORGANIZER" },
                    { "2c9623fe-4d0d-44fe-b7cf-4321229557c7", "af0976d1-c589-4cb3-926a-95b038708e72", "Exhibitor", "EXHIBITOR" },
                    { "dcb04a96-b27a-41fa-ba37-fcb6630ca397", "64c9731c-45fa-482e-9624-d8b42fb083ec", "User", "USER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c9623fe-4d0d-44fe-b7cf-4321229557c7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83da72ed-83ef-4516-9a03-822aff944e4f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dcb04a96-b27a-41fa-ba37-fcb6630ca397");

            migrationBuilder.DropColumn(
                name: "Curriculum",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SuscribedConferences",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SuscribedEvents",
                table: "AspNetUsers");
        }
    }
}
