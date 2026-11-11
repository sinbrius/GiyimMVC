using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GiyimStokMvc.Migrations
{
    /// <inheritdoc />
    public partial class SeedVeriEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "ModelCode", "Name" },
                values: new object[,]
                {
                    { 1, "Tişört", "TS-2026-BLK", "Oversize Unisex Basic Tişört" },
                    { 2, "Ceket", "JK-9941-DNM", "Oversize Ceket Tipi Denim Gömlek" },
                    { 3, "Sweatshirt", "SW-1010-CP", "Kanguru Cepli Kapüşonlu Sweatshirt" },
                    { 4, "Elbise", "DR-4040-FLR", "Kruvaze Yaka Çiçekli Desen Elbise" }
                });

            migrationBuilder.InsertData(
                table: "ProductVariants",
                columns: new[] { "Id", "Barcode", "Color", "CriticalStockLevel", "ImageUrl", "ProductId", "PurchasePrice", "SalePrice", "Size", "StockQuantity" },
                values: new object[,]
                {
                    { 1, "8690000000011", "Siyah", 3, "https://images.unsplash.com/photo-1521572267360-ee0c2909d518?w=500&q=80", 1, 120.00m, 349.90m, "M", 15 },
                    { 2, "8690000000028", "Beyaz", 3, "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?w=500&q=80", 1, 120.00m, 349.90m, "L", 2 },
                    { 3, "8690000000035", "Mavi Denim", 3, "https://images.unsplash.com/photo-1576995853123-5a10305d93c0?w=500&q=80", 2, 350.00m, 899.00m, "L", 8 },
                    { 4, "8690000000042", "Gri Melanj", 3, "https://images.unsplash.com/photo-1556905055-8f358a7a47b2?w=500&q=80", 3, 220.00m, 649.50m, "XL", 0 },
                    { 5, "8690000000059", "Kırmızı Çiçekli", 4, "https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=500&q=80", 4, 450.00m, 1199.90m, "S", 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
