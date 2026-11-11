namespace GiyimStokMvc.Models
{
    public class StockTransaction
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public ProductVariant? ProductVariant { get; set; }

        public IslemTuru Type { get; set; }
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public string? Note { get; set; }

        public int? UserId { get; set; }
        public AppUser? User { get; set; }
        public OdemeTipi OdemeTipi { get; set; } = OdemeTipi.Nakit;
        public decimal NakitTutar { get; set; } = 0;
        public decimal KartTutar { get; set; } = 0;
    }
}