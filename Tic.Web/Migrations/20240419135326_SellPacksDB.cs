using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tic.Web.Migrations
{
    /// <inheritdoc />
    public partial class SellPacksDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SellPacks",
                columns: table => new
                {
                    SellPackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellControl = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    PlanCategoryId = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    NamePlan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ServerId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Impuesto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DateClose = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellPacks", x => x.SellPackId);
                    table.ForeignKey(
                        name: "FK_SellPacks_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellPacks_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "ManagerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellPacks_PlanCategories_PlanCategoryId",
                        column: x => x.PlanCategoryId,
                        principalTable: "PlanCategories",
                        principalColumn: "PlanCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellPacks_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellPacks_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SellPackDetails",
                columns: table => new
                {
                    SellPackDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellPackId = table.Column<int>(type: "int", nullable: false),
                    OrderTicketDetailId = table.Column<int>(type: "int", nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellPackDetails", x => x.SellPackDetailId);
                    table.ForeignKey(
                        name: "FK_SellPackDetails_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellPackDetails_OrderTicketDetails_OrderTicketDetailId",
                        column: x => x.OrderTicketDetailId,
                        principalTable: "OrderTicketDetails",
                        principalColumn: "OrderTicketDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellPackDetails_SellPacks_SellPackId",
                        column: x => x.SellPackId,
                        principalTable: "SellPacks",
                        principalColumn: "SellPackId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SellPackDetails_CorporateId_SellPackDetailId_OrderTicketDetailId",
                table: "SellPackDetails",
                columns: new[] { "CorporateId", "SellPackDetailId", "OrderTicketDetailId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellPackDetails_OrderTicketDetailId",
                table: "SellPackDetails",
                column: "OrderTicketDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SellPackDetails_SellPackId",
                table: "SellPackDetails",
                column: "SellPackId");

            migrationBuilder.CreateIndex(
                name: "IX_SellPacks_CorporateId_SellControl",
                table: "SellPacks",
                columns: new[] { "CorporateId", "SellControl" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellPacks_ManagerId",
                table: "SellPacks",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_SellPacks_PlanCategoryId",
                table: "SellPacks",
                column: "PlanCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SellPacks_PlanId",
                table: "SellPacks",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_SellPacks_ServerId",
                table: "SellPacks",
                column: "ServerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SellPackDetails");

            migrationBuilder.DropTable(
                name: "SellPacks");
        }
    }
}
