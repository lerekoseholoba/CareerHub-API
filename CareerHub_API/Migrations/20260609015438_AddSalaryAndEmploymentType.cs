using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerHub_API.Migrations
{
    /// <inheritdoc />
    public partial class AddSalaryAndEmploymentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClosingDate",
                table: "job_listings",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EmploymentType",
                table: "job_listings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "job_listings",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryMax",
                table: "job_listings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryMin",
                table: "job_listings",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CoverLetter",
                table: "applications",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResumeUrl",
                table: "applications",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "applicants",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "applicants",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-1111-1111-1111-111111111111"),
                column: "PasswordHash",
                value: "");

            migrationBuilder.UpdateData(
                table: "applicants",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-2222-2222-2222-222222222222"),
                column: "PasswordHash",
                value: "");

            migrationBuilder.UpdateData(
                table: "applicants",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-3333-3333-3333-333333333333"),
                column: "PasswordHash",
                value: "");

            migrationBuilder.UpdateData(
                table: "applicants",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-4444-4444-4444-444444444444"),
                column: "PasswordHash",
                value: "");

            migrationBuilder.UpdateData(
                table: "applicants",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-5555-5555-5555-eeeeeeeeeeee"),
                column: "PasswordHash",
                value: "");

            migrationBuilder.UpdateData(
                table: "applications",
                keyColumns: new[] { "ApplicantId", "JobListingId" },
                keyValues: new object[] { new Guid("aaaaaaaa-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111") },
                columns: new[] { "CoverLetter", "ResumeUrl" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "applications",
                keyColumns: new[] { "ApplicantId", "JobListingId" },
                keyValues: new object[] { new Guid("bbbbbbbb-2222-2222-2222-222222222222"), new Guid("22222222-2222-2222-2222-222222222222") },
                columns: new[] { "CoverLetter", "ResumeUrl" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "applications",
                keyColumns: new[] { "ApplicantId", "JobListingId" },
                keyValues: new object[] { new Guid("cccccccc-3333-3333-3333-333333333333"), new Guid("33333333-3333-3333-3333-333333333333") },
                columns: new[] { "CoverLetter", "ResumeUrl" },
                values: new object[] { "", "" });

            migrationBuilder.UpdateData(
                table: "applications",
                keyColumns: new[] { "ApplicantId", "JobListingId" },
                keyValues: new object[] { new Guid("dddddddd-4444-4444-4444-444444444444"), new Guid("44444444-4444-4444-4444-444444444444") },
                columns: new[] { "CoverLetter", "ResumeUrl", "Status" },
                values: new object[] { "", "", 4 });

            migrationBuilder.UpdateData(
                table: "applications",
                keyColumns: new[] { "ApplicantId", "JobListingId" },
                keyValues: new object[] { new Guid("eeeeeeee-5555-5555-5555-eeeeeeeeeeee"), new Guid("55555555-5555-5555-5555-555555555555") },
                columns: new[] { "CoverLetter", "ResumeUrl", "Status" },
                values: new object[] { "", "", 3 });

            migrationBuilder.UpdateData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ClosingDate", "EmploymentType", "SalaryMax", "SalaryMin" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 0m, 0m });

            migrationBuilder.UpdateData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "ClosingDate", "EmploymentType", "SalaryMax", "SalaryMin" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 0m, 0m });

            migrationBuilder.UpdateData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "ClosingDate", "EmploymentType", "SalaryMax", "SalaryMin" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 0m, 0m });

            migrationBuilder.UpdateData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ClosingDate", "EmploymentType", "SalaryMax", "SalaryMin" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 0m, 0m });

            migrationBuilder.UpdateData(
                table: "job_listings",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "ClosingDate", "EmploymentType", "SalaryMax", "SalaryMin" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 0m, 0m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingDate",
                table: "job_listings");

            migrationBuilder.DropColumn(
                name: "EmploymentType",
                table: "job_listings");

            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "job_listings");

            migrationBuilder.DropColumn(
                name: "SalaryMax",
                table: "job_listings");

            migrationBuilder.DropColumn(
                name: "SalaryMin",
                table: "job_listings");

            migrationBuilder.DropColumn(
                name: "CoverLetter",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "ResumeUrl",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "applicants");

            migrationBuilder.UpdateData(
                table: "applications",
                keyColumns: new[] { "ApplicantId", "JobListingId" },
                keyValues: new object[] { new Guid("dddddddd-4444-4444-4444-444444444444"), new Guid("44444444-4444-4444-4444-444444444444") },
                column: "Status",
                value: 3);

            migrationBuilder.UpdateData(
                table: "applications",
                keyColumns: new[] { "ApplicantId", "JobListingId" },
                keyValues: new object[] { new Guid("eeeeeeee-5555-5555-5555-eeeeeeeeeeee"), new Guid("55555555-5555-5555-5555-555555555555") },
                column: "Status",
                value: 4);
        }
    }
}
