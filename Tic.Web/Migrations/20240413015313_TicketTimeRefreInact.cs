using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tic.Web.Migrations
{
    /// <inheritdoc />
    public partial class TicketTimeRefreInact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zones_Cities_CityId",
                table: "Zones");

            migrationBuilder.DropForeignKey(
                name: "FK_Zones_States_StateId",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Zones_StateId",
                table: "Zones");

            migrationBuilder.CreateTable(
                name: "TicketInactives",
                columns: table => new
                {
                    TicketInactiveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tiempo = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketInactives", x => x.TicketInactiveId);
                });

            migrationBuilder.CreateTable(
                name: "TicketRefreshes",
                columns: table => new
                {
                    TicketRefreshId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tiempo = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketRefreshes", x => x.TicketRefreshId);
                });

            migrationBuilder.CreateTable(
                name: "TicketTimes",
                columns: table => new
                {
                    TicketTimeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tiempo = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    ScriptConsumo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScriptContinuo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTimes", x => x.TicketTimeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zones_StateId_CityId_ZoneName_CorporateId",
                table: "Zones",
                columns: new[] { "StateId", "CityId", "ZoneName", "CorporateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketInactives_Tiempo",
                table: "TicketInactives",
                column: "Tiempo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketRefreshes_Tiempo",
                table: "TicketRefreshes",
                column: "Tiempo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketTimes_Tiempo",
                table: "TicketTimes",
                column: "Tiempo",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_Cities_CityId",
                table: "Zones",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_States_StateId",
                table: "Zones",
                column: "StateId",
                principalTable: "States",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zones_Cities_CityId",
                table: "Zones");

            migrationBuilder.DropForeignKey(
                name: "FK_Zones_States_StateId",
                table: "Zones");

            migrationBuilder.DropTable(
                name: "TicketInactives");

            migrationBuilder.DropTable(
                name: "TicketRefreshes");

            migrationBuilder.DropTable(
                name: "TicketTimes");

            migrationBuilder.DropIndex(
                name: "IX_Zones_StateId_CityId_ZoneName_CorporateId",
                table: "Zones");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_StateId",
                table: "Zones",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_Cities_CityId",
                table: "Zones",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_States_StateId",
                table: "Zones",
                column: "StateId",
                principalTable: "States",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
