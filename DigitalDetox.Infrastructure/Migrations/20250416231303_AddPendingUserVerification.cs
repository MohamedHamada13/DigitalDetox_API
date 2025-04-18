using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalDetox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPendingUserVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DetoxPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 4, 17, 1, 13, 1, 345, DateTimeKind.Local).AddTicks(2840),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 4, 15, 22, 42, 56, 146, DateTimeKind.Local).AddTicks(4830));

            migrationBuilder.CreateTable(
                name: "PendingUserVerifications",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingUserVerifications");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DetoxPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 4, 15, 22, 42, 56, 146, DateTimeKind.Local).AddTicks(4830),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 4, 17, 1, 13, 1, 345, DateTimeKind.Local).AddTicks(2840));
        }
    }
}
