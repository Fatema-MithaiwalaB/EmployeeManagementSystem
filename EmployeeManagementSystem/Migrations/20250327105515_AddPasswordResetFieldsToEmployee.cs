using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetFieldsToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetToken",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpiry",
                table: "Employees",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetToken",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpiry",
                table: "Employees");
        }
    }
}
