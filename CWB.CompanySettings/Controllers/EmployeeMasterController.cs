using CWB.CommonUtils.Common;
using CWB.CompanySettings.CompanySettingsUtils;
using CWB.CompanySettings.Services.EmployeeMaster;
using CWB.CompanySettings.ViewModels.EmployeeMaster;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Controllers
{
    [ApiController]
    [Authorize]
    public class EmployeeMasterController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IEmployeeSerivce _employeeSerivce;
        public EmployeeMasterController(ILoggerManager logger, IEmployeeSerivce employeeSerivce)
        {
            _logger = logger;
            _employeeSerivce = employeeSerivce;
        }
        [HttpGet]
        [Route(ApiRoutes.EmployeeMaster.GetAllUiList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<UiListVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllUiListAsync(long tenantId)
        {
            var designations = await _employeeSerivce.GetAllUiList(tenantId);
            return Ok(designations);
        }
        [HttpPost]
        [Route(ApiRoutes.EmployeeMaster.PostUilist)]
        [Produces(AppContentTypes.ContentType, Type = typeof(UiListVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostUilist([FromBody] UiListVM designationVM)
        {
            var result = await _employeeSerivce.PostUilist(designationVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.EmployeeMaster.DelUiList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult DelUiList(long designationId)
        {
            var docTypes = _employeeSerivce.DelUiList(designationId);
            return Ok(docTypes);
        }
        [HttpGet]
        [Route(ApiRoutes.EmployeeMaster.GetAllOrgChart)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Org_ChartVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult GetAllOrgChart(long tenantId)
        {
            var designations = _employeeSerivce.GetAllOrgChart(tenantId);
            return Ok(designations);
        }
        [HttpPost]
        [Route(ApiRoutes.EmployeeMaster.PostOrgChart)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Org_ChartVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostOrgChart([FromBody] Org_ChartVM designationVM)
        {
            var result = await _employeeSerivce.PostOrgChart(designationVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.EmployeeMaster.DelOrgChart)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult DelOrgChart(long designationId)
        {
            var docTypes = _employeeSerivce.DelOrgChart(designationId);
            return Ok(docTypes);
        }
        [HttpGet]
        [Route(ApiRoutes.EmployeeMaster.GetAllRoleList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Role_ListVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult GetAllRoleList(long tenantId)
        {
            var designations = _employeeSerivce.GetAllRoleList(tenantId);
            return Ok(designations);
        }
        [HttpPost]
        [Route(ApiRoutes.EmployeeMaster.PostRoleList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Org_ChartVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostRoleList([FromBody] Role_ListVM designationVM)
        {
            var result = await _employeeSerivce.PostRoleList(designationVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.EmployeeMaster.DelRoleList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult DelRoleList(long designationId)
        {
            var docTypes = _employeeSerivce.DelRoleList(designationId);
            return Ok(docTypes);
        }
        [HttpGet]
        [Route(ApiRoutes.EmployeeMaster.GetAllRoleUiList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Role_UI_ListVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult GetAllRoleUiList(long tenantId)
        {
            var designations = _employeeSerivce.GetAllRoleUiList(tenantId);
            return Ok(designations);
        }
        [HttpPost]
        [Route(ApiRoutes.EmployeeMaster.PostRoleUiList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Role_UI_ListVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostRoleUiList([FromBody] Role_UI_ListVM designationVM)
        {
            var result = await _employeeSerivce.PostRoleUiList(designationVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.EmployeeMaster.DelRoleUiList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult DelRoleUiList(long designationId)
        {
            var docTypes = _employeeSerivce.DelRoleUiList(designationId);
            return Ok(docTypes);
        }
        [HttpGet]
        [Route(ApiRoutes.EmployeeMaster.GetAllEmplRoleList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Empl_Role_ListVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult GetAllEmplRoleList(long tenantId)
        {
            var designations = _employeeSerivce.GetAllEmplRoleList(tenantId);
            return Ok(designations);
        }
        [HttpPost]
        [Route(ApiRoutes.EmployeeMaster.PostEmplRoleList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Empl_Role_ListVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostEmplRoleList([FromBody] Empl_Role_ListVM designationVM)
        {
            var result = await _employeeSerivce.PostEmplRoleList(designationVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.EmployeeMaster.DelEmplRoleList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult DelEmplRoleList(long designationId)
        {
            var docTypes = _employeeSerivce.DelEmplRoleList(designationId);
            return Ok(docTypes);
        }
        [HttpGet]
        [Route(ApiRoutes.EmployeeMaster.GetAllPermissionList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Permission_ListVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllPermissionListAsync(long tenantId)
        {
            var designations = await _employeeSerivce.GetAllPermissionListAsync(tenantId);
            return Ok(designations);
        }
        [HttpPost]
        [Route(ApiRoutes.EmployeeMaster.PostPermissionList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Permission_ListVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostPermissionList([FromBody] Permission_ListVM designationVM)
        {
            var result = await _employeeSerivce.PostPermissionList(designationVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.EmployeeMaster.DelPermissionList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult DelPermissionList(long designationId)
        {
            var docTypes = _employeeSerivce.DelPermissionList(designationId);
            return Ok(docTypes);
        }
        [HttpGet]
        [Route(ApiRoutes.EmployeeMaster.GetAllEmployee)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<EmployeeVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult GetAllEmployee(long tenantId)
        {
            var designations = _employeeSerivce.GetAllEmployee(tenantId);
            return Ok(designations);
        }
        [HttpPost]
        [Route(ApiRoutes.EmployeeMaster.PostEmployee)]
        [Produces(AppContentTypes.ContentType, Type = typeof(EmployeeVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostEmployee([FromBody] EmployeeVM designationVM)
        {
            var result = await _employeeSerivce.PostEmployee(designationVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.EmployeeMaster.DelEmployee)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult DelEmployee(long designationId)
        {
            var docTypes = _employeeSerivce.DelEmployee(designationId);
            return Ok(docTypes);
        }
    }
}
