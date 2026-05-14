using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StajKariyerWeb.Models;
using System.Collections.Generic;

namespace StajKariyerWeb.Controllers
{
    [Authorize(Roles = "Student")]
    public class RoadmapController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            PopulateDropdowns();
            return View(new RoadmapViewModel());
        }

        [HttpPost]
        public IActionResult Index(RoadmapViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.TargetArea) ||
                string.IsNullOrWhiteSpace(model.TargetCompany) ||
                string.IsNullOrWhiteSpace(model.CurrentLevel) ||
                string.IsNullOrWhiteSpace(model.TargetRole))
            {
                model.ErrorMessage = "Lütfen tüm hedeflerinizi eksiksiz belirleyin.";
                PopulateDropdowns();
                return View(model);
            }

            model.Steps = GenerateCustomRoadmap(model.TargetArea, model.CurrentLevel);
            
            PopulateDropdowns();
            return View(model);
        }

        private void PopulateDropdowns()
        {
            ViewBag.Areas = new List<SelectListItem>
            {
                new SelectListItem { Value = "Yapay Zeka", Text = "Yapay Zeka" },
                new SelectListItem { Value = "Veri Bilimi", Text = "Veri Bilimi" },
                new SelectListItem { Value = "Web", Text = "Web Geliştirme" },
                new SelectListItem { Value = "Backend", Text = "Backend Geliştirme" },
                new SelectListItem { Value = "Mobil", Text = "Mobil Geliştirme" },
                new SelectListItem { Value = "Siber Güvenlik", Text = "Siber Güvenlik" },
                new SelectListItem { Value = "DevOps", Text = "DevOps" },
                new SelectListItem { Value = "Oyun Geliştirme", Text = "Oyun Geliştirme" }
            };

            ViewBag.Companies = new List<SelectListItem>
            {
                new SelectListItem { Value = "AI Tech", Text = "AI Tech" },
                new SelectListItem { Value = "Analytica", Text = "Analytica" },
                new SelectListItem { Value = "ApiCore", Text = "ApiCore" },
                new SelectListItem { Value = "AppForge", Text = "AppForge" },
                new SelectListItem { Value = "BackendPro", Text = "BackendPro" },
                new SelectListItem { Value = "CloudOps", Text = "CloudOps" },
                new SelectListItem { Value = "CyberSec", Text = "CyberSec" },
                new SelectListItem { Value = "DataSense", Text = "DataSense" },
                new SelectListItem { Value = "DeepMindTR", Text = "DeepMindTR" },
                new SelectListItem { Value = "DeployNet", Text = "DeployNet" },
                new SelectListItem { Value = "FrontBase", Text = "FrontBase" },
                new SelectListItem { Value = "FullStackHub", Text = "FullStackHub" },
                new SelectListItem { Value = "GameCraft", Text = "GameCraft" },
                new SelectListItem { Value = "InfraStack", Text = "InfraStack" },
                new SelectListItem { Value = "InsightIQ", Text = "InsightIQ" },
                new SelectListItem { Value = "MobileX", Text = "MobileX" },
                new SelectListItem { Value = "PixelForge", Text = "PixelForge" },
                new SelectListItem { Value = "PocketWare", Text = "PocketWare" },
                new SelectListItem { Value = "SecureNet", Text = "SecureNet" },
                new SelectListItem { Value = "ServerNet", Text = "ServerNet" },
                new SelectListItem { Value = "SimuTech", Text = "SimuTech" },
                new SelectListItem { Value = "ThreatShield", Text = "ThreatShield" },
                new SelectListItem { Value = "VisionLab", Text = "VisionLab" },
                new SelectListItem { Value = "WebSoft", Text = "WebSoft" }
            }.OrderBy(c => c.Text).ToList();

            ViewBag.Levels = new List<SelectListItem>
            {
                new SelectListItem { Value = "Başlangıç", Text = "Başlangıç (Hiç/Az tecrübe)" },
                new SelectListItem { Value = "Orta", Text = "Orta (Projeler geliştirebiliyorum)" },
                new SelectListItem { Value = "İyi", Text = "İyi (İleri seviye projelerim var)" }
            };

            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "Intern", Text = "Stajyer (Intern)" },
                new SelectListItem { Value = "Junior", Text = "Başlangıç (Junior)" },
                new SelectListItem { Value = "Specialist", Text = "Uzman (Specialist)" }
            };
        }

        private List<RoadmapStep> GenerateCustomRoadmap(string targetArea, string currentLevel)
        {
            var steps = new List<RoadmapStep>();
            
            // Step 1 based on Level
            if (currentLevel == "Başlangıç")
            {
                steps.Add(new RoadmapStep { Title = "Temel Eğitim", Description = "Alanınızın temel prensiplerini ve sintaksını öğrenin.", Action = "Çevrimiçi bir temel kurs bitir ve basit örnekler yaz.", EstimatedTime = "1-2 Hafta" });
            }
            else if (currentLevel == "Orta")
            {
                steps.Add(new RoadmapStep { Title = "Mimari ve Best Practices", Description = "Alanınızdaki best practice (en iyi uygulama) kurallarını öğrenin.", Action = "Clean code ve design pattern konularını araştır, mevcut kodlarını refactor et.", EstimatedTime = "1 Hafta" });
            }
            else
            {
                steps.Add(new RoadmapStep { Title = "İleri Seviye Optimizasyon", Description = "Performans ve güvenlik konularına odaklanın.", Action = "Sistem tasarımını (System Design) öğren ve kod optimizasyonları yap.", EstimatedTime = "2 Hafta" });
            }

            // Area Specific Steps
            if (targetArea == "Veri Bilimi")
            {
                steps.Add(new RoadmapStep { Title = "Veri Tabanı Hakimiyeti", Description = "SQL ile karmaşık sorgular yazma yeteneğini geliştir.", Action = "Relational database mantığını kavra, SQL ile join ve aggregate fonksiyonlarını kullan.", EstimatedTime = "2 Hafta" });
                steps.Add(new RoadmapStep { Title = "Python & Analiz", Description = "Pandas, NumPy gibi kütüphaneleri etkin kullan.", Action = "Python ile açık kaynaklı veri setleri üzerinde keşifsel veri analizi (EDA) projeleri geliştir.", EstimatedTime = "3 Hafta" });
                steps.Add(new RoadmapStep { Title = "Dashboard Geliştirme", Description = "Analizlerini görselleştirerek anlatılabilir hale getir.", Action = "Power BI veya Tableau kullanarak profesyonel raporlar hazırla.", EstimatedTime = "1 Hafta" });
            }
            else if (targetArea == "Yapay Zeka")
            {
                steps.Add(new RoadmapStep { Title = "Matematik ve Algoritmalar", Description = "Lineer cebir, olasılık ve temel ML algoritmalarını anla.", Action = "Scikit-learn ile basit makine öğrenmesi modelleri kur ve değerlendir.", EstimatedTime = "3 Hafta" });
                steps.Add(new RoadmapStep { Title = "Derin Öğrenme Temelleri", Description = "Yapay sinir ağları mimarilerini incele.", Action = "TensorFlow veya PyTorch ile temel bir Computer Vision veya NLP projesi yap.", EstimatedTime = "4 Hafta" });
                steps.Add(new RoadmapStep { Title = "Model Deployment", Description = "Geliştirdiğin modelleri canlıya al.", Action = "FastAPI veya Flask ile yazdığın modeli bir API'ye dönüştür.", EstimatedTime = "2 Hafta" });
            }
            else if (targetArea == "Web" || targetArea == "Backend")
            {
                steps.Add(new RoadmapStep { Title = "API Geliştirme", Description = "RESTful API standartlarında servisler geliştir.", Action = "ASP.NET Core veya Node.js ile CRUD işlemleri yapan bir API yaz.", EstimatedTime = "2 Hafta" });
                steps.Add(new RoadmapStep { Title = "Veri Tabanı ve ORM", Description = "Veritabanı ilişkilerini kur ve ORM kullan.", Action = "Entity Framework Core ile Code-First veritabanı modellemesi yap.", EstimatedTime = "2 Hafta" });
                steps.Add(new RoadmapStep { Title = "Güvenlik ve Kimlik Doğrulama", Description = "JWT ve Role-based erişimi projelerine entegre et.", Action = "API'ne Identity veya JWT Authentication ekle.", EstimatedTime = "1 Hafta" });
            }
            else if (targetArea == "Mobil")
            {
                steps.Add(new RoadmapStep { Title = "UI/UX ve State Management", Description = "Kullanıcı dostu arayüzler ve stabil durum yönetimi.", Action = ".NET MAUI veya Flutter ile kompleks bir UI tasarımı yap ve state management kullan.", EstimatedTime = "3 Hafta" });
                steps.Add(new RoadmapStep { Title = "API Entegrasyonu", Description = "Dış servislerle güvenli bir şekilde haberleş.", Action = "Mobil uygulamandan bir REST API tüket, verileri ekranda listele.", EstimatedTime = "1 Hafta" });
            }
            else if (targetArea == "Siber Güvenlik")
            {
                steps.Add(new RoadmapStep { Title = "Ağ Güvenliği ve Linux", Description = "Temel ağ protokolleri ve işletim sistemi mantığını kavra.", Action = "Linux terminal komutlarını öğren, temel ağ analiz araçlarını (Wireshark) kullan.", EstimatedTime = "3 Hafta" });
                steps.Add(new RoadmapStep { Title = "Sızma Testleri", Description = "Uygulama açıklarını bulma konusunda pratik yap.", Action = "HackTheBox veya TryHackMe platformlarında başlangıç seviyesi makineleri çöz.", EstimatedTime = "4 Hafta" });
            }
            else if (targetArea == "DevOps")
            {
                steps.Add(new RoadmapStep { Title = "Konteynerleştirme", Description = "Uygulamaları çevreden bağımsız hale getir.", Action = "Docker öğren, örnek bir uygulamayı Dockerize et ve Docker Compose yaz.", EstimatedTime = "2 Hafta" });
                steps.Add(new RoadmapStep { Title = "CI/CD Pipeline", Description = "Sürekli entegrasyon ve dağıtım süreçleri kur.", Action = "GitHub Actions veya GitLab CI ile otomatik build ve test pipeline'ı oluştur.", EstimatedTime = "2 Hafta" });
            }
            else if (targetArea == "Oyun Geliştirme")
            {
                steps.Add(new RoadmapStep { Title = "Oyun Motoru Hakimiyeti", Description = "Oyun motoru arayüzüne ve script yapısına alış.", Action = "Unity'de basit bir mekanik (zıplama, koşma vb.) kodla.", EstimatedTime = "2 Hafta" });
                steps.Add(new RoadmapStep { Title = "Fizik ve Çarpışma", Description = "Oyun içi etkileşimleri yönet.", Action = "Collider ve Rigidbody mantığını kullanarak mini bir 2D platform prototipi yap.", EstimatedTime = "1 Hafta" });
            }
            else
            {
                steps.Add(new RoadmapStep { Title = "Temel Kavramlar", Description = "Alanının ana başlıklarını öğren.", Action = "Kapsamlı bir yol haritası çıkar ve kaynak araştırması yap.", EstimatedTime = "1 Hafta" });
                steps.Add(new RoadmapStep { Title = "Uygulamalı Projeler", Description = "Öğrendiklerini pratiğe dök.", Action = "Küçük çaplı 2 proje geliştir.", EstimatedTime = "3 Hafta" });
            }

            // Final Step (Common)
            steps.Add(new RoadmapStep { Title = "Portföy ve Başvuru", Description = "Geliştirdiğin tüm projeleri sunulabilir hale getir.", Action = "GitHub'a yükle, README dosyalarını düzenle ve CV'ni güncelleyip hedefine başvur.", EstimatedTime = "Sürekli" });

            return steps;
        }
    }
}
