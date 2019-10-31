using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConventionManager.Data.Migrations
{
    public partial class FixedEventsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
