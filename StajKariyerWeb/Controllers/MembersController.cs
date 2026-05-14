using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajKariyerWeb.Data;
using StajKariyerWeb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StajKariyerWeb.Controllers
{
    [Authorize(Roles = "Student")]
    public class MembersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public MembersController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userManager.FindByIdAsync(currentUserId!);
            
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var currentUserMostRecommended = await GetMostRecommendedArea(currentUserId!);

            var studentUsers = await _userManager.GetUsersInRoleAsync("Student");
            var otherUsers = studentUsers
                .Where(u => u.Id != currentUserId)
                .OrderBy(u => u.FullName)
                .ToList();

            var similarMembers = new List<SimilarMemberDto>();
            var allOtherMembers = new List<ApplicationUser>();

            foreach (var user in otherUsers)
            {
                int score = 0;
                var reasons = new List<string>();

                if (!string.IsNullOrEmpty(currentUser.Department) && currentUser.Department == user.Department)
                {
                    score += 30;
                    reasons.Add("+30 Aynı Bölüm");
                }

                if (!string.IsNullOrEmpty(currentUser.City) && currentUser.City == user.City)
                {
                    score += 20;
                    reasons.Add("+20 Aynı Şehir");
                }

                if (!string.IsNullOrEmpty(currentUser.University) && currentUser.University == user.University)
                {
                    score += 20;
                    reasons.Add("+20 Aynı Üniversite");
                }

                var userMostRecommended = await GetMostRecommendedArea(user.Id);
                if (!string.IsNullOrEmpty(currentUserMostRecommended) && currentUserMostRecommended == userMostRecommended)
                {
                    score += 30;
                    reasons.Add("+30 Aynı Kariyer Hedefi");
                }

                if (score > 0)
                {
                    similarMembers.Add(new SimilarMemberDto
                    {
                        User = user,
                        SimilarityScore = score,
                        MatchReasons = reasons
                    });
                }
                else
                {
                    allOtherMembers.Add(user);
                }
            }

            var topSimilarMembers = similarMembers
                .OrderByDescending(s => s.SimilarityScore)
                .Take(3)
                .ToList();

            var topSimilarUserIds = topSimilarMembers.Select(s => s.User.Id).ToHashSet();
            foreach (var s in similarMembers.Where(sm => !topSimilarUserIds.Contains(sm.User.Id)))
            {
                allOtherMembers.Add(s.User);
            }
            
            allOtherMembers = allOtherMembers.OrderBy(u => u.FullName).ToList();

            var viewModel = new MembersViewModel
            {
                TopSimilarMembers = topSimilarMembers,
                AllOtherMembers = allOtherMembers
            };

            return View(viewModel);
        }

        private async Task<string?> GetMostRecommendedArea(string userId)
        {
            var histories = await _context.PredictionHistories
                .Where(h => h.UserId == userId && !string.IsNullOrEmpty(h.MatchedArea))
                .ToListAsync();

            if (!histories.Any())
                return null;

            return histories
                .GroupBy(h => h.MatchedArea)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
        }
    }
}
