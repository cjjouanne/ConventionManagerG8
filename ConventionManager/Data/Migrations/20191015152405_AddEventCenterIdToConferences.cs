using Microsoft.EntityFrameworkCore.Migrations;

namespace ConventionManager.Data.Migrations
{
    public partial class AddEventCenterIdToConferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventCenterId",
                table: "Conferences",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventCenterId",
                table: "Conferences");
        }
    }
}
