using CWB.App.AppUtils;
using CWB.App.Models;
using CWB.App.Models.EmployeeMaster;
using CWB.App.Services.CompanySettings;
using CWB.App.Services.EmployeeMaster;
using CWB.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _deptService;

        public HomeController(ILoggerManager logger, IEmployeeService employeeService, IDepartmentService deptService)
        {
            _employeeService = employeeService;
            _deptService = deptService;
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Logout", "Home");
            }
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                var expiryUtc = jwtToken.ValidTo; // Always in UTC

                if (expiryUtc <= DateTime.UtcNow)
                {
                    // Token expired — redirect to logout
                    return RedirectToAction("Logout", "Home");
                }
            }
            ViewBag.AccessToken = token; // Store the token in ViewBag
            ClaimsPrincipal userClaim = HttpContext.User;
            string fullName = AppUtil.GetUsername(userClaim);
            var emp = await _employeeService.GetAllEmployee();
            //if(emp == null)
            //{
            //    return RedirectToAction("Logout", "Home");
            //}
            var e = emp.Where(e => e.UserName == fullName).FirstOrDefault();
            if (e != null)
            {
                if(e.HeadOfDepartment == "Y")
                {
                    return View();
                }
                //if (e.ChangedPassword == 1)
                //{
                //    var res = new List<Role_UI_ListVM>();
                //    HttpContext.Session.SetString("Permissions", JsonConvert.SerializeObject(res));
                //    return View();
                //}
                var empDeptList = await _deptService.GetDept_Employee();
                var deptroles = await _deptService.GetDept_Role_List();
                var designation = await _employeeService.GetAllUilist();
                var role = await _employeeService.GetAllRoleList();
                var roleui = await _employeeService.GetAllRoleUiList();
                var employeeUiLists = await _deptService.GetEmployee_UI_List();

                var sorgs = empDeptList.Where(r => r.Employee_Id == e.Employee_ID && r.Active == 'Y').ToList();
                var empUiList = employeeUiLists.Where(r => r.Employee_Id == e.Employee_ID && r.Active == 'Y').ToList();

                List<Role_UI_ListVM> role_UI_ListVMs = new List<Role_UI_ListVM>();
                if (sorgs == null)
                {
                    var res = new List<Role_UI_ListVM>();
                    HttpContext.Session.SetString("Permissions", JsonConvert.SerializeObject(res));
                    return View();
                }

                //---------------------------
                // 1) Department-Based Role UI List (FromDept = "Y")
                //---------------------------
                foreach (var sorg in sorgs)
                {
                    var deptrole = deptroles.Where(d => d.Dept_Struct_Id == sorg.Dept_Posn).ToList();
                    foreach (var dept in deptrole)
                    {
                        var result = roleui.Where(r => r.RoleId == dept.Role_Access_Id).ToList();
                        foreach (var rui in result)
                        {
                            var vm = new Role_UI_ListVM();

                            // Copy base details
                            vm.Role_Ui_ListId = rui.Role_Ui_ListId;
                            vm.RoleId = rui.RoleId;
                            vm.Ui_Id = rui.Ui_Id;

                            // Get Role Name
                            var r = role.FirstOrDefault(u => u.Role_ListId == rui.RoleId);
                            vm.RoleName = r?.Role_Desc ?? "";

                            // Build Menu Hierarchy
                            BuildUiHierarchy(vm, designation, rui.Ui_Id);

                            // Map Permissions
                            MapPermissions(vm, rui.PermissionId);

                            vm.Add_dateStr = sorg.Add_date.ToString("dd-MM-yyyy");
                            vm.Deact_dateStr = sorg.Deact_date != DateTime.MinValue ? sorg.Deact_date.ToString("dd-MM-yyyy") : string.Empty;
                            vm.Active = sorg.Active;
                            vm.FromDept = "Y";

                            role_UI_ListVMs.Add(vm);
                        }
                    }
                }

                //---------------------------
                // 2) Employee-Specific UI List (FromDept = "N")
                //---------------------------
                foreach (var emui in empUiList)
                {
                    var vm = new Role_UI_ListVM();

                    // Copy base details
                    vm.Role_Ui_ListId = emui.Employee_UI_ListId;
                    vm.Ui_Id = emui.Ui_Id.ToString();
                    vm.RoleId = 0; // Since this is directly assigned to Employee
                    var empDept = sorgs.FirstOrDefault();

                    if (empDept != null)
                    {
                        // 2. Find role mapping for this department
                        var deptRole = deptroles.FirstOrDefault(d => d.Dept_Struct_Id == empDept.Dept_Posn);

                        if (deptRole != null)
                        {
                            // 3. Lookup the RoleName from role master
                            //var roleInfo = role.FirstOrDefault(r => r.Role_ListId == deptRole.Role_Access_Id);
                            vm.RoleName = string.Empty;
                        }
                        else
                        {
                            vm.RoleName = string.Empty; // No role mapping found
                        }
                    }
                    else
                    {
                        vm.RoleName = string.Empty; // No department found for employee
                    }

                    // Build Menu Hierarchy
                    BuildUiHierarchy(vm, designation, emui.Ui_Id);

                    // Map Access_Level to permissions
                    MapPermissions(vm, emui.Access_Level);

                    vm.Add_dateStr = emui.Add_date.ToString("dd-MM-yyyy");
                    vm.Deact_dateStr = emui.Deact_date != DateTime.MinValue ? emui.Deact_date.ToString("dd-MM-yyyy") : string.Empty;
                    vm.Active = emui.Active;
                    vm.FromDept = "N";

                    role_UI_ListVMs.Add(vm);
                }


                HttpContext.Session.SetString("Permissions", JsonConvert.SerializeObject(role_UI_ListVMs));
            }
            return View();
        }

        private void BuildUiHierarchy(Role_UI_ListVM vm, IEnumerable<UiListVM> designation, string uiIdString)
        {
            if (string.IsNullOrWhiteSpace(uiIdString))
            {
                vm.UiLevel = "";
                return;
            }

            // ✅ Split by commas, trim, convert to int
            var uiIds = uiIdString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(id => Convert.ToInt32(id.Trim()))
                                  .ToList();

            // ✅ Map to UI name labels
            var uiNames = uiIds
                .Select(id => designation.FirstOrDefault(d => d.UiListId == id)?.UI_Name_Label)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .ToList();

            // ✅ Assign Menu1..Menu5 if needed
            if (uiNames.Count > 0) vm.Menu1 = uiNames.ElementAtOrDefault(0);
            if (uiNames.Count > 1) vm.Menu2 = uiNames.ElementAtOrDefault(1);
            if (uiNames.Count > 2) vm.Menu3 = uiNames.ElementAtOrDefault(2);
            if (uiNames.Count > 3) vm.Menu4 = uiNames.ElementAtOrDefault(3);
            if (uiNames.Count > 4) vm.Menu5 = uiNames.ElementAtOrDefault(4);

            // ✅ Build UiLevel like "Masters+Item Masters+View/Edit Part No+Edit Part"
            vm.UiLevel = string.Join("+", uiNames);
        }
        private void MapPermissions(Role_UI_ListVM vm, long permissionId)
        {
            vm.View_Allowed = "N";
            vm.Add_Edit_Allowed = "N";
            vm.Delete_Allowed = "N";
            vm.Approval_Allowed = "N";
            vm.PermissionId = permissionId;

            switch (permissionId)
            {
                case 2: vm.View_Allowed = "Y"; break;
                case 3: vm.View_Allowed = "Y"; vm.Add_Edit_Allowed = "Y"; break;
                case 4: vm.View_Allowed = "Y"; vm.Add_Edit_Allowed = "Y"; vm.Delete_Allowed = "Y"; break;
                case 5: vm.View_Allowed = "Y"; vm.Add_Edit_Allowed = "Y"; vm.Delete_Allowed = "Y"; vm.Approval_Allowed = "Y"; break;
            }
        }


        public IActionResult Privacy()
        {
            return View(new CWB.App.Models.Contacts.CompanyVM());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Login()
        {

            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return SignOut("Cookies", "oidc");
        }
    }
}
