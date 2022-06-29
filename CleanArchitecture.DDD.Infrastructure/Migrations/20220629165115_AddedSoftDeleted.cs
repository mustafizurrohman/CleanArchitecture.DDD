using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.DDD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedSoftDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "Addresses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "Addresses");
        }
    }
}
