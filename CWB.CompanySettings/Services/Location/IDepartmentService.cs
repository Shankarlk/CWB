using CWB.CompanySettings.ViewModels.Location;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Services.Location
{
    public interface IDepartmentService
    {
        Task<IEnumerable<ShopDepartmentListVM>> GetAllDepartments(long PlantId, long TenantId);
        Task<ShopDepartmentVM> Department(ShopDepartmentVM shopDepartmentVM);
        bool CheckDepartmentExisit(CheckDepartmentVM checkDepartmentVM);
        IEnumerable<DepartmentListWithPlantVM> GetDepartmentListWithPlants(List<long> DepartmentIds, long TenantId);
    }
}
