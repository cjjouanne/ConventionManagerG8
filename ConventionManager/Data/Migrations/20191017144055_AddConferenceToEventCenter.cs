using Microsoft.EntityFrameworkCore.Migrations;

namespace ConventionManager.Data.Migrations
{
    public partial class AddConferenceToEventCenter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConferenceId",
                table: "EventCenters");

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_EventCenterId",
                table: "Conferences",
                column: "EventCenterId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Conferences_EventCenters_EventCenterId",
                table: "Conferences",
                column: "EventCenterId",
                principalTable: "EventCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conferences_EventCenters_EventCenterId",
                table: "Conferences");

            migrationBuilder.DropIndex(
                name: "IX_Conferences_EventCenterId",
                table: "Conferences");

            migrationBuilder.AddColumn<int>(
                name: "ConferenceId",
                table: "EventCenters",
                nullable: false,
                defaultValue: 0);
        }
    }
}
