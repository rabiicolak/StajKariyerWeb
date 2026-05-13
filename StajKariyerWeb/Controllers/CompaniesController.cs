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
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMatchService _matchService;

        public CompaniesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMatchService matchService)
        {
            _context = context;
            _userManager = userManager;
            _matchService = matchService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var companies = await _context.CompanyProfiles
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(companies);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var company = await _context.CompanyProfiles.FindAsync(id);

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

            var topStudents = suitableStudents
                .OrderByDescending(s => s.Score)
                .Take(10)
                .ToList();

            // Check if current user has applied
            var currentUser = await _userManager.GetUserAsync(User);
            bool hasApplied = false;
            if (currentUser != null)
            {
                hasApplied = await _context.JobApplications
                    .AnyAsync(j => j.CompanyProfileId == id && j.StudentId == currentUser.Id);
            }

            var viewModel = new CompanyDetailsViewModel
            {
                Company = company,
                SuitableStudents = topStudents,
                HasApplied = hasApplied
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(int companyId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var company = await _context.CompanyProfiles.FindAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }

            bool hasApplied = await _context.JobApplications
                .AnyAsync(j => j.CompanyProfileId == companyId && j.StudentId == currentUser.Id);

            if (hasApplied)
            {
                TempData["ErrorMessage"] = "Bu firmaya zaten başvuru yaptınız.";
                return RedirectToAction("Details", new { id = companyId });
            }

            var application = new JobApplication
            {
                CompanyProfileId = companyId,
                StudentId = currentUser.Id,
                ApplicationDate = DateTime.Now,
                Status = "Beklemede"
            };

            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Başvurunuz başarıyla alındı!";
            return RedirectToAction("Details", new { id = companyId });
        }
    }
}
