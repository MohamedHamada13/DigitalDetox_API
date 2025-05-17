using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalDetox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUser_UsageV21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WebIconUrl",
                table: "Apps",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WebIconUrl",
                table: "Apps");
        }
    }
}
