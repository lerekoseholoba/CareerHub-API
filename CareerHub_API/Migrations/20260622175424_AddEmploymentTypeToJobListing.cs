using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerHub_API.Migrations
{
    /// <inheritdoc />
    public partial class AddEmploymentTypeToJobListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EmploymentType",
                table: "job_listings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "EmploymentType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "EmploymentType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "EmploymentType",
                value: 2);

            migrationBuilder.UpdateData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                column: "EmploymentType",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EmploymentType",
                table: "job_listings",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "EmploymentType",
                value: "Full-Time");

            migrationBuilder.UpdateData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "EmploymentType",
                value: "Full-Time");

            migrationBuilder.UpdateData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "EmploymentType",
                value: "Contract");

            migrationBuilder.UpdateData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                column: "EmploymentType",
                value: "Full-Time");
        }
    }
}
