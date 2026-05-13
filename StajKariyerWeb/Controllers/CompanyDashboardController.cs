using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajKariyerWeb.Data;
using StajKariyerWeb.Models;
using StajKariyerWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajKariyerWeb.Controllers
{
    [Authorize]
    public class CompanyDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMatchService _matchService;

        public CompanyDashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMatchService matchService)
        {
            _context = context;
            _userManager = userManager;
            _matchService = matchService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            int companyId = 1,
            string? filterCity = null,
            string? filterDepartment = null,
            string? filterRelatedArea = null,
            int? filterMinScore = null)
        {
            var company = await _context.CompanyProfiles.FindAsync(companyId);

            if (company == null)
            {
                return NotFound();
            }

            var allUsers = await _userManager.Users.ToListAsync();
            var suitableStudents = new List<SuitableStudentViewModel>();

            foreach (var user in allUsers)
            {
                var result = await _matchService.CalculateMatchScoreAsync(company, user);
                if (result.Score > 0)
                {
                    suitableStudents.Add(result);
                }
            }

            // All suitable students stats before filtering
            int totalSuitable = suitableStudents.Count;
            int highestScore = suitableStudents.Any() ? suitableStudents.Max(s => s.Score) : 0;
            double averageScore = suitableStudents.Any() ? suitableStudents.Average(s => s.Score) : 0;
            
            string mostMatchedSkill = suitableStudents.SelectMany(s => s.MatchedFeatures)
                .GroupBy(f => f)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "Veri Yok";
                
            string mostActiveCity = suitableStudents.Where(s => !string.IsNullOrEmpty(s.City))
                .GroupBy(s => s.City)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "Veri Yok";

            // Apply Filters
            var filteredStudents = suitableStudents.AsEnumerable();

            if (!string.IsNullOrEmpty(filterCity))
            {
                filteredStudents = filteredStudents.Where(s => !string.IsNullOrEmpty(s.City) && s.City.Contains(filterCity, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(filterDepartment))
            {
                filteredStudents = filteredStudents.Where(s => !string.IsNullOrEmpty(s.Department) && s.Department.Contains(filterDepartment, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(filterRelatedArea))
            {
                filteredStudents = filteredStudents.Where(s => !string.IsNullOrEmpty(s.ShortDescription) && s.ShortDescription.Contains(filterRelatedArea, StringComparison.OrdinalIgnoreCase));
            }

            if (filterMinScore.HasValue)
            {
                filteredStudents = filteredStudents.Where(s => s.Score >= filterMinScore.Value);
            }

            var finalStudentsList = filteredStudents
                .OrderByDescending(s => s.Score)
                .ToList();

            // Get Job Applications
            var jobApplications = await _context.JobApplications
                .Include(j => j.Student)
                .Include(j => j.CompanyProfile)
                .Where(j => j.CompanyProfileId == companyId)
                .OrderByDescending(j => j.ApplicationDate)
                .ToListAsync();

            var viewModel = new CompanyDashboardViewModel
            {
                Company = company,
                SuitableStudents = finalStudentsList,
                JobApplications = jobApplications,
                TotalSuitableStudents = totalSuitable,
                HighestMatchScore = highestScore,
                AverageMatchScore = averageScore,
                MostMatchedSkill = mostMatchedSkill,
                MostActiveCity = mostActiveCity,
                FilterCity = filterCity,
                FilterDepartment = filterDepartment,
                FilterRelatedArea = filterRelatedArea,
                FilterMinScore = filterMinScore
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateApplicationStatus(int applicationId, string newStatus, int companyId)
        {
            var application = await _context.JobApplications.FindAsync(applicationId);
            
            if (application != null)
            {
                application.Status = newStatus;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Başvuru durumu güncellendi.";
            }

            return RedirectToAction("Index", new { companyId = companyId });
        }
    }
}
