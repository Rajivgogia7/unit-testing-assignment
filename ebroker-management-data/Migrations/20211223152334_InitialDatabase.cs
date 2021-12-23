using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace ebroker_management_data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equity",
                columns: table => new
                {
                    EquityId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    EquityCode = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    EquityName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equity", x => x.EquityId);
                });

            migrationBuilder.CreateTable(
                name: "Trader",
                columns: table => new
                {
                    TraderId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TraderCode = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TraderName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Funds = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trader", x => x.TraderId);
                });

            migrationBuilder.CreateTable(
                name: "TraderEquity",
                columns: table => new
                {
                    TraderEquityId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TraderId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    EquityId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraderEquity", x => x.TraderEquityId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equity");

            migrationBuilder.DropTable(
                name: "Trader");

            migrationBuilder.DropTable(
                name: "TraderEquity");
        }
    }
}
