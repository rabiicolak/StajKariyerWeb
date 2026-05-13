using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StajKariyerWeb.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        public int CompanyProfileId { get; set; }

        [Required]
        public string StudentId { get; set; } = string.Empty;

        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [Required]
        public string Status { get; set; } = "Beklemede"; // Beklemede, İncelendi, Kabul, Reddedildi

        public string? Note { get; set; }

        [ForeignKey("CompanyProfileId")]
        public virtual CompanyProfile? CompanyProfile { get; set; }

        [ForeignKey("StudentId")]
        public virtual ApplicationUser? Student { get; set; }
    }
}
