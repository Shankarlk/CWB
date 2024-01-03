using CWB.CommonUtils.Common;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using CWB.Masters.Domain;
using CWB.Masters.MastersUtils;
using CWB.Masters.MastersUtils.ItemMaster;
using CWB.Masters.Services.Company;
using CWB.Masters.Services.ItemMaster;
using CWB.Masters.Services.Routings;
using CWB.Masters.ViewModels.Company;
using CWB.Masters.ViewModels.Routings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CWB.Masters.MastersUtils.ApiRoutes;

namespace CWB.Masters.Controllers
{
    [ApiController]
    [Authorize(Roles = Roles.ADMIN)]
    public class RoutingsController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRoutingService _routingService;
        private readonly IMasterPartService _masterPartService;
        private readonly IManufacturedPartNoDetailService _manufacturedPartNoDetailService;
        private readonly ICompanyService _companyService;
        public RoutingsController(ILoggerManager logger
            ,IRoutingService routingService
            , IMasterPartService masterPartService
            , IManufacturedPartNoDetailService manufacturedPartNoDetailService
            , ICompanyService companyService)
        { 
            _logger = logger;
            _routingService = routingService;
            _masterPartService = masterPartService;
            _companyService = companyService;
            _manufacturedPartNoDetailService = manufacturedPartNoDetailService;      
        }

        
        [HttpGet]
        [Route(ApiRoutes.Routings.RoutingList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(IEnumerable<RoutingVM>))]
        public IEnumerable<RoutingVM> RoutingList(int manufPartId)
        {
            return _routingService.GetRoutingsForManufId(manufPartId);
        }

        [HttpGet]
        [Route(ApiRoutes.Routings.RoutingSteps)]
        [Produces(AppContentTypes.ContentType, Type = typeof(IEnumerable<RoutingStepVM>))]
        public IEnumerable<RoutingStepVM> RoutingSteps(int routingId)
        {
            return _routingService.GetStepsForRoutingId(routingId);
        }

        [HttpGet]
        [Route(ApiRoutes.Routings.StepParts)]
        [Produces(AppContentTypes.ContentType, Type = typeof(IEnumerable<RoutingStepPartVM>))]
        public IEnumerable<RoutingStepPartVM> StepParts(int stepId)
        {
            return _routingService.GetPartsForStepId(stepId);
        }

        [HttpGet]
        [Route(ApiRoutes.Routings.StepPartsByManufId)]
        [Produces(AppContentTypes.ContentType, Type = typeof(IEnumerable<RoutingStepPartVM>))]
        public IEnumerable<RoutingStepPartVM> StepPartsByManuId(int manufId)
        {
            return _routingService.GetPartsForManufId(manufId);
        }

        




