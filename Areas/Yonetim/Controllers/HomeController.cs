using Microsoft.AspNetCore.Mvc;
using GiyimStokMvc.Areas.Yonetim.Services;

namespace GiyimStokMvc.Areas.Yonetim.Controllers
{
    [Area("Yonetim")]
    public class HomeController : Controller
    {
        private readonly IYonetimService _yonetimService;

        public HomeController(IYonetimService yonetimService)
        {
            _yonetimService = yonetimService;
        }

        // 1. Modal'dan gelen PIN doğrulama isteğini karşılar
        [HttpPost]
        public async Task<IActionResult> YoneticiDogrula([FromBody] string pin)
        {
            var sonuc = await _yonetimService.YoneticiDogrulaAsync(pin);
            if (!sonuc.Success)
                return Json(new { success = false, message = sonuc.Message });

            HttpContext.Session.SetString("AdminYetkili", "Evet");
            return Json(new { success = true });
        }

        // 2. Yönetici Paneli Ana Sayfası
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("AdminYetkili") != "Evet")
            {
                return RedirectToAction("LogListesi", "Stok", new { area = "" });
            }

            var personeller = await _yonetimService.GetPersonelListesiAsync();
            return View(personeller);
        }

        // 3. Yeni Personel Ekleme İşlemi
        [HttpPost]
        public async Task<IActionResult> PersonelEkle(string fullName, string userCode, string department, string username)
        {
            if (HttpContext.Session.GetString("AdminYetkili") != "Evet")
                return RedirectToAction("LogListesi", "Stok", new { area = "" });

            var result = await _yonetimService.PersonelEkleAsync(fullName, userCode, department, username);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> StokLoglari()
        {
            if (HttpContext.Session.GetString("AdminYetkili") != "Evet")
                return RedirectToAction("Index", "Stok", new { area = "" });

            var stokLoglari = await _yonetimService.GetStokLogListesiAsync();
            return View(stokLoglari);
        }
    }
}