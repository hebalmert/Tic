using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tic.Web.Migrations
{
    /// <inheritdoc />
    public partial class MarkIpServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IpNetworks",
                columns: table => new
                {
                    IpNetworkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Assigned = table.Column<bool>(type: "bit", nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpNetworks", x => x.IpNetworkId);
                    table.ForeignKey(
                        name: "FK_IpNetworks_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Marks",
                columns: table => new
                {
                    MarkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarkName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marks", x => x.MarkId);
                    table.ForeignKey(
                        name: "FK_Marks_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarkModels",
                columns: table => new
                {
                    MarkModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarkModelName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MarkId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkModels", x => x.MarkModelId);
                    table.ForeignKey(
                        name: "FK_MarkModels_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MarkModels_Marks_MarkId",
                        column: x => x.MarkId,
                        principalTable: "Marks",
                        principalColumn: "MarkId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    ServerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IpNetworkId = table.Column<int>(type: "int", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    WanName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ApiPort = table.Column<int>(type: "int", nullable: false),
                    MarkId = table.Column<int>(type: "int", nullable: false),
                    MarkModelId = table.Column<int>(type: "int", nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.ServerId);
                    table.ForeignKey(
                        name: "FK_Servers_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Servers_IpNetworks_IpNetworkId",
                        column: x => x.IpNetworkId,
                        principalTable: "IpNetworks",
                        principalColumn: "IpNetworkId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Servers_MarkModels_MarkModelId",
                        column: x => x.MarkModelId,
                        principalTable: "MarkModels",
                        principalColumn: "MarkModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Servers_Marks_MarkId",
                        column: x => x.MarkId,
                        principalTable: "Marks",
                        principalColumn: "MarkId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Servers_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "ZoneId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IpNetworks_CorporateId",
                table: "IpNetworks",
                column: "CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_IpNetworks_Ip_CorporateId",
                table: "IpNetworks",
                columns: new[] { "Ip", "CorporateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarkModels_CorporateId",
                table: "MarkModels",
                column: "CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkModels_MarkId",
                table: "MarkModels",
                column: "MarkId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkModels_MarkModelName_CorporateId_MarkId",
                table: "MarkModels",
                columns: new[] { "MarkModelName", "CorporateId", "MarkId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Marks_CorporateId",
                table: "Marks",
                column: "CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_Marks_MarkName_CorporateId",
                table: "Marks",
                columns: new[] { "MarkName", "CorporateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servers_CorporateId",
                table: "Servers",
                column: "CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_Servers_IpNetworkId",
                table: "Servers",
                column: "IpNetworkId");

            migrationBuilder.CreateIndex(
                name: "IX_Servers_MarkId",
                table: "Servers",
                column: "MarkId");

            migrationBuilder.CreateIndex(
                name: "IX_Servers_MarkModelId",
                table: "Servers",
                column: "MarkModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Servers_ServerName_CorporateId",
                table: "Servers",
                columns: new[] { "ServerName", "CorporateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servers_ZoneId",
                table: "Servers",
                column: "ZoneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Servers");

            migrationBuilder.DropTable(
                name: "IpNetworks");

            migrationBuilder.DropTable(
                name: "MarkModels");

            migrationBuilder.DropTable(
                name: "Marks");
        }
    }
}
