using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajKariyerWeb.Data;
using StajKariyerWeb.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StajKariyerWeb.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var histories = await _context.PredictionHistories
                                        .Where(h => h.UserId == userId)
                                        .ToListAsync();

            var viewModel = new DashboardViewModel();

            // Tahmin İstatistikleri
            viewModel.TotalPredictions = histories.Count;

            if (histories.Any())
            {
                var lastPrediction = histories.OrderByDescending(h => h.TahminTarihi).First();
                viewModel.LastPredictionDate = lastPrediction.TahminTarihi;

                // En çok önerilen kariyer alanı
                var mostRecommended = histories
                                        .GroupBy(h => h.MatchedArea)
                                        .OrderByDescending(g => g.Count())
                                        .Select(g => g.Key)
                                        .FirstOrDefault();
                
                viewModel.MostRecommendedCareer = mostRecommended;
            }
            else
            {
                viewModel.MostRecommendedCareer = "Henüz tahmin yapılmadı.";
            }

            // Profil Tamamlama Yüzdesi Hesaplama
            int filledFields = 0;
            int totalFields = 8;

            if (!string.IsNullOrWhiteSpace(user.FullName)) filledFields++;
            if (!string.IsNullOrWhiteSpace(user.Phone)) filledFields++;
            if (!string.IsNullOrWhiteSpace(user.City)) filledFields++;
            if (!string.IsNullOrWhiteSpace(user.Department)) filledFields++;
            if (!string.IsNullOrWhiteSpace(user.University)) filledFields++;
            if (!string.IsNullOrWhiteSpace(user.ShortDescription)) filledFields++;
            if (!string.IsNullOrWhiteSpace(user.ProfilePhotoPath)) filledFields++;
            if (!string.IsNullOrWhiteSpace(user.CVPath)) filledFields++;

            viewModel.ProfileCompletionPercentage = (int)((filledFields / (double)totalFields) * 100);

            return View(viewModel);
        }
    }
}
