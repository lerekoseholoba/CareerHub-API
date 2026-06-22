using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CareerHub_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedAndJobResponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "companies",
                columns: new[] { "Id", "Industry", "Name", "Website" },
                values: new object[,]
                {
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Healthcare", "HealthPlus", "https://healthplus.com" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Education", "EduSmart", "https://edusmart.com" }
                });

            migrationBuilder.UpdateData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "ClosingDate", "Description", "PostedDate" },
                values: new object[] { new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ASP.NET Core backend development role", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "job_listings",
                columns: new[] { "Id", "ClosingDate", "CompanyId", "Description", "EmploymentType", "IsOpen", "Location", "PostedDate", "SalaryMax", "SalaryMin", "Title" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2027, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "React developer role", "Full-Time", true, "Johannesburg", new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 55000m, 25000m, "Frontend Developer" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2027, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Finance data analysis role", "Contract", true, "Pretoria", new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 45000m, 20000m, "Data Analyst" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2027, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Build healthcare technology solutions", "Full-Time", true, "Durban", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 70000m, 35000m, "Healthcare Systems Engineer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "companies",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "companies",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.UpdateData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "ClosingDate", "Description", "PostedDate" },
                values: new object[] { new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Seed job", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });
        }
    }
}
