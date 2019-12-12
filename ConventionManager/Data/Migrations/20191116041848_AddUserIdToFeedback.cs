using Microsoft.EntityFrameworkCore.Migrations;

namespace ConventionManager.Data.Migrations
{
    public partial class AddUserIdToFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ExhibitorFeedback",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EventFeedback",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExhibitorFeedback_UserId",
                table: "ExhibitorFeedback",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventFeedback_UserId",
                table: "EventFeedback",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventFeedback_AspNetUsers_UserId",
                table: "EventFeedback",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExhibitorFeedback_AspNetUsers_UserId",
                table: "ExhibitorFeedback",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventFeedback_AspNetUsers_UserId",
                table: "EventFeedback");

            migrationBuilder.DropForeignKey(
                name: "FK_ExhibitorFeedback_AspNetUsers_UserId",
                table: "ExhibitorFeedback");

            migrationBuilder.DropIndex(
                name: "IX_ExhibitorFeedback_UserId",
                table: "ExhibitorFeedback");

            migrationBuilder.DropIndex(
                name: "IX_EventFeedback_UserId",
                table: "EventFeedback");
            
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ExhibitorFeedback");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EventFeedback");
        }
    }
}
