using CWB.CommonUtils.Common;
using CWB.CompanySettings.CompanySettingsUtils;
using CWB.CompanySettings.Services.Location;
using CWB.CompanySettings.ViewModels.Location;
using CWB.CompanySettings.ViewModelValidators.Location;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Controllers
{
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IDepartmentService _departmentService;

        public DepartmentController(ILoggerManager logger, IDepartmentService departmentService)
        {
            _logger = logger;
            _departmentService = departmentService;
        }

        /// <summary>
        /// Get Departments by Plant Id and Tenant Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.Department.GetDepartments)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<ShopDepartmentVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetDepartments(long Id, long TenantId)
        {
            var departments = await _departmentService.GetAllDepartments(Id, TenantId);
            return Ok(departments);
        }

        /// <summary>
        /// get departmets with plants
        /// </summary>
        /// <param name="departmentPlantVM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.Department.GetDepartmentsWithPlants)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<ShopDepartmentVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult GetDepartmentsWithPlant(DepartmentPlantVM departmentPlantVM)
        {
            var departments = _departmentService.GetDepartmentListWithPlants(departmentPlantVM.DepartmentIds, departmentPlantVM.TenantId);
            return Ok(departments);
        }


        /// <summary>
        /// Add or Edit Department
        /// </summary>
        /// <param name="shopDepartmentVM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.Department.PostDepartment)]
        [Produces(AppContentTypes.ContentType, Type = typeof(ShopDepartmentVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostPlant([FromBody] ShopDepartmentVM shopDepartmentVM)
        {
            var validator = new DepartmentVMValidator();
            var validationResult = await validator.ValidateAsync(shopDepartmentVM);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            //check if duplicate
            var plantExist = _departmentService.CheckDepartmentExisit(new CheckDepartmentVM
            {
                PlantId = shopDepartmentVM.PlantId,
                Name = shopDepartmentVM.Name,
                TenantId = shopDepartmentVM.TenantId,
                DepartmentId = shopDepartmentVM.DepartmentId
            });
            if (plantExist)
            {
                ModelState.AddModelError("Name", $"Department: {shopDepartmentVM.Name} Already Exist");
                return BadRequest(ModelState);
            }
            var result = await _departmentService.Department(shopDepartmentVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Department.GetStoresIDs)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Stores_Dept_IDListVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetStoresIDs()
        {
            // var companyTypes = _plantService.GetPlants(tenantId);
            var storesid =await  _departmentService.GetStoresIDs();
            return Ok(storesid);
        }
        /// <summary>
        /// Check if Department Exist
        /// </summary>
        /// <param name="checkDepartmentVM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.Department.CheckDepartment)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> CheckDepartment([FromBody] CheckDepartmentVM checkDepartmentVM)
        {
            var validator = new CheckDepartmentVMValidator();
            var validationResult = await validator.ValidateAsync(checkDepartmentVM);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            //check if duplicate
            var departmentExist = _departmentService.CheckDepartmentExisit(checkDepartmentVM);
            return Ok(departmentExist);
        }

        [HttpGet]
        [Route(ApiRoutes.Department.DelDepartment)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult DelDepartment(long departmentId)
        {
            var docTypes = _departmentService.DelDepartment(departmentId);
            return Ok(docTypes);
        }

        [HttpGet]
        [Route(ApiRoutes.Department.GetDept_Role_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<PlantVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetDept_Role_List(long tenantId)
        {
            // var companyTypes = _plantService.GetPlants(tenantId);
            var plants = _departmentService.GetDept_Role_List(tenantId);
            return Ok(plants);
        }
        [HttpPost]
        [Route(ApiRoutes.Department.PostDept_Role_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Dept_Role_ListVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostDept_Role_List([FromBody] Dept_Role_ListVM cityVM)
        {
            var result = await _departmentService.PostDept_Role_List(cityVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Department.DelDept_Role_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult DelDept_Role_List(long sectionId)
        {
            var docTypes = _departmentService.DelDept_Role_List(sectionId);
            return Ok(docTypes);
        }
        [HttpGet]
        [Route(ApiRoutes.Department.GetEmployee_Pwd)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<PlantVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetEmployee_Pwd(long tenantId)
        {
            // var companyTypes = _plantService.GetPlants(tenantId);
            var plants = _departmentService.GetEmployee_Pwd(tenantId);
            return Ok(plants);
        }
        [HttpPost]
        [Route(ApiRoutes.Department.PostEmployee_Pwd)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Employee_PwdVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostEmployee_Pwd([FromBody] Employee_PwdVM cityVM)
        {
            var result = await _departmentService.PostEmployee_Pwd(cityVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Department.DelEmployee_Pwd)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult DelEmployee_Pwd(long sectionId)
        {
            var docTypes = _departmentService.DelEmployee_Pwd(sectionId);
            return Ok(docTypes);
        }
        [HttpGet]
        [Route(ApiRoutes.Department.GetDept_Employee)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<PlantVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetDept_Employee(long tenantId)
        {
            // var companyTypes = _plantService.GetPlants(tenantId);
            var plants = _departmentService.GetDept_Employee(tenantId);
            return Ok(plants);
        }
        [HttpPost]
        [Route(ApiRoutes.Department.PostDept_Employee)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Dept_EmployeeVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostDept_Employee([FromBody] Dept_EmployeeVM cityVM)
        {
            var result = await _departmentService.PostDept_Employee(cityVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Department.DelDept_Employee)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult DelDept_Employee(long sectionId)
        {
            var docTypes = _departmentService.DelDept_Employee(sectionId);
            return Ok(docTypes);
        }
        [HttpGet]
        [Route(ApiRoutes.Department.GetEmployee_UI_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<PlantVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetEmployee_UI_List(long tenantId)
        {
            // var companyTypes = _plantService.GetPlants(tenantId);
            var plants = _departmentService.GetEmployee_UI_List(tenantId);
            return Ok(plants);
        }
        [HttpPost]
        [Route(ApiRoutes.Department.PostEmployee_UI_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Employee_UI_ListVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostEmployee_UI_List([FromBody] Employee_UI_ListVM cityVM)
        {
            var result = await _departmentService.PostEmployee_UI_List(cityVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Department.DelEmployee_UI_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult DelEmployee_UI_List(long sectionId)
        {
            var docTypes = _departmentService.DelEmployee_UI_List(sectionId);
            return Ok(docTypes);
        }
    }
}
