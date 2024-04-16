using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tic.Web.Migrations
{
    /// <inheritdoc />
    public partial class PlanUpdateProxy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ContinueTime",
                table: "Plans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MacCookies",
                table: "Plans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Proxy",
                table: "Plans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ShareUser",
                table: "Plans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContinueTime",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "MacCookies",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "Proxy",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "ShareUser",
                table: "Plans");
        }
    }
}
