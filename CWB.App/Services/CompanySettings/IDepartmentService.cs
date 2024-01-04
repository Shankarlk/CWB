using CWB.App.Models.Departments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.App.Services.CompanySettings
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentListVM>> GetDepartments(long Id);
        Task<ShopDepartmentVM> PostDepartment(ShopDepartmentVM shop);
    }
}
