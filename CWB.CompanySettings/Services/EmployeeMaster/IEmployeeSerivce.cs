using CWB.CompanySettings.ViewModels.EmployeeMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Services.EmployeeMaster
{
    public interface IEmployeeSerivce
    {
        Task<IEnumerable<UiListVM>> GetAllUiList(long TenantId);
        Task<UiListVM> PostUilist(UiListVM uiListVM);
        //bool CheckDesignationExisit(CheckDesignationVM checkDesignationVM);
        Task<bool> DelUiList(long designationId);
        Task<Org_ChartVM> PostOrgChart(Org_ChartVM uiListVM);
        IEnumerable<Org_ChartVM> GetAllOrgChart(long TenantId);
        Task<bool> DelOrgChart(long designationId);
        Task<Role_ListVM> PostRoleList(Role_ListVM uiListVM);
        IEnumerable<Role_ListVM> GetAllRoleList(long TenantId);
        Task<bool> DelRoleList(long designationId);
        Task<Role_UI_ListVM> PostRoleUiList(Role_UI_ListVM uiListVM);
        IEnumerable<Role_UI_ListVM> GetAllRoleUiList(long TenantId);
        Task<bool> DelRoleUiList(long designationId);
        Task<Empl_Role_ListVM> PostEmplRoleList(Empl_Role_ListVM uiListVM);
        IEnumerable<Empl_Role_ListVM> GetAllEmplRoleList(long TenantId);
        Task<bool> DelEmplRoleList(long designationId);
        Task<Permission_ListVM> PostPermissionList(Permission_ListVM uiListVM);
        Task<IEnumerable<Permission_ListVM>> GetAllPermissionListAsync(long TenantId);
        Task<bool> DelPermissionList(long designationId);
        Task<EmployeeVM> PostEmployee(EmployeeVM uiListVM);
        IEnumerable<EmployeeVM> GetAllEmployee(long TenantId);
        Task<bool> DelEmployee(long designationId);





    }
}
