using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace StajKariyerWeb.Models
{
    public class EditCompanyProfileViewModel
    {
        [Required(ErrorMessage = "Firma adı zorunludur.")]
        [Display(Name = "Firma Adı")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Sektör")]
        public string? Sector { get; set; }

        [Display(Name = "İlgili Alan (Örn: Web, Veri Bilimi)")]
        public string? RelatedArea { get; set; }

        [Display(Name = "Şehir")]
        public string? City { get; set; }

        [Display(Name = "Firma Hakkında Kısa Tanıtım")]
        public string? Description { get; set; }

        [Display(Name = "Web Sitesi")]
        public string? WebsiteUrl { get; set; }

        [Display(Name = "Aranan Yetkinlikler (Virgülle ayırın)")]
        public string? RequiredSkills { get; set; }

        [Display(Name = "Firma Logosu (Opsiyonel)")]
        public IFormFile? Logo { get; set; }

        public string? ExistingLogoPath { get; set; }
    }
}
