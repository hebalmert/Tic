using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tic.Web.Migrations
{
    /// <inheritdoc />
    public partial class PlaCatServ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanCategories",
                columns: table => new
                {
                    PlanCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanCategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanCategories", x => x.PlanCategoryId);
                    table.ForeignKey(
                        name: "FK_PlanCategories_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    PlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "date", nullable: true),
                    DateEdit = table.Column<DateTime>(type: "date", nullable: true),
                    ServerId = table.Column<int>(type: "int", nullable: false),
                    PlanCategoryId = table.Column<int>(type: "int", nullable: false),
                    PlanName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SpeedUp = table.Column<int>(type: "int", nullable: false),
                    SpeedUpType = table.Column<int>(type: "int", nullable: false),
                    SpeedDown = table.Column<int>(type: "int", nullable: false),
                    TasaReuso = table.Column<int>(type: "int", nullable: false),
                    SpeedDownType = table.Column<int>(type: "int", nullable: false),
                    TicketTimeId = table.Column<int>(type: "int", nullable: false),
                    TicketInactiveId = table.Column<int>(type: "int", nullable: false),
                    TicketRefreshId = table.Column<int>(type: "int", nullable: false),
                    TaxId = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Impuesto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.PlanId);
                    table.ForeignKey(
                        name: "FK_Plans_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Plans_PlanCategories_PlanCategoryId",
                        column: x => x.PlanCategoryId,
                        principalTable: "PlanCategories",
                        principalColumn: "PlanCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_Taxes_TaxId",
                        column: x => x.TaxId,
                        principalTable: "Taxes",
                        principalColumn: "TaxId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_TicketInactives_TicketInactiveId",
                        column: x => x.TicketInactiveId,
                        principalTable: "TicketInactives",
                        principalColumn: "TicketInactiveId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_TicketRefreshes_TicketRefreshId",
                        column: x => x.TicketRefreshId,
                        principalTable: "TicketRefreshes",
                        principalColumn: "TicketRefreshId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plans_TicketTimes_TicketTimeId",
                        column: x => x.TicketTimeId,
                        principalTable: "TicketTimes",
                        principalColumn: "TicketTimeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanCategories_CorporateId",
                table: "PlanCategories",
                column: "CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanCategories_PlanCategoryName_CorporateId",
                table: "PlanCategories",
                columns: new[] { "PlanCategoryName", "CorporateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plans_CorporateId_PlanName_ServerId",
                table: "Plans",
                columns: new[] { "CorporateId", "PlanName", "ServerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plans_PlanCategoryId",
                table: "Plans",
                column: "PlanCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_ServerId",
                table: "Plans",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_TaxId",
                table: "Plans",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_TicketInactiveId",
                table: "Plans",
                column: "TicketInactiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_TicketRefreshId",
                table: "Plans",
                column: "TicketRefreshId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_TicketTimeId",
                table: "Plans",
                column: "TicketTimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "PlanCategories");
        }
    }
}
