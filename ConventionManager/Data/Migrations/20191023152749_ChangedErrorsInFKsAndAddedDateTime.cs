using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConventionManager.Data.Migrations
{
    public partial class ChangedErrorsInFKsAndAddedDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sponsors_Conferences_ConferenceId",
                table: "Sponsors");

            migrationBuilder.AlterColumn<int>(
                name: "ConferenceId",
                table: "Sponsors",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Conferences",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Conferences",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Events_ConferenceId",
                table: "Events",
                column: "ConferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Conferences_ConferenceId",
                table: "Events",
                column: "ConferenceId",
                principalTable: "Conferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsors_Conferences_ConferenceId",
                table: "Sponsors",
                column: "ConferenceId",
                principalTable: "Conferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Conferences_ConferenceId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Sponsors_Conferences_ConferenceId",
                table: "Sponsors");

            migrationBuilder.DropIndex(
                name: "IX_Events_ConferenceId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Conferences");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Conferences");

            migrationBuilder.AlterColumn<int>(
                name: "ConferenceId",
                table: "Sponsors",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Sponsors_Conferences_ConferenceId",
                table: "Sponsors",
                column: "ConferenceId",
                principalTable: "Conferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
