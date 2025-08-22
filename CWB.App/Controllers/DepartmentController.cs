using CWB.App.Models.Departments;
using CWB.App.Services.CompanySettings;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class DepartmentController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IDepartmentService _departmentService;

        public DepartmentController(ILoggerManager logger, IDepartmentService departmentService)
        {
            _logger = logger;
            _departmentService = departmentService;
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
            return Json(result);
        }
    }
}
