using CWB.App.Models.OperationList;
using CWB.App.Services.Masters;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class OperationListController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IOperationService _operationService;

        public OperationListController(ILoggerManager logger, IOperationService operationService)
        {
            _logger = logger;
            _operationService = operationService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _operationService.GetOperationsList();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Operations()
        {
            var result = await _operationService.GetOperationsList();
            return Ok(result);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Operation(OperationVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _operationService.Operation(model);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Operation(long Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _operationService.Operation(Id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<JsonResult> IsOperationExist(long? OperationId, string Operation)
        {
            var result = await _operationService.CheckIfOperationExisit(OperationId.HasValue ? OperationId.Value : 0, Operation);
            return Json(!result);
        }

        [HttpGet]
        public async Task<JsonResult> GetDocTypes(long Id)
        {
            var result = await _operationService.GetOperationDocTypes(Id);
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetOperationalDocuments(long Id)
        {
            var result = await _operationService.GetOperationDocTypesList(Id);
            return Json(result);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OperationalDocuments(OperationDocumentTypeVM operationDocumentTypeVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _operationService.OperationDocument(operationDocumentTypeVM);
            return Ok(result);
        }
    }
}
