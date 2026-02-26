using CWB.CommonUtils.Common;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using CWB.Masters.Domain;
using CWB.Masters.Domain.Routings;
using CWB.Masters.MastersUtils;
using CWB.Masters.MastersUtils.ItemMaster;
using CWB.Masters.Services.Company;
using CWB.Masters.Services.DocumentManagement;
using CWB.Masters.Services.ItemMaster;
using CWB.Masters.Services.OperationList;
using CWB.Masters.Services.Routings;
using CWB.Masters.ViewModels.Company;
using CWB.Masters.ViewModels.OperationList;
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
        private readonly IDocumentManagementService _documentManagementService;
        private readonly IOperationListService _operationListService;
        public RoutingsController(ILoggerManager logger
            ,IRoutingService routingService
            , IMasterPartService masterPartService
            , IManufacturedPartNoDetailService manufacturedPartNoDetailService
            , ICompanyService companyService, IDocumentManagementService documentManagementService
            , IOperationListService operationListService)
        { 
            _logger = logger;
            _routingService = routingService;
            _masterPartService = masterPartService;
            _companyService = companyService;
            _manufacturedPartNoDetailService = manufacturedPartNoDetailService;
            _documentManagementService = documentManagementService;
            _operationListService = operationListService;
        }

        
        [HttpGet]
        [Route(ApiRoutes.Routings.RoutingList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(IEnumerable<RoutingVM>))]
        public IEnumerable<RoutingVM> RoutingList(int manufPartId)
        {
            return _routingService.GetRoutingsForManufId(manufPartId);
        }
        [HttpGet]
        [Route(ApiRoutes.Routings.OptimizedRoutingListItems)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<RoutingListItemVM>))]
        public async Task<IActionResult> RoutingListItems(long tenantId)
        {
            // Step 1: Fetch Routing List Items using the Service
            var result = await _routingService.GetRoutingListItemsAsync(tenantId);
            if (result == null || !result.Any())
                return Ok(new List<RoutingListItemVM>());

            // Fetch Document List
            var docListVMs = (await _documentManagementService.GetAllDocList(tenantId)).ToList();

            // Step 2: Fetch all routing steps once
            var routingStepsMap = new Dictionary<long, List<RoutingStepVM>>();
            foreach (var item in result)
            {
                // Note: GetStepsForRoutingId is synchronous in IRoutingService interface provided
                var steps = _routingService.GetStepsForRoutingId(item.RoutingId);
                if (steps != null)
                {
                    routingStepsMap[item.RoutingId] = steps.ToList();
                }
            }

            // Step 3: Collect all unique StepOperation IDs
            var allStepOps = routingStepsMap.Values
                .SelectMany(x => x)
                .Select(s => Convert.ToInt64(s.StepOperation))
                .Distinct()
                .ToList();

            // Step 4: Prefetch all doc types
            var opDocsMap = new Dictionary<long, List<OperationalDocumentListVM>>();
            foreach (var opId in allStepOps)
            {
                // Note: GetOperationDocumentTypes is synchronous in IOperationListService interface provided
                var opDocs = _operationListService.GetOperationDocumentTypes(tenantId, opId);
                if (opDocs != null && opDocs.Any())
                    opDocsMap[opId] = opDocs.ToList();
            }

            // Step 5: Convert docListVMs to HashSet for O(1) lookups
            // Assuming DocListVM has RoutingId property as implied by the logic
            var docSet = new HashSet<(long DocumentTypeId, long RoutingId)>(
                docListVMs.Select(d => (d.DocumentTypeId, d.RoutingId))
            );

            // Step 6: Logic to determine MandocAvl
            foreach (var item in result)
            {
                if (!routingStepsMap.TryGetValue(item.RoutingId, out var steps) || steps.Count == 0)
                {
                    item.MandocAvl = "N/A";
                    continue;
                }

                bool anyYes = false;
                bool anyNo = false;

                foreach (var op in steps)
                {
                    var opId = Convert.ToInt64(op.StepOperation);

                    if (!opDocsMap.TryGetValue(opId, out var docmand))
                    {
                        item.MandocAvl = "N/A";
                        break;
                    }

                    foreach (var docMand in docmand)
                    {
                        if (docSet.Contains((docMand.DocumentTypeId, item.RoutingId)))
                        {
                            anyYes = true;
                        }
                        else
                        {
                            anyNo = true;
                        }

                        if (anyYes && anyNo)
                            break;
                    }

                    if (item.MandocAvl == "N/A" || (anyYes && anyNo))
                        break;
                }

                if (item.MandocAvl != "N/A")
                {
                    item.MandocAvl = anyYes ? "Yes" : "No";
                }
            }

            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.Routings.RoutingAllList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(IEnumerable<RoutingVM>))]
        public IEnumerable<RoutingVM> RoutingAllList(long tenantId)
        {
            return _routingService.GetAllRoutingsForManufId(tenantId);
        }

        [HttpGet]
        [Route(ApiRoutes.Routings.RoutingSteps)]
        [Produces(AppContentTypes.ContentType, Type = typeof(IEnumerable<RoutingStepVM>))]
        public IEnumerable<RoutingStepVM> RoutingSteps(int routingId)
        {
            return _routingService.GetStepsForRoutingId(routingId);
        }
        [HttpGet]
        [Route(ApiRoutes.Routings.AllRoutingSteps)]
        [Produces(AppContentTypes.ContentType, Type = typeof(IEnumerable<RoutingStepVM>))]
        public IEnumerable<RoutingStepVM> GetAllStepsForRouting(long tenantId)
        {
            return _routingService.GetAllStepsForRouting(tenantId);
        }
        [HttpGet]
        [Route(ApiRoutes.Routings.GetRoutingNameStepPartName)]
        [Produces(AppContentTypes.ContentType, Type = typeof(IEnumerable<RoutingStepVM>))]
        public IEnumerable<RoutingStepVM> GetRoutingNameStepPartName(long opid, long tenantId)
        {
            return _routingService.GetRoutingNameStepPartName(opid, tenantId);
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
        public async Task<List<RoutingListItemVM>> GetRoutingListItmes(long tenantId)
        {
            // Fetch all needed data upfront
            var manufParts = _manufacturedPartNoDetailService
                .GetAllManufacturedPartNoDetailsByTypeTenant(tenantId)
                .ToList();

            var partIds = manufParts.Select(mfs => mfs.PartId).ToList();

            var mps = _masterPartService.GetAllMasterPartsWithIds(partIds).ToList();
            var cos = (await _companyService.GetCompaniesByTenant(tenantId)).ToList();
            var routings = (await _routingService.GetAllRoutings()).ToList();

            // Convert routings into lookup for faster O(1) matching
            var routingLookup = routings
                .GroupBy(r => r.ManufacturedPartId)
                .ToDictionary(
                    g => g.Key,
                    g => new
                    {
                        Count = g.Count(),
                        First = g.FirstOrDefault()
                    });

            // Single LINQ query combining everything
            var retList = (from manuf in manufParts
                           join co in cos on manuf.CompanyId equals co.CompanyId
                           join mp in mps on manuf.PartId equals mp.MasterPartId
                           select new RoutingListItemVM
                           {
                               ManufacturedPartId = manuf.ManufacturedPartNoDetailId,
                               CompanyName = co.CompanyName,
                               PartNo = mp.PartNo,
                               PartDescription = mp.PartDescription,
                               MasterPartType = manuf.ManufacturedPartType == 1 ? "ManufacturedPart" : "Assembly",
                               NoOfRoutes = routingLookup.ContainsKey(manuf.ManufacturedPartNoDetailId)
                                                ? routingLookup[manuf.ManufacturedPartNoDetailId].Count
                                                : 0,
                               RoutingId = routingLookup.ContainsKey(manuf.ManufacturedPartNoDetailId)
                                                ? (int)routingLookup[manuf.ManufacturedPartNoDetailId].First.Id
                                                : 0,
                               Status = mp.Status,
                               HasRouting = routingLookup.ContainsKey(manuf.ManufacturedPartNoDetailId)
                           }).ToList();

            return retList;
            //var result = await _routingService.GetRoutingListItemsAsync(tenantId);
            //return result;
            // var manufParts = _manufacturedPartNoDetailService.GetAllManufacturedPartNoDetailsByTypeTenant(tenantId).ToList();
            // var partIds = from mfs in manufParts select mfs.PartId;
            // List<int> partIdLilst = partIds.ToList();
            // var mps = _masterPartService.GetAllMasterPartsWithIds(partIdLilst);
            //var cos  = await _companyService.GetCompaniesByTenant(tenantId);

            //var query = from manuf in manufParts
            //            join co in cos on manuf.CompanyId equals co.CompanyId
            //            select new RoutingListItemVM
            //            {
            //                ManufacturedPartId = manuf.ManufacturedPartNoDetailId,
            //                CompanyName = co.CompanyName,
            //                MasterPartType = manuf.ManufacturedPartType == 1 ? "ManufacturedPart" : "Assembly"
            //            };
            //List<RoutingListItemVM> list = query.ToList();
            //var query1 = from manuf in manufParts
            //             join mp in mps on manuf.PartId equals mp.MasterPartId
            //             select new RoutingListItemVM
            //             {
            //                 ManufacturedPartId = manuf.ManufacturedPartNoDetailId,
            //                 PartNo = mp.PartNo,
            //                 PartDescription = mp.PartDescription
            //             };
            //List<RoutingListItemVM> list1 = query1.ToList();
            //var query2 = from rli in list
            //             join ril1 in list1 on rli.ManufacturedPartId equals ril1.ManufacturedPartId
            //             select new RoutingListItemVM
            //             {
            //                 ManufacturedPartId = rli.ManufacturedPartId,
            //                 CompanyName = rli.CompanyName,
            //                 PartNo = ril1.PartNo,
            //                 PartDescription = ril1.PartDescription,
            //                 MasterPartType = rli.MasterPartType,
            //                 Status = "---",
            //                 HasRouting = false,
            //                 NoOfRoutes = 0

            //             };
            //List<RoutingListItemVM> retList = query2.ToList();
            //try
            //{
            //    var routings = await _routingService.GetAllRoutings();
            //    foreach (var routing in routings)
            //    {
            //        foreach (var item in retList)
            //        {
            //            if(routing.ManufacturedPartId == item.ManufacturedPartId)
            //            {
            //                item.NoOfRoutes++;
            //                item.RoutingId = (int)routing.Id;
            //                item.Status = routing.Status;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex) { }
            //return retList;
        }
        
        [HttpGet]
        [Route(ApiRoutes.Routings.PostDeleteRouting)]
        [Produces(AppContentTypes.ContentType, Type = typeof(RoutingVM))]
        public async Task<IActionResult> PostDeleteRouting(int routingId)
        {
            /* var validator = new RawMaterialDetailVMValidator();
             var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
             if (!validationResult.IsValid)
                 return BadRequest(validationResult.Errors);*/
            try
            {
                var result = await _routingService.DeleteRouting(routingId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException.Message;
                var src = ex.InnerException.Source;
                return Ok("Error");
            }
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

                // 2. Copy only if OrigRoutingId exists
                if (routingVM.OrigRoutingId > 0 && result.RoutingId > 0)
                {
                    int originRoutingId = routingVM.OrigRoutingId;
                    int newRoutingId = result.RoutingId;

                    await _routingService.CopySteps(originRoutingId, newRoutingId);
                    await _routingService.CopyStepParts(originRoutingId, newRoutingId);
                    await _routingService.CopyStepMachines(originRoutingId, newRoutingId);
                    await _routingService.CopySubCons(originRoutingId, newRoutingId);
                    await _routingService.CopySubConsWSs(originRoutingId, newRoutingId);
                }

                // 3. Always return created routing
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException.Message;
                var src = ex.InnerException.Source;
                return Ok("Error");
            }
        }


        [HttpPost]
        [Route(ApiRoutes.Routings.PreferredRouting)]
        [Produces(AppContentTypes.ContentType, Type = typeof(RoutingVM))]
        public async Task<IActionResult> PreferredRouting([FromBody] RoutingVM routingVM)
        {
            /* var validator = new RawMaterialDetailVMValidator();
             var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
             if (!validationResult.IsValid)
                 return BadRequest(validationResult.Errors);*/
            try
            {
                var result = await _routingService.PreferredRouting(routingVM);
                return Ok(result);
            }
            catch (Exception ex)
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
        [HttpPost]
        [Route(ApiRoutes.Routings.ChangeRoutingStepSequence)]
        [Produces(AppContentTypes.ContentType, Type = typeof(RoutingStepVM))]
        public async Task<IActionResult> ChangeSequence([FromBody] IEnumerable<RoutingStepVM> routingStepVM)
        {
           /* var validator = new RawMaterialDetailVMValidator();
            var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);*/
            var result = await _routingService.ChangeSequence(routingStepVM);
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
        [Route(ApiRoutes.Routings.DeleteStep)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteStep(int stepId)
        {
            /* var validator = new RawMaterialDetailVMValidator();
             var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
             if (!validationResult.IsValid)
                 return BadRequest(validationResult.Errors);*/
            var result = await _routingService.DeleteStep(stepId);
            return Ok(result);
        }
        

        [HttpGet]
        [Route(ApiRoutes.Routings.DeleteStepPart)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteStepPart(int stepId, int stepPartId)
        {
            /* var validator = new RawMaterialDetailVMValidator();
             var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
             if (!validationResult.IsValid)
                 return BadRequest(validationResult.Errors);*/
            var result = await _routingService.DeleteStepPart(stepId, stepPartId);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.Routings.DeleteStepMachine)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteStepMachine(int stepId,int machineId)
        {
            /* var validator = new RawMaterialDetailVMValidator();
             var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
             if (!validationResult.IsValid)
                 return BadRequest(validationResult.Errors);*/
            var result = await _routingService.DeleteStepMachine(stepId,machineId);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.Routings.DeleteSubConDetails)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteSubConDetails(int subConDetailsId,int stepId)
        {
            /* var validator = new RawMaterialDetailVMValidator();
             var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
             if (!validationResult.IsValid)
                 return BadRequest(validationResult.Errors);*/
            var result = await _routingService.DeleteSubConDetails(subConDetailsId,stepId);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.Routings.DeleteSubWSConDetails)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteSubConWSDetails(int subConWSId)
        {
            /* var validator = new RawMaterialDetailVMValidator();
             var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
             if (!validationResult.IsValid)
                 return BadRequest(validationResult.Errors);*/
            var result = await _routingService.DeleteSubConWSDetails(subConWSId);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.Routings.DeleteStepSupplier)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteStepSupplier(int stepId,int supplierId)
        {
            /* var validator = new RawMaterialDetailVMValidator();
             var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
             if (!validationResult.IsValid)
                 return BadRequest(validationResult.Errors);*/
            var result = await _routingService.DeleteStep(stepId);
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

        [HttpGet]
        [Route(ApiRoutes.Routings.PreferredStepMachine)]
        [Produces(AppContentTypes.ContentType, Type = typeof(RoutingStepMachineVM))]
        public async Task<IActionResult> PreferredStepMachine(string routingStepId,string routingStepMachineId, int maxMachineCount)
        {
            /*  var validator = new RawMaterialDetailVMValidator();
              var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
              if (!validationResult.IsValid)
                  return BadRequest(validationResult.Errors);*/
            var result = await _routingService.PreferredStepMachine(routingStepId,routingStepMachineId,maxMachineCount);
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
        [HttpGet]
        [Route(ApiRoutes.Routings.AllStepMachines)]
        [Produces(AppContentTypes.ContentType, Type = typeof(IEnumerable<RoutingStepMachineVM>))]
        public async Task<IActionResult> AllStepMachines(long tenantId)
        {
            /*  var validator = new RawMaterialDetailVMValidator();
              var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
              if (!validationResult.IsValid)
                  return BadRequest(validationResult.Errors);*/
            var result = await _routingService.AllStepMachines(tenantId);
            return Ok(result);
        }

        [HttpPost]
        [Route(ApiRoutes.Routings.SubConDetails)]
        [Produces(AppContentTypes.ContentType, Type = typeof(SubConDetailsVM))]
        public async Task<IActionResult> SubConDetails([FromBody] SubConDetailsVM subConDetailsVM)
        {
            /* var validator = new RawMaterialDetailVMValidator();
             var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
             if (!validationResult.IsValid)
                 return BadRequest(validationResult.Errors);*/
            var result = await _routingService.AddSubConDetails(subConDetailsVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Routings.SubConWSDetails)]
        [Produces(AppContentTypes.ContentType, Type = typeof(SubConWorkStepDetailsVM))]
        public async Task<IActionResult> Sub([FromBody] SubConWorkStepDetailsVM subConWorkStepDetailsVM)
        {
            /* var validator = new RawMaterialDetailVMValidator();
             var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
             if (!validationResult.IsValid)
                 return BadRequest(validationResult.Errors);*/
            var result = await _routingService.AddSubConWorkStepDetails(subConWorkStepDetailsVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Routings.SubCons)]
        [Produces(AppContentTypes.ContentType, Type = typeof(IEnumerable<SubConDetailsVM>))]
        public async Task<IActionResult> SubCons(int stepId)
        {
            /*  var validator = new RawMaterialDetailVMValidator();
              var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
              if (!validationResult.IsValid)
                  return BadRequest(validationResult.Errors);*/
            var result = await _routingService.SubCons(stepId);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Routings.SubConWSS)]
        [Produces(AppContentTypes.ContentType, Type = typeof(IEnumerable<SubConWorkStepDetailsVM>))]
        public async Task<IActionResult> SubConWSS(int stepId,int subConDetailsId)
        {
            /*  var validator = new RawMaterialDetailVMValidator();
              var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
              if (!validationResult.IsValid)
                  return BadRequest(validationResult.Errors);*/
            var result = await _routingService.SubConWSS(stepId,subConDetailsId);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.Routings.PreferredSubCon)]
        [Produces(AppContentTypes.ContentType, Type = typeof(SubConDetailsVM))]
        public async Task<IActionResult> PreferredSubCon(int subConDetailsId)
        {
            /* var validator = new RawMaterialDetailVMValidator();
             var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
             if (!validationResult.IsValid)
                 return BadRequest(validationResult.Errors);*/
            try
            {
                var result = await _routingService.PreferredSubCon(subConDetailsId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException.Message;
                var src = ex.InnerException.Source;
                return Ok("Error");
            }
        }

        [HttpPost]
        [Route(ApiRoutes.Routings.PostStatusLog)]
        [Produces(AppContentTypes.ContentType, Type = typeof(RoutingStepVM))]
        public async Task<IActionResult> PostStatusLog([FromBody] RoutingStatusLogVM routingStepVM)
        {
            var result = await _routingService.PostRoutingStatusLog(routingStepVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Routings.GetStatusLog)]
        [Produces(AppContentTypes.ContentType, Type = typeof(IEnumerable<RoutingStepMachineVM>))]
        public async Task<IActionResult> GetStatusLog(int routingId,long tenantId)
        {
            var result = _routingService.GetRoutingStatusLog(routingId, tenantId);
            return Ok(result);
        }
        //preferredsubcon

        [HttpPost]
        [Route(ApiRoutes.Routings.AltRouting)]
        [Produces(AppContentTypes.ContentType, Type = typeof(RoutingVM))]
        public async Task<IActionResult> AltRouting([FromBody] RoutingVM routingVM)
        {
            /* var validator = new RawMaterialDetailVMValidator();
             var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
             if (!validationResult.IsValid)
                 return BadRequest(validationResult.Errors);*/
            try
            {
                int origroutingid = routingVM.OrigRoutingId;
                var result = await _routingService.AltRouting(routingVM);
                int newroutingid = routingVM.RoutingId;
                if (newroutingid > 0)
                {
                    await _routingService.CopySteps(origroutingid, newroutingid);
                    await _routingService.CopyStepParts(origroutingid, newroutingid);
                    await _routingService.CopyStepMachines(origroutingid, newroutingid);
                    await _routingService.CopySubCons(origroutingid, newroutingid);
                    await _routingService.CopySubConsWSs(origroutingid, newroutingid);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException.Message;
                var src = ex.InnerException.Source;
                return Ok("Error");
            }
        }
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
