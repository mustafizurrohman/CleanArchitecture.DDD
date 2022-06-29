using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.DDD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedDoctorSpeciality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Unknown");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Doctors");
        }
    }
}
