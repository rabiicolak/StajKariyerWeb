using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajKariyerWeb.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StajKariyerWeb.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public MembersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Tüm kullanıcıları getir, ancak mevcut giriş yapan kullanıcıyı hariç tut
            var members = await _userManager.Users
                .Where(u => u.Id != currentUserId)
                .OrderBy(u => u.FullName)
                .ToListAsync();

            return View(members);
        }
    }
}
