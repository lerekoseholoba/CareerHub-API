using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CareerHub_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "applicants",
                columns: new[] { "Id", "Email", "Name" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-1111-1111-1111-111111111111"), "john@example.com", "John Doe" },
                    { new Guid("bbbbbbbb-2222-2222-2222-222222222222"), "sarah@example.com", "Sarah Lee" },
                    { new Guid("cccccccc-3333-3333-3333-333333333333"), "michael@example.com", "Michael Smith" },
                    { new Guid("dddddddd-4444-4444-4444-444444444444"), "aisha@example.com", "Aisha Khan" },
                    { new Guid("eeeeeeee-5555-5555-5555-eeeeeeeeeeee"), "david@example.com", "David Brown" }
                });

            migrationBuilder.InsertData(
                table: "companies",
                columns: new[] { "Id", "Industry", "Name", "Website" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Software", "TechNova", "https://technova.com" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Finance", "FinCore", "https://fincore.com" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Healthcare", "HealthPlus", "https://healthplus.com" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Education", "EduSmart", "https://edusmart.com" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Energy", "GreenEnergy", "https://greenenergy.com" }
                });

            migrationBuilder.InsertData(
                table: "job_listings",
                columns: new[] { "Id", "CompanyId", "Description", "Location", "PostedDate", "Title" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Build APIs with .NET", "Remote", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Backend Developer" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Analyse financial data", "Johannesburg", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Financial Analyst" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Patient care and support", "Cape Town", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nurse Specialist" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Teach computer science", "Pretoria", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lecturer" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Renewable energy systems", "Durban", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Energy Engineer" }
                });

            migrationBuilder.InsertData(
                table: "applications",
                columns: new[] { "ApplicantId", "JobListingId", "Status", "SubmittedAt" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111"), 0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("bbbbbbbb-2222-2222-2222-222222222222"), new Guid("22222222-2222-2222-2222-222222222222"), 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("cccccccc-3333-3333-3333-333333333333"), new Guid("33333333-3333-3333-3333-333333333333"), 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("dddddddd-4444-4444-4444-444444444444"), new Guid("44444444-4444-4444-4444-444444444444"), 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("eeeeeeee-5555-5555-5555-eeeeeeeeeeee"), new Guid("55555555-5555-5555-5555-555555555555"), 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "applications",
                keyColumns: new[] { "ApplicantId", "JobListingId" },
                keyValues: new object[] { new Guid("aaaaaaaa-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "applications",
                keyColumns: new[] { "ApplicantId", "JobListingId" },
                keyValues: new object[] { new Guid("bbbbbbbb-2222-2222-2222-222222222222"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "applications",
                keyColumns: new[] { "ApplicantId", "JobListingId" },
                keyValues: new object[] { new Guid("cccccccc-3333-3333-3333-333333333333"), new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "applications",
                keyColumns: new[] { "ApplicantId", "JobListingId" },
                keyValues: new object[] { new Guid("dddddddd-4444-4444-4444-444444444444"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "applications",
                keyColumns: new[] { "ApplicantId", "JobListingId" },
                keyValues: new object[] { new Guid("eeeeeeee-5555-5555-5555-eeeeeeeeeeee"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "applicants",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "applicants",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "applicants",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "applicants",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "applicants",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-5555-5555-5555-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "companies",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "companies",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "companies",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "companies",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "companies",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));
        }
    }
}
