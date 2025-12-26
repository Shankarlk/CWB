using CWB.App.Models.Departments;
using CWB.App.Services.CompanySettings;
using CWB.App.Services.EmployeeMaster;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class DepartmentController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeeService _employeeService;

        public DepartmentController(ILoggerManager logger, IDepartmentService departmentService, IEmployeeService employeeService)
        {
            _logger = logger;
            _departmentService = departmentService;
            _employeeService = employeeService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetDepartments()
        {
            var result = await _departmentService.GetDepartments(1);
            var sections = await _departmentService.GetSections();
            foreach (var item in result)
            {
                var sectionNames = sections
                    .Where(s => s.ShopDepartmentId == item.DepartmentId)
                    .Select(s => s.Name);

                item.Section = string.Join(", ", sectionNames);
            }
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> GetDepartmentsLevel()
        {
            var result = await _departmentService.GetDepartments(1);
            var deptRole = await _departmentService.GetDept_Role_List();
            var roles = await _employeeService.GetAllRoleList();

            var deptDict = result.ToDictionary(d => d.DepartmentId);

            var finalList = new List<ShopDepartmentVM>();

            foreach (var dept in result)
            {
                var deptRol = deptRole.Where(r => r.Dept_Struct_Id == dept.DepartmentId).ToList();
                var roleNames = (from dr in deptRol
                                 join r in roles on dr.Role_Access_Id equals r.Role_ListId
                                 select r.Role_Desc).ToList();
                var vm = new ShopDepartmentVM
                {
                    DepartmentId = dept.DepartmentId,
                    Name = dept.Name,
                    NoOfShifts = dept.NoOfShifts,
                    PlantId = dept.PlantId,
                    Level_No = dept.Level_No,
                    Part_Of = dept.Part_Of,
                    Activity = dept.Activity,
                    ProdDept = dept.ProdDept,
                    TenantId = dept.TenantId,
                    PlantName = dept.PlantName,
                    Section = dept.Section,
                    RoleName = string.Join(", ", roleNames)
                };

                // Walk up the parent chain and fill levels
                var chain = new List<string>();
                var current = dept;
                while (current != null)
                {
                    chain.Insert(0, current.Name); // prepend
                    if (current.Part_Of == 0 || !deptDict.ContainsKey(current.Part_Of))
                        break;

                    current = deptDict[current.Part_Of];
                }

                // Assign to Level1..Level5
                if (chain.Count > 0) vm.Level1 = chain[0];
                if (chain.Count > 1) vm.Level2 = chain[1];
                if (chain.Count > 2) vm.Level3 = chain[2];
                if (chain.Count > 3) vm.Level4 = chain[3];
                if (chain.Count > 4) vm.Level5 = chain[4];

                finalList.Add(vm);
            }

            return Json(finalList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostDepartment(ShopDepartmentVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(model.ProdDept == null)
            {
                model.ProdDept = false;
            }
            if (model.NoOfShifts == 0)
            {
                model.NoOfShifts= 1;
            }
            var result = await _departmentService.PostDepartment(model);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> deldept(int departmentId)
        {
            var result = await _departmentService.DelDepartment(departmentId);
            return Ok(result);
        }

        [HttpGet]
        
        public async Task<IActionResult> TestDept(int departmentId)
        {
            var result = await _departmentService.DelDepartment(departmentId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostSection(SectionsVM model)
        {
            var result = await _departmentService.PostSections(model);
            return Ok(result);
        }
        [HttpGet]
        public async Task<JsonResult> CheckSection(string city)
        {
            var result = await _departmentService.CheckSection(city);
            return Json(!result);
        }
        [HttpGet]
        public async Task<JsonResult> GetSections()
        {
            var result = await _departmentService.GetSections();
            var departments = await _departmentService.GetDepartments(1);
            var deptLookup = departments.ToDictionary(d => d.DepartmentId, d => d);
            foreach (var item in result)
            {
                if (deptLookup.TryGetValue(item.ShopDepartmentId, out var sec))
                {
                    item.DeptName = sec.Name ?? string.Empty;
                }
                else
                {
                    item.DeptName = string.Empty;
                }
            }
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> GetDept_Employee()
        {
            // 1. Get all employee-department mappings
            var empDeptList = await _departmentService.GetDept_Employee();

            // 2. Get the full department hierarchy
            var allDepartments = await _departmentService.GetDepartments(1);

            // Convert departments to dictionary for fast lookup
            var deptDict = allDepartments.ToDictionary(d => d.DepartmentId);

            var finalList = new List<Dept_EmployeeVM>();

            foreach (var emp in empDeptList)
            {
                // Find department details for the employee's assigned department
                if (!deptDict.TryGetValue(emp.Dept_Posn, out var dept))
                {
                    // Skip if department not found
                    continue;
                }

                // Walk up the hierarchy to fill Level1..Level5
                var chain = new List<string>();
                var current = dept;

                while (current != null)
                {
                    chain.Insert(0, current.Name); // prepend department name
                    if (current.Part_Of == 0 || !deptDict.ContainsKey(current.Part_Of))
                        break;

                    current = deptDict[current.Part_Of];
                }
                // Build final view model
                var vm = new Dept_EmployeeVM
                {
                    Dept_EmployeeId = emp.Dept_EmployeeId,
                    Employee_Id = emp.Employee_Id,
                    Dept_Posn = emp.Dept_Posn,
                    Active = emp.Active,
                    Add_dateStr = emp.Add_date.ToString("dd-MM-yyyy"),
                    Deact_dateStr = emp.Deact_date != DateTime.MinValue
                                    ? emp.Deact_date.ToString("dd-MM-yyyy")
                                    : string.Empty,
                    Level1 = chain.Count > 0 ? chain[0] : "-",
                    Level2 = chain.Count > 1 ? chain[1] : "-",
                    Level3 = chain.Count > 2 ? chain[2] : "-",
                    Level4 = chain.Count > 3 ? chain[3] : "-",
                    Level5 = chain.Count > 4 ? chain[4] : "-"
                };

                finalList.Add(vm);
            }

            return Json(finalList);
        }

        [HttpGet]
        public async Task<JsonResult> GetDept_Role_List()
        {
            var result = await _departmentService.GetDept_Role_List();
            var roles = await _employeeService.GetAllRoleList();
            var rolelookup = roles.ToDictionary(d => d.Role_ListId, d => d);
            foreach (var item in result)
            {
                if (rolelookup.TryGetValue(item.Role_Access_Id, out var sec))
                {
                    item.RoleName = sec.Role_Desc ?? string.Empty;
                }
                else
                {
                    item.RoleName = string.Empty;
                }
            }
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostDept_Role_List(Dept_Role_ListVM model)
        {
            var result = await _departmentService.PostDept_Role_List(model);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostDept_Employee(Dept_EmployeeVM model)
        {
            var result = await _departmentService.PostDept_Employee(model);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostEmployee_UI_List(Employee_UI_ListVM model)
        {
            var result = await _departmentService.PostEmployee_UI_List(model);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DelSections(long designationId)
        {
            var result = await _departmentService.DelSections(designationId);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> DelDept_Employee(long designationId)
        {
            var deptemp = await _departmentService.GetDept_Employee();
            var model = deptemp.Where(de => de.Dept_EmployeeId == designationId).FirstOrDefault();
            if (model == null)
            {
                return NotFound(new { message = $"No record found for Dept_EmployeeId {designationId}" });
            }
            model.Deact_date = DateTime.Now;
            model.Active = 'N';
            var result = await _departmentService.PostDept_Employee(model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> DelEmployee_UI_List(long designationId)
        {
            var deptemp = await _departmentService.GetEmployee_UI_List();
            var model = deptemp.Where(de => de.Employee_UI_ListId == designationId).FirstOrDefault();
            if (model == null)
            {
                return NotFound(new { message = $"No record found for Employee_UI_ListId {designationId}" });
            }
            model.Deact_date = DateTime.Now;
            model.Active = 'N';
            var result = await _departmentService.PostEmployee_UI_List(model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> DelDept_Role_List(long designationId)
        {
            var result = await _departmentService.DelDept_Role_List(designationId);
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> GetUnassignedDepartments()
        {
            var result = await _departmentService.GetDepartments(1); // all depts
            var deptEmployees = await _departmentService.GetDept_Employee(); // mapping dept-employee
            var deptRole = await _departmentService.GetDept_Role_List();
            var roles = await _employeeService.GetAllRoleList();

            var assignedDeptIds = deptEmployees.Select(e => e.Dept_Posn).Distinct().ToHashSet();

            var deptDict = result.ToDictionary(d => d.DepartmentId);
            var finalList = new List<ShopDepartmentVM>();

            foreach (var dept in result)
            {
                if (assignedDeptIds.Contains(dept.DepartmentId))
                    continue; // skip already assigned department

                var deptRol = deptRole.Where(r => r.Dept_Struct_Id == dept.DepartmentId).ToList();
                var roleNames = (from dr in deptRol
                                 join r in roles on dr.Role_Access_Id equals r.Role_ListId
                                 select r.Role_Desc).ToList();

                var vm = new ShopDepartmentVM
                {
                    DepartmentId = dept.DepartmentId,
                    Name = dept.Name,
                    NoOfShifts = dept.NoOfShifts,
                    PlantId = dept.PlantId,
                    Level_No = dept.Level_No,
                    Part_Of = dept.Part_Of,
                    Activity = dept.Activity,
                    ProdDept = dept.ProdDept,
                    TenantId = dept.TenantId,
                    PlantName = dept.PlantName,
                    Section = dept.Section,
                    RoleName = string.Join(", ", roleNames)
                };

                // Walk up the parent chain and fill levels
                var chain = new List<string>();
                var current = dept;
                while (current != null)
                {
                    chain.Insert(0, current.Name);
                    if (current.Part_Of == 0 || !deptDict.ContainsKey(current.Part_Of))
                        break;

                    current = deptDict[current.Part_Of];
                }

                if (chain.Count > 0) vm.Level1 = chain[0];
                if (chain.Count > 1) vm.Level2 = chain[1];
                if (chain.Count > 2) vm.Level3 = chain[2];
                if (chain.Count > 3) vm.Level4 = chain[3];
                if (chain.Count > 4) vm.Level5 = chain[4];

                finalList.Add(vm);
            }

            return Json(finalList);
        }

    }
}
