using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkipSmart.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmailVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email_verification_code",
                table: "users");

            migrationBuilder.DropColumn(
                name: "email_verification_sent_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "is_email_verified",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "email_verification_code",
                table: "users",
                type: "integer",
                maxLength: 6,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "email_verification_sent_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "is_email_verified",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
