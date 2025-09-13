using CWB.CompanySettings.ViewModels.Location;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Services.Location
{
    public interface IDepartmentService
    {
        Task<IEnumerable<ShopDepartmentVM>> GetAllDepartments(long PlantId, long TenantId);
        Task<ShopDepartmentVM> Department(ShopDepartmentVM shopDepartmentVM);
        bool CheckDepartmentExisit(CheckDepartmentVM checkDepartmentVM);
        IEnumerable<ShopDepartmentVM> GetDepartmentListWithPlants(List<long> DepartmentIds, long TenantId);
        Task<bool> DelDepartment(long departmentId);
        IEnumerable<Dept_Role_ListVM> GetDept_Role_List(long TenantId);
        Task<Dept_Role_ListVM> PostDept_Role_List(Dept_Role_ListVM plantWdVM);
        Task<bool> DelDept_Role_List(long designationId);
        IEnumerable<Employee_PwdVM> GetEmployee_Pwd(long TenantId);
        Task<Employee_PwdVM> PostEmployee_Pwd(Employee_PwdVM plantWdVM);
        Task<bool> DelEmployee_Pwd(long designationId);
        IEnumerable<Dept_EmployeeVM> GetDept_Employee(long TenantId);
        Task<Dept_EmployeeVM> PostDept_Employee(Dept_EmployeeVM plantWdVM);
        Task<bool> DelDept_Employee(long designationId);
        IEnumerable<Employee_UI_ListVM> GetEmployee_UI_List(long TenantId);
        Task<Employee_UI_ListVM> PostEmployee_UI_List(Employee_UI_ListVM plantWdVM);
        Task<bool> DelEmployee_UI_List(long designationId);

    }
}
