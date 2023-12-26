using CWB.Masters.ViewModels.ItemMaster;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.Masters.Services.ItemMaster
{
    public interface IManufacturedPartNoDetailService
    {
        Task<ManufacturedPartNoDetailVM> ManufacturedPartNoDetail(ManufacturedPartNoDetailVM manufacturedPartNoDetailVM);
        IEnumerable<ManufacturedPartNoDetailVM> GetManufacturedPartNoDetailsByTypeTenant(long manPartTypeId, string companyName, long tenantID);

        IEnumerable<ManufacturedPartNoDetailVM> GetAllManufacturedPartNoDetailsByTypeTenant(long tenantID);

        Task<MPMakeFromVM> MPMakeFrom(MPMakeFromVM manufacturedPartNoDetailVM);
        IEnumerable<MPMakeFromVM> GetMPMakeFromList(string manufPartId, long tenantID);
        Task<MPMakeFromVM> GetMPMakeFrom(long Id);
        Task<MPMakeFromVM> RemMakeFrom(MPMakeFromVM mPMakeFromListVM);



        Task<MPBOMVM> MPBOM(MPBOMVM manufacturedPartNoDetailVM);
        IEnumerable<MPBOMVM> GetMPBOMList(string manufPartId, long tenantID);
        Task<MPBOMVM> GetMPBOM(long Id);
        Task<MPBOMVM> RemBOM(MPBOMVM bomVM);

        IEnumerable<UOMVM> GetUOMsByTenantId(long tenantId);

        Task<UOMVM> UOM(UOMVM uOMVm);
        bool CheckPartNo(long partId);
        Task<ManufacturedPartNoDetailVM> GetManuPart(int partId, long tenantId);
    }
}
