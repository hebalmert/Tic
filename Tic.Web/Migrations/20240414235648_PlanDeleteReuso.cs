using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tic.Web.Migrations
{
    /// <inheritdoc />
    public partial class PlanDeleteReuso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TasaReuso",
                table: "Plans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TasaReuso",
                table: "Plans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
