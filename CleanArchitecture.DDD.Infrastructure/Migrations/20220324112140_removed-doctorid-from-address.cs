using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.DDD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeddoctoridfromaddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Doctors_DoctorID",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_DoctorID",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "DoctorID",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_AddressId",
                table: "Doctors",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Addresses_AddressId",
                table: "Doctors",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Addresses_AddressId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_AddressId",
                table: "Doctors");

            migrationBuilder.AddColumn<Guid>(
                name: "DoctorID",
                table: "Addresses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_DoctorID",
                table: "Addresses",
                column: "DoctorID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Doctors_DoctorID",
                table: "Addresses",
                column: "DoctorID",
                principalTable: "Doctors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
