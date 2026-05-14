using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajKariyerWeb.Data;
using StajKariyerWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StajKariyerWeb.Controllers
{
    [Authorize(Roles = "Student")]
    public class ApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> MyApplications()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var applications = await _context.JobApplications
                .Include(j => j.CompanyProfile)
                .Where(j => j.StudentId == currentUser.Id)
                .OrderByDescending(j => j.ApplicationDate)
                .ToListAsync();

            return View(applications);
        }
    }
}
