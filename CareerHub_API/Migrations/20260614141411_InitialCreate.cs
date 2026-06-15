using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CareerHub_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "applicants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applicants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Website = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Industry = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "job_listings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PostedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClosingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsOpen = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    EmploymentType = table.Column<string>(type: "text", nullable: false),
                    SalaryMin = table.Column<decimal>(type: "numeric", nullable: false),
                    SalaryMax = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_listings", x => x.Id);
                    table.CheckConstraint("CK_JobListing_ClosingDate", "\"ClosingDate\" >= \"PostedDate\"");
                    table.CheckConstraint("CK_JobListing_SalaryRange", "\"SalaryMax\" >= \"SalaryMin\"");
                    table.ForeignKey(
                        name: "FK_job_listings_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "applications",
                columns: table => new
                {
                    JobListingId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicantId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ResumeUrl = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CoverLetter = table.Column<string>(type: "text", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applications", x => new { x.JobListingId, x.ApplicantId });
                    table.ForeignKey(
                        name: "FK_applications_applicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "applicants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_applications_job_listings_JobListingId",
                        column: x => x.JobListingId,
                        principalTable: "job_listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "applicants",
                columns: new[] { "Id", "Email", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-1111-1111-1111-111111111111"), "john@example.com", "John Doe", "" },
                    { new Guid("bbbbbbbb-2222-2222-2222-222222222222"), "sarah@example.com", "Sarah Lee", "" },
                    { new Guid("cccccccc-3333-3333-3333-333333333333"), "michael@example.com", "Michael Smith", "" },
                    { new Guid("dddddddd-4444-4444-4444-444444444444"), "aisha@example.com", "Aisha Khan", "" },
                    { new Guid("eeeeeeee-5555-5555-5555-eeeeeeeeeeee"), "david@example.com", "David Brown", "" }
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
                columns: new[] { "Id", "ClosingDate", "CompanyId", "Description", "EmploymentType", "IsOpen", "Location", "PostedDate", "SalaryMax", "SalaryMin", "Title" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Description for Backend Developer position #1", "Full-Time", true, "Cape Town", new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 31000m, 21000m, "Backend Developer" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Description for Frontend Developer position #2", "Part-Time", true, "Johannesburg", new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 32000m, 22000m, "Frontend Developer" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Description for Full Stack Developer position #3", "Contract", true, "Pretoria", new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 33000m, 23000m, "Full Stack Developer" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Description for Cloud Engineer position #4", "Internship", true, "Durban", new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 34000m, 24000m, "Cloud Engineer" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Description for DevOps Engineer position #5", "Full-Time", true, "Remote", new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 35000m, 25000m, "DevOps Engineer" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Description for Software Engineer position #6", "Part-Time", true, "Cape Town", new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 36000m, 26000m, "Software Engineer" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Description for Data Analyst position #7", "Contract", true, "Johannesburg", new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 37000m, 27000m, "Data Analyst" },
                    { new Guid("00000000-0000-0000-0000-000000000008"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Description for Business Analyst position #8", "Internship", true, "Pretoria", new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), 38000m, 28000m, "Business Analyst" },
                    { new Guid("00000000-0000-0000-0000-000000000009"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Description for Financial Analyst position #9", "Full-Time", true, "Durban", new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 39000m, 29000m, "Financial Analyst" },
                    { new Guid("00000000-0000-0000-0000-000000000010"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Description for Project Manager position #10", "Part-Time", true, "Remote", new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 40000m, 30000m, "Project Manager" },
                    { new Guid("00000000-0000-0000-0000-000000000011"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Description for Backend Developer position #11", "Contract", true, "Cape Town", new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), 41000m, 31000m, "Backend Developer" },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Description for Frontend Developer position #12", "Internship", true, "Johannesburg", new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), 42000m, 32000m, "Frontend Developer" },
                    { new Guid("00000000-0000-0000-0000-000000000013"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Description for Full Stack Developer position #13", "Full-Time", true, "Pretoria", new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), 43000m, 33000m, "Full Stack Developer" },
                    { new Guid("00000000-0000-0000-0000-000000000014"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Description for Cloud Engineer position #14", "Part-Time", true, "Durban", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 44000m, 34000m, "Cloud Engineer" },
                    { new Guid("00000000-0000-0000-0000-000000000015"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Description for DevOps Engineer position #15", "Contract", true, "Remote", new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), 45000m, 35000m, "DevOps Engineer" },
                    { new Guid("00000000-0000-0000-0000-000000000016"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Description for Software Engineer position #16", "Internship", true, "Cape Town", new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), 46000m, 36000m, "Software Engineer" },
                    { new Guid("00000000-0000-0000-0000-000000000017"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Description for Data Analyst position #17", "Full-Time", true, "Johannesburg", new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), 47000m, 37000m, "Data Analyst" },
                    { new Guid("00000000-0000-0000-0000-000000000018"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Description for Business Analyst position #18", "Part-Time", true, "Pretoria", new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), 48000m, 38000m, "Business Analyst" },
                    { new Guid("00000000-0000-0000-0000-000000000019"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Description for Financial Analyst position #19", "Contract", true, "Durban", new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), 49000m, 39000m, "Financial Analyst" },
                    { new Guid("00000000-0000-0000-0000-000000000020"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Description for Project Manager position #20", "Internship", true, "Remote", new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Utc), 50000m, 40000m, "Project Manager" },
                    { new Guid("00000000-0000-0000-0000-000000000021"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Description for Backend Developer position #21", "Full-Time", true, "Cape Town", new DateTime(2025, 1, 22, 0, 0, 0, 0, DateTimeKind.Utc), 51000m, 41000m, "Backend Developer" },
                    { new Guid("00000000-0000-0000-0000-000000000022"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Description for Frontend Developer position #22", "Part-Time", true, "Johannesburg", new DateTime(2025, 1, 23, 0, 0, 0, 0, DateTimeKind.Utc), 52000m, 42000m, "Frontend Developer" },
                    { new Guid("00000000-0000-0000-0000-000000000023"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Description for Full Stack Developer position #23", "Contract", true, "Pretoria", new DateTime(2025, 1, 24, 0, 0, 0, 0, DateTimeKind.Utc), 53000m, 43000m, "Full Stack Developer" },
                    { new Guid("00000000-0000-0000-0000-000000000024"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Description for Cloud Engineer position #24", "Internship", true, "Durban", new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), 54000m, 44000m, "Cloud Engineer" },
                    { new Guid("00000000-0000-0000-0000-000000000025"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Description for DevOps Engineer position #25", "Full-Time", true, "Remote", new DateTime(2025, 1, 26, 0, 0, 0, 0, DateTimeKind.Utc), 55000m, 45000m, "DevOps Engineer" },
                    { new Guid("00000000-0000-0000-0000-000000000026"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Description for Software Engineer position #26", "Part-Time", true, "Cape Town", new DateTime(2025, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), 56000m, 46000m, "Software Engineer" },
                    { new Guid("00000000-0000-0000-0000-000000000027"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Description for Data Analyst position #27", "Contract", true, "Johannesburg", new DateTime(2025, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), 57000m, 47000m, "Data Analyst" },
                    { new Guid("00000000-0000-0000-0000-000000000028"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Description for Business Analyst position #28", "Internship", true, "Pretoria", new DateTime(2025, 1, 29, 0, 0, 0, 0, DateTimeKind.Utc), 58000m, 48000m, "Business Analyst" },
                    { new Guid("00000000-0000-0000-0000-000000000029"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Description for Financial Analyst position #29", "Full-Time", true, "Durban", new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Utc), 59000m, 49000m, "Financial Analyst" },
                    { new Guid("00000000-0000-0000-0000-000000000030"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Description for Project Manager position #30", "Part-Time", true, "Remote", new DateTime(2025, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), 60000m, 50000m, "Project Manager" },
                    { new Guid("00000000-0000-0000-0000-000000000031"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Description for Backend Developer position #31", "Contract", true, "Cape Town", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 61000m, 51000m, "Backend Developer" },
                    { new Guid("00000000-0000-0000-0000-000000000032"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Description for Frontend Developer position #32", "Internship", true, "Johannesburg", new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), 62000m, 52000m, "Frontend Developer" },
                    { new Guid("00000000-0000-0000-0000-000000000033"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Description for Full Stack Developer position #33", "Full-Time", true, "Pretoria", new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), 63000m, 53000m, "Full Stack Developer" },
                    { new Guid("00000000-0000-0000-0000-000000000034"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Description for Cloud Engineer position #34", "Part-Time", true, "Durban", new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), 64000m, 54000m, "Cloud Engineer" },
                    { new Guid("00000000-0000-0000-0000-000000000035"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Description for DevOps Engineer position #35", "Contract", true, "Remote", new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), 65000m, 55000m, "DevOps Engineer" },
                    { new Guid("00000000-0000-0000-0000-000000000036"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Description for Software Engineer position #36", "Internship", true, "Cape Town", new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), 66000m, 56000m, "Software Engineer" },
                    { new Guid("00000000-0000-0000-0000-000000000037"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Description for Data Analyst position #37", "Full-Time", true, "Johannesburg", new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), 67000m, 57000m, "Data Analyst" },
                    { new Guid("00000000-0000-0000-0000-000000000038"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Description for Business Analyst position #38", "Part-Time", true, "Pretoria", new DateTime(2025, 2, 8, 0, 0, 0, 0, DateTimeKind.Utc), 68000m, 58000m, "Business Analyst" },
                    { new Guid("00000000-0000-0000-0000-000000000039"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Description for Financial Analyst position #39", "Contract", true, "Durban", new DateTime(2025, 2, 9, 0, 0, 0, 0, DateTimeKind.Utc), 69000m, 59000m, "Financial Analyst" },
                    { new Guid("00000000-0000-0000-0000-000000000040"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Description for Project Manager position #40", "Internship", true, "Remote", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 70000m, 60000m, "Project Manager" },
                    { new Guid("00000000-0000-0000-0000-000000000041"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Description for Backend Developer position #41", "Full-Time", true, "Cape Town", new DateTime(2025, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), 71000m, 61000m, "Backend Developer" },
                    { new Guid("00000000-0000-0000-0000-000000000042"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Description for Frontend Developer position #42", "Part-Time", true, "Johannesburg", new DateTime(2025, 2, 12, 0, 0, 0, 0, DateTimeKind.Utc), 72000m, 62000m, "Frontend Developer" },
                    { new Guid("00000000-0000-0000-0000-000000000043"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Description for Full Stack Developer position #43", "Contract", true, "Pretoria", new DateTime(2025, 2, 13, 0, 0, 0, 0, DateTimeKind.Utc), 73000m, 63000m, "Full Stack Developer" },
                    { new Guid("00000000-0000-0000-0000-000000000044"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Description for Cloud Engineer position #44", "Internship", true, "Durban", new DateTime(2025, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), 74000m, 64000m, "Cloud Engineer" },
                    { new Guid("00000000-0000-0000-0000-000000000045"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Description for DevOps Engineer position #45", "Full-Time", true, "Remote", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 75000m, 65000m, "DevOps Engineer" },
                    { new Guid("00000000-0000-0000-0000-000000000046"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Description for Software Engineer position #46", "Part-Time", true, "Cape Town", new DateTime(2025, 2, 16, 0, 0, 0, 0, DateTimeKind.Utc), 76000m, 66000m, "Software Engineer" },
                    { new Guid("00000000-0000-0000-0000-000000000047"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Description for Data Analyst position #47", "Contract", true, "Johannesburg", new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc), 77000m, 67000m, "Data Analyst" },
                    { new Guid("00000000-0000-0000-0000-000000000048"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Description for Business Analyst position #48", "Internship", true, "Pretoria", new DateTime(2025, 2, 18, 0, 0, 0, 0, DateTimeKind.Utc), 78000m, 68000m, "Business Analyst" },
                    { new Guid("00000000-0000-0000-0000-000000000049"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Description for Financial Analyst position #49", "Full-Time", true, "Durban", new DateTime(2025, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), 79000m, 69000m, "Financial Analyst" },
                    { new Guid("00000000-0000-0000-0000-000000000050"), new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Description for Project Manager position #50", "Part-Time", true, "Remote", new DateTime(2025, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), 80000m, 70000m, "Project Manager" }
                });

            migrationBuilder.InsertData(
                table: "applications",
                columns: new[] { "ApplicantId", "JobListingId", "CoverLetter", "ResumeUrl", "Status", "SubmittedAt" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-1111-1111-1111-111111111111"), new Guid("00000000-0000-0000-0000-000000000001"), "", "", 0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("bbbbbbbb-2222-2222-2222-222222222222"), new Guid("00000000-0000-0000-0000-000000000002"), "", "", 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("cccccccc-3333-3333-3333-333333333333"), new Guid("00000000-0000-0000-0000-000000000003"), "", "", 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("dddddddd-4444-4444-4444-444444444444"), new Guid("00000000-0000-0000-0000-000000000004"), "", "", 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("eeeeeeee-5555-5555-5555-eeeeeeeeeeee"), new Guid("00000000-0000-0000-0000-000000000005"), "", "", 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_applicants_Email",
                table: "applicants",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_applications_ApplicantId",
                table: "applications",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_companies_Name",
                table: "companies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_job_listings_CompanyId",
                table: "job_listings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_job_listings_Title",
                table: "job_listings",
                column: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applications");

            migrationBuilder.DropTable(
                name: "applicants");

            migrationBuilder.DropTable(
                name: "job_listings");

            migrationBuilder.DropTable(
                name: "companies");
        }
    }
}
