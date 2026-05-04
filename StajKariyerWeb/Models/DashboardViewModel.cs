using System;

namespace StajKariyerWeb.Models
{
    public class DashboardViewModel
    {
        public int TotalPredictions { get; set; }
        public DateTime? LastPredictionDate { get; set; }
        public string? MostRecommendedCareer { get; set; }
        public int ProfileCompletionPercentage { get; set; }
    }
}
