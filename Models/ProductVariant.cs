namespace GiyimStokMvc.Models;

public class ProductVariant
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public string Color { get; set; }
    public string Size { get; set; }
    public string Barcode { get; set; }
    public int StockQuantity { get; set; }
    public int CriticalStockLevel { get; set; }
    public decimal SalePrice { get; set; }

    public decimal PurchasePrice { get; set; }
    public string? ImageUrl { get; set; }
    public decimal DiscountRate { get; set; } = 0; // Yüzdelik indirim oranı (Örn: 20 = %20 indirim)
    public ICollection<StockTransaction> StockTransactions { get; set; }
}