using GiyimStokMvc.Models;

namespace GiyimStokMvc.Areas.Yonetim.Services
{
    public interface IYonetimService
    {
        Task<(bool Success, string Message)> YoneticiDogrulaAsync(string pin);
        Task<List<AppUser>> GetPersonelListesiAsync();
        Task<(bool Success, string Message)> PersonelEkleAsync(string fullName, string userCode, string department, string username);
        Task<(bool Success, string Message)> IndirimUygulaAsync(string? category, int? productId, decimal discountRate);
        Task<List<StockTransaction>> GetStokLogListesiAsync();
    }
}