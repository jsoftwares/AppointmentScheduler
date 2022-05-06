using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentScheduler.Migrations
{
    public partial class UpdateFieldNameInAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isDoctorApproved",
                table: "Appointments",
                newName: "IsDoctorApproved");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDoctorApproved",
                table: "Appointments",
                newName: "isDoctorApproved");
        }
    }
}
