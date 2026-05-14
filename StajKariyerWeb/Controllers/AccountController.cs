using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using StajKariyerWeb.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
namespace StajKariyerWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _env;
        private readonly StajKariyerWeb.Data.ApplicationDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment env,
            StajKariyerWeb.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.IsInRoleAsync(user, "Company"))
                {
                    return RedirectToAction("Index", "CompanyDashboard");
                }
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "E-posta veya şifre hatalı.";
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var role = model.UserType == "Company" ? "Company" : "Student";
                await _userManager.AddToRoleAsync(user, role);

                if (role == "Company")
                {
                    var companyProfile = new CompanyProfile
                    {
                        Name = model.FullName,
                        ApplicationUserId = user.Id
                    };
                    _context.CompanyProfiles.Add(companyProfile);
                    await _context.SaveChangesAsync();
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                
                if (role == "Company")
                {
                    return RedirectToAction("Index", "CompanyDashboard");
                }
                return RedirectToAction("Index", "Dashboard");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToAction("Login");
            }
            if (User.IsInRole("Company"))
                return RedirectToAction("Index", "CompanyDashboard");
                
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction("Login", "Account");
            }
            if (User.IsInRole("Company"))
            {
                return RedirectToAction("EditCompanyProfile");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var model = new ProfileViewModel
            {
                FullName = user.FullName ?? string.Empty,
                Phone = user.Phone,
                City = user.City,
                Department = user.Department,
                University = user.University,
                ShortDescription = user.ShortDescription,
                ExistingProfilePhotoPath = user.ProfilePhotoPath,
                ExistingCVPath = user.CVPath
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileViewModel model)
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction("Login", "Account");
            }
            if (User.IsInRole("Company"))
            {
                return RedirectToAction("EditCompanyProfile");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (!ModelState.IsValid)
            {
                model.ExistingProfilePhotoPath = user.ProfilePhotoPath;
                model.ExistingCVPath = user.CVPath;
                return View(model);
            }

            user.FullName = model.FullName;
            user.Phone = model.Phone;
            user.City = model.City;
            user.Department = model.Department;
            user.University = model.University;
            user.ShortDescription = model.ShortDescription;

            // Profile Photo Upload
            if (model.ProfilePhoto != null && model.ProfilePhoto.Length > 0)
            {
                var ext = Path.GetExtension(model.ProfilePhoto.FileName).ToLowerInvariant();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                {
                    ModelState.AddModelError("ProfilePhoto", "Sadece jpg, jpeg ve png dosyaları yüklenebilir.");
                    model.ExistingProfilePhotoPath = user.ProfilePhotoPath;
                    model.ExistingCVPath = user.CVPath;
                    return View(model);
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "profile-images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + ext;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePhoto.CopyToAsync(fileStream);
                }

                user.ProfilePhotoPath = "/uploads/profile-images/" + uniqueFileName;
            }

            // CV Upload
            if (model.CVFile != null && model.CVFile.Length > 0)
            {
                var ext = Path.GetExtension(model.CVFile.FileName).ToLowerInvariant();
                if (ext != ".pdf" && ext != ".doc" && ext != ".docx")
                {
                    ModelState.AddModelError("CVFile", "Sadece pdf, doc ve docx dosyaları yüklenebilir.");
                    model.ExistingProfilePhotoPath = user.ProfilePhotoPath;
                    model.ExistingCVPath = user.CVPath;
                    return View(model);
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "cv-files");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + ext;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.CVFile.CopyToAsync(fileStream);
                }

                user.CVPath = "/uploads/cv-files/" + uniqueFileName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            model.ExistingProfilePhotoPath = user.ProfilePhotoPath;
            model.ExistingCVPath = user.CVPath;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditCompanyProfile()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!User.IsInRole("Company"))
            {
                return RedirectToAction("EditProfile");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var company = _context.CompanyProfiles.FirstOrDefault(c => c.ApplicationUserId == userId);
            
            if (company == null)
            {
                return RedirectToAction("Index", "CompanyDashboard");
            }

            var model = new EditCompanyProfileViewModel
            {
                Name = company.Name,
                Sector = company.Sector,
                RelatedArea = company.RelatedArea,
                City = company.City,
                Description = company.Description,
                WebsiteUrl = company.WebsiteUrl,
                RequiredSkills = company.RequiredSkills,
                ExistingLogoPath = company.LogoPath
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCompanyProfile(EditCompanyProfileViewModel model)
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!User.IsInRole("Company"))
            {
                return RedirectToAction("EditProfile");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var company = _context.CompanyProfiles.FirstOrDefault(c => c.ApplicationUserId == userId);

            if (company == null) return NotFound();

            if (!ModelState.IsValid)
            {
                model.ExistingLogoPath = company.LogoPath;
                return View(model);
            }

            company.Name = model.Name;
            company.Sector = model.Sector;
            company.RelatedArea = model.RelatedArea;
            company.City = model.City;
            company.Description = model.Description;
            company.WebsiteUrl = model.WebsiteUrl;
            company.RequiredSkills = model.RequiredSkills;

            // Logo Upload
            if (model.Logo != null && model.Logo.Length > 0)
            {
                var ext = Path.GetExtension(model.Logo.FileName).ToLowerInvariant();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                {
                    ModelState.AddModelError("Logo", "Sadece jpg, jpeg ve png dosyaları yüklenebilir.");
                    model.ExistingLogoPath = company.LogoPath;
                    return View(model);
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "company-logos");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + ext;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Logo.CopyToAsync(fileStream);
                }

                company.LogoPath = "/uploads/company-logos/" + uniqueFileName;
            }

            _context.CompanyProfiles.Update(company);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Firma profili başarıyla güncellendi.";
            return RedirectToAction("Index", "CompanyDashboard");
        }
    }
}