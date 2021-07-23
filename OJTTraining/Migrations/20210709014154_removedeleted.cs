using Microsoft.EntityFrameworkCore.Migrations;

namespace OJTTraining.Migrations
{
    public partial class removedeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Rooms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Rooms",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
