using System;
using System.ComponentModel.DataAnnotations;

namespace StajKariyerWeb.Models
{
    public class CompanyProfile
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string? Sector { get; set; }
        public string? RelatedArea { get; set; }
        public string? City { get; set; }
        
        [MaxLength(2000)]
        public string? Description { get; set; }
        
        public string? WebsiteUrl { get; set; }
        public string? LogoPath { get; set; }
        public string? RequiredSkills { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
