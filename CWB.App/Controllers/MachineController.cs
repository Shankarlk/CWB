using CWB.App.Models.Machine;
using CWB.App.Services.CompanySettings;
using CWB.App.Services.Masters;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class MachineController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMachineService _machineService;
        private readonly IPlantService _plantService;
        private readonly IOperationService _operationService;

        public MachineController(ILoggerManager logger, IMachineService machineService, IPlantService plantService,
            IOperationService operationService)
        {
            _logger = logger;
            _machineService = machineService;
            _plantService = plantService;
            _operationService = operationService;
        }
        public async Task<IActionResult> Index()
        {
            await PlantsViewBag();
            var machinesList = await _machineService.GetMachinesList();
            return View(machinesList);
        }

        #region Machine Type        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MachineType(MachineTypeVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _machineService.MachineType(model);
            return Ok(result);
        }

        [HttpPost]
        public async Task<JsonResult> IsMachineTypeExist(long? MachineTypeTypeId, string MachineTypeName)
        {
            var result = await _machineService.CheckMachineType(new MachineTypeVM
            {
                MachineTypeName = MachineTypeName,
                MachineTypeTypeId = MachineTypeTypeId.HasValue ? MachineTypeTypeId.Value : 0
            });
            return Json(!result);
        }

        #endregion

        #region Machine

        [HttpPost]
        public async Task<JsonResult> IsMachineNameExist(long? MachineMachineId, string MachineMachineName)
        {
            var result = await _machineService.CheckMachine(new CheckMachineVM
            {
                CompareValue = MachineMachineName,
                MachineId = MachineMachineId.HasValue ? MachineMachineId.Value : 0,
                CompareKey = "Name"
            });
            return Json(!result);
        }

        [HttpPost]
        public async Task<JsonResult> IsMachineSlNoExist(long? MachineMachineId, string MachineMachineSlNo)
        {
            var result = await _machineService.CheckMachine(new CheckMachineVM
            {
                CompareValue = MachineMachineSlNo,
                MachineId = MachineMachineId.HasValue ? MachineMachineId.Value : 0,
                CompareKey = "SlNo"
            });
            return Json(!result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Machine(MachineVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _machineService.Machine(model);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Machine(long Id)
        {
            var result = await _machineService.GetMachine(Id);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetMachines()
        {
            var machinesList = await _machineService.GetMachinesList();
            return Ok(machinesList);
        }

        #endregion

        #region Machine Proc Document

        [HttpGet]
        public async Task<JsonResult> GetDocTypes(long Id)
        {
            var result = await _machineService.GetMachineDocTypes(Id);
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetMachineProcDocs(long Id)
        {
            var result = await _machineService.GetMachineProcsDocLists(Id);
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MachineProcDoc(MachineProcDocumentVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _machineService.MachineProcDoc(model);
            return Ok(result);
        }
        #endregion

        #region Private Functions - ViewBag
        private async Task PlantsViewBag()
        {
            var plants = await _plantService.GetPlants();
            ViewBag.Plants = plants.Select(p => new SelectListItem { Text = p.Name, Value = p.PlantId.ToString() }).ToList();

            var machineTypes = await _machineService.GetMachineTypes();
            ViewBag.MachineTypes = machineTypes.Select(m => new SelectListItem { Text = m.MachineTypeName, Value = m.MachineTypeTypeId.ToString() }).ToList();

            var operations = await _operationService.GetOperationsList();
            ViewBag.Operations = operations.Select(m => new SelectListItem { Text = m.Operation, Value = m.OperationId.ToString() }).ToList();
        }


        #endregion
    }
}
