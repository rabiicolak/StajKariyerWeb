using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajKariyerWeb.Data;
using StajKariyerWeb.Models;
using StajKariyerWeb.Services;
using System.Security.Claims;

namespace StajKariyerWeb.Controllers
{
    public class CareerController : Controller
    {
        private readonly ApiService _apiService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CareerController(ApiService apiService, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _apiService = apiService;
            _context = context;
            _userManager = userManager;
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

                if (User.Identity?.IsAuthenticated == true)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId != null)
                    {
                        var history = new PredictionHistory
                        {
                            UserId = userId,
                            TahminTarihi = DateTime.Now,
                            GNO = request.GNO.ToString(),
                            IlgiliAlan = request.Ilgili_Alan,
                            Proje = request.Proje,
                            Prediction = result.Prediction,
                            MatchedArea = result.MatchedArea
                        };

                        _context.PredictionHistories.Add(history);
                        await _context.SaveChangesAsync();
                    }
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

        [HttpGet]
        public async Task<IActionResult> History()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var historyList = await _context.PredictionHistories
                                            .Where(h => h.UserId == userId)
                                            .OrderByDescending(h => h.TahminTarihi)
                                            .ToListAsync();

            return View(historyList);
        }
    }
}