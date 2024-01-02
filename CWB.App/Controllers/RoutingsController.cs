using CWB.App.Models.Routing;
using CWB.App.Services.Masters;
using CWB.App.Services.Routings;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class RoutingsController : Controller
    {

        private readonly ILoggerManager _logger;

        private readonly IRoutingService _routingService;
        private readonly IMastersServices _mastersServices;
        public RoutingsController(ILoggerManager logger, IRoutingService routingService,IMastersServices mastersServices)
        {
            _logger = logger;
            _routingService = routingService;
            _mastersServices = mastersServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> RoutingListItems()
        {
            var result = await _routingService.GetRoutingListItems();
            return Json(result);
        }
        //GetRoutingListItmes

        [HttpPost]
        public async Task<IActionResult> AddNewRouting(RoutingVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.Routing(model);
            return Ok(result);
        }


        public async Task<IActionResult> RoutingDetails(int manufPartId)
        {
            var result = await _routingService.GetRoutingListItems();
            var query = from litem in result
                        where litem.ManufacturedPartId == manufPartId
                        select litem;
            RoutingListItemVM routingListItemVM = query.FirstOrDefault();
            if(routingListItemVM == null)
            {
                routingListItemVM = new RoutingListItemVM();
                routingListItemVM.RoutingVMs = new List<RoutingVM>();
            }
            else
            {
                var resultList = await _routingService.Routings(manufPartId);
                routingListItemVM.RoutingVMs = resultList.ToList();
            }
            return View(routingListItemVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAltRouting(RoutingVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.Routing(model);
            return Ok(result);
        }

        public IActionResult AddNextStep()
        {
            return Ok("Ok");
        }

        [HttpPost]
        public async Task<IActionResult> SaveStep(RoutingStepVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.RoutingStep(model);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddBomToAssembly(RoutingStepPartVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.RoutingStepPart(model);
            return Ok(result);
        }

        public async Task<IActionResult> RoutingSteps(int routingId)
        {
            var result = await _routingService.RoutingSteps(routingId);
            return Ok(result);
        }

        public async Task<IActionResult> StepParts(int stepId)
        {
            var result = await _routingService.StepParts(stepId);
            return Ok(result);
        }


        public async Task<IActionResult> BOMs(int manufId, int stepId)
        {
            var boms = await _mastersServices.BOMS(""+manufId);
            var manfsps = await _routingService.StepPartsByManufId(manufId);
            var stepparts = await _routingService.StepParts(stepId);

            var query = from bom in boms
                        select new StepBOMVM
                        {
                            BOMPartNo = bom.BOMPartNo,
                            BOMManufPartId = bom.BOMManufPartId,
                            BOMPartId = bom.BOMPartId,
                            BOMPartDesc = bom.BOMPartDesc,
                            MPBOMId = bom.MPBOMId,
                            Quantity = bom.Quantity,
                            QuantityUsed = 0,
                            BalanceQuantity = bom.Quantity,
                            StepPartId = -1,
                            RoutingStepId = -1
                        };
            List<StepBOMVM> lst = query.ToList();
            //Calculate balance quantity for boms
            try
            {
                foreach (RoutingStepPartVM sp in manfsps)
                {
                    foreach (StepBOMVM spbom in lst)
                    {
                        if (spbom != null && sp != null)
                        {
                            if (spbom.MPBOMId == sp.BOMId)
                            {
                                spbom.QuantityUsed += sp.QuantityAssembly;
                                spbom.BalanceQuantity = spbom.Quantity - spbom.QuantityUsed;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                string msg = ex.InnerException.Message;
                string src = ex.InnerException.Source;
            }
            //reset quantity used on BOM... just for display purpoo
            foreach (StepBOMVM spbom in lst)
            {
                spbom.QuantityUsed = 0;
            }
            List<StepBOMVM> list1 = new List<StepBOMVM> ();
            foreach (RoutingStepPartVM sp in stepparts)
            {
                foreach (StepBOMVM bom in lst)
                {
                    if (bom.MPBOMId == sp.BOMId)
                    {
                        StepBOMVM obj = new StepBOMVM {
                            BOMPartNo = bom.BOMPartNo,
                            BOMManufPartId = bom.BOMManufPartId,
                            BOMPartId = bom.BOMPartId,
                            BOMPartDesc = bom.BOMPartDesc,
                            MPBOMId = bom.MPBOMId,
                            Quantity = 0,
                            QuantityUsed = sp.QuantityAssembly,
                            BalanceQuantity = 0,
                            StepPartId = (int)sp.RoutingStepPartId,
                            RoutingStepId = (int)sp.RoutingStepId
                        };
                        list1.Add(obj);
                        break;
                    }
                }
            }
            lst.AddRange(list1);
            return Ok(lst);
        }

        public async Task<IActionResult> StepBOMs(int stepId)
        {
            var result = await _routingService.StepParts(stepId);
            return Ok(result);
        }



        public IActionResult   Routings(int manufPartId)
        {
            return Ok("hello");
        }



        [HttpPost]
        public async Task<IActionResult> SaveStepSupplier(RoutingStepSupplierVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.RoutingStepSupplier(model);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> SaveStepMachine(RoutingStepMachineVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.RoutingStepMachine(model);
            return Ok(result);
        }

        public async Task<IActionResult> StepSuppliers(int stepId)
        {
            var result = await _routingService.StepSuppliers(stepId);
            return Ok(result);
        }

        public async Task<IActionResult> StepMachines(int stepId)
        {
            var result = await _routingService.StepMachines(stepId);
            return Ok(result);
        }

    }
}
