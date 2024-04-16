using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tic.Web.Migrations
{
    /// <inheritdoc />
    public partial class MkMkConsumoAtPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MikrotikId",
                table: "Plans",
                newName: "MkId");

            migrationBuilder.AddColumn<string>(
                name: "MkContinuoId",
                table: "Plans",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MkContinuoId",
                table: "Plans");

            migrationBuilder.RenameColumn(
                name: "MkId",
                table: "Plans",
                newName: "MikrotikId");
        }
    }
}
