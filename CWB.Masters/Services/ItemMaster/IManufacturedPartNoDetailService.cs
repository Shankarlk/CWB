using CWB.Masters.ViewModels.ItemMaster;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.Masters.Services.ItemMaster
{
    public interface IManufacturedPartNoDetailService
    {
        Task<ManufacturedPartNoDetailVM> ManufacturedPartNoDetail(ManufacturedPartNoDetailVM manufacturedPartNoDetailVM);
        IEnumerable<ManufacturedPartNoDetailListVM> GetManufacturedPartNoDetailsByTypeTenant(long manPartTypeId, string companyName);

        Task<ManufacturedPartNoDetailVM> MPMakeFrom(ManufacturedPartNoDetailVM manufacturedPartNoDetailVM);
        IEnumerable<MPMakeFromListVM> GetMPMakeFromListByPartNumberNTenant(string partNumber, long tenantID);
        Task<ManufacturedPartNoDetailVM> MPBOM(ManufacturedPartNoDetailVM manufacturedPartNoDetailVM);
        IEnumerable<UOMVM> GetUOMsByTenantId(long tenantId);
    }
}
