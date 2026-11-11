namespace GiyimStokMvc.Models.DbContext;
using Microsoft.EntityFrameworkCore;
public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AppDbContext(Microsoft.EntityFrameworkCore.DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public Microsoft.EntityFrameworkCore.DbSet<Product> Products { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<StockTransaction> StockTransactions { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<ProductVariant> ProductVariants { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<AppUser> Users { get; set; }

    protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<ProductVariant>().HasIndex(pv => pv.Barcode).IsUnique();
        
        modelBuilder.Entity<ProductVariant>()
                .Property(v => v.SalePrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ProductVariant>()
                .Property(v => v.PurchasePrice)
                .HasColumnType("decimal(18,2)");
        
        base.OnModelCreating(modelBuilder);

    // ... Mevcut konfigürasyonların (Barcode Index, Decimal ayarları vb.) ...

    // 1. ANA ÜRÜNLER (Products)
    modelBuilder.Entity<Product>().HasData(
        new Product { Id = 1, ModelCode = "TS-2026-BLK", Name = "Oversize Unisex Basic Tişört", Category = "Tişört" },
        new Product { Id = 2, ModelCode = "JK-9941-DNM", Name = "Oversize Ceket Tipi Denim Gömlek", Category = "Ceket" },
        new Product { Id = 3, ModelCode = "SW-1010-CP", Name = "Kanguru Cepli Kapüşonlu Sweatshirt", Category = "Sweatshirt" },
        new Product { Id = 4, ModelCode = "DR-4040-FLR", Name = "Kruvaze Yaka Çiçekli Desen Elbise", Category = "Elbise" }
    );
    modelBuilder.Entity<AppUser>().HasData(
    new AppUser { Id = 1, FullName = "Feyzanur Duymuş", Username = "feyza", Role = "Admin" , UserCode = "ADM001" },
    new AppUser { Id = 2, FullName = "Ahmet Kasiyer", Username = "ahmet", Role = "Kasiyer" , UserCode = "KSY001" }
    );

    // 2. VARYANTLAR, BARKODLAR VE RESİMLER (ProductVariants)
    modelBuilder.Entity<ProductVariant>().HasData(
        // Tişört Varyantları
        new ProductVariant
        {
            Id = 1,
            ProductId = 1,
            Barcode = "8690000000011",
            Color = "Siyah",
            Size = "M",
            StockQuantity = 15,
            CriticalStockLevel = 3,
            SalePrice = 349.90m,
            PurchasePrice = 120.00m,
            ImageUrl = "https://images.unsplash.com/photo-1521572267360-ee0c2909d518?w=500&q=80"
        },
        new ProductVariant
        {
            Id = 2,
            ProductId = 1,
            Barcode = "8690000000028",
            Color = "Beyaz",
            Size = "L",
            StockQuantity = 2, // KRİTİK STOK TESTİ İÇİN
            CriticalStockLevel = 3,
            SalePrice = 349.90m,
            PurchasePrice = 120.00m,
            ImageUrl = "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?w=500&q=80"
        },

        // Denim Gömlek Varyantı
        new ProductVariant
        {
            Id = 3,
            ProductId = 2,
            Barcode = "8690000000035",
            Color = "Mavi Denim",
            Size = "L",
            StockQuantity = 8,
            CriticalStockLevel = 3,
            SalePrice = 899.00m,
            PurchasePrice = 350.00m,
            ImageUrl = "https://images.unsplash.com/photo-1576995853123-5a10305d93c0?w=500&q=80"
        },

        // Sweatshirt Varyantları
        new ProductVariant
        {
            Id = 4,
            ProductId = 3,
            Barcode = "8690000000042",
            Color = "Gri Melanj",
            Size = "XL",
            StockQuantity = 0, // TÜKENDİ UYARISI TESTİ İÇİN
            CriticalStockLevel = 3,
            SalePrice = 649.50m,
            PurchasePrice = 220.00m,
            ImageUrl = "https://images.unsplash.com/photo-1556905055-8f358a7a47b2?w=500&q=80"
        },

        // Elbise Varyantı
        new ProductVariant
        {
            Id = 5,
            ProductId = 4,
            Barcode = "8690000000059",
            Color = "Kırmızı Çiçekli",
            Size = "S",
            StockQuantity = 20,
            CriticalStockLevel = 4,
            SalePrice = 1199.90m,
            PurchasePrice = 450.00m,
            ImageUrl = "https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=500&q=80"
        }
    );
    }
}