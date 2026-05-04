using Microsoft.AspNetCore.Identity;

namespace StajKariyerWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? Department { get; set; }
        public string? University { get; set; }
        public string? ShortDescription { get; set; }
        public string? ProfilePhotoPath { get; set; }
        public string? CVPath { get; set; }
    }
}
