using CWB.App.Services.CompanySettings;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<JsonResult> GetDepartments(long Id)
        {
            var result = await _departmentService.GetDepartments(Id);
            return Json(result);
        }
    }
}
