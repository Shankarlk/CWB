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
                if (e.ChangedPassword == 1)
                {
                    var res = new List<Role_UI_ListVM>();
                    HttpContext.Session.SetString("Permissions", JsonConvert.SerializeObject(res));
                    return View();
                }
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
                if (sorgs != null && sorgs.Count > 0)
                {
                    //-----------------------------------------
                    // 1) Department-Based Role UI (FromDept = "Y")
                    //-----------------------------------------
                    foreach (var sorg in sorgs)
                    {
                        var deptRoleList = deptroles.Where(d => d.Dept_Struct_Id == sorg.Dept_Posn).ToList();

                        foreach (var deptRole in deptRoleList)
                        {
                            var result = roleui.Where(r => r.RoleId == deptRole.Role_Access_Id).ToList();

                            foreach (var rui in result)
                            {
                                var vm = new Role_UI_ListVM
                                {
                                    Role_Ui_ListId = rui.Role_Ui_ListId,
                                    RoleId = rui.RoleId,
                                    Ui_Id = rui.Ui_Id,
                                    Active = sorg.Active,
                                    FromDept = "Y",
                                    Add_dateStr = sorg.Add_date.ToString("dd-MM-yyyy"),
                                    Deact_dateStr = sorg.Deact_date != DateTime.MinValue
                                                     ? sorg.Deact_date.ToString("dd-MM-yyyy")
                                                     : string.Empty
                                };

                                // Lookup Role Name
                                var r = role.FirstOrDefault(u => u.Role_ListId == rui.RoleId);
                                vm.RoleName = r?.Role_Desc ?? "";

                                // Build Menu Hierarchy
                                var d = designation.FirstOrDefault(u => u.UiListId == rui.Ui_Id);
                                if (d != null)
                                {
                                    var chain = new List<string>();
                                    var current = d;

                                    while (current != null)
                                    {
                                        chain.Insert(0, current.UI_Name_Label);
                                        if (current.UI_Part_linked_to == 0) break;
                                        current = designation.FirstOrDefault(m => m.UiListId == current.UI_Part_linked_to);
                                    }

                                    vm.UiLevel = string.Join("+", chain);
                                    vm.Menu1 = chain.Count > 0 ? chain[0] : "";
                                    vm.Menu2 = chain.Count > 1 ? chain[1] : "";
                                    vm.Menu3 = chain.Count > 2 ? chain[2] : "";
                                    vm.Menu4 = chain.Count > 3 ? chain[3] : "";
                                    vm.Menu5 = chain.Count > 4 ? chain[4] : "";
                                }

                                // Map Permissions
                                //MapPermissions(vm, rui.PermissionId);

                                role_UI_ListVMs.Add(vm);
                            }
                        }
                    }
                }

                foreach (var emui in empUiList)
                {
                    var vm = new Role_UI_ListVM
                    {
                        Role_Ui_ListId = emui.Employee_UI_ListId,
                        Ui_Id = emui.Ui_Id,
                        RoleId = 0, // directly assigned UI, no dept role
                        Active = emui.Active,
                        FromDept = "N",
                        Add_dateStr = emui.Add_date.ToString("dd-MM-yyyy"),
                        Deact_dateStr = emui.Deact_date != DateTime.MinValue
                                         ? emui.Deact_date.ToString("dd-MM-yyyy")
                                         : string.Empty
                    };

                    // Try to infer role name from employee's dept mapping
                    var empDept = sorgs.FirstOrDefault();
                    if (empDept != null)
                    {
                        var deptRole = deptroles.FirstOrDefault(d => d.Dept_Struct_Id == empDept.Dept_Posn);
                        if (deptRole != null)
                        {
                            var roleInfo = role.FirstOrDefault(r => r.Role_ListId == deptRole.Role_Access_Id);
                            vm.RoleName = roleInfo?.Role_Desc ?? "";
                        }
                    }

                    // Build Menu Hierarchy
                    var d = designation.FirstOrDefault(u => u.UiListId == emui.Ui_Id);
                    if (d != null)
                    {
                        var chain = new List<string>();
                        var current = d;

                        while (current != null)
                        {
                            chain.Insert(0, current.UI_Name_Label);
                            if (current.UI_Part_linked_to == 0) break;
                            current = designation.FirstOrDefault(m => m.UiListId == current.UI_Part_linked_to);
                        }

                        vm.UiLevel = string.Join("+", chain);
                        vm.Menu1 = chain.Count > 0 ? chain[0] : "";
                        vm.Menu2 = chain.Count > 1 ? chain[1] : "";
                        vm.Menu3 = chain.Count > 2 ? chain[2] : "";
                        vm.Menu4 = chain.Count > 3 ? chain[3] : "";
                        vm.Menu5 = chain.Count > 4 ? chain[4] : "";
                    }

                    // Map Access_Level to permissions
                    //MapPermissions(vm, emui.Access_Level);

                    role_UI_ListVMs.Add(vm);
                }

                HttpContext.Session.SetString("Permissions", JsonConvert.SerializeObject(role_UI_ListVMs));
            }
            return View();
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
