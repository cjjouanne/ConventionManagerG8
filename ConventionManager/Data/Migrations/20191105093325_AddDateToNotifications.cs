using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConventionManager.Data.Migrations
{
    public partial class AddDateToNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SentOn",
                table: "Notifications",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentOn",
                table: "Notifications");
            
        }
    }
}
