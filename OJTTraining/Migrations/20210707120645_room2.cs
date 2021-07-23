using Microsoft.EntityFrameworkCore.Migrations;

namespace OJTTraining.Migrations
{
    public partial class room2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomStatuses",
                table: "RoomStatuses");

            migrationBuilder.RenameTable(
                name: "RoomStatuses",
                newName: "RoomStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomStatus",
                table: "RoomStatus",
                column: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomStatus",
                table: "RoomStatus");

            migrationBuilder.RenameTable(
                name: "RoomStatus",
                newName: "RoomStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomStatuses",
                table: "RoomStatuses",
                column: "Status");
        }
    }
}
