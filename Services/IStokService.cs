using GiyimStokMvc.Models;

namespace GiyimStokMvc.Services
{
    public interface IStokService
    {
        Task<List<ProductVariant>> GetDepoListesiAsync();
        Task<List<AppUser>> GetPersonelListesiAsync();
        Task<(bool Success, string Message)> PersonelEkleAsync(string fullName, string userCode, string department, string Username);
        Task<List<StockTransaction>> GetLogListesiAsync();
        Task<List<ProductVariant>> GetKasaUrunListesiAsync();
        Task<List<ProductVariant>> GetKritikStokListesiAsync();
        Task<(bool Success, string Message)> UrunVeyaStokEkleAsync(
            string modelCode, string name, string color, string size, 
            string barcode, int stockQuantity, int criticalStockLevel, 
            decimal salePrice, IFormFile? imageFile);
        Task<object> UrunGetirByBarcodeAsync(string barcode);
        Task<(bool Success, string Message)> SepetOnaylaAsync(SepetOnayDto dto);
    }
    public class SepetOnayDto
    {
        public List<SepetItemDto> Sepet { get; set; } = new List<SepetItemDto>();
        public string UserCode { get; set; } = string.Empty;
        public OdemeTipi OdemeTipi { get; set; } = OdemeTipi.Nakit;
        public decimal NakitTutar { get; set; }
        public decimal KartTutar { get; set; }
    }

    public class SepetItemDto
    {
        public string Barcode { get; set; } = string.Empty;
        public int Adet { get; set; }
    }
}