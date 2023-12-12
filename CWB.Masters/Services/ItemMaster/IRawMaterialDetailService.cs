using CWB.Masters.ViewModels.ItemMaster;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.Masters.Services.ItemMaster
{
    public interface IRawMaterialDetailService
    {
        Task<RawMaterialDetailVM> RawMaterialDetail(RawMaterialDetailVM rawMaterialDetailVM);
        IEnumerable<RawMaterialDetailListVM> GetRawMaterialDetailsByTenant(long tenantID);
    }
}
