using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace OJTTraining.Migrations
{
    public partial class patient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientGUID = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RoomNumber = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    CheckinDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CheckoutDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RegisterDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RegisterPIC = table.Column<string>(type: "text", nullable: true),
                    UpdateDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatePIC = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientGUID);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomNumber = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    RegisterDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RegisterPIC = table.Column<string>(type: "text", nullable: true),
                    UpdateDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatePIC = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomNumber);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
