using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalDetox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuth4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PendingUserVerifications",
                table: "PendingUserVerifications");

            migrationBuilder.RenameTable(
                name: "PendingUserVerifications",
                newName: "UserStoreTemporary");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DetoxPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 4, 18, 6, 42, 57, 295, DateTimeKind.Local).AddTicks(7295),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 4, 18, 6, 39, 40, 798, DateTimeKind.Local).AddTicks(767));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserStoreTemporary",
                table: "UserStoreTemporary",
                column: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserStoreTemporary",
                table: "UserStoreTemporary");

            migrationBuilder.RenameTable(
                name: "UserStoreTemporary",
                newName: "PendingUserVerifications");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DetoxPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 4, 18, 6, 39, 40, 798, DateTimeKind.Local).AddTicks(767),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 4, 18, 6, 42, 57, 295, DateTimeKind.Local).AddTicks(7295));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PendingUserVerifications",
                table: "PendingUserVerifications",
                column: "Email");
        }
    }
}
