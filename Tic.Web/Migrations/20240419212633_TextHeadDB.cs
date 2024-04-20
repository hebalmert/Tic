using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tic.Web.Migrations
{
    /// <inheritdoc />
    public partial class TextHeadDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeadTexts",
                columns: table => new
                {
                    HeadTextId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextoEncabezado = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeadTexts", x => x.HeadTextId);
                    table.ForeignKey(
                        name: "FK_HeadTexts_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SellOneCachiers",
                columns: table => new
                {
                    SellOneCachierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellControl = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    CachierId = table.Column<int>(type: "int", nullable: false),
                    PlanCategoryId = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    NamePlan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ServerId = table.Column<int>(type: "int", nullable: false),
                    OrderTicketDetailId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Impuesto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DateAnulado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Anulada = table.Column<bool>(type: "bit", nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellOneCachiers", x => x.SellOneCachierId);
                    table.ForeignKey(
                        name: "FK_SellOneCachiers_Cachiers_CachierId",
                        column: x => x.CachierId,
                        principalTable: "Cachiers",
                        principalColumn: "CachierId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellOneCachiers_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellOneCachiers_OrderTicketDetails_OrderTicketDetailId",
                        column: x => x.OrderTicketDetailId,
                        principalTable: "OrderTicketDetails",
                        principalColumn: "OrderTicketDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellOneCachiers_PlanCategories_PlanCategoryId",
                        column: x => x.PlanCategoryId,
                        principalTable: "PlanCategories",
                        principalColumn: "PlanCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellOneCachiers_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellOneCachiers_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CachierPorcents",
                columns: table => new
                {
                    CachierPorcentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "date", nullable: true),
                    CachierId = table.Column<int>(type: "int", nullable: false),
                    SellOneCachierId = table.Column<int>(type: "int", nullable: false),
                    OrderTicketDetailId = table.Column<int>(type: "int", nullable: false),
                    Porcentaje = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NamePlan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Comision = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DatePagado = table.Column<DateTime>(type: "date", nullable: true),
                    Control = table.Column<int>(type: "int", nullable: false),
                    Pagado = table.Column<bool>(type: "bit", nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CachierPorcents", x => x.CachierPorcentId);
                    table.ForeignKey(
                        name: "FK_CachierPorcents_Cachiers_CachierId",
                        column: x => x.CachierId,
                        principalTable: "Cachiers",
                        principalColumn: "CachierId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CachierPorcents_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CachierPorcents_OrderTicketDetails_OrderTicketDetailId",
                        column: x => x.OrderTicketDetailId,
                        principalTable: "OrderTicketDetails",
                        principalColumn: "OrderTicketDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CachierPorcents_SellOneCachiers_SellOneCachierId",
                        column: x => x.SellOneCachierId,
                        principalTable: "SellOneCachiers",
                        principalColumn: "SellOneCachierId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CachierPorcents_CachierId",
                table: "CachierPorcents",
                column: "CachierId");

            migrationBuilder.CreateIndex(
                name: "IX_CachierPorcents_CorporateId",
                table: "CachierPorcents",
                column: "CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_CachierPorcents_OrderTicketDetailId",
                table: "CachierPorcents",
                column: "OrderTicketDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_CachierPorcents_SellOneCachierId",
                table: "CachierPorcents",
                column: "SellOneCachierId");

            migrationBuilder.CreateIndex(
                name: "IX_HeadTexts_CorporateId_TextoEncabezado",
                table: "HeadTexts",
                columns: new[] { "CorporateId", "TextoEncabezado" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellOneCachiers_CachierId",
                table: "SellOneCachiers",
                column: "CachierId");

            migrationBuilder.CreateIndex(
                name: "IX_SellOneCachiers_CorporateId_SellControl",
                table: "SellOneCachiers",
                columns: new[] { "CorporateId", "SellControl" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellOneCachiers_OrderTicketDetailId",
                table: "SellOneCachiers",
                column: "OrderTicketDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SellOneCachiers_PlanCategoryId",
                table: "SellOneCachiers",
                column: "PlanCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SellOneCachiers_PlanId",
                table: "SellOneCachiers",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_SellOneCachiers_SellOneCachierId_OrderTicketDetailId_CorporateId",
                table: "SellOneCachiers",
                columns: new[] { "SellOneCachierId", "OrderTicketDetailId", "CorporateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellOneCachiers_ServerId",
                table: "SellOneCachiers",
                column: "ServerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CachierPorcents");

            migrationBuilder.DropTable(
                name: "HeadTexts");

            migrationBuilder.DropTable(
                name: "SellOneCachiers");
        }
    }
}
