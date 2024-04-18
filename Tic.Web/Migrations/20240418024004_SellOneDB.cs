using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tic.Web.Migrations
{
    /// <inheritdoc />
    public partial class SellOneDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SellOnes",
                columns: table => new
                {
                    SellOneId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellControl = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    PlanCategoryId = table.Column<int>(type: "int", nullable: false),
                    ServerId = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    NamePlan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderTicketDetailId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(15,2)", precision: 18, scale: 2, nullable: false),
                    Impuesto = table.Column<decimal>(type: "decimal(15,2)", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(15,2)", precision: 18, scale: 2, nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellOnes", x => x.SellOneId);
                    table.ForeignKey(
                        name: "FK_SellOnes_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellOnes_OrderTicketDetails_OrderTicketDetailId",
                        column: x => x.OrderTicketDetailId,
                        principalTable: "OrderTicketDetails",
                        principalColumn: "OrderTicketDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellOnes_PlanCategories_PlanCategoryId",
                        column: x => x.PlanCategoryId,
                        principalTable: "PlanCategories",
                        principalColumn: "PlanCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellOnes_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SellOnes_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SellOnes_CorporateId",
                table: "SellOnes",
                column: "CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_SellOnes_OrderTicketDetailId",
                table: "SellOnes",
                column: "OrderTicketDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SellOnes_PlanCategoryId",
                table: "SellOnes",
                column: "PlanCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SellOnes_PlanId",
                table: "SellOnes",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_SellOnes_SellControl_CorporateId",
                table: "SellOnes",
                columns: new[] { "SellControl", "CorporateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellOnes_SellOneId_OrderTicketDetailId_CorporateId",
                table: "SellOnes",
                columns: new[] { "SellOneId", "OrderTicketDetailId", "CorporateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellOnes_ServerId",
                table: "SellOnes",
                column: "ServerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SellOnes");
        }
    }
}
