using System.Collections.Generic;

namespace StajKariyerWeb.Models
{
    public class CompanyDashboardViewModel
    {
        public CompanyProfile Company { get; set; } = new CompanyProfile();
        public List<SuitableStudentViewModel> SuitableStudents { get; set; } = new List<SuitableStudentViewModel>();
        public List<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
        
        // Stats
        public int TotalSuitableStudents { get; set; }
        public int HighestMatchScore { get; set; }
        public double AverageMatchScore { get; set; }
        public string? MostMatchedSkill { get; set; }
        public string? MostActiveCity { get; set; }

        // Filters
        public string? FilterCity { get; set; }
        public string? FilterDepartment { get; set; }
        public string? FilterRelatedArea { get; set; }
        public int? FilterMinScore { get; set; }
    }
}
