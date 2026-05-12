using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajKariyerWeb.Data;
using StajKariyerWeb.Models;
using StajKariyerWeb.Services;
using System.Security.Claims;

namespace StajKariyerWeb.Controllers
{
    public class CareerController : Controller
    {
        private readonly ApiService _apiService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CareerController(ApiService apiService, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _apiService = apiService;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new PredictionRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Index(PredictionRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Ilgili_Alan) ||
                    string.IsNullOrWhiteSpace(request.Proje))
                {
                    ViewBag.Error = "Lütfen tüm alanları doldurun.";
                    return View(request);
                }

                var result = await _apiService.GetPredictionAsync(request);

                if (result == null)
                {
                    ViewBag.Error = "API'den veri alınamadı.";
                    return View(request);
                }

                if (User.Identity?.IsAuthenticated == true)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (userId != null)
                    {
                        var history = new PredictionHistory
                        {
                            UserId = userId,
                            TahminTarihi = DateTime.Now,
                            GNO = request.GNO.ToString(),
                            IlgiliAlan = request.Ilgili_Alan,
                            Proje = request.Proje,
                            Prediction = result.Prediction,
                            MatchedArea = result.MatchedArea
                        };

                        _context.PredictionHistories.Add(history);
                        await _context.SaveChangesAsync();
                    }
                }

                ViewBag.Result = result;
                ViewBag.Roadmap = GenerateRoadmap(result.Prediction, result.MatchedArea);
                return View(request);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.ToString();
                return View(request);
            }
        }

        [HttpGet]
        public async Task<IActionResult> History()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var historyList = await _context.PredictionHistories
                                            .Where(h => h.UserId == userId)
                                            .OrderByDescending(h => h.TahminTarihi)
                                            .ToListAsync();

            return View(historyList);
        }

        private List<string> GenerateRoadmap(string prediction, string matchedArea)
        {
            var area = (prediction + " " + matchedArea).ToLower();
            
            if (area.Contains("veri") || area.Contains("data"))
            {
                return new List<string>
                {
                    "SQL ve veritabanı becerilerini geliştir.",
                    "Python ile veri analizi projeleri yap.",
                    "Power BI veya Tableau öğren.",
                    "GitHub'a en az 2 veri analizi projesi ekle."
                };
            }
            else if (area.Contains("yapay") || area.Contains("zeka") || area.Contains("ai") || area.Contains("makine"))
            {
                return new List<string>
                {
                    "Python ve NumPy/Pandas temellerini güçlendir.",
                    "Makine öğrenmesi algoritmalarını öğren.",
                    "Scikit-learn ile küçük modeller geliştir.",
                    "Model değerlendirme metriklerini öğren."
                };
            }
            else if (area.Contains("web") || area.Contains("front") || area.Contains("full"))
            {
                return new List<string>
                {
                    "HTML, CSS ve JavaScript temellerini güçlendir.",
                    "ASP.NET Core MVC veya React ile proje geliştir.",
                    "REST API tüketimi ve authentication konularını çalış.",
                    "Portföy sitesini yayına al."
                };
            }
            else if (area.Contains("backend") || area.Contains("arka") || area.Contains("sunucu") || area.Contains("api"))
            {
                return new List<string>
                {
                    "C# ve ASP.NET Core Web API pratiği yap.",
                    "SQL Server ve Entity Framework Core öğren.",
                    "Authentication/Authorization konularını geliştir.",
                    "Docker ve deploy süreçlerini öğren."
                };
            }
            else if (area.Contains("mobil") || area.Contains("mobile") || area.Contains("android") || area.Contains("ios"))
            {
                return new List<string>
                {
                    ".NET MAUI veya Flutter temellerini öğren.",
                    "Mobil UI/UX prensiplerini incele.",
                    "API ile haberleşen mobil uygulama geliştir.",
                    "Uygulamayı test edip portföyüne ekle."
                };
            }
            else if (area.Contains("siber") || area.Contains("güvenlik") || area.Contains("cyber") || area.Contains("security"))
            {
                return new List<string>
                {
                    "Ağ temelleri ve Linux komutlarını öğren.",
                    "OWASP Top 10 konularını çalış.",
                    "Basit zafiyet analizleri yap.",
                    "Güvenli kodlama prensiplerini öğren."
                };
            }
            else if (area.Contains("devops") || area.Contains("dev ops"))
            {
                return new List<string>
                {
                    "Linux ve terminal kullanımını geliştir.",
                    "Docker ve Docker Compose öğren.",
                    "CI/CD mantığını öğren.",
                    "Nginx ve deployment pratiği yap."
                };
            }
            else if (area.Contains("oyun") || area.Contains("game"))
            {
                return new List<string>
                {
                    "Unity veya Unreal Engine temellerini öğren.",
                    "C# veya C++ ile oyun mekaniği geliştir.",
                    "Basit bir 2D/3D oyun prototipi yap.",
                    "Oyunu GitHub veya itch.io üzerinde paylaş."
                };
            }
            else
            {
                return new List<string>
                {
                    "Teknik becerilerini düzenli geliştir.",
                    "En az 2 proje geliştirip GitHub'a ekle.",
                    "CV ve profil bilgilerini güncel tut.",
                    "Alanına uygun staj ilanlarını takip et."
                };
            }
        }
    }
}