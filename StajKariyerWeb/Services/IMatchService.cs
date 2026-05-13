using StajKariyerWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StajKariyerWeb.Services
{
    public interface IMatchService
    {
        Task<SuitableStudentViewModel> CalculateMatchScoreAsync(CompanyProfile company, ApplicationUser student);
    }
}
