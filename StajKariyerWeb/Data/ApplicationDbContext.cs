using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StajKariyerWeb.Models;
using System;

namespace StajKariyerWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PredictionHistory> PredictionHistories { get; set; }
        public DbSet<CompanyProfile> CompanyProfiles { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CompanyProfile>().HasData(
                new CompanyProfile 
                { 
                    Id = 1, 
                    Name = "DeepMindTR", 
                    Sector = "Teknoloji", 
                    RelatedArea = "Yapay Zeka", 
                    City = "İstanbul", 
                    Description = "Yapay zeka ve derin öğrenme modelleri geliştiren yenilikçi teknoloji firması.",
                    WebsiteUrl = "https://example.com/deepmindtr",
                    RequiredSkills = "Python, TensorFlow, PyTorch, Makine Öğrenmesi",
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new CompanyProfile 
                { 
                    Id = 2, 
                    Name = "DataSense", 
                    Sector = "Veri Analitiği", 
                    RelatedArea = "Veri Bilimi", 
                    City = "Ankara", 
                    Description = "Büyük veri analizi, iş zekası ve veri madenciliği çözümleri sunar.",
                    WebsiteUrl = "https://example.com/datasense",
                    RequiredSkills = "SQL, Python, Power BI, Veri Modelleme",
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new CompanyProfile 
                { 
                    Id = 3, 
                    Name = "WebSoft", 
                    Sector = "Yazılım Geliştirme", 
                    RelatedArea = "Web", 
                    City = "İzmir", 
                    Description = "Modern, hızlı ve ölçeklenebilir web uygulamaları ve e-ticaret altyapıları tasarlar.",
                    WebsiteUrl = "https://example.com/websoft",
                    RequiredSkills = "HTML/CSS, JavaScript, React, ASP.NET Core",
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new CompanyProfile 
                { 
                    Id = 4, 
                    Name = "BackendPro", 
                    Sector = "Finans & Teknoloji", 
                    RelatedArea = "Backend", 
                    City = "İstanbul", 
                    Description = "Yüksek trafikli sistemler için güvenli, performanslı ve sağlam API altyapıları oluşturur.",
                    WebsiteUrl = "https://example.com/backendpro",
                    RequiredSkills = "C#, Java, SQL Server, Microservices, Docker",
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new CompanyProfile 
                { 
                    Id = 5, 
                    Name = "CyberSec", 
                    Sector = "Siber Güvenlik", 
                    RelatedArea = "Siber Güvenlik", 
                    City = "Ankara", 
                    Description = "Kurumsal sızma testleri, ağ güvenliği ve güvenlik danışmanlığı hizmeti verir.",
                    WebsiteUrl = "https://example.com/cybersec",
                    RequiredSkills = "Ağ Güvenliği, Sızma Testi, Linux, OWASP",
                    CreatedAt = new DateTime(2026, 1, 1)
                }
            );
        }
    }
}