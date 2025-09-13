using CWB.App.AppUtils;
using CWB.App.Models.EmployeeMaster;
using CWB.App.Services.CompanySettings;
using CWB.App.Services.EmailServices;
using CWB.App.Services.EmployeeMaster;
using CWB.App.Services.Masters;
using CWB.Constants.UserIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMastersServices _mastersService;
        private readonly IEmployeeService _employeeService;
        private readonly IPlantService _plantService;
        private readonly IDepartmentService _deptService;
        private readonly EmailService _emailService;
        private readonly KafkaEmailProducer _emailProducer;
        public EmployeeController(ILogger<EmployeeController> logger, IMastersServices mastersService,IEmployeeService employeeService,
            IPlantService plantService,IDepartmentService departmentService, EmailService emailService,
            KafkaEmailProducer emailProducer)
        {
            _logger = logger;
            _mastersService = mastersService;
            _employeeService = employeeService;
            _emailService = emailService;
            _plantService = plantService;
            _deptService = departmentService;
            _emailProducer = emailProducer;
        }
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal userClaim = HttpContext.User;
            string fullName = AppUtil.GetUsername(userClaim);
            var emp = await _employeeService.GetAllEmployee();
            var e = emp.Where(e => e.UserName == fullName).FirstOrDefault();
            if (e != null)
            {
                var org = await _employeeService.GetAllOrgChart();
                var designation = await _employeeService.GetAllUilist();
                var sorg = org.Where(r => r.Employee_Id == e.Employee_ID).FirstOrDefault();
                var role = await _employeeService.GetAllRoleList();
                if (sorg != null)
                {
                    var roleui = await _employeeService.GetAllRoleUiList();
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
                        if (item.PermissionId == 1)
                        {
                            item.View_Allowed = "N";
                            item.Add_Edit_Allowed = "N";
                            item.Delete_Allowed = "N";
                            item.Approval_Allowed = "N";
                        }
                        else if (item.PermissionId == 2)
                        {
                            item.View_Allowed = "Y";
                            item.Add_Edit_Allowed = "N";
                            item.Delete_Allowed = "N";
                            item.Approval_Allowed = "N";
                        }
                        else if (item.PermissionId == 3)
                        {
                            item.View_Allowed = "Y";
                            item.Add_Edit_Allowed = "Y";
                            item.Delete_Allowed = "N";
                            item.Approval_Allowed = "N";
                        }
                        else if (item.PermissionId == 4)
                        {
                            item.View_Allowed = "Y";
                            item.Add_Edit_Allowed = "Y";
                            item.Delete_Allowed = "Y";
                            item.Approval_Allowed = "N";
                        }
                        else if (item.PermissionId == 5)
                        {
                            item.View_Allowed = "Y";
                            item.Add_Edit_Allowed = "Y";
                            item.Delete_Allowed = "Y";
                            item.Approval_Allowed = "Y";
                        }
                    }
                    var permissionresult = result;

                    ViewData["PermissionResult"] = permissionresult;
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployee()
        {
            var designation = await _employeeService.GetAllEmployee();
            //var loc = await _plantService.GetPlants();
            foreach (var item in designation)
            {
                item.DateOfJoinStr = item.Date_Of_Joining.ToString("d");
                //var l = loc.Where(l => l.PlantId == item.Plant_Id).FirstOrDefault();
                //item.Location = l.Name;
            }
            return Ok(designation);
        }
        [HttpGet]
        public async Task<IActionResult> GetUnique(string empNo)
        {
            var result = await _employeeService.GetAllEmployee();
            bool exists = result.Any(e => e.Employee_No == empNo);
            return Json(!exists);
        }
        [HttpPost]
        public async Task<IActionResult> PostEmployee(EmployeeVM model)
        {
            if(model.Password == null)
            {
                model.Password = GeneratePassword(10);
                model.ChangedPassword = 1;
            }
            var result = await _employeeService.PostEmployee(model);
            string plainText = $"Hi {model.Employee_name},\n\n" +
                               "Your Account has been Created successfully!\n" +
                               $"You Username: {model.Email}\n" +
                               $"You Password: {model.Password}\n\n" +
                               "Thanks,\nKGK-Engineers";
            //var producer = new KafkaEmailProducer();
            try
            {
                await _emailProducer.SendEmailRequestAsync(model.Email, "Welcome to KGK-Engineers!", plainText);
            }
            catch (Exception ex)
            {

                //throw;
            }
            if (model.Date_Of_Resigning != null)
            {
                var GetDept_Employee = await _deptService.GetDept_Employee();
                var empdept = GetDept_Employee.Where(i => i.Employee_Id == result.Employee_ID).ToList();
                foreach (var item in empdept)
                {
                    item.Deact_date = DateTime.Now;
                    item.Active = 'N';
                    var PostDept_Employee = await _deptService.PostDept_Employee(item);
                }
                var GetEmployee_UI_Lists = await _deptService.GetEmployee_UI_List();
                var GetEmployee_UI_List = GetEmployee_UI_Lists.Where(de => de.Employee_Id == result.Employee_ID).ToList();
                foreach (var item in GetEmployee_UI_List)
                {
                    item.Deact_date = DateTime.Now;
                    item.Active = 'N';
                    var PostEmployee_UI_List = await _deptService.PostEmployee_UI_List(item);
                }
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> ResetEmpPassword(long EmpId)
        {
            var result = await _employeeService.GetAllEmployee();
            var model = result.Where(e => e.Employee_ID == EmpId).FirstOrDefault();
            model.ChangedPassword = 1;
            model.Password = GeneratePassword(10);
            var Post = await _employeeService.PostEmployee(model);
            string plainText = $"Hi {model.Employee_name},\n\n" +
                               "Your Password has been Reset!\n" +
                               $"Your Username: {model.Email}\n" +
                               $"Your New Password: {model.Password}\n\n" +
                               "Thanks,\nKGK-Engineers";
            //var producer = new KafkaEmailProducer();
            try
            {
                await _emailProducer.SendEmailRequestAsync(model.Email, "Password Reset!", plainText);
            }
            catch (Exception ex)
            {

                //throw;
            }
            return Ok(Post);
        }
        public static string GeneratePassword(int length = 10)
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "@#$%*!";

            Random random = new Random();

            // Ensure at least one of each required character
            string password =
                upper[random.Next(upper.Length)].ToString() +
                lower[random.Next(lower.Length)].ToString() +
                special[random.Next(special.Length)].ToString() +
                digits[random.Next(digits.Length)].ToString();

            // Remaining characters (mix of all types)
            string allChars = upper + lower + digits + special;
            int remaining = length - password.Length;
            password += new string(Enumerable.Range(0, remaining)
                               .Select(x => allChars[random.Next(allChars.Length)]).ToArray());

            // Shuffle the password so first chars aren't always in the same order
            return new string(password.OrderBy(c => random.Next()).ToArray());
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(string to, string subject, string body)
        {
            await _emailService.SendEmailAsync(to, subject, body);
            return Ok("Email sent successfully!");
        }

        [HttpGet]
        public async Task<IActionResult> DelEmployee(long designationId)
        {
            var result = await _employeeService.DelEmployee(designationId);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUilist()
        {
            var designation = await _employeeService.GetAllUilist();
            foreach (var item in designation)
            {
                if (item.UI_Part_linked_to == 0)
                {
                    item.Menu1 = item.UI_Name_Label;
                }
                else
                {
                    var menu2 = designation.Where(m => m.UiListId == item.UI_Part_linked_to).FirstOrDefault();
                    if(menu2.UI_Part_linked_to == 0)
                    {
                        item.Menu2 = item.UI_Name_Label;
                        item.Menu1 = menu2.UI_Name_Label;
                    }
                    else
                    {
                        var menu3 = designation.Where(m => m.UiListId == menu2.UI_Part_linked_to).FirstOrDefault();
                        if (menu3.UI_Part_linked_to == 0)
                        {
                            item.Menu1 = menu3.UI_Name_Label;
                            item.Menu2 = menu2.UI_Name_Label;
                            item.Menu3 = item.UI_Name_Label;
                        }
                        else
                        {
                            var menu4 = designation.Where(m => m.UiListId == menu3.UI_Part_linked_to).FirstOrDefault();
                            if (menu4.UI_Part_linked_to == 0)
                            {
                                item.Menu1 = menu4.UI_Name_Label;
                                item.Menu2 = menu3.UI_Name_Label;
                                item.Menu3 = menu2.UI_Name_Label;
                                item.Menu4 = item.UI_Name_Label;
                            }
                            else
                            {
                                var menu5 = designation.Where(m => m.UiListId == menu4.UI_Part_linked_to).FirstOrDefault();
                                if (menu5.UI_Part_linked_to == 0)
                                {
                                    item.Menu1 = menu5.UI_Name_Label;
                                    item.Menu2 = menu4.UI_Name_Label;
                                    item.Menu3 = menu3.UI_Name_Label;
                                    item.Menu4 = menu2.UI_Name_Label;
                                    item.Menu5 = item.UI_Name_Label;
                                }
                            }
                        }
                    }
                }
            }
            return Ok(designation);
        }

        [HttpGet]
        public async Task<IActionResult> GetUniqueUiName(string uiName)
        {
            var designation = await _employeeService.GetAllUilist();
            bool result = true;
            foreach (var item in designation)
            {
                if (item.UI_Name_Label == uiName)
                {
                    result = false;
                    return Ok(result);
                }
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostUilist(UiListVM model)
        {
            var designation = await _employeeService.GetAllUilist();
            if (model.UI_Part_linked_to == 0)
            {
                model.MenuLevelId = 1;
            }
            else
            {
                var menu2 = designation.Where(m => m.UiListId == model.UI_Part_linked_to).FirstOrDefault();
                if (menu2.UI_Part_linked_to == 0)
                {
                    model.MenuLevelId = 2;
                }
                else
                {
                    var menu3 = designation.Where(m => m.UiListId == menu2.UI_Part_linked_to).FirstOrDefault();
                    if (menu3.UI_Part_linked_to == 0)
                    {
                        model.MenuLevelId = 3;
                    }
                    else
                    {
                        var menu4 = designation.Where(m => m.UiListId == menu3.UI_Part_linked_to).FirstOrDefault();
                        if (menu4.UI_Part_linked_to == 0)
                        {
                            model.MenuLevelId = 4;
                        }
                        else
                        {
                            var menu5 = designation.Where(m => m.UiListId == menu4.UI_Part_linked_to).FirstOrDefault();
                            if (menu5.UI_Part_linked_to == 0)
                            {
                                model.MenuLevelId = 5;
                            }
                        }
                    }
                }
            }
            var result = await _employeeService.PostUilist(model);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> CheckUiList(long designationId)
        {
            var result = await _employeeService.GetAllEmplRoleUiList();
            bool r;
            if (result.Any())
            {
                foreach (var item in result)
                {
                    if (item.Ui_Id == designationId)
                    {
                        r = false;
                        return Json(r);
                    }
                }
            }
            else
            {
                var roleui = await _employeeService.GetAllRoleUiList();
                foreach (var item in roleui)
                {
                    if (item.Ui_Id == designationId)
                    {
                        r = false;
                        return Json(r);
                    }
                }
            }
            r = true;
            return Json(r);
        }
        [HttpGet]
        public async Task<IActionResult> DelUiList(long designationId)
        {
            var result = await _employeeService.DelUiList(designationId);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRoleList()
        {
            var designation = await _employeeService.GetAllRoleList();
            var roleui = await _employeeService.GetAllRoleUiList();
            List<Role_ListVM> roles = new List<Role_ListVM>();
            foreach (var item in designation)
            {
                var r = roleui.Where(r => r.RoleId == item.Role_ListId).FirstOrDefault();
                if(r!=null)
                {
                    roles.Add(item);
                }
            }
            return Ok(roles);
        }
        [HttpGet]
        public async Task<IActionResult> GetUniqueRole(string roleName)
        {
            var designation = await _employeeService.GetAllRoleList();
            bool result = true;
            foreach (var item in designation)
            {
                if(item.Role_Desc== roleName)
                {
                    result = false;
                    return Ok(result);
                }
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostRolelist(Role_ListVM model)
        {
            var result = await _employeeService.PostRolelist(model);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrgChart()
        {
            var designation = await _employeeService.GetAllOrgChart();
            var loc = await _plantService.GetPlants();
            var dept = await _deptService.GetDepartments(1);
            var role = await _employeeService.GetAllRoleList();
            var employee = await _employeeService.GetAllEmployee();
            foreach (var item in designation)
            {
                item.Level = item.Level_No.ToString();
                var l = loc.Where(l => l.PlantId == item.Location_id).FirstOrDefault();
                var d = dept.Where(l => l.DepartmentId == item.Dept_ID).FirstOrDefault();
                var r = role.Where(l => l.Role_ListId == item.Role_NameId).FirstOrDefault();
                if (r != null)
                {
                    item.RoleName = r.Role_Desc;
                }
                //var o = designation.Where(l => l.Employee_Id == item.Reporting_to).FirstOrDefault();
                var e = employee.Where(l => l.Employee_ID == item.Employee_Id).FirstOrDefault();
                item.Location = l.Name;
                item.Department = d.Name;
                //item.Employee = e.Employee_name;
                //if(o != null)
                //{
                //    var em = employee.Where(l => l.Employee_ID == item.Reporting_to).FirstOrDefault();
                //    var rr = role.Where(l => l.Role_ListId == o.Role_NameId).FirstOrDefault();
                //    if (rr != null)
                //    {
                //        item.Reporting = em.Employee_name + " / " + rr.Role_Desc;
                //    }
                //}
            }
            var groupedDesignations = designation
   .GroupBy(d => d.Dept_ID)
   .Select(g =>
   {
       var firstItem = g.First();
       var roles = g.Where(d => d.Role_NameId != null)
                    .Select(d => role.FirstOrDefault(r => r.Role_ListId == d.Role_NameId)?.Role_Desc)
                    .Where(r => r != null);

       var roleIds = g.Select(d => d.Role_NameId).Where(id => id != null);
       var orgChartIds = g.Select(d => d.Org_ChartId).Where(id => id != null); 
       var lowestLevel = g.Where(d => d.Level_No > 0 && d.Level_No < 5)
                           .Select(d => d.Level_No)
                           .DefaultIfEmpty() // Handle case where no levels exist
                           .Min();

       firstItem.RoleName = string.Join(", ", roles);
       firstItem.RoleIds = string.Join(", ", roleIds);
       firstItem.OrgChartIds = string.Join(", ", orgChartIds);
       firstItem.Level = lowestLevel.ToString();

       return firstItem;
   })
   .ToList();

            return Ok(groupedDesignations);
        }
        [HttpGet]
        public async Task<IActionResult> GetOrgChart()
        {
            var designation = await _employeeService.GetAllOrgChart();
            return Ok(designation);
        }
        [HttpPost]
        public async Task<IActionResult> PostOrgChart(Org_ChartVM model)
        {
            var result = await _employeeService.PostOrgChart(model);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DelOrgChart(long designationId)
        {
            var result = await _employeeService.DelOrgChart(designationId);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPermission()
        {
            var designation = await _employeeService.GetAllPermission();
            return Ok(designation);
        }

        [HttpPost]
        public async Task<IActionResult> PostRoleUiList(Role_UI_ListVM model)
        {
            if (model.RoleId==0)
            {
                var org = await _employeeService.GetAllOrgChart();
                if (model.EmployeeId!=0)
                {
                    var sorg = org.Where(r => r.Dept_ID == model.DepartmentId).FirstOrDefault();
                    model.RoleId = sorg.Role_NameId;
                }
            }
            var result = await _employeeService.PostRoleUiList(model);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DelRoleUiList(long designationId)
        {
            var result = await _employeeService.DelRoleUilist(designationId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DelRoleList(long designationId)
        {
            var result = await _employeeService.DelRoleList(designationId);
            var uilist = await _employeeService.GetAllRoleUiList();
            var ui = uilist.Where(r => r.RoleId == designationId).ToList();
            if (ui != null)
            {
                foreach (var u in ui)
                {
                    var delui = await _employeeService.DelRoleUilist(u.Role_Ui_ListId);
                }
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRoleUiList()
        {
            var result = await _employeeService.GetAllRoleUiList();
            var designation = await _employeeService.GetAllUilist();
            var role = await _employeeService.GetAllRoleList();
            var resultRole = result.Where(r => r.EmployeeId == 0).ToList();
            foreach (var item in resultRole)
            {
                var d = designation.Where(u => u.UiListId == item.Ui_Id).FirstOrDefault();
                var r = role.Where(u => u.Role_ListId == item.RoleId).FirstOrDefault();
                if (r != null)
                {
                    item.RoleName = r.Role_Desc;
                    item.WorkDone = r.Work_Done;
                    if (d != null)
                    {
                        if (d.UI_Part_linked_to == 0)
                        {
                            item.UiLevel = d.UI_Name_Label;
                        }
                        else
                        {
                            var menu2 = designation.Where(m => m.UiListId == d.UI_Part_linked_to).FirstOrDefault();
                            if (menu2.UI_Part_linked_to == 0)
                            {
                                item.UiLevel = menu2.UI_Name_Label + "+" + d.UI_Name_Label;
                            }
                            else
                            {
                                var menu3 = designation.Where(m => m.UiListId == menu2.UI_Part_linked_to).FirstOrDefault();
                                if (menu3.UI_Part_linked_to == 0)
                                {
                                    item.UiLevel = menu3.UI_Name_Label + "+" + menu2.UI_Name_Label + "+" + d.UI_Name_Label;
                                }
                                else
                                {
                                    var menu4 = designation.Where(m => m.UiListId == menu3.UI_Part_linked_to).FirstOrDefault();
                                    if (menu4.UI_Part_linked_to == 0)
                                    {
                                        item.UiLevel = menu4.UI_Name_Label + "+" + menu3.UI_Name_Label + "+" + menu2.UI_Name_Label + "+" + d.UI_Name_Label;
                                    }
                                    else
                                    {
                                        var menu5 = designation.Where(m => m.UiListId == menu4.UI_Part_linked_to).FirstOrDefault();
                                        if (menu5.UI_Part_linked_to == 0)
                                        {
                                            item.UiLevel = menu5.UI_Name_Label + "+" + menu4.UI_Name_Label + "+" + menu3.UI_Name_Label + "+" + menu2.UI_Name_Label + "+" + d.UI_Name_Label;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (item.PermissionId == 1)
                    {
                        item.View_Allowed = "N";
                        item.Add_Edit_Allowed = "N";
                        item.Delete_Allowed = "N";
                        item.Approval_Allowed = "N";
                    }
                    else if (item.PermissionId == 2)
                    {
                        item.View_Allowed = "Y";
                        item.Add_Edit_Allowed = "N";
                        item.Delete_Allowed = "N";
                        item.Approval_Allowed = "N";
                    }
                    else if (item.PermissionId == 3)
                    {
                        item.View_Allowed = "Y";
                        item.Add_Edit_Allowed = "Y";
                        item.Delete_Allowed = "N";
                        item.Approval_Allowed = "N";
                    }
                    else if (item.PermissionId == 4)
                    {
                        item.View_Allowed = "Y";
                        item.Add_Edit_Allowed = "Y";
                        item.Delete_Allowed = "Y";
                        item.Approval_Allowed = "N";
                    }
                    else if (item.PermissionId == 5)
                    {
                        item.View_Allowed = "Y";
                        item.Add_Edit_Allowed = "Y";
                        item.Delete_Allowed = "Y";
                        item.Approval_Allowed = "Y";
                    }
                }
            }
            foreach (var item in role)
            {
                Role_UI_ListVM role_UI_s = new Role_UI_ListVM
                {
                    RoleId = item.Role_ListId,
                    RoleName = item.Role_Desc,
                    WorkDone =item.Work_Done
                };
                if (!resultRole.Any(r => r.RoleId == item.Role_ListId))
                {
                    resultRole.Add(role_UI_s);
                }

            }
            return Ok(resultRole);
        }
        [HttpGet]
        public async Task<IActionResult> GetRoleUiList(long roleId)
        {
            var result = await _employeeService.GetAllRoleUiList();
            result= result.Where(r => r.RoleId == roleId).ToList();
            var designation = await _employeeService.GetAllUilist();
            var role = await _employeeService.GetAllRoleList();
            var permission = await _employeeService.GetAllPermission();
            foreach (var item in result)
            {
                var d = designation.Where(u => u.UiListId == item.Ui_Id).FirstOrDefault();
                var r = role.Where(u => u.Role_ListId == item.RoleId).FirstOrDefault();
                item.RoleName = r.Role_Desc;
                if (d != null)
                {
                    if (d.UI_Part_linked_to == 0)
                    {
                        item.Menu1 = d.UI_Name_Label;
                    }
                    else
                    {
                        var menu2 = designation.Where(m => m.UiListId == d.UI_Part_linked_to).FirstOrDefault();
                        if (menu2.UI_Part_linked_to == 0)
                        {
                            item.Menu2 = d.UI_Name_Label;
                            item.Menu1 = menu2.UI_Name_Label;
                        }
                        else
                        {
                            var menu3 = designation.Where(m => m.UiListId == menu2.UI_Part_linked_to).FirstOrDefault();
                            if (menu3.UI_Part_linked_to == 0)
                            {
                                item.Menu1 = menu3.UI_Name_Label;
                                item.Menu2 = menu2.UI_Name_Label;
                                item.Menu3 = d.UI_Name_Label;
                            }
                            else
                            {
                                var menu4 = designation.Where(m => m.UiListId == menu3.UI_Part_linked_to).FirstOrDefault();
                                if (menu4.UI_Part_linked_to == 0)
                                {
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
                    var p = permission.Where(p => p.PermissionId == item.PermissionId).FirstOrDefault();
                    item.Permission = p.Permission;
                }
                
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmplRoleUiList(long employeeId)
        {
            // Fetch all required data
            var org = await _deptService.GetDept_Employee();
            var deptroles = await _deptService.GetDept_Role_List();
            var sorgs = org.Where(r => r.Employee_Id == employeeId).ToList();
            var designation = await _employeeService.GetAllUilist();
            var role = await _employeeService.GetAllRoleList();
            var roleui = await _employeeService.GetAllRoleUiList();
            var employeeUiLists = await _deptService.GetEmployee_UI_List();
            var employeeUiList = employeeUiLists.Where(r => r.Employee_Id == employeeId).ToList();

            if (sorgs == null || sorgs.Count == 0)
                return Ok(new List<Role_UI_ListVM>());

            List<Role_UI_ListVM> role_UI_ListVMs = new List<Role_UI_ListVM>();

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
            foreach (var emui in employeeUiList)
            {
                var vm = new Role_UI_ListVM();

                // Copy base details
                vm.Role_Ui_ListId = emui.Employee_UI_ListId;
                vm.Ui_Id = emui.Ui_Id;
                vm.RoleId = 0; // Since this is directly assigned to Employee
                var empDept = sorgs.FirstOrDefault();

                if (empDept != null)
                {
                    // 2. Find role mapping for this department
                    var deptRole = deptroles.FirstOrDefault(d => d.Dept_Struct_Id == empDept.Dept_Posn);

                    if (deptRole != null)
                    {
                        // 3. Lookup the RoleName from role master
                        var roleInfo = role.FirstOrDefault(r => r.Role_ListId == deptRole.Role_Access_Id);
                        vm.RoleName = roleInfo != null ? roleInfo.Role_Desc : string.Empty;
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

            return Ok(role_UI_ListVMs);
        }
        private void BuildUiHierarchy(Role_UI_ListVM vm, IEnumerable<UiListVM> designation, long uiId)
        {
            var current = designation.FirstOrDefault(u => u.UiListId == uiId);
            var hierarchy = new List<string>();

            while (current != null)
            {
                hierarchy.Insert(0, current.UI_Name_Label);
                current = designation.FirstOrDefault(u => u.UiListId == current.UI_Part_linked_to && current.UI_Part_linked_to != 0);
            }

            // Assign to Menu1..Menu5
            if (hierarchy.Count > 0) vm.Menu1 = hierarchy[0];
            if (hierarchy.Count > 1) vm.Menu2 = hierarchy[1];
            if (hierarchy.Count > 2) vm.Menu3 = hierarchy[2];
            if (hierarchy.Count > 3) vm.Menu4 = hierarchy[3];
            if (hierarchy.Count > 4) vm.Menu5 = hierarchy[4];

            vm.UiLevel = string.Join("+", hierarchy);
        }
        private void MapPermissions(Role_UI_ListVM vm, long permissionId)
        {
            vm.View_Allowed = "N";
            vm.Add_Edit_Allowed = "N";
            vm.Delete_Allowed = "N";
            vm.Approval_Allowed = "N";

            switch (permissionId)
            {
                case 2: vm.View_Allowed = "Y"; break;
                case 3: vm.View_Allowed = "Y"; vm.Add_Edit_Allowed = "Y"; break;
                case 4: vm.View_Allowed = "Y"; vm.Add_Edit_Allowed = "Y"; vm.Delete_Allowed = "Y"; break;
                case 5: vm.View_Allowed = "Y"; vm.Add_Edit_Allowed = "Y"; vm.Delete_Allowed = "Y"; vm.Approval_Allowed = "Y"; break;
            }
        }


        //[HttpGet]
        //public async Task<IActionResult> GetEmplRoleUiList(long employeeId)
        //{
        //    var org = await _deptService.GetDept_Employee();
        //    var deptroles = await _deptService.GetDept_Role_List();
        //    var sorgs = org.Where(r => r.Employee_Id == employeeId).ToList();
        //    var designation = await _employeeService.GetAllUilist();
        //    var role = await _employeeService.GetAllRoleList();
        //    var roleui = await _employeeService.GetAllRoleUiList();
        //    var employeeUiLists = await _deptService.GetEmployee_UI_List();
        //    var employeeUiList = employeeUiLists.Where(r => r.Employee_Id == employeeId).ToList();
        //    if (sorgs!=null)
        //    {
        //        List<Role_UI_ListVM> role_UI_ListVMs = new List<Role_UI_ListVM>();
        //        foreach (var sorg in sorgs)
        //        {
        //            var deptrole = deptroles.Where(d => d.Dept_Struct_Id == sorg.Dept_Posn).ToList();
        //            foreach (var dept in deptrole)
        //            {
        //                var result = roleui.Where(r => r.RoleId == dept.Role_Access_Id).ToList();
        //                foreach (var item in result)
        //                {
        //                    var d = designation.Where(u => u.UiListId == item.Ui_Id).FirstOrDefault();
        //                    var r = role.Where(u => u.Role_ListId == item.RoleId).FirstOrDefault();
        //                    item.RoleName = r.Role_Desc;
        //                    if (d != null)
        //                    {
        //                        if (d.UI_Part_linked_to == 0)
        //                        {
        //                            item.UiLevel = d.UI_Name_Label;
        //                            item.Menu1 = d.UI_Name_Label;
        //                        }
        //                        else
        //                        {
        //                            var menu2 = designation.Where(m => m.UiListId == d.UI_Part_linked_to).FirstOrDefault();
        //                            if (menu2.UI_Part_linked_to == 0)
        //                            {
        //                                item.UiLevel = menu2.UI_Name_Label + "+" + d.UI_Name_Label;
        //                                item.Menu2 = d.UI_Name_Label;
        //                                item.Menu1 = menu2.UI_Name_Label;
        //                            }
        //                            else
        //                            {
        //                                var menu3 = designation.Where(m => m.UiListId == menu2.UI_Part_linked_to).FirstOrDefault();
        //                                if (menu3.UI_Part_linked_to == 0)
        //                                {
        //                                    item.UiLevel = menu3.UI_Name_Label + "+" + menu2.UI_Name_Label + "+" + d.UI_Name_Label;
        //                                    item.Menu1 = menu3.UI_Name_Label;
        //                                    item.Menu2 = menu2.UI_Name_Label;
        //                                    item.Menu3 = d.UI_Name_Label;
        //                                }
        //                                else
        //                                {
        //                                    var menu4 = designation.Where(m => m.UiListId == menu3.UI_Part_linked_to).FirstOrDefault();
        //                                    if (menu4.UI_Part_linked_to == 0)
        //                                    {
        //                                        item.UiLevel = menu4.UI_Name_Label + "+" + menu3.UI_Name_Label + "+" + menu2.UI_Name_Label + "+" + d.UI_Name_Label;
        //                                        item.Menu1 = menu4.UI_Name_Label;
        //                                        item.Menu2 = menu3.UI_Name_Label;
        //                                        item.Menu3 = menu2.UI_Name_Label;
        //                                        item.Menu4 = d.UI_Name_Label;
        //                                    }
        //                                    else
        //                                    {
        //                                        var menu5 = designation.Where(m => m.UiListId == menu4.UI_Part_linked_to).FirstOrDefault();
        //                                        if (menu5.UI_Part_linked_to == 0)
        //                                        {
        //                                            item.UiLevel = menu5.UI_Name_Label + "+" + menu4.UI_Name_Label + "+" + menu3.UI_Name_Label + "+" + menu2.UI_Name_Label + "+" + d.UI_Name_Label;
        //                                            item.Menu1 = menu5.UI_Name_Label;
        //                                            item.Menu2 = menu4.UI_Name_Label;
        //                                            item.Menu3 = menu3.UI_Name_Label;
        //                                            item.Menu4 = menu2.UI_Name_Label;
        //                                            item.Menu5 = d.UI_Name_Label;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    if (item.PermissionId == 1)
        //                    {
        //                        item.View_Allowed = "N";
        //                        item.Add_Edit_Allowed = "N";
        //                        item.Delete_Allowed = "N";
        //                        item.Approval_Allowed = "N";
        //                    }
        //                    else if (item.PermissionId == 2)
        //                    {
        //                        item.View_Allowed = "Y";
        //                        item.Add_Edit_Allowed = "N";
        //                        item.Delete_Allowed = "N";
        //                        item.Approval_Allowed = "N";
        //                    }
        //                    else if (item.PermissionId == 3)
        //                    {
        //                        item.View_Allowed = "Y";
        //                        item.Add_Edit_Allowed = "Y";
        //                        item.Delete_Allowed = "N";
        //                        item.Approval_Allowed = "N";
        //                    }
        //                    else if (item.PermissionId == 4)
        //                    {
        //                        item.View_Allowed = "Y";
        //                        item.Add_Edit_Allowed = "Y";
        //                        item.Delete_Allowed = "Y";
        //                        item.Approval_Allowed = "N";
        //                    }
        //                    else if (item.PermissionId == 5)
        //                    {
        //                        item.View_Allowed = "Y";
        //                        item.Add_Edit_Allowed = "Y";
        //                        item.Delete_Allowed = "Y";
        //                        item.Approval_Allowed = "Y";
        //                    }
        //                    item.Add_dateStr = sorg.Add_date.ToString("dd-MM-yyyy");
        //                    item.Deact_dateStr = sorg.Deact_date != DateTime.MinValue
        //                            ? sorg.Deact_date.ToString("dd-MM-yyyy")
        //                            : string.Empty;
        //                    item.Active = sorg.Active;
        //                    item.FromDept = "Y";
        //                    role_UI_ListVMs.Add(item);
        //                    foreach (var emui in employeeUiList)
        //                    {
        //                        var de = designation.Where(u => u.UiListId == emui.Ui_Id).FirstOrDefault();
        //                        var re = role.Where(u => u.Role_ListId == item.RoleId).FirstOrDefault();
        //                        item.RoleName = r.Role_Desc;
        //                        if (de != null)
        //                        {
        //                            if (de.UI_Part_linked_to == 0)
        //                            {
        //                                item.UiLevel = de.UI_Name_Label;
        //                                item.Menu1 = de.UI_Name_Label;
        //                            }
        //                            else
        //                            {
        //                                var menu2 = designation.Where(m => m.UiListId == de.UI_Part_linked_to).FirstOrDefault();
        //                                if (menu2.UI_Part_linked_to == 0)
        //                                {
        //                                    item.UiLevel = menu2.UI_Name_Label + "+" + de.UI_Name_Label;
        //                                    item.Menu2 = de.UI_Name_Label;
        //                                    item.Menu1 = menu2.UI_Name_Label;
        //                                }
        //                                else
        //                                {
        //                                    var menu3 = designation.Where(m => m.UiListId == menu2.UI_Part_linked_to).FirstOrDefault();
        //                                    if (menu3.UI_Part_linked_to == 0)
        //                                    {
        //                                        item.UiLevel = menu3.UI_Name_Label + "+" + menu2.UI_Name_Label + "+" + de.UI_Name_Label;
        //                                        item.Menu1 = menu3.UI_Name_Label;
        //                                        item.Menu2 = menu2.UI_Name_Label;
        //                                        item.Menu3 = de.UI_Name_Label;
        //                                    }
        //                                    else
        //                                    {
        //                                        var menu4 = designation.Where(m => m.UiListId == menu3.UI_Part_linked_to).FirstOrDefault();
        //                                        if (menu4.UI_Part_linked_to == 0)
        //                                        {
        //                                            item.UiLevel = menu4.UI_Name_Label + "+" + menu3.UI_Name_Label + "+" + menu2.UI_Name_Label + "+" + de.UI_Name_Label;
        //                                            item.Menu1 = menu4.UI_Name_Label;
        //                                            item.Menu2 = menu3.UI_Name_Label;
        //                                            item.Menu3 = menu2.UI_Name_Label;
        //                                            item.Menu4 = de.UI_Name_Label;
        //                                        }
        //                                        else
        //                                        {
        //                                            var menu5 = designation.Where(m => m.UiListId == menu4.UI_Part_linked_to).FirstOrDefault();
        //                                            if (menu5.UI_Part_linked_to == 0)
        //                                            {
        //                                                item.UiLevel = menu5.UI_Name_Label + "+" + menu4.UI_Name_Label + "+" + menu3.UI_Name_Label + "+" + menu2.UI_Name_Label + "+" + de.UI_Name_Label;
        //                                                item.Menu1 = menu5.UI_Name_Label;
        //                                                item.Menu2 = menu4.UI_Name_Label;
        //                                                item.Menu3 = menu3.UI_Name_Label;
        //                                                item.Menu4 = menu2.UI_Name_Label;
        //                                                item.Menu5 = de.UI_Name_Label;
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else if (emui.Access_Level == 2)
        //                        {
        //                            item.View_Allowed = "Y";
        //                            item.Add_Edit_Allowed = "N";
        //                            item.Delete_Allowed = "N";
        //                            item.Approval_Allowed = "N";
        //                        }
        //                        else if (emui.Access_Level == 3)
        //                        {
        //                            item.View_Allowed = "Y";
        //                            item.Add_Edit_Allowed = "Y";
        //                            item.Delete_Allowed = "N";
        //                            item.Approval_Allowed = "N";
        //                        }
        //                        else if (emui.Access_Level == 4)
        //                        {
        //                            item.View_Allowed = "Y";
        //                            item.Add_Edit_Allowed = "Y";
        //                            item.Delete_Allowed = "Y";
        //                            item.Approval_Allowed = "N";
        //                        }
        //                        else if (emui.Access_Level == 5)
        //                        {
        //                            item.View_Allowed = "Y";
        //                            item.Add_Edit_Allowed = "Y";
        //                            item.Delete_Allowed = "Y";
        //                            item.Approval_Allowed = "Y";
        //                        }
        //                        item.Add_dateStr = emui.Add_date.ToString("dd-MM-yyyy");
        //                        item.Deact_dateStr = emui.Deact_date != DateTime.MinValue
        //                                ? sorg.Deact_date.ToString("dd-MM-yyyy")
        //                                : string.Empty;
        //                        item.Active = emui.Active;
        //                        item.Role_Ui_ListId = emui.Employee_UI_ListId;
        //                        item.FromDept = "N";
        //                        item.FromDept = "N";
        //                        role_UI_ListVMs.Add(item);
        //                    }
        //                }
        //            }
        //        }
        //        return Ok(role_UI_ListVMs);

        //    }
        //    return Ok(sorgs);
        //}

        [HttpPost]
        public async Task<IActionResult> PostEmplRoleUiList(Empl_Role_ListVM model)
        {
            var result = await _employeeService.PostEmplRoleUiList(model);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DelEmplRoleUilist(long designationId)
        {
            var result = await _employeeService.DelEmplRoleUilist(designationId);
            return Ok(result);
        }
    }
}
