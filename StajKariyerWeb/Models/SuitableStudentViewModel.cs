using System.Collections.Generic;

namespace StajKariyerWeb.Models
{
    public class SuitableStudentViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? University { get; set; }
        public string? Department { get; set; }
        public string? City { get; set; }
        public string? ShortDescription { get; set; }
        public string? CVPath { get; set; }
        public int Score { get; set; }
        public List<string> MatchedFeatures { get; set; } = new List<string>();
        public List<string> MissingSkills { get; set; } = new List<string>();
        public string? Insight { get; set; }
    }
}
