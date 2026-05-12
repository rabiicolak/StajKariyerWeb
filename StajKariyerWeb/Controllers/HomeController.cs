using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajKariyerWeb.Data;
using StajKariyerWeb.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StajKariyerWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != null)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var histories = await _context.PredictionHistories
                                                    .Where(h => h.UserId == userId)
                                                    .ToListAsync();

                        var viewModel = new HomeDashboardViewModel();

                        // User Info
                        viewModel.FullName = user.FullName ?? user.UserName;

                        // Tahmin İstatistikleri
                        viewModel.TotalPredictions = histories.Count;

                        if (histories.Any())
                        {
                            var lastPrediction = histories.OrderByDescending(h => h.TahminTarihi).First();
                            viewModel.LastPredictionDate = lastPrediction.TahminTarihi;

                            // En çok önerilen kariyer alanı
                            var mostRecommended = histories
                                                    .Where(h => !string.IsNullOrEmpty(h.MatchedArea))
                                                    .GroupBy(h => h.MatchedArea)
                                                    .OrderByDescending(g => g.Count())
                                                    .Select(g => g.Key)
                                                    .FirstOrDefault();
                            
                            viewModel.MostRecommendedCareer = mostRecommended ?? "Henüz tahmin yapılmadı.";
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

                        // Rastgele 3 Üye Getirme
                        var randomMembers = await _context.Users
                                                .Where(u => u.Id != userId)
                                                .OrderBy(u => Guid.NewGuid())
                                                .Take(3)
                                                .ToListAsync();
                                                
                        viewModel.RecommendedMembers = randomMembers;

                        return View(viewModel);
                    }
                }
            }

            return View();
        }
    }
}