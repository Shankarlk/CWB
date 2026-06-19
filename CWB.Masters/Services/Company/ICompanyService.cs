using CWB.Masters.ViewModels.Company;
using CWB.Masters.ViewModels.ItemMaster;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CWB.Masters.Services.Company
{
    public interface ICompanyService
    {
        IEnumerable<CompanyTypeVM> GetCompanyTypes();
       

        Task<IEnumerable<CompaniesVM>> GetCompaniesByTenant(long tenantID);
        Task<IEnumerable<CompanyVM>> GetCompaniesByTenantGro(long tenantID);
        Task<IEnumerable<CompaniesVM>> GetCompaniesByCompanyNTenant(long companyID, long tenantID);
        Task<CompanyVM> Company(CompanyVM companyVM);
        Task<CompanyVM> PostGroCompany(CompanyVM companyVM);
        Task<bool> DeleteCompany(long companyID, long tenantId);
        Task<bool> DeleteDivision(long divisionID, long tenantId);
        bool CheckIfCompanyExisit(CheckCompanyVM checkCompanyVM);
        bool CheckIfDivisionExisit(CheckDivisionVM checkDivisionVM);
        Task<CompanyVM> GetCompany(long companyID,long tenantId);
        Task<long> GetCompanyId(string co);
        Task<CompaniesVM> GetCompanyByName(string companyName, long tenantId);

    }
}
