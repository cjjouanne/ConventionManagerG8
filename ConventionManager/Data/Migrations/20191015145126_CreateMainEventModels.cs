using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ConventionManager.Data.Migrations
{
    public partial class CreateMainEventModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Events",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModeratorId",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConferenceId",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Events",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "endtDate",
                table: "Events",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "startDate",
                table: "Events",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<List<int>>(
                name: "ExhibitorsId",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "Events",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventCenters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ConferenceId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Type = table.Column<string>(maxLength: 30, nullable: false),
                    Location = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCenters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FoodEventId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    TypeOfFood = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Foods_Events_FoodEventId",
                        column: x => x.FoodEventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EventCenterId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Capacity = table.Column<int>(nullable: false),
                    Location = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_EventCenters_EventCenterId",
                        column: x => x.EventCenterId,
                        principalTable: "EventCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_RoomId",
                table: "Events",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_FoodEventId",
                table: "Foods",
                column: "FoodEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_EventCenterId",
                table: "Rooms",
                column: "EventCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Rooms_RoomId",
                table: "Events",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Rooms_RoomId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "Foods");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "EventCenters");

            migrationBuilder.DropIndex(
                name: "IX_Events_RoomId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ModeratorId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ConferenceId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "endtDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "startDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ExhibitorsId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Topic",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Events",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);
        }
    }
}
