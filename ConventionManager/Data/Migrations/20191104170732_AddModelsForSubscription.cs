using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ConventionManager.Data.Migrations
{
    public partial class AddModelsForSubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendantsId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ExhibitorsId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AttendantsId",
                table: "Conferences");

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<string>(nullable: true),
                    ConferenceId = table.Column<int>(nullable: false),
                    EventId = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_EventId",
                table: "Subscriptions",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.AddColumn<List<string>>(
                name: "AttendantsId",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<List<int>>(
                name: "ExhibitorsId",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "AttendantsId",
                table: "Conferences",
                nullable: true);
        }
    }
}
