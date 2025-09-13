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
        Task<bool> CheckSection(string city);
        Task<SectionsVM> PostSections(SectionsVM shop);
        Task<IEnumerable<SectionsVM>> GetSections();
        Task<bool> DelSections(long designationId);
        Task<Dept_EmployeeVM> PostDept_Employee(Dept_EmployeeVM shop);
        Task<IEnumerable<Dept_EmployeeVM>> GetDept_Employee();
        Task<bool> DelDept_Employee(long designationId);
        Task<Employee_UI_ListVM> PostEmployee_UI_List(Employee_UI_ListVM shop);
        Task<IEnumerable<Employee_UI_ListVM>> GetEmployee_UI_List();
        Task<bool> DelEmployee_UI_List(long designationId);
        Task<Dept_Role_ListVM> PostDept_Role_List(Dept_Role_ListVM shop);
        Task<IEnumerable<Dept_Role_ListVM>> GetDept_Role_List();
        Task<bool> DelDept_Role_List(long designationId);
        Task<Employee_PwdVM> PostEmployee_Pwd(Employee_PwdVM shop);
        Task<IEnumerable<Employee_PwdVM>> GetEmployee_Pwd();
        Task<bool> DelEmployee_Pwd(long designationId);
    }
}
