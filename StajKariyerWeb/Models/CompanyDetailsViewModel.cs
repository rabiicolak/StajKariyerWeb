
using System.Collections.Generic;

namespace StajKariyerWeb.Models
{
    public class CompanyDetailsViewModel
    {
        public CompanyProfile Company { get; set; } = new CompanyProfile();
        public List<SuitableStudentViewModel> SuitableStudents { get; set; } = new List<SuitableStudentViewModel>();
        public bool HasApplied { get; set; } = false;
    }
}
