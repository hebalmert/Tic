using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tic.Web.Migrations
{
    /// <inheritdoc />
    public partial class SellOneManagerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "SellOnes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SellOnes_ManagerId",
                table: "SellOnes",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellOnes_Managers_ManagerId",
                table: "SellOnes",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "ManagerId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellOnes_Managers_ManagerId",
                table: "SellOnes");

            migrationBuilder.DropIndex(
                name: "IX_SellOnes_ManagerId",
                table: "SellOnes");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "SellOnes");
        }
    }
}
