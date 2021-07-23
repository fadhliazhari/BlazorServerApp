using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace OJTTraining.Migrations
{
    public partial class capacity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomStatus");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Rooms");

            migrationBuilder.AddColumn<int>(
                name: "RoomCapacity",
                table: "Rooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomCapacity",
                table: "Rooms");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Rooms",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RoomStatus",
                columns: table => new
                {
                    Status = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RegisterDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RegisterPIC = table.Column<string>(type: "text", nullable: true),
                    UpdateDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatePIC = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomStatus", x => x.Status);
                });
        }
    }
}
