using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConventionManager.Data.Migrations
{
    public partial class ModifiedAttendantsOfEventsAndConferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<string>>(
                name: "AttendantsId",
                table: "Events",
                nullable: true,
                oldClrType: typeof(List<int>),
                oldNullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "AttendantsId",
                table: "Conferences",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendantsId",
                table: "Conferences");

            migrationBuilder.AlterColumn<List<int>>(
                name: "AttendantsId",
                table: "Events",
                nullable: true,
                oldClrType: typeof(List<string>),
                oldNullable: true);
        }
    }
}
