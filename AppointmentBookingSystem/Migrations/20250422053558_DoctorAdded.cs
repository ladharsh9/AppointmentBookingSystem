using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class DoctorAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DoctorName",
                table: "Slots",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Slots_StartTime",
                table: "Slots",
                column: "StartTime",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Slots_StartTime",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "DoctorName",
                table: "Slots");
        }
    }
}
