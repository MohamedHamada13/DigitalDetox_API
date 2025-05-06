using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalDetox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRestPasswordFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChecked",
                table: "OtpCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DetoxPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 5, 6, 18, 3, 35, 85, DateTimeKind.Local).AddTicks(238),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 5, 6, 17, 41, 49, 70, DateTimeKind.Local).AddTicks(5471));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChecked",
                table: "OtpCodes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DetoxPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 5, 6, 17, 41, 49, 70, DateTimeKind.Local).AddTicks(5471),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 5, 6, 18, 3, 35, 85, DateTimeKind.Local).AddTicks(238));
        }
    }
}
