using CWB.App.Models.Departments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.App.Services.CompanySettings
{
    public interface IDepartmentService
    {
        Task<bool> DelDepartment(int departmentId);
        Task<IEnumerable<ShopDepartmentVM>> GetDepartments(long Id);
        Task<ShopDepartmentVM> PostDepartment(ShopDepartmentVM shop);
        Task<SectionsVM> PostSections(SectionsVM shop);
        Task<IEnumerable<SectionsVM>> GetSections();
        Task<bool> CheckSection(string city);
    }
}
