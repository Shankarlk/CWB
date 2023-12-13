using CWB.App.Services.CompanySettings;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class PlantController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IPlantService _plantService;

        public PlantController(ILoggerManager logger, IPlantService plantService)
        {
            _logger = logger;
            _plantService = plantService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetPlants()
        {
            var result = await _plantService.GetPlants();
            return Json(result);
        }


    }
}
