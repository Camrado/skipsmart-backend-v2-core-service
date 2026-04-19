using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkipSmart.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGroupDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_groups_group_id",
                table: "users");

            migrationBuilder.AlterColumn<Guid>(
                name: "group_id",
                table: "users",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "fk_users_groups_group_id",
                table: "users",
                column: "group_id",
                principalTable: "groups",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_groups_group_id",
                table: "users");

            migrationBuilder.AlterColumn<Guid>(
                name: "group_id",
                table: "users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_users_groups_group_id",
                table: "users",
                column: "group_id",
                principalTable: "groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
