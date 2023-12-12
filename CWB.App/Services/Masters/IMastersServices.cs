using CWB.App.Models.Contacts;
using CWB.App.Models.ItemMaster;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.App.Services.Masters
{
    public interface IMastersServices
    {
        Task<IEnumerable<CompanyTypeVM>> GetCompanyTypes();
        Task<IEnumerable<ContactsVM>> GetCompanies();

        Task<IEnumerable<UOMVM>> GetUOMs(long tenantId);


        Task<IEnumerable<ManufacturedPartNoDetailVM>> GetManufacturedPartNoDetailList(long ManufPartType, string companyName);
        Task<String> HelloWorld();
        

        Task<IEnumerable<ContactsVM>> GetDivisionsByCompanyId(long Id);
        Task<CompanyVM> Company(CompanyVM companyVM);
        Task<bool> CheckIfCompanyExisit(long CompanyId, string CompanyName);
        Task<bool> CheckIfDivisionExisit(long CompanyId, long DivisionId, string DivisionName);
        Task<ManufacturedPartNoDetailVM> ManufacturedPartNoDetail(ManufacturedPartNoDetailVM manufacturedPartNoDetailVM);
        Task<ManufacturedPartNoDetailVM> MPMakeFrom(ManufacturedPartNoDetailVM manufacturedPartNoDetailVM);
        Task<ManufacturedPartNoDetailVM> MPBOM(ManufacturedPartNoDetailVM manufacturedPartNoDetailVM);
        Task<IEnumerable<MPMakeFromListVM>> GetMPMakeFromListBypartNumber(string partNumber);
        Task<RawMaterialDetailVM> RawMaterialDetail(RawMaterialDetailVM rawMaterialDetailVM);
        Task<BoughtOutFinishDetailVM> BoughtOutFinishDetail(BoughtOutFinishDetailVM boughtOutFinishDetailVM);
    }
}
