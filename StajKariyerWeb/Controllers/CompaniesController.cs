using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajKariyerWeb.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StajKariyerWeb.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
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

            return View(company);
        }
    }
}
