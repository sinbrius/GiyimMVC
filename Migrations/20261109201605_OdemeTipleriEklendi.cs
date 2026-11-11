using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiyimStokMvc.Migrations
{
    /// <inheritdoc />
    public partial class OdemeTipleriEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "KartTutar",
                table: "StockTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NakitTutar",
                table: "StockTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "OdemeTipi",
                table: "StockTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KartTutar",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "NakitTutar",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "OdemeTipi",
                table: "StockTransactions");
        }
    }
}
