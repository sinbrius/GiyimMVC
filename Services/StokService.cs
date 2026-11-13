using GiyimStokMvc.Models;
using GiyimStokMvc.Models.DbContext;
using Microsoft.EntityFrameworkCore;

namespace GiyimStokMvc.Services
{
    public class StokService : IStokService
    {
        private readonly AppDbContext _context;

        public StokService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<StockTransaction>> GetLogListesiAsync()
{
    return await _context.StockTransactions
        .Include(t => t.ProductVariant)
            .ThenInclude(v => v!.Product)
        .Include(t => t.User)
        .OrderByDescending(t => t.TransactionDate)
        .Take(20)
        .ToListAsync();
}

        public async Task<List<ProductVariant>> GetDepoListesiAsync()
        {
            return await _context.ProductVariants
                .Include(v => v.Product)
                .OrderBy(v => v.Product!.Name)
                .ToListAsync();
        }
        public async Task<List<AppUser>> GetPersonelListesiAsync()
        {
            return await _context.Users
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }
        public async Task<(bool Success, string Message)> PersonelEkleAsync(string fullName, string userCode, string department, string username)
{
    if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(userCode) || string.IsNullOrWhiteSpace(department))
    {
        return (false, "Lütfen tüm alanları doldurun!");
    }

    // Aynı kodla başka personel var mı kontrolü
    bool kodVarMi = await _context.Users.AnyAsync(u => u.UserCode == userCode.Trim());
    if (kodVarMi)
    {
        return (false, "Bu Personel Kodu (PIN) zaten kullanımda! Lütfen yeni kod üretin.");
    }

    var yeniPersonel = new AppUser
    {
        FullName = fullName.Trim(),
        UserCode = userCode.Trim().ToUpper(),
        Role = department,
        Username = string.IsNullOrWhiteSpace(username) ? userCode.Trim().ToLower() : username.Trim()
    };

    _context.Users.Add(yeniPersonel);
    await _context.SaveChangesAsync();

    return (true, "Yeni personel başarıyla eklendi!");
}
        public async Task<List<ProductVariant>> GetKritikStokListesiAsync()
        {
            return await _context.ProductVariants
                .Include(v => v.Product)
                .Where(v => v.StockQuantity <= v.CriticalStockLevel)
                .OrderBy(v => v.StockQuantity)
                .ToListAsync();
        }

        public async Task<List<ProductVariant>> GetKasaUrunListesiAsync()
        {
            return await _context.ProductVariants
                .Include(v => v.Product)
                .Where(v => v.StockQuantity > 0)
                .OrderBy(v => v.Product!.Category)
                .ThenBy(v => v.Product!.Name)
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> UrunVeyaStokEkleAsync(
            string modelCode, string name, string color, string size, 
            string barcode, int stockQuantity, int criticalStockLevel, 
            decimal salePrice, IFormFile? imageFile)
        {
            if (string.IsNullOrWhiteSpace(barcode))
                return (false, "Barkod alanı zorunludur!");

            string? savedImagePath = await SaveImageFileAsync(imageFile);

            var existingVariant = await _context.ProductVariants
                .Include(v => v.Product)
                .FirstOrDefaultAsync(v => v.Barcode == barcode.Trim());

            if (existingVariant != null)
            {
                existingVariant.StockQuantity += stockQuantity;
                if (salePrice > 0) existingVariant.SalePrice = salePrice;
                if (!string.IsNullOrEmpty(savedImagePath)) existingVariant.ImageUrl = savedImagePath;

                await _context.SaveChangesAsync();
                return (true, $"'{existingVariant.Product?.Name}' stoğuna +{stockQuantity} adet eklendi! Güncel Stok: {existingVariant.StockQuantity}");
            }

            if (string.IsNullOrWhiteSpace(name))
                return (false, "Yeni ürün eklerken Ürün Adı zorunludur!");

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ModelCode == modelCode.Trim());
            if (product == null)
            {
                product = new Product
                {
                    ModelCode = string.IsNullOrWhiteSpace(modelCode) ? "MDL-" + Guid.NewGuid().ToString()[..5].ToUpper() : modelCode.Trim(),
                    Name = name.Trim()
                };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }

            var variant = new ProductVariant
            {
                ProductId = product.Id,
                Barcode = barcode.Trim(),
                Color = color.Trim(),
                Size = size.Trim().ToUpper(),
                StockQuantity = stockQuantity,
                CriticalStockLevel = criticalStockLevel > 0 ? criticalStockLevel : 3,
                SalePrice = salePrice,
                ImageUrl = savedImagePath
            };

            _context.ProductVariants.Add(variant);
            await _context.SaveChangesAsync();

            return (true, $"'{name} ({color}/{size})' başarıyla depoya eklendi!");
        }

        public async Task<object> UrunGetirByBarcodeAsync(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
                return new { success = false, message = "Lütfen bir barkod giriniz!" };

            var variant = await _context.ProductVariants
                .Include(v => v.Product)
                .FirstOrDefaultAsync(v => v.Barcode == barcode.Trim());

            if (variant == null)
                return new { success = false, message = "Bu barkoda ait ürün bulunamadı!" };

            if (variant.StockQuantity <= 0)
                return new { success = false, message = "Bu ürünün stoğu tükenmiş!" };

            return new
            {
                success = true,
                barcode = variant.Barcode,
                name = variant.Product?.Name ?? "Ürün",
                modelCode = variant.Product?.ModelCode,
                color = variant.Color,
                size = variant.Size,
                variant = $"{variant.Color} / {variant.Size}",
                price = variant.SalePrice,
                stock = variant.StockQuantity,
                imageUrl = variant.ImageUrl
            };
        }

        public async Task<(bool Success, string Message)> SepetOnaylaAsync(SepetOnayDto dto)
{
    if (dto == null || dto.Sepet == null || !dto.Sepet.Any()) 
        return (false, "Sepet boş!");

    if (string.IsNullOrWhiteSpace(dto.UserCode))
        return (false, "Lütfen Personel Kodunuzu / PIN giriniz!");

    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserCode == dto.UserCode.Trim());
    if (user == null)
    {
        return (false, "Geçersiz Personel Kodu / PIN! Lütfen kontrol edin.");
    }

    foreach (var item in dto.Sepet)
    {
        var variant = await _context.ProductVariants.FirstOrDefaultAsync(v => v.Barcode == item.Barcode);
        if (variant != null)
        {
            if (variant.StockQuantity < item.Adet)
            {
                return (false, $"'{variant.Barcode}' barkodlu ürün için yeterli stok yok!");
            }

            variant.StockQuantity -= item.Adet;

            _context.StockTransactions.Add(new StockTransaction
            {
                ProductVariantId = variant.Id,
                Quantity = item.Adet,
                Type = IslemTuru.Satis,
                TransactionDate = DateTime.Now,
                UserId = user.Id,

                // Ödeme detayları kaydediliyor
                OdemeTipi = dto.OdemeTipi,
                NakitTutar = dto.NakitTutar,
                KartTutar = dto.KartTutar
            });
        }
    }

    await _context.SaveChangesAsync();
    return (true, $"Satış başarıyla tamamlandı! İşlemi Yapan: {user.FullName}");
}

        private async Task<string?> SaveImageFileAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) return null;

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString()[..8] + "_" + Path.GetFileName(imageFile.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return "/images/" + uniqueFileName;
        }
    }
}