using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace OJTTraining.Migrations
{
    public partial class room : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Rooms",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RoomStatuses",
                columns: table => new
                {
                    Status = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    RegisterDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RegisterPIC = table.Column<string>(type: "text", nullable: true),
                    UpdateDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatePIC = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomStatuses", x => x.Status);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomStatuses");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Rooms",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);
        }
    }
}
