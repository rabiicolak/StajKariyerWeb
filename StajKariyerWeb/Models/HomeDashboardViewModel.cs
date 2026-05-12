using System;
using System.Collections.Generic;

namespace StajKariyerWeb.Models
{
    public class HomeDashboardViewModel
    {
        public string? FullName { get; set; }
        public int TotalPredictions { get; set; }
        public int ProfileCompletionPercentage { get; set; }
        public string? MostRecommendedCareer { get; set; }
        public DateTime? LastPredictionDate { get; set; }
        public List<ApplicationUser> RecommendedMembers { get; set; } = new List<ApplicationUser>();
    }
}
