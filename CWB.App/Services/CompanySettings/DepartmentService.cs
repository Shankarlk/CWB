using CWB.App.AppUtils;
using CWB.App.Models.Departments;
using CWB.CommonUtils.Common;
using CWB.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.App.Services.CompanySettings
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ILoggerManager _logger;
        private readonly ApiUrls _apiUrls;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly long tenantId;

        public DepartmentService(ILoggerManager logger, ApiUrls apiUrlsOptions, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _apiUrls = apiUrlsOptions;
            _httpContextAccessor = httpContextAccessor;
            tenantId = long.Parse(AppUtil.GetTenantId(_httpContextAccessor.HttpContext.User));
        }

        public async Task<bool> DelDepartment(int departmentId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/deldepartment/{departmentId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<ShopDepartmentVM>> GetDepartments(long Id)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/departments/{Id}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<ShopDepartmentVM>>.GetAsync(uri, headers);
        }
        public async Task<Stores_Dept_IDListVM> GetAllStoresIDs()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/getallstoresid/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<Stores_Dept_IDListVM>.GetAsync(uri, headers);
        }
        public async Task<ShopDepartmentVM> PostDepartment(ShopDepartmentVM shop)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/department");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            shop.TenantId = tenantId;
            return await RestHelper<ShopDepartmentVM>.PostAsync(uri,shop, headers);
        }
        public async Task<SectionsVM> PostSections(SectionsVM shop)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/postsection");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            shop.TenantId = tenantId;
            return await RestHelper<SectionsVM>.PostAsync(uri,shop, headers);
        }
        public async Task<IEnumerable<SectionsVM>> GetSections()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/getsections/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<SectionsVM>>.GetAsync(uri, headers);
        }
        public async Task<bool> DelSections(long designationId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/deletesection/{designationId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<Employee_PwdVM> PostEmployee_Pwd(Employee_PwdVM shop)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/postemployepwd");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            shop.TenantId = tenantId;
            return await RestHelper<Employee_PwdVM>.PostAsync(uri,shop, headers);
        }
        public async Task<IEnumerable<Employee_PwdVM>> GetEmployee_Pwd()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/getemployepwd/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Employee_PwdVM>>.GetAsync(uri, headers);
        }
        public async Task<bool> DelEmployee_Pwd(long designationId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/deleteemployepwd/{designationId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<Dept_Role_ListVM> PostDept_Role_List(Dept_Role_ListVM shop)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/postdeptrolelist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            shop.TenantId = tenantId;
            return await RestHelper<Dept_Role_ListVM>.PostAsync(uri,shop, headers);
        }
        public async Task<IEnumerable<Dept_Role_ListVM>> GetDept_Role_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/getdeptrolelist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Dept_Role_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<bool> DelDept_Role_List(long designationId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/deletedeptrolelist/{designationId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<Dept_EmployeeVM> PostDept_Employee(Dept_EmployeeVM shop)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/postdeptemp");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            shop.TenantId = tenantId;
            return await RestHelper<Dept_EmployeeVM>.PostAsync(uri,shop, headers);
        }
        public async Task<IEnumerable<Dept_EmployeeVM>> GetDept_Employee()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/getdeptemp/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Dept_EmployeeVM>>.GetAsync(uri, headers);
        }
        public async Task<bool> DelDept_Employee(long designationId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/deletedeptemp/{designationId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<Employee_UI_ListVM> PostEmployee_UI_List(Employee_UI_ListVM shop)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/postempuilist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            shop.TenantId = tenantId;
            return await RestHelper<Employee_UI_ListVM>.PostAsync(uri,shop, headers);
        }
        public async Task<IEnumerable<Employee_UI_ListVM>> GetEmployee_UI_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/getempuilist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Employee_UI_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<bool> DelEmployee_UI_List(long designationId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/deleteempuilist/{designationId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<bool> CheckSection(string city)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/checksection/{city}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
    }
}
