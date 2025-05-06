using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalDetox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateRestPasswordFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DetoxPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 5, 6, 17, 41, 49, 70, DateTimeKind.Local).AddTicks(5471),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 4, 23, 1, 32, 24, 334, DateTimeKind.Local).AddTicks(8153));

            migrationBuilder.CreateTable(
                name: "OtpCodes",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpCodes", x => x.Email);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OtpCodes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DetoxPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 4, 23, 1, 32, 24, 334, DateTimeKind.Local).AddTicks(8153),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 5, 6, 17, 41, 49, 70, DateTimeKind.Local).AddTicks(5471));
        }
    }
}
