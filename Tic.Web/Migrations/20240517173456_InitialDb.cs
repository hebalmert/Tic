using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tic.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CodPhone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "SoftPlans",
                columns: table => new
                {
                    SoftPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaxMikrotik = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 18, scale: 2, nullable: false),
                    TimeMonth = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftPlans", x => x.SoftPlanId);
                });

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

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    StateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.StateId);
                    table.ForeignKey(
                        name: "FK_States_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Corporates",
                columns: table => new
                {
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Document = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SoftPlanId = table.Column<int>(type: "int", nullable: false),
                    ToStar = table.Column<DateTime>(type: "date", nullable: false),
                    ToEnd = table.Column<DateTime>(type: "date", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corporates", x => x.CorporateId);
                    table.ForeignKey(
                        name: "FK_Corporates_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Corporates_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Corporates_SoftPlans_SoftPlanId",
                        column: x => x.SoftPlanId,
                        principalTable: "SoftPlans",
                        principalColumn: "SoftPlanId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Corporates_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Job = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorporateId = table.Column<int>(type: "int", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId");
                });

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

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentName = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.DocumentTypeId);
                    table.ForeignKey(
                        name: "FK_DocumentTypes_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "Managers",
                columns: table => new
                {
                    ManagerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorporateId = table.Column<int>(type: "int", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Job = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.ManagerId);
                    table.ForeignKey(
                        name: "FK_Managers_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Registers",
                columns: table => new
                {
                    RegisterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderTickets = table.Column<int>(type: "int", nullable: false),
                    Tickets = table.Column<int>(type: "int", nullable: false),
                    Sells = table.Column<int>(type: "int", nullable: false),
                    SellCachier = table.Column<int>(type: "int", nullable: false),
                    PorcentCacheir = table.Column<int>(type: "int", nullable: false),
                    PayPorcentCacheir = table.Column<int>(type: "int", nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registers", x => x.RegisterId);
                    table.ForeignKey(
                        name: "FK_Registers_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    TaxId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.TaxId);
                    table.ForeignKey(
                        name: "FK_Taxes_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    ZoneId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    ZoneName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.ZoneId);
                    table.ForeignKey(
                        name: "FK_Zones_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Zones_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Zones_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
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

            migrationBuilder.CreateTable(
                name: "Cachiers",
                columns: table => new
                {
                    CachierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    MultiServer = table.Column<bool>(type: "bit", nullable: false),
                    ServerId = table.Column<int>(type: "int", nullable: true),
                    Porcentaje = table.Column<bool>(type: "bit", nullable: false),
                    RateCachier = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cachiers", x => x.CachierId);
                    table.ForeignKey(
                        name: "FK_Cachiers_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cachiers_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "DocumentTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cachiers_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Restrict);
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
                    SpeedDownType = table.Column<int>(type: "int", nullable: false),
                    TicketTimeId = table.Column<int>(type: "int", nullable: false),
                    TicketInactiveId = table.Column<int>(type: "int", nullable: false),
                    TicketRefreshId = table.Column<int>(type: "int", nullable: false),
                    ShareUser = table.Column<int>(type: "int", nullable: false),
                    Proxy = table.Column<bool>(type: "bit", nullable: false),
                    MacCookies = table.Column<bool>(type: "bit", nullable: false),
                    ContinueTime = table.Column<bool>(type: "bit", nullable: false),
                    TaxId = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Impuesto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    MkId = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    MkContinuoId = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
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

            migrationBuilder.CreateTable(
                name: "OrderTickets",
                columns: table => new
                {
                    OrderTicketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenControl = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    PlanCategoryId = table.Column<int>(type: "int", nullable: false),
                    ServerId = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    NamePlan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Impuesto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Mikrotik = table.Column<bool>(type: "bit", nullable: true),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTickets", x => x.OrderTicketId);
                    table.ForeignKey(
                        name: "FK_OrderTickets_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderTickets_PlanCategories_PlanCategoryId",
                        column: x => x.PlanCategoryId,
                        principalTable: "PlanCategories",
                        principalColumn: "PlanCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderTickets_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderTickets_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "OrderTicketDetails",
                columns: table => new
                {
                    OrderTicketDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderTicketId = table.Column<int>(type: "int", nullable: false),
                    Control = table.Column<int>(type: "int", nullable: false),
                    DateCreado = table.Column<DateTime>(type: "date", nullable: false),
                    ServerId = table.Column<int>(type: "int", nullable: false),
                    Velocidad = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    UserSystem = table.Column<bool>(type: "bit", nullable: false),
                    UserCachier = table.Column<bool>(type: "bit", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: true),
                    CachierId = table.Column<int>(type: "int", nullable: true),
                    Vendido = table.Column<bool>(type: "bit", nullable: false),
                    DateVenta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SellTotal = table.Column<bool>(type: "bit", nullable: false),
                    SellOne = table.Column<bool>(type: "bit", nullable: false),
                    SellOneCachier = table.Column<bool>(type: "bit", nullable: false),
                    VentaId = table.Column<int>(type: "int", nullable: true),
                    Anulado = table.Column<bool>(type: "bit", nullable: false),
                    DateAnulado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MkId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CorporateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTicketDetails", x => x.OrderTicketDetailId);
                    table.ForeignKey(
                        name: "FK_OrderTicketDetails_Corporates_CorporateId",
                        column: x => x.CorporateId,
                        principalTable: "Corporates",
                        principalColumn: "CorporateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderTicketDetails_OrderTickets_OrderTicketId",
                        column: x => x.OrderTicketId,
                        principalTable: "OrderTickets",
                        principalColumn: "OrderTicketId",
                        onDelete: ReferentialAction.Restrict);
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
                    ManagerId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_SellOnes_Managers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "ManagerId",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CorporateId",
                table: "AspNetUsers",
                column: "CorporateId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

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
                name: "IX_Cachiers_CorporateId_Document",
                table: "Cachiers",
                columns: new[] { "CorporateId", "Document" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cachiers_CorporateId_FullName",
                table: "Cachiers",
                columns: new[] { "CorporateId", "FullName" },
                unique: true,
                filter: "[FullName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cachiers_DocumentTypeId",
                table: "Cachiers",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cachiers_ServerId",
                table: "Cachiers",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cachiers_UserName",
                table: "Cachiers",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChainCodes_CorporateId",
                table: "ChainCodes",
                column: "CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                table: "Cities",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Corporates_CityId",
                table: "Corporates",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Corporates_CountryId",
                table: "Corporates",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Corporates_Name",
                table: "Corporates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Corporates_SoftPlanId",
                table: "Corporates",
                column: "SoftPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Corporates_StateId",
                table: "Corporates",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_CorporateId",
                table: "DocumentTypes",
                column: "CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_DocumentName_CorporateId",
                table: "DocumentTypes",
                columns: new[] { "DocumentName", "CorporateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HeadTexts_CorporateId_TextoEncabezado",
                table: "HeadTexts",
                columns: new[] { "CorporateId", "TextoEncabezado" },
                unique: true);

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
                name: "IX_Managers_CorporateId",
                table: "Managers",
                column: "CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_UserName",
                table: "Managers",
                column: "UserName",
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
                name: "IX_OrderTicketDetails_CorporateId_Control",
                table: "OrderTicketDetails",
                columns: new[] { "CorporateId", "Control" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderTicketDetails_OrderTicketId",
                table: "OrderTicketDetails",
                column: "OrderTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTickets_CorporateId_OrdenControl",
                table: "OrderTickets",
                columns: new[] { "CorporateId", "OrdenControl" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderTickets_PlanCategoryId",
                table: "OrderTickets",
                column: "PlanCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTickets_PlanId",
                table: "OrderTickets",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTickets_ServerId",
                table: "OrderTickets",
                column: "ServerId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Registers_CorporateId",
                table: "Registers",
                column: "CorporateId");

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

            migrationBuilder.CreateIndex(
                name: "IX_SellOnes_CorporateId",
                table: "SellOnes",
                column: "CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_SellOnes_ManagerId",
                table: "SellOnes",
                column: "ManagerId");

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

            migrationBuilder.CreateIndex(
                name: "IX_SoftPlans_Name",
                table: "SoftPlans",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId",
                table: "States",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_CorporateId_Rate",
                table: "Taxes",
                columns: new[] { "CorporateId", "Rate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_CorporateId_TaxName",
                table: "Taxes",
                columns: new[] { "CorporateId", "TaxName" },
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

            migrationBuilder.CreateIndex(
                name: "IX_Zones_CityId",
                table: "Zones",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_CorporateId",
                table: "Zones",
                column: "CorporateId");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_StateId_CityId_ZoneName_CorporateId",
                table: "Zones",
                columns: new[] { "StateId", "CityId", "ZoneName", "CorporateId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CachierPorcents");

            migrationBuilder.DropTable(
                name: "ChainCodes");

            migrationBuilder.DropTable(
                name: "HeadTexts");

            migrationBuilder.DropTable(
                name: "Registers");

            migrationBuilder.DropTable(
                name: "SellOnes");

            migrationBuilder.DropTable(
                name: "SellPackDetails");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SellOneCachiers");

            migrationBuilder.DropTable(
                name: "SellPacks");

            migrationBuilder.DropTable(
                name: "Cachiers");

            migrationBuilder.DropTable(
                name: "OrderTicketDetails");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "OrderTickets");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "PlanCategories");

            migrationBuilder.DropTable(
                name: "Servers");

            migrationBuilder.DropTable(
                name: "Taxes");

            migrationBuilder.DropTable(
                name: "TicketInactives");

            migrationBuilder.DropTable(
                name: "TicketRefreshes");

            migrationBuilder.DropTable(
                name: "TicketTimes");

            migrationBuilder.DropTable(
                name: "IpNetworks");

            migrationBuilder.DropTable(
                name: "MarkModels");

            migrationBuilder.DropTable(
                name: "Zones");

            migrationBuilder.DropTable(
                name: "Marks");

            migrationBuilder.DropTable(
                name: "Corporates");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "SoftPlans");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
