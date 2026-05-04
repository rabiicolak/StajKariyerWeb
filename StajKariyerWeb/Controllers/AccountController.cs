using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using StajKariyerWeb.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace StajKariyerWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _env;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
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
                await _signInManager.SignInAsync(user, isPersistent: false);
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
    }
}