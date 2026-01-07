using CWB.App.Models.OperationList;
using CWB.App.Services.DocumentMagement;
using CWB.App.Services.Masters;
using CWB.App.Services.Routings;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class OperationListController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IOperationService _operationService;
        private readonly IDocMangService _docMangService;
        private readonly IRoutingService _routingService;
        private readonly IMastersServices _mastersServices;

        public OperationListController(ILoggerManager logger, IOperationService operationService,
            IDocMangService docMangService, IRoutingService routingService, IMastersServices mastersServices)
        {
            _logger = logger;
            _operationService = operationService;
            _docMangService = docMangService;
            _routingService = routingService;
            _mastersServices = mastersServices;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _operationService.GetOperationsList();
            foreach (var item in result)
            {
                if(item.IsMultiplePartsOfBOMUsed == true)
                {
                    item.Bom = "Y";
                }
                else
                {
                    item.Bom = "N";
                }
                if(item.Inhouse == 1)
                {
                    item.InhouseStr = "Y";
                }
                else
                {
                    item.InhouseStr = "N";
                }
                if(item.Subcon == 1)
                {
                    item.SubConstr = "Y";
                }
                else
                {
                    item.SubConstr = "N";
                }
            }
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Operations()
        {
            var result = await _operationService.GetOperationsList();
            foreach (var item in result)
            {
                if (item.IsMultiplePartsOfBOMUsed == true)
                {
                    item.Bom = "Y";
                }
                else
                {
                    item.Bom = "N";
                }
                if (item.Inhouse == 1)
                {
                    item.InhouseStr = "Y";
                }
                else
                {
                    item.InhouseStr = "N";
                }
                if (item.Subcon == 1)
                {
                    item.SubConstr = "Y";
                }
                else
                {
                    item.SubConstr = "N";
                }
            }
            return Ok(result);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Operation(OperationListVM model)
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

        //[HttpGet]
        //public async Task<JsonResult> GetDocTypes(long Id)
        //{
        //    var result = await _operationService.GetOperationDocTypes(Id);
        //    return Json(result);
        //}

        [HttpGet]
        public async Task<JsonResult> GetOperationalDocuments(long Id)
        {
            //var result = await _operationService.GetOperationDocTypesList(Id);
            var result = await _operationService.GetOperationalDocTypesByOptId(Id);
            var doctype = await _docMangService.GetAllDocumentType();
            foreach (var item in result)
            {
                foreach (var doc in doctype)
                {
                    if (item.DocumentTypeId ==doc.DocumentTypeId)
                    {
                        item.DocumentType = doc.DocumentName;
                    }
                }
                if (item.IsMandatory == true)
                {
                    item.IsMandatoryStr = "Y";
                }
                else
                {
                    item.IsMandatoryStr = "N";
                }
            }
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> DeletOperationDoc(long opDocId)
        {
            var result = await _operationService.DeletOperationDoc(opDocId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteOperations(long opDocId)
        {
            var allRoute = await _routingService.AllRoutings();
            var routemc = await _routingService.AllRoutingSteps();
            var parts = await _mastersServices.GetAllManufacturedPartNoDetailList();
            if (routemc.Any(x => x.StepOperation == opDocId.ToString()))
            {
                var rt = routemc.FirstOrDefault(x => x.StepOperation == opDocId.ToString());
                var routname = allRoute.FirstOrDefault(x => x.RoutingId == rt.RoutingId);
                var master =  parts.FirstOrDefault(p => p.ManufacturedPartNoDetailId == routname.ManufacturedPartId);
                var masterpart = await _mastersServices.ItemMasterPartById(master.PartId);
                var routingName = routname?.RoutingName ?? "Unknown Routing";
                var stepNumber = rt.StepNumber?.ToString() ?? "Unknown Step";
                var partNo = masterpart?.PartNo ?? "Unknown Part";

                string msg =
                    $"This Operation is already used in the Routing {routingName} " +
                    $"and Routing Step : {stepNumber} of the PartNo {partNo}. " +
                    $"Delete the Routing Step in the Routings.";
                return Ok(msg);
            }
            var result = await _operationService.DeleteOperations(opDocId);
            return Ok(result);
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
