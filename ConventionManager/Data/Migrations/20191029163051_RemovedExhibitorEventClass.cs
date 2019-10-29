using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConventionManager.Data.Migrations
{
    public partial class RemovedExhibitorEventClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<List<int>>(
                name: "PracticalSessionsEvent_ExhibitorsId",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PracticalSessionsEvent_Topic",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<List<int>>(
                name: "TalkEvent_ExhibitorsId",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TalkEvent_Topic",
                table: "Events",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1fc232f5-d854-4f4a-a697-70e3aaa2f3cb", "39de7f41-ea74-4d00-ba9d-774791c2078d", "Organizer", "ORGANIZER" },
                    { "24454cac-4b35-4b3e-852b-2a82c35a3127", "363d2a52-b426-4a2c-bd53-14af31e9d3e0", "Exhibitor", "EXHIBITOR" },
                    { "f1154f97-c8f1-4ba6-b670-cc88b10b99ac", "ba0c34e8-6a88-4455-b668-64a62201d508", "User", "USER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1fc232f5-d854-4f4a-a697-70e3aaa2f3cb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24454cac-4b35-4b3e-852b-2a82c35a3127");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f1154f97-c8f1-4ba6-b670-cc88b10b99ac");

            migrationBuilder.DropColumn(
                name: "PracticalSessionsEvent_ExhibitorsId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PracticalSessionsEvent_Topic",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TalkEvent_ExhibitorsId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TalkEvent_Topic",
                table: "Events");

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
    }
}
