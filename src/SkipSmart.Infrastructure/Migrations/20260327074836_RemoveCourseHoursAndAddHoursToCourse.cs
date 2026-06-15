using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkipSmart.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCourseHoursAndAddHoursToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "course_hours");

            migrationBuilder.AddColumn<decimal>(
                name: "hours",
                table: "courses",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hours",
                table: "courses");

            migrationBuilder.CreateTable(
                name: "course_hours",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    hours = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_course_hours", x => x.id);
                    table.ForeignKey(
                        name: "fk_course_hours_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_course_hours_course_id",
                table: "course_hours",
                column: "course_id");
        }
    }
}
