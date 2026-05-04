using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace StajKariyerWeb.Models
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Ad Soyad alanı zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Telefon")]
        public string? Phone { get; set; }

        [Display(Name = "Şehir")]
        public string? City { get; set; }

        [Display(Name = "Bölüm")]
        public string? Department { get; set; }

        [Display(Name = "Üniversite")]
        public string? University { get; set; }

        [Display(Name = "Kısa Açıklama")]
        public string? ShortDescription { get; set; }

        // Mevcut dosya yolları (görüntüleme için)
        public string? ExistingProfilePhotoPath { get; set; }
        public string? ExistingCVPath { get; set; }

        // Yeni yüklenecek dosyalar
        [Display(Name = "Profil Fotoğrafı")]
        public IFormFile? ProfilePhoto { get; set; }

        [Display(Name = "CV Dosyası")]
        public IFormFile? CVFile { get; set; }
    }
}
