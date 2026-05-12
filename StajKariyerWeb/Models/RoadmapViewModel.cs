using System.Collections.Generic;

namespace StajKariyerWeb.Models
{
    public class RoadmapViewModel
    {
        public string? TargetArea { get; set; }
        public string? TargetCompany { get; set; }
        public string? CurrentLevel { get; set; }
        public string? TargetRole { get; set; }

        public List<RoadmapStep> Steps { get; set; } = new List<RoadmapStep>();
        public string? ErrorMessage { get; set; }
    }

    public class RoadmapStep
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string EstimatedTime { get; set; } = string.Empty;
    }
}
