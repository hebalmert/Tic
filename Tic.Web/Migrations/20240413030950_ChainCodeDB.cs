using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tic.Web.Migrations
{
    /// <inheritdoc />
    public partial class ChainCodeDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChainCodes",
                columns: table => new
                {
                    ChainCodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cadena = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Largo = table.Column<int>(type: "int", nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChainCodes", x => x.ChainCodeId);
                    table.ForeignKey(
                        name: "FK_ChainCodes_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChainCodes_CorporateId",
                table: "ChainCodes",
                column: "CorporateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChainCodes");
        }
    }
}
