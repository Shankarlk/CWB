using CWB.App.AppUtils;
using CWB.App.Models;
using CWB.App.Models.EmployeeMaster;
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

        public HomeController(ILoggerManager logger, IEmployeeService employeeService)
        {
            _employeeService = employeeService;
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
                var org = await _employeeService.GetAllOrgChart();
                var designation = await _employeeService.GetAllUilist();
                var sorgs = org.Where(r => r.Dept_ID == e.Home_Dept_Id).ToList();
                var role = await _employeeService.GetAllRoleList();
                var roleui = await _employeeService.GetAllRoleUiList();
                if (sorgs != null)
                {
                    List<Role_UI_ListVM> role_UI_ListVMs = new List<Role_UI_ListVM>();
                    foreach (var sorg in sorgs)
                    {
                        var result = roleui.Where(r => r.RoleId == sorg.Role_NameId).ToList();
                        foreach (var item in result)
                        {
                            var d = designation.Where(u => u.UiListId == item.Ui_Id).FirstOrDefault();
                            var r = role.Where(u => u.Role_ListId == item.RoleId).FirstOrDefault();
                            item.RoleName = r.Role_Desc;

                            if (d != null)
                            {
                                if (d.UI_Part_linked_to == 0)
                                {
                                    item.UiLevel = d.UI_Name_Label;
                                    item.Menu1 = d.UI_Name_Label;
                                }
                                else
                                {
                                    var menu2 = designation.Where(m => m.UiListId == d.UI_Part_linked_to).FirstOrDefault();
                                    if (menu2.UI_Part_linked_to == 0)
                                    {
                                        item.UiLevel = menu2.UI_Name_Label + "+" + d.UI_Name_Label;
                                        item.Menu2 = d.UI_Name_Label;
                                        item.Menu1 = menu2.UI_Name_Label;
                                    }
                                    else
                                    {
                                        var menu3 = designation.Where(m => m.UiListId == menu2.UI_Part_linked_to).FirstOrDefault();
                                        if (menu3.UI_Part_linked_to == 0)
                                        {
                                            item.UiLevel = menu3.UI_Name_Label + "+" + menu2.UI_Name_Label + "+" + d.UI_Name_Label;
                                            item.Menu1 = menu3.UI_Name_Label;
                                            item.Menu2 = menu2.UI_Name_Label;
                                            item.Menu3 = d.UI_Name_Label;
                                        }
                                        else
                                        {
                                            var menu4 = designation.Where(m => m.UiListId == menu3.UI_Part_linked_to).FirstOrDefault();
                                            if (menu4.UI_Part_linked_to == 0)
                                            {
                                                item.UiLevel = menu4.UI_Name_Label + "+" + menu3.UI_Name_Label + "+" + menu2.UI_Name_Label + "+" + d.UI_Name_Label;
                                                item.Menu1 = menu4.UI_Name_Label;
                                                item.Menu2 = menu3.UI_Name_Label;
                                                item.Menu3 = menu2.UI_Name_Label;
                                                item.Menu4 = d.UI_Name_Label;
                                            }
                                            else
                                            {
                                                var menu5 = designation.Where(m => m.UiListId == menu4.UI_Part_linked_to).FirstOrDefault();
                                                if (menu5.UI_Part_linked_to == 0)
                                                {
                                                    item.UiLevel = menu5.UI_Name_Label + "+" + menu4.UI_Name_Label + "+" + menu3.UI_Name_Label + "+" + menu2.UI_Name_Label + "+" + d.UI_Name_Label;
                                                    item.Menu1 = menu5.UI_Name_Label;
                                                    item.Menu2 = menu4.UI_Name_Label;
                                                    item.Menu3 = menu3.UI_Name_Label;
                                                    item.Menu4 = menu2.UI_Name_Label;
                                                    item.Menu5 = d.UI_Name_Label;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            role_UI_ListVMs.Add(item);
                        }
                    }
                    var permissionresult = role_UI_ListVMs;
                    HttpContext.Session.SetString("Permissions", JsonConvert.SerializeObject(role_UI_ListVMs));

                    //ViewData["PermissionResult"] = permissionresult;
                }
                else
                {
                    var res = new List<Role_UI_ListVM>();
                    HttpContext.Session.SetString("Permissions", JsonConvert.SerializeObject(res));
                }
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
