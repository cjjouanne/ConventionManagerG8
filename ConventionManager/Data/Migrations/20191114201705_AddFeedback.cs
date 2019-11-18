using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ConventionManager.Data.Migrations
{
    public partial class AddFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventFeedback",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EventId = table.Column<int>(nullable: false),
                    Overall = table.Column<int>(nullable: false),
                    Organization = table.Column<int>(nullable: false),
                    Attention = table.Column<int>(nullable: false),
                    RoomQuality = table.Column<int>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    WouldRecommend = table.Column<int>(nullable: false),
                    Other = table.Column<string>(nullable: true),
                    HasFood = table.Column<bool>(nullable: false),
                    FoodQuality = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventFeedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventFeedback_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExhibitorFeedback",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ExhibitorId = table.Column<string>(nullable: true),
                    Overall = table.Column<int>(nullable: false),
                    Preparation = table.Column<int>(nullable: false),
                    Attitude = table.Column<int>(nullable: false),
                    Voice = table.Column<int>(nullable: false),
                    Connection = table.Column<int>(nullable: false),
                    Other = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExhibitorFeedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExhibitorFeedback_AspNetUsers_ExhibitorId",
                        column: x => x.ExhibitorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventFeedback_EventId",
                table: "EventFeedback",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_ExhibitorFeedback_ExhibitorId",
                table: "ExhibitorFeedback",
                column: "ExhibitorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventFeedback");

            migrationBuilder.DropTable(
                name: "ExhibitorFeedback");
            
        }
    }
}
