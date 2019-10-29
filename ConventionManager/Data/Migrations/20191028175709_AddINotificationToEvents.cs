using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConventionManager.Data.Migrations
{
    public partial class AddINotificationToEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "Events",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "endtDate",
                table: "Events",
                newName: "EndDate");

            migrationBuilder.AddColumn<List<int>>(
                name: "AttendantsId",
                table: "Events",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a643dcea-126b-4ab3-b95b-3f638fff4cbe", "ef57370e-6057-4d61-b5f2-45aca9192dac", "Organizer", "ORGANIZER" },
                    { "ef61267d-8d3d-46e7-90ad-480f92f72867", "a00c45af-5717-41de-8bee-121178b0c59b", "Exhibitor", "EXHIBITOR" },
                    { "827ec21b-7096-4c4d-9059-a7afd409b188", "4601ee65-6df2-4a51-905a-31ac7cf68d5c", "User", "USER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "827ec21b-7096-4c4d-9059-a7afd409b188");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a643dcea-126b-4ab3-b95b-3f638fff4cbe");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ef61267d-8d3d-46e7-90ad-480f92f72867");

            migrationBuilder.DropColumn(
                name: "AttendantsId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Events",
                newName: "startDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Events",
                newName: "endtDate");

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
    }
}
