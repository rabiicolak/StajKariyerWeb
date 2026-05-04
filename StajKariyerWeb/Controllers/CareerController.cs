using Microsoft.AspNetCore.Mvc;
using StajKariyerWeb.Models;
using StajKariyerWeb.Services;

namespace StajKariyerWeb.Controllers
{
    public class CareerController : Controller
    {
        private readonly ApiService _apiService;

        public CareerController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new PredictionRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Index(PredictionRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Ilgili_Alan) ||
                    string.IsNullOrWhiteSpace(request.Proje))
                {
                    ViewBag.Error = "Lütfen tüm alanları doldurun.";
                    return View(request);
                }

                var result = await _apiService.GetPredictionAsync(request);

                if (result == null)
                {
                    ViewBag.Error = "API'den veri alınamadı.";
                    return View(request);
                }

                ViewBag.Result = result;
                return View(request);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.ToString();
                return View(request);
            }
        }
    }
}