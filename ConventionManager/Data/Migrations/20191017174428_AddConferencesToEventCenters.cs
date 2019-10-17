using Microsoft.EntityFrameworkCore.Migrations;

namespace ConventionManager.Data.Migrations
{
    public partial class AddConferencesToEventCenters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Conferences_EventCenterId",
                table: "Conferences");

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_EventCenterId",
                table: "Conferences",
                column: "EventCenterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Conferences_EventCenterId",
                table: "Conferences");

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_EventCenterId",
                table: "Conferences",
                column: "EventCenterId",
                unique: true);
        }
    }
}
