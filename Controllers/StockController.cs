using GiyimStokMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace GiyimStokMvc.Controllers
{
    public class StokController : Controller
    {
        private readonly IStokService _stokService;

        public StokController(IStokService stokService)
        {
            _stokService = stokService;
        }

        public async Task<IActionResult> Index()
        {
            var urunler = await _stokService.GetKasaUrunListesiAsync();
            return View(urunler);
        }

        public async Task<IActionResult> DepoListesi()
        {
            var urunler = await _stokService.GetDepoListesiAsync();
            return View(urunler);
        }

        public IActionResult UrunEkle() => View();

        [HttpPost]
        public async Task<IActionResult> UrunEkle(string modelCode, string name, string color, string size, string barcode, int stockQuantity, int criticalStockLevel, decimal salePrice, IFormFile? imageFile)
        {
            var result = await _stokService.UrunVeyaStokEkleAsync(modelCode, name, color, size, barcode, stockQuantity, criticalStockLevel, salePrice, imageFile);
            
            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction("DepoListesi");
        }

        [HttpGet]
        public async Task<IActionResult> UrunGetir(string barcode)
        {
            var data = await _stokService.UrunGetirByBarcodeAsync(barcode);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> SepetOnayla([FromBody] SepetOnayDto dto)
        {
            if (dto == null || dto.Sepet == null || !dto.Sepet.Any())
            {
                return Json(new { success = false, message = "Sepet boş veya sunucuya ulaşmadı!" });
            }

            var result = await _stokService.SepetOnaylaAsync(dto);
            return Json(new { success = result.Success, message = result.Message });
        }

        public async Task<IActionResult> LogListesi()
        {
            var loglar = await _stokService.GetLogListesiAsync();
            return View(loglar);
        }
        public async Task<IActionResult> KritikStoklar()
        {
            var kritikUrunler = await _stokService.GetKritikStokListesiAsync();
            return View(kritikUrunler);
        }
    }
}