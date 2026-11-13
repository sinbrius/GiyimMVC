using GiyimStokMvc.Models;
using GiyimStokMvc.Models.DbContext;
using Microsoft.EntityFrameworkCore;

namespace GiyimStokMvc.Areas.Yonetim.Services
{
    public class YonetimService : IYonetimService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public YonetimService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // 1. Yönetici PIN Doğrulama (Önce Veritabanındaki Yönetici/Admin Personellere Bakar, Yoksa Master PIN'e Bakar)
        public async Task<(bool Success, string Message)> YoneticiDogrulaAsync(string pin)
        {
            if (string.IsNullOrWhiteSpace(pin))
                return (false, "PIN kodu boş olamaz!");

            var temizPin = pin.Trim();

            // Veritabanında rolü Yönetici veya Admin olan personeli kontrol et
            var yetkiliPersonel = await _context.Users
                .FirstOrDefaultAsync(u => u.UserCode.ToLower() == temizPin.ToLower() 
                                       && (u.Role == "Yönetici" || u.Role == "Admin"));

            if (yetkiliPersonel != null)
                return (true, $"Hoş geldiniz, {yetkiliPersonel.FullName}");

            // Yedek Master PIN kontrolü (appsettings.json içinde "AdminPin": "1234" varsa)
            var masterPin = _configuration["AdminPin"];
            if (!string.IsNullOrEmpty(masterPin) && temizPin == masterPin)
                return (true, "Master yönetici girişi doğrulandı.");

            return (false, "Geçersiz PIN kodu veya yönetici yetkiniz yok!");
        }

        // 2. Personel Listesini Getir
        public async Task<List<AppUser>> GetPersonelListesiAsync()
        {
            return await _context.Users.OrderByDescending(u => u.Id).ToListAsync();
        }

        // 3. Yeni Personel Ekle
        public async Task<(bool Success, string Message)> PersonelEkleAsync(string fullName, string userCode, string department, string username)
        {
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(userCode))
                return (false, "Ad Soyad ve PIN Kodu zorunludur!");

            bool kodVarMi = await _context.Users.AnyAsync(u => u.UserCode.ToLower() == userCode.Trim().ToLower());
            if (kodVarMi)
                return (false, "Bu PIN kodu başka bir personele tanımlı!");

            var yeniPersonel = new AppUser
            {
                FullName = fullName.Trim(),
                UserCode = userCode.Trim().ToUpper(),
                Role = department, // Yönetici, Kasiyer, Depo vb.
                Username = string.IsNullOrWhiteSpace(username) ? fullName.Trim().ToLower().Replace(" ", "") : username.Trim()
            };

            _context.Users.Add(yeniPersonel);
            await _context.SaveChangesAsync();

            return (true, $"{yeniPersonel.FullName} başarıyla eklendi. (PIN: {yeniPersonel.UserCode})");
        }

        // 4. Kategori veya Tekil Ürüne İndirim Tanımlama
        public async Task<(bool Success, string Message)> IndirimUygulaAsync(string? category, int? productId, decimal discountRate)

        {
            if (discountRate < 0 || discountRate > 100)
                return (false, "İndirim oranı 0 ile 100 arasında olmalıdır!");

            if (productId.HasValue)
            {
                var urun = await _context.ProductVariants.FindAsync(productId.Value);
                if (urun == null) return (false, "Ürün bulunamadı!");

                urun.DiscountRate = discountRate;
                await _context.SaveChangesAsync();
                return (true, $"{urun.Product.Name} ürününe %{discountRate} indirim tanımlandı.");
            }
            else if (!string.IsNullOrWhiteSpace(category))
            {
                var kategoriUrunleri = await _context.ProductVariants
                    .Where(p => p.Product.Category == category)
                    .ToListAsync();

                if (!kategoriUrunleri.Any())
                    return (false, "Bu kategoride ürün bulunamadı!");

                foreach (var item in kategoriUrunleri)
                {
                    item.DiscountRate = discountRate;
                }

                await _context.SaveChangesAsync();
                return (true, $"{category} kategorisindeki {kategoriUrunleri.Count} ürüne %{discountRate} indirim uygulandı!");
            }

            return (false, "Lütfen bir kategori veya ürün seçin.");
        }

        public async Task<List<StockTransaction>> GetStokLogListesiAsync()
        {
           return await _context.StockTransactions
        .Include(t => t.ProductVariant)
            .ThenInclude(v => v!.Product)
        .Include(t => t.User)
        .OrderByDescending(t => t.TransactionDate)
        .Take(200)
        .ToListAsync();
        }
    }
}