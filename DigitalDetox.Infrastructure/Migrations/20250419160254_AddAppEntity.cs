using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalDetox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAppEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScreenTimeLogs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DetoxPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 4, 19, 18, 2, 53, 345, DateTimeKind.Local).AddTicks(3740),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 4, 18, 6, 42, 57, 295, DateTimeKind.Local).AddTicks(7295));

            migrationBuilder.CreateTable(
                name: "Apps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyUsageLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsageTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DailyLogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyUsageLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyUsageLogs_Apps_AppId",
                        column: x => x.AppId,
                        principalTable: "Apps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DailyUsageLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyUsageLogs_AppId_UserId_DailyLogDate",
                table: "DailyUsageLogs",
                columns: new[] { "AppId", "UserId", "DailyLogDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyUsageLogs_UserId",
                table: "DailyUsageLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyUsageLogs");

            migrationBuilder.DropTable(
                name: "Apps");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DetoxPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 4, 18, 6, 42, 57, 295, DateTimeKind.Local).AddTicks(7295),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 4, 19, 18, 2, 53, 345, DateTimeKind.Local).AddTicks(3740));

            migrationBuilder.CreateTable(
                name: "ScreenTimeLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AppUsed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScreenTimeMinutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenTimeLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreenTimeLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScreenTimeLogs_UserId",
                table: "ScreenTimeLogs",
                column: "UserId");
        }
    }
}
