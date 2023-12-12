using CWB.Masters.ViewModels.Company;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.Masters.Services.Company
{
    public interface ICompanyService
    {
        IEnumerable<CompanyTypeVM> GetCompanyTypes();
        Task<IEnumerable<CompaniesVM>> GetCompaniesByTenant(long tenantID);

        Task<IEnumerable<CompaniesVM>> GetCompaniesByCompanyNTenant(long companyID, long tenantID);
        Task<CompanyVM> Company(CompanyVM companyVM);
        bool CheckIfCompanyExisit(CheckCompanyVM checkCompanyVM);
        bool CheckIfDivisionExisit(CheckDivisionVM checkDivisionVM);
    }
}