        [HttpGet]
        [Route(ApiRoutes.Routings.RoutingListItems)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<RoutingListItemVM>))]
        public async Task<List<RoutingListItemVM>> GetRoutingListItmes()
        {
             var manufParts = _manufacturedPartNoDetailService.GetAllManufacturedPartNoDetailsByTypeTenant(1).ToList();
             var partIds = from mfs in manufParts select mfs.PartId;
             List<int> partIdLilst = partIds.ToList();
             var mps = _masterPartService.GetAllMasterPartsWithIds(partIdLilst);
            var cos  = await _companyService.GetCompaniesByTenant(1);
            //var routings = await _routingService.GetAllRoutings();
            var query = from manuf in manufParts
                        join co in cos on manuf.CompanyId equals co.CompanyId
                        select new RoutingListItemVM
                        {
                            ManufacturedPartId = manuf.ManufacturedPartNoDetailId,
                            CompanyName = co.CompanyName,
                            MasterPartType = manuf.ManufacturedPartType == 1 ? "ManufacturedPart" : "Assembly"
                        };
            List<RoutingListItemVM> list = query.ToList();
            var query1 = from manuf in manufParts
                         join mp in mps on manuf.PartId equals mp.MasterPartId
                         select new RoutingListItemVM
                         {
                             ManufacturedPartId = manuf.ManufacturedPartNoDetailId,
                             PartNo = mp.PartNo,
                             PartDescription = mp.PartDescription
                         };
            List<RoutingListItemVM> list1 = query1.ToList();
            var query2 = from rli in list
                         join ril1 in list1 on rli.ManufacturedPartId equals ril1.ManufacturedPartId
                         select new RoutingListItemVM
                         {
                             ManufacturedPartId = rli.ManufacturedPartId,
                             CompanyName = rli.CompanyName,
                             PartNo = ril1.PartNo,
                             PartDescription = ril1.PartDescription,
                             MasterPartType = rli.MasterPartType,
                             Status = "---",
                             HasRouting = false
                         };
            List<RoutingListItemVM> retList = query2.ToList();
            /*foreach (var routing in routings)
            {
                foreach (var item in retList)
                {
                    if(routing.ManufacturedPartId == item.ManufacturedPartId)
                    {
                        item.HasRouting = true;
                    }
                }
            }*/
            return retList;
        }

        [HttpPost]
        [Route(ApiRoutes.Routings.PostNewRouting)]
        [Produces(AppContentTypes.ContentType, Type = typeof(RoutingVM))]
        public async Task<IActionResult> PostNewRouting([FromBody] RoutingVM routingVM)
        {
            /* var validator = new RawMaterialDetailVMValidator();
             var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
             if (!validationResult.IsValid)
                 return BadRequest(validationResult.Errors);*/
            try
            {
                var result = await _routingService.Routing(routingVM);
                return Ok(result);
            }catch (Exception ex)
            {
                var msg = ex.InnerException.Message;
                var src = ex.InnerException.Source;
                return Ok("Error");
            }
        }


        [HttpPost]
        [Route(ApiRoutes.Routings.PostRoutingStep)]
        [Produces(AppContentTypes.ContentType, Type = typeof(RoutingStepVM))]
        public async Task<IActionResult> PostRoutingStep([FromBody] RoutingStepVM routingStepVM)
        {
           /* var validator = new RawMaterialDetailVMValidator();
            var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);*/
            var result = await _routingService.RoutingStep(routingStepVM);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.Routings.GetRoutingStep)]
        [Produces(AppContentTypes.ContentType, Type = typeof(RoutingStepVM))]
        public async Task<IActionResult> GetRoutingStep(int stepId)
        {
            /* var validator = new RawMaterialDetailVMValidator();
             var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
             if (!validationResult.IsValid)
                 return BadRequest(validationResult.Errors);*/
            var result = await _routingService.GetStep(stepId);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.Routings.DelStep)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DelStep(int stepId)
        {
            /* var validator = new RawMaterialDetailVMValidator();
             var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
             if (!validationResult.IsValid)
                 return BadRequest(validationResult.Errors);*/
            var result = await _routingService.DelStep(stepId);
            return Ok(result);
        }
        


        [HttpPost]
        [Route(ApiRoutes.Routings.PostRoutingStepPart)]
        [Produces(AppContentTypes.ContentType, Type = typeof(RoutingStepPartVM))]
        public async Task<IActionResult> PostRoutingStepPart([FromBody] RoutingStepPartVM routingStepPartVM)
        {
          /*  var validator = new RawMaterialDetailVMValidator();
            var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);*/
            var result = await _routingService.RoutingStepPart(routingStepPartVM);
            return Ok(result);
        }

        [HttpPost]
        [Route(ApiRoutes.Routings.PostRoutingStepSupplier)]
        [Produces(AppContentTypes.ContentType, Type = typeof(RoutingStepSupplierVM))]
        public async Task<IActionResult> PostRoutingStepSupplier([FromBody] RoutingStepSupplierVM model)
        {
            /*  var validator = new RawMaterialDetailVMValidator();
              var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
              if (!validationResult.IsValid)
                  return BadRequest(validationResult.Errors);*/
            var result = await _routingService.RoutingStepSupplier(model);
            return Ok(result);
        }


        [HttpPost]
        [Route(ApiRoutes.Routings.PostRoutingStepMachine)]
        [Produces(AppContentTypes.ContentType, Type = typeof(RoutingStepMachineVM))]
        public async Task<IActionResult> PostRoutingStepMachine([FromBody] RoutingStepMachineVM model)
        {
            /*  var validator = new RawMaterialDetailVMValidator();
              var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
              if (!validationResult.IsValid)
                  return BadRequest(validationResult.Errors);*/
            var result = await _routingService.RoutingStepMachine(model);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.Routings.StepSuppliers)]
        [Produces(AppContentTypes.ContentType, Type = typeof(IEnumerable<RoutingStepSupplierVM>))]
        public async Task<IActionResult> StepSuppliers(int stepId)
        {
            /*  var validator = new RawMaterialDetailVMValidator();
              var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
              if (!validationResult.IsValid)
                  return BadRequest(validationResult.Errors);*/
            var result = await _routingService.StepSuppliers(stepId);
            return Ok(result);
        }


        [HttpGet]
        [Route(ApiRoutes.Routings.StepMachines)]
        [Produces(AppContentTypes.ContentType, Type = typeof(IEnumerable<RoutingStepMachineVM>))]
        public async Task<IActionResult> StepMachines(int stepId)
        {
            /*  var validator = new RawMaterialDetailVMValidator();
              var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
              if (!validationResult.IsValid)
                  return BadRequest(validationResult.Errors);*/
            var result = await _routingService.StepMachines(stepId);
            return Ok(result);
        }


        //stepsuppliers
    }
}


/***
 * 
 * 
        /**
         * 
        public List<RoutingListItemVM> GetRoutingListItmes()
        {
           // var manufParts = _manufacturedPartNoDetailService.GetAllManufacturedPartNoDetailsByTypeTenant().ToList();
            //var partIds = from mfs in manufParts select mfs.PartId;
            //List<int> partIdLilst = partIds.ToList();
           // var mps = _masterPartService.GetAllMasterPartsWithIds(partIdLilst);
           // var cos = _companyService.GetCompaniesByTenant(1);

            return null;
        }*/
//var coIds = from mfs in manufParts select mfs.CompanyId;
//List<int> companyIdList = coIds.ToList();
