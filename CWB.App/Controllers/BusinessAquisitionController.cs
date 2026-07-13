using CWB.App.AppUtils;
using CWB.App.Models.BusinessProcesses;
using CWB.App.Models.ItemMaster;
using CWB.App.Models.Routing;
using CWB.App.Services.BusinessProcesses;
using CWB.App.Services.Masters;
using CWB.App.Services.ProductionPlanWo;
using CWB.App.Services.Routings;
using CWB.App.Services.CompanySettings;
using CWB.App.Models.Plants;
using CWB.Constants.UserIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using CWB.App.Models.WorkOrder;

namespace CWB.App.Controllers
{

    /**
     *    public const string GetPOLogs = Base + "/getpologs/{tenantId}";
            public const string GetSalesOrders = Base + "/getsalesorders/{tenantId}/{customerOderId}";
            public const string GetCustomerOrders = Base + "/getcustomerorders/{tenantId}";
            public const string GetSchedules = Base + "/getdeliveryschedules/{tenantId}/{customerOrderId}";
            public const string HelloWorld = Base + "/helloworld/{tenantId}";

            public const string PostSalesOrder = Base + "/salesorder";
            public const string PostCustomerOrder = Base + "/custorder";
            public const string PostDeliverySchedule = Base + "/deliveryschedule";
            public const string PostOrderStatus = Base + "/orderstatus";
            public const string PostPOLog = Base + "/polog";

            public const string RemoveSalesOrder = Base + "/removesalesorder/{tenantId}/{salesOrderId}";
            public const string RemoveCustomerOder = Base + "/removecustomerorder/{tenantId}/{customerOrderId}";
            public const string RemoveDeliverSchedule = Base + "/removedeliveryschedule/{tenantId}/{scheduleId}";
            public const string RemoveOrderStatus = Base + "/removeorderstatus/{tenantId}/{orderStatusId}";
     */

    [Authorize(Roles = Roles.ADMIN)]
    public class BusinessAquisitionController : Controller
    {
        private readonly ILogger<BusinessAquisitionController> _logger;
        private readonly IBAService _baService;
        private readonly IMastersServices _masterService;
        private readonly IRoutingService _routingService;
        private readonly IWOService _woService;
        private readonly IMachineService _machineService;
        private readonly IDepartmentService _departmentService;
        private readonly IPlantService _plantService;
        public BusinessAquisitionController(ILogger<BusinessAquisitionController> logger
            , IWOService woService,
            IBAService baService,IMastersServices masterServices, IRoutingService routingService, IPlantService plantService
            , IMachineService machineService, IDepartmentService departmentService)
        {
            _logger = logger;
            _baService = baService;
            _masterService = masterServices;
            _routingService = routingService;
            _woService = woService;
            _plantService = plantService;
            _machineService = machineService;
            _departmentService = departmentService;
        }
        public IActionResult Index()
        {
            _logger.LogTrace("BA--Index--Loading");
            return View();
        }

        //[HttpGet]
        //public IActionResult WO()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> WOpost(WorkOrdersVM workOrdersVM)
        {
            List<WorkOrdersVM> workOrdersVMs = new List<WorkOrdersVM>();
            List<WorkOrdersVM> timeslotwo = new List<WorkOrdersVM>();
            WorkOrdersVM postWO = null;
            var message = await checkmissing(workOrdersVM);
            Selected_Sales_OrderVM selected_Sales_OrderVMs1 = (Selected_Sales_OrderVM)((OkObjectResult)message).Value;
            
            if (selected_Sales_OrderVMs1.PartNo == "WorkOrder Created")
            {
                
            

                  ManufacturedPartNoDetailVM manuf = await _masterService.GetManufPart((int)workOrdersVM.PartId);
                if (manuf.ManufacturedPartType == 1)
                {
                    var mpmakefromlist = await _masterService.GetMPMakeFromListByPartId(manuf.ManufacturedPartNoDetailId.ToString());

                    var selectedMakeFrom = mpmakefromlist.OrderByDescending(x => x.PreferedRawMaterial).FirstOrDefault();
                    workOrdersVM.Input_Part_No = selectedMakeFrom.MPPartId;

                }
                else
                {
                    workOrdersVM.Input_Part_No = 0;
                }
                workOrdersVM.PartType = (int)manuf.ManufacturedPartType;
            RoutingVM rout = new RoutingVM();
            var resultList = await _routingService.Routings((int)manuf.ManufacturedPartNoDetailId);
            foreach (var item in resultList)
            {
                if(item.PreferredRouting == 1)
                {
                    rout = item;
                }
                else
                {
                    rout= (resultList).Take(1).FirstOrDefault();
                }
            }
            if(workOrdersVM.WOID == 0)
            {
                var result = (await _routingService.RoutingSteps(rout.RoutingId)).Take(1).FirstOrDefault();
                var reverse = (await _routingService.RoutingSteps(rout.RoutingId)).Take(1).LastOrDefault();
                workOrdersVM.RoutingId = rout.RoutingId;
                workOrdersVM.StartingOpNo = result?.StepId != null
                                            ? int.TryParse(result.StepId.ToString(), out int opNo) ? opNo : 0
                                            : 0;
                workOrdersVM.EndingOpNo = reverse?.StepId != null
                                            ? int.TryParse(reverse.StepId.ToString(), out int eopNo) ? eopNo : 0
                                            : 0;
            }
            if (workOrdersVM.PartType == 1)
            {
                workOrdersVM.Parentlevel = 'N';
                
            }
            else
            {
                var mpBOM = await _masterService.BOMS(workOrdersVM.PartId.ToString());
                if (mpBOM != null && mpBOM.Any())
                {
                    foreach (var bom in mpBOM)
                    {
                        var assy = await _masterService.GetManufPart((int)bom.BOMPartId);
                        if (assy != null)
                        {
                            workOrdersVM.Parentlevel = 'Y';
                        }
                        else
                        {
                            workOrdersVM.Parentlevel = 'N';
                        }
                    }
                }
                else
                {
                    workOrdersVM.Parentlevel = 'Y';
                }
            }
                long quantitynotallocated = 0;
                var departmentstores = await _departmentService.GetAllStoresIDs();
                var invpart = await _woService.GetAllInventory_MasterBypartidWithFlag("Internal", departmentstores.Stores_DirMatl_ID, 0, 0, workOrdersVM.PartId);
                if (invpart.Any())
                {
                    var inventorypart = invpart.First();
                    if (inventorypart != null)
                    {
                        var invcount = Convert.ToInt64(inventorypart.Current_QntOnHand);
                        var varsoallocationbypartid = await _baService.GetSOAllocationlistbyPartid(workOrdersVM.PartId);
                        if (varsoallocationbypartid.Any())
                        {
                            var totalAllocatedQty = varsoallocationbypartid.Where(x => x.Dispatch_Complete == 'N').Sum(x => x.Allocated_Qnty);
                            if (totalAllocatedQty > 0)
                            {
                                quantitynotallocated = Convert.ToInt64(inventorypart.Current_QntOnHand) - totalAllocatedQty;
                            }
                            else
                            {
                                quantitynotallocated = Convert.ToInt64(inventorypart.Current_QntOnHand);
                            }
                        }
                        else
                        {
                            quantitynotallocated = Convert.ToInt64(inventorypart.Current_QntOnHand);
                        }

                    }
                    else
                    {
                        quantitynotallocated = 0;
                    }

                    if (quantitynotallocated < 0)
                        quantitynotallocated = 0;
                }
                else
                {
                    quantitynotallocated = 0;
                }
                if (quantitynotallocated == 0)
                {
                    postWO = await _baService.PostWO(workOrdersVM);
                }
                else if (quantitynotallocated < workOrdersVM.CalcWOQty)
                {

                    workOrdersVM.CalcWOQty = workOrdersVM.CalcWOQty - Convert.ToInt32(quantitynotallocated);
                    postWO = await _baService.PostWO(workOrdersVM);
                }
                else if (quantitynotallocated >= workOrdersVM.CalcWOQty)
                {
                    SO_Alloc_ListVM co = new SO_Alloc_ListVM()
                    {
                        PartId = workOrdersVM.PartId,
                        SO_ID = workOrdersVM.SalesOrderId,
                        Allocated_Qnty = workOrdersVM.CalcWOQty

                    };
                    var postsoallocation = await _baService.PostSOAllocation(co);
                    //directpostsoallocation
                    postWO = null;
                }


                if(postWO!=null)
                {
                    timeslotwo.Add(postWO);
                    await EnsureTimeslotsForWO(timeslotwo);
                }





                //postWO = await _baService.PostWO(workOrdersVM);
                List<BOMTempVM> bompost = new List<BOMTempVM>();
                if (postWO != null && postWO.WOID > 0)
                {

                ManufacturedPartNoDetailVM mf = await _masterService.GetManufPart((int)postWO.PartId);
                string partype = "";
                if (postWO.PartType == 1)
                {
                    partype = "Manf";
                }
                else if (postWO.PartType == 2)
                {
                    partype = "Assy";
                }
                else
                {
                    partype = manuf.MasterPartType;
                }
                BOMTempVM bomdata = new BOMTempVM
                {
                    WorkOrderId = postWO.WOID,
                    PartId = postWO.PartId,
                    PartType = partype,
                    Parentlevel = postWO.Parentlevel,
                    TenantId = postWO.TenantId
                };
                bompost.Add(bomdata);
                SalesOrderVM salesOrderVM = new SalesOrderVM() {
                    SalesOrderId = postWO.SalesOrderId,
                    WorkOrderId =postWO.WOID,
                    WorkOrderNo = postWO.WONumber,
                    Status=3
                };
                var salesOrder = await _baService.PostSalesOrder(salesOrderVM);

            }
            var postbom = await _baService.BOMTempPOst(bompost);
           // return Ok(selected_Sales_OrderVMs1);
            }
            
                else
                {
                    // return missing details to UI popup
                   // return Ok(selected_Sales_OrderVMs1);
                }
            object created = null;

            if (selected_Sales_OrderVMs1.PartNo == "WorkOrder Created" && postWO != null && postWO.WOID > 0)
            {
                created = new
                {
                    woid = postWO.WOID,
                    partId = postWO.PartId,
                    salesOrderId = postWO.SalesOrderId,
                    woNumber = postWO.WONumber
                };
            }

            return Ok(new
            {
                missing = selected_Sales_OrderVMs1,
                created = created
            });

        }


        public async Task<IActionResult> checkmissing(WorkOrdersVM workorder)
        {
           

            var timeslotList = await _woService.GetAllTimeslot_List();
            var machineList = await _machineService.GetMachinesList();

            ManufacturedPartNoDetailVM mf = await _masterService.GetManufPart((int)workorder.PartId);
            var routingList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
            if (!routingList.Any())
            {

                Selected_Sales_OrderVM somisss = new Selected_Sales_OrderVM();
                somisss.SoNumber = workorder.SaleOrderNo;
                somisss.PartNo = workorder.PartNo;
                somisss.RoutingName = "There is no Routings";
                 
                return Ok(somisss);
            }

            var routing = routingList.FirstOrDefault(r => r.PreferredRouting == 1)
                          ?? routingList.First();

            var routingSteps = await _routingService.RoutingSteps(routing.RoutingId);
            if (!routingSteps.Any())
            {

                Selected_Sales_OrderVM somiss = new Selected_Sales_OrderVM();
                somiss.SoNumber = workorder.SaleOrderNo; ;
                somiss.PartNo = workorder.PartNo;
                somiss.RoutingName = routing.RoutingName;
                somiss.StepNo = "There are no Steps";
                return Ok(somiss);
            }
            List<Selected_Sales_OrderVM> steps = new List<Selected_Sales_OrderVM>();

            foreach (var step in routingSteps)
            {
                // ================= MACHINE STEP =================
                if (step.StepLocation == "1")
                {
                    var machines = await _routingService.StepMachines((int)step.StepId);
                    if (!machines.Any())
                    {
                        Selected_Sales_OrderVM somiss = new Selected_Sales_OrderVM();
                        somiss.SoNumber = workorder.SaleOrderNo; 
                        somiss.PartNo = workorder.PartNo;
                        somiss.RoutingName = routing.RoutingName;
                        somiss.StepNo = step.StepNumber;
                        somiss.Machines = "There are no machines";
                        steps.Add(somiss);
                        continue;
                        //return Ok(somiss);
                    }
                    foreach (var machinetime in machines)
                    {
                        var mc = await _machineService.GetMachine(machinetime.MachineId);

                        if (machinetime.FirstPieceProcessingTime == "00:00:00" ||
                            machinetime.FloorToFloorTime == "00:00:00" ||
                            machinetime.SetupTime == "00:00:00" ||
                            machinetime.NoOfPartsPerLoading == 0)
                        {
                            var missingFields = new List<string>();

                            if (machinetime.FloorToFloorTime == "00:00:00")
                                missingFields.Add("FloorToFloorTime");

                            if (machinetime.FirstPieceProcessingTime == "00:00:00")
                                missingFields.Add("FirstPartProcessingTime");

                            if (machinetime.SetupTime == "00:00:00")
                                missingFields.Add("SetupTime");

                            if (machinetime.NoOfPartsPerLoading == 0)
                                missingFields.Add("NoOfPartsPerLoading");
                            Selected_Sales_OrderVM somiss = new Selected_Sales_OrderVM();
                            somiss.SoNumber = workorder.SaleOrderNo; 
                            somiss.PartNo = workorder.PartNo;
                            somiss.RoutingName = routing.RoutingName;
                            somiss.StepNo = step.StepNumber;
                            somiss.Machines = mc.MachineMachineSlNo + " " + string.Join(", ", missingFields) + "  is not set";
                            steps.Add(somiss);
                            continue;
                            // return Ok(somiss);
                        }

                        var plantWd = await _plantService.GetPlantWD(mc.MachinePlantId);
                        if (plantWd == null || plantWd.PlantId==0)
                        {

                            Selected_Sales_OrderVM somiss = new Selected_Sales_OrderVM();
                            somiss.SoNumber = workorder.SaleOrderNo;
                            somiss.PartNo = workorder.PartNo;
                            somiss.RoutingName = routing.RoutingName;
                            somiss.StepNo = step.StepNumber;
                            somiss.Machines = mc.MachineMachineSlNo + " Plant working Details is not set";
                            steps.Add(somiss);
                            continue;
                            //return Ok(somiss);
                        }

                        //if (!timeslotList.Any(t => t.PlantId == mc.MachinePlantId))
                        //{
                        //    var plant = await _plantService.GetPlant(mc.MachinePlantId);

                        //    Selected_Sales_OrderVM somiss = new Selected_Sales_OrderVM();
                        //    somiss.SoNumber = workorder.SaleOrderNo;
                        //    somiss.PartNo = workorder.PartNo;
                        //    somiss.RoutingName = routing.RoutingName;
                        //    somiss.StepNo = step.StepNumber;
                        //    somiss.Machines = mc.MachineMachineSlNo + " For Plant : " + plant.Name + " Timeslotlist is not set";
                        //    steps.Add(somiss);
                        //    continue;

                        //    // return Ok(somiss);
                        //}
                    }
                }

                // ================= SUBCON STEP =================
                else if (step.StepLocation == "2")
                {
                    var subCons = await _routingService.SubCons((int)step.StepId);
                    if (!subCons.Any())
                    {

                        Selected_Sales_OrderVM somiss = new Selected_Sales_OrderVM();
                        somiss.SoNumber = workorder.SaleOrderNo;
                        somiss.PartNo = workorder.PartNo;
                        somiss.RoutingName = routing.RoutingName;
                        somiss.StepNo = step.StepNumber;
                        somiss.Subcons = "There are no Subcons";
                        steps.Add(somiss);
                        continue;
                        //return Ok(somiss);
                    }

                    foreach (var sub in subCons)
                    {
                        var contacts = await _masterService.GetDivisionsByCompanyId(sub.SupplierId);
                        var subDetails = await _routingService.SubConWSS(
                            (int)step.StepId, (int)sub.SubConDetailsId);

                        if (Convert.ToInt32(sub.TransportTime) == 0)
                        {
                            
                            Selected_Sales_OrderVM somiss = new Selected_Sales_OrderVM();
                            somiss.SoNumber = workorder.SaleOrderNo;
                            somiss.PartNo = workorder.PartNo;
                            somiss.RoutingName = routing.RoutingName;
                            somiss.StepNo = step.StepNumber;
                            somiss.Subcons = "Transport time is not setup";
                            steps.Add(somiss);
                            continue;
                            //return Ok(somiss);
                        }
                        var subdesc = subDetails.FirstOrDefault();
                        if (subdesc == null)
                        {
                            Selected_Sales_OrderVM somiss = new Selected_Sales_OrderVM();
                            somiss.SoNumber = workorder.SaleOrderNo;
                            somiss.PartNo = workorder.PartNo;
                            somiss.RoutingName = routing.RoutingName;
                            somiss.StepNo = step.StepNumber;
                            somiss.Subcons = "There are no Subcon step work details";
                            steps.Add(somiss);
                            continue;
                            //return Ok(somiss);
                        }
                        if (subdesc.FloorToFloorTime == "00:00:00" || subdesc.SetupTime == "00:00:00" || subdesc.NoOfPartsPerLoading == 0)
                        {
                            var fieldname = subdesc.FloorToFloorTime == "00:00:00" ? "FloorToFloorTime" : "";
                            fieldname = subdesc.SetupTime == "00:00:00" ? "SetupTime" : "";
                            fieldname = subdesc.NoOfPartsPerLoading == 0 ? "NoOfPartsPerLoading" : "";
                            var missingFields = new List<string>();

                            if (subdesc.FloorToFloorTime == "00:00:00")
                                missingFields.Add("FloorToFloorTime");

                            if (subdesc.SetupTime == "00:00:00")
                                missingFields.Add("SetupTime");

                            if (subdesc.NoOfPartsPerLoading == 0)
                                missingFields.Add("NoOfPartsPerLoading");
                            Selected_Sales_OrderVM somiss = new Selected_Sales_OrderVM();
                            somiss.SoNumber = workorder.SaleOrderNo;
                            somiss.PartNo = workorder.PartNo;
                            somiss.RoutingName = routing.RoutingName;
                            somiss.StepNo = step.StepNumber;
                            somiss.Subcons = " Subcon step work details " + string.Join(',', missingFields) + " is not setup";
                            steps.Add(somiss);
                            continue;
                            //return Ok(somiss);
                        }

                        var machine = machineList
                            .FirstOrDefault(m => m.MachineTypeId == subdesc.MachineType);

                        var plantWd = await _plantService.GetPlantWD(machine.PlantId);
                        if (plantWd == null)
                        {

                            Selected_Sales_OrderVM somiss = new Selected_Sales_OrderVM();
                            somiss.SoNumber = workorder.SaleOrderNo;
                            somiss.PartNo = workorder.PartNo;
                            somiss.RoutingName = routing.RoutingName;
                            somiss.StepNo = step.StepNumber;
                            somiss.Subcons = " For Subcon SLno : " + machine.SlNo + " Plant working Details is not set";
                            steps.Add(somiss);
                            continue;
                            //return Ok(somiss);
                        }
                        //if (!timeslotList.Any(t => t.PlantId == machine.PlantId))
                        //{
                        //    var plant = await _plantService.GetPlant(machine.PlantId);

                        //    Selected_Sales_OrderVM somiss = new Selected_Sales_OrderVM();
                        //    somiss.SoNumber = workorder.SaleOrderNo;
                        //    somiss.PartNo = workorder.PartNo;
                        //    somiss.RoutingName = routing.RoutingName;
                        //    somiss.StepNo = step.StepNumber;
                        //    somiss.Subcons = plant.Name + " Timeslotlist is not set";
                        //    steps.Add(somiss);
                        //    continue;
                        //    //return Ok(somiss);
                        //}
                    }
                }
            }
            if(steps.Count()>0)
            {
                var result = new Selected_Sales_OrderVM
                {
                    SoNumber = steps.First().SoNumber,
                    PartNo = steps.First().PartNo,
                    RoutingName = steps.First().RoutingName,
                    StepNo = string.Join(", ", steps.Where(s => s.StepNo != null).Select(s => s.StepNo).Distinct()),
                    Machines = string.Join(", ", steps.Where(s => !string.IsNullOrWhiteSpace(s.Machines)).Select(s => s.Machines).Distinct()),
                    Subcons = string.Join(", ", steps.Where(s => !string.IsNullOrWhiteSpace(s.Subcons)).Select(s => s.Subcons) .Distinct()
       )
                };

                return Ok(result);
            }

            Selected_Sales_OrderVM somis = new Selected_Sales_OrderVM();
            somis.SoNumber = workorder.SaleOrderNo;
            somis.PartNo = "WorkOrder Created";

            return Ok(somis);


        }


        [HttpPost]
        public async Task<IActionResult> MultipleWOPost([FromBody] IEnumerable<WorkOrdersVM> listworkOrdersVM)
        {
            List<Selected_Sales_OrderVM> selected_Sales_OrderVMs = new List<Selected_Sales_OrderVM>();
            List<WorkOrdersVM> workOrdersVMs = new List<WorkOrdersVM>();
            List<WorkOrdersVM> finalWOList = new List<WorkOrdersVM>();
           
            foreach (var workOrdersVM in listworkOrdersVM)
            { 
                var message = await checkmissing(workOrdersVM);
                Selected_Sales_OrderVM selected_Sales_OrderVMs1 = (Selected_Sales_OrderVM)((OkObjectResult)message).Value;
                selected_Sales_OrderVMs.Add(selected_Sales_OrderVMs1);
                if(selected_Sales_OrderVMs1.PartNo != "WorkOrder Created")
                {
                    continue;
                }
                ManufacturedPartNoDetailVM manuf = await _masterService.GetManufPart((int)workOrdersVM.PartId);
                if(manuf.ManufacturedPartType==1)
                {
                    var mpmakefromlist = await _masterService.GetMPMakeFromListByPartId(manuf.ManufacturedPartNoDetailId.ToString());

                    var selectedMakeFrom = mpmakefromlist.OrderByDescending(x => x.PreferedRawMaterial).FirstOrDefault();
                    workOrdersVM.Input_Part_No = selectedMakeFrom.MPPartId;

                }
                else
                {
                    workOrdersVM.Input_Part_No = 0;
                }
                workOrdersVM.PartType = (int)manuf.ManufacturedPartType;
                RoutingVM rout = new RoutingVM();
                var resultList = await _routingService.Routings((int)manuf.ManufacturedPartNoDetailId);
                foreach (var item in resultList)
                {
                    if (item.PreferredRouting == 1)
                    {
                        rout = item;
                    }
                    else
                    {
                        rout = (resultList).Take(1).FirstOrDefault();
                    }
                }
                var result = (await _routingService.RoutingSteps(rout.RoutingId)).Take(1).FirstOrDefault();
                workOrdersVM.RoutingId = rout.RoutingId;
                workOrdersVM.StartingOpNo = (int)(result?.StepId != null
                    ? result.StepId
                    : 0);

                workOrdersVM.EndingOpNo = (int)(result?.StepId != null
                    ? result.StepId
                    : 0);
                if (workOrdersVM.PartType == 1)
                {
                    workOrdersVM.Parentlevel = 'N';
                }
                else
                {
                    //workOrdersVM.Parentlevel = 'Y';
                    var mpBOM = await _masterService.BOMS(workOrdersVM.PartId.ToString());
                    if(mpBOM != null && mpBOM.Any() )
                    {
                        foreach (var bom in mpBOM)
                        {
                            var assy = await _masterService.GetManufPart((int)bom.BOMPartId);
                            if (assy != null)
                            {
                                workOrdersVM.Parentlevel = 'Y';
                            }
                            else
                            {
                                workOrdersVM.Parentlevel = 'N';
                            }
                        }
                    }
                    else
                    {
                        workOrdersVM.Parentlevel = 'Y';
                    }
                }
                workOrdersVMs.Add(workOrdersVM);
            }
            var departmentstores = await _departmentService.GetAllStoresIDs();
            foreach (var item in workOrdersVMs)
            {
                long quantitynotallocated = 0;
                var invpart = await _woService.GetAllInventory_MasterBypartidWithFlag("Internal", departmentstores.Stores_DirMatl_ID, 0, 0, item.PartId);
                if (invpart.Any())
                {
                    var inventorypart = invpart.First();
                    if (inventorypart != null)
                    {
                        var invcount = Convert.ToInt64(inventorypart.Current_QntOnHand);
                        var varsoallocationbypartid = await _baService.GetSOAllocationlistbyPartid(item.PartId);
                        if (varsoallocationbypartid.Any())
                        {
                            var totalAllocatedQty = varsoallocationbypartid.Where(x => x.Dispatch_Complete == 'N').Sum(x => x.Allocated_Qnty);
                            if (totalAllocatedQty > 0)
                            {
                                quantitynotallocated = Convert.ToInt64(inventorypart.Current_QntOnHand) - totalAllocatedQty;
                            }
                            else
                            {
                                quantitynotallocated = Convert.ToInt64(inventorypart.Current_QntOnHand);
                            }
                        }
                        else
                        {
                            quantitynotallocated = Convert.ToInt64(inventorypart.Current_QntOnHand);
                        }

                    }
                    else
                    {
                        quantitynotallocated = 0;
                    }

                    if (quantitynotallocated < 0)
                        quantitynotallocated = 0;
                }
                else
                {
                    quantitynotallocated = 0;
                }
                if (quantitynotallocated == 0)
                {
                    // postWO = await _baService.PostWO(workOrdersVM);
                    finalWOList.Add(item);
                }
                else if (quantitynotallocated < item.CalcWOQty)
                {

                    item.CalcWOQty = item.CalcWOQty - Convert.ToInt32(quantitynotallocated);
                    finalWOList.Add(item);
                    //postWO = await _baService.PostWO(workOrdersVM);
                }
                else if (quantitynotallocated >= item.CalcWOQty)
                {
                    SO_Alloc_ListVM co = new SO_Alloc_ListVM()
                    {
                        PartId = item.PartId,
                        SO_ID = item.SalesOrderId,
                        Allocated_Qnty = item.CalcWOQty

                    };
                    var postsoallocation = await _baService.PostSOAllocation(co);
                    //directpostsoallocation
                    //  postWO = null;
                }

            }


           

            var postWO = await _baService.MultiplePostWO(finalWOList);
            await EnsureTimeslotsForWO(postWO);
            List<BOMTempVM> bompost = new List<BOMTempVM>();
            foreach (var item in postWO)
            {
                if (item.WOID > 0)
                {

                    ManufacturedPartNoDetailVM manuf = await _masterService.GetManufPart((int)item.PartId);
                    string partype = "";
                    if(item.PartType == 1)
                    {
                        partype = "Manf";
                    }
                    else if(item.PartType == 2)
                    {
                        partype = "Assy";
                    }
                    else
                    {
                        partype = manuf.MasterPartType;
                    }
                    BOMTempVM bomdata = new BOMTempVM
                    {
                        WorkOrderId = item.WOID,
                        PartId = item.PartId,
                        PartType = partype,
                        Parentlevel = item.Parentlevel,
                        TenantId = item.TenantId
                    };
                    bompost.Add(bomdata);
                    SalesOrderVM salesOrderVM = new SalesOrderVM()
                    {
                        SalesOrderId = item.SalesOrderId,
                        WorkOrderId = item.WOID,
                        WorkOrderNo =  item.WONumber
                    };
                    var salesOrder = await _baService.PostSalesOrder(salesOrderVM);
                }
            }
            var postbom = await _baService.BOMTempPOst(bompost);
            //return Ok(selected_Sales_OrderVMs);
            return Ok(new
            {
                missing = selected_Sales_OrderVMs,
                created = postWO
            });
            //return Ok(listworkOrdersVM);
        }
        
        [HttpPost]
        public async Task<IActionResult> PostWoSoRel([FromBody] IEnumerable<WOSOVM> wOSOVMs)
        {
            var postwoso = await _baService.PostWoSoRel(wOSOVMs);
            return Ok(postwoso);
        }
        [HttpPost]
        public async Task<IActionResult> MultipleWOPostsplit([FromBody] IEnumerable<WorkOrdersVM> listworkOrdersVM)
        {
            List<Selected_Sales_OrderVM> selected_Sales_OrderVMs = new List<Selected_Sales_OrderVM>();
            List<WorkOrdersVM> workOrdersVMs = new List<WorkOrdersVM>();
            List<WorkOrdersVM> finalWOList = new List<WorkOrdersVM>();

            foreach (var workOrdersVM in listworkOrdersVM)
            {
                workOrdersVM.IsSplit = 1;

                ManufacturedPartNoDetailVM manuf = await _masterService.GetManufPart((int)workOrdersVM.PartId);
                if (manuf.ManufacturedPartType == 1)
                {
                    var mpmakefromlist = await _masterService.GetMPMakeFromListByPartId(manuf.ManufacturedPartNoDetailId.ToString());

                    var selectedMakeFrom = mpmakefromlist.OrderByDescending(x => x.PreferedRawMaterial).FirstOrDefault();
                    workOrdersVM.Input_Part_No = selectedMakeFrom.MPPartId;

                }
                else
                {
                    workOrdersVM.Input_Part_No = 0;
                }
                workOrdersVM.PartType = (int)manuf.ManufacturedPartType;
                RoutingVM rout = new RoutingVM();
                var resultList = await _routingService.Routings((int)manuf.ManufacturedPartNoDetailId);
                foreach (var item in resultList)
                {
                    if (item.PreferredRouting == 1)
                    {
                        rout = item;
                    }
                    else
                    {
                        rout = (resultList).Take(1).FirstOrDefault();
                    }
                }
                var result = (await _routingService.RoutingSteps(rout.RoutingId)).Take(1).FirstOrDefault();
                workOrdersVM.RoutingId = rout.RoutingId;
                workOrdersVM.StartingOpNo = (int)(result?.StepId != null
                    ? result.StepId
                    : 0);

                workOrdersVM.EndingOpNo = (int)(result?.StepId != null
                    ? result.StepId
                    : 0);
                if (workOrdersVM.PartType == 1)
                {
                    workOrdersVM.Parentlevel = 'N';
                }
                else
                {
                    //workOrdersVM.Parentlevel = 'Y';
                    var mpBOM = await _masterService.BOMS(workOrdersVM.PartId.ToString());
                    if (mpBOM != null && mpBOM.Any())
                    {
                        foreach (var bom in mpBOM)
                        {
                            var assy = await _masterService.GetManufPart((int)bom.BOMPartId);
                            if (assy != null)
                            {
                                workOrdersVM.Parentlevel = 'Y';
                            }
                            else
                            {
                                workOrdersVM.Parentlevel = 'N';
                            }
                        }
                    }
                    else
                    {
                        workOrdersVM.Parentlevel = 'Y';
                    }
                }
                workOrdersVMs.Add(workOrdersVM);
            }
         




            var postWO = await _baService.MultiplePostWO(workOrdersVMs);

            
            return Ok( postWO);
            //return Ok(listworkOrdersVM);
        }
        [HttpPost]
        public async Task<IActionResult> WOpostUpdate(WorkOrdersVM workOrdersVM)
        {
            List<WorkOrdersVM> workOrdersVMs = new List<WorkOrdersVM>();
            WorkOrdersVM postWO = null;
            
            
                ManufacturedPartNoDetailVM manuf = await _masterService.GetManufPart((int)workOrdersVM.PartId);
                if (manuf.ManufacturedPartType == 1)
                {
                    var mpmakefromlist = await _masterService.GetMPMakeFromListByPartId(manuf.ManufacturedPartNoDetailId.ToString());

                    var selectedMakeFrom = mpmakefromlist.OrderByDescending(x => x.PreferedRawMaterial).FirstOrDefault();
                    workOrdersVM.Input_Part_No = selectedMakeFrom.MPPartId;

                }
                else
                {
                    workOrdersVM.Input_Part_No = 0;
                }
                workOrdersVM.PartType = (int)manuf.ManufacturedPartType;
                RoutingVM rout = new RoutingVM();
                var resultList = await _routingService.Routings((int)manuf.ManufacturedPartNoDetailId);
                foreach (var item in resultList)
                {
                    if (item.PreferredRouting == 1)
                    {
                        rout = item;
                    }
                    else
                    {
                        rout = (resultList).Take(1).FirstOrDefault();
                    }
                }
                if (workOrdersVM.WOID == 0)
                {
                    var result = (await _routingService.RoutingSteps(rout.RoutingId)).Take(1).FirstOrDefault();
                    var reverse = (await _routingService.RoutingSteps(rout.RoutingId)).Take(1).LastOrDefault();
                    workOrdersVM.RoutingId = rout.RoutingId;
                    workOrdersVM.StartingOpNo = result?.StepId != null
                                                ? int.TryParse(result.StepId.ToString(), out int opNo) ? opNo : 0
                                                : 0;
                    workOrdersVM.EndingOpNo = reverse?.StepId != null
                                                ? int.TryParse(reverse.StepId.ToString(), out int eopNo) ? eopNo : 0
                                                : 0;
                }
                if (workOrdersVM.PartType == 1)
                {
                    workOrdersVM.Parentlevel = 'N';

                }
                else
                {
                    var mpBOM = await _masterService.BOMS(workOrdersVM.PartId.ToString());
                    if (mpBOM != null && mpBOM.Any())
                    {
                        foreach (var bom in mpBOM)
                        {
                            var assy = await _masterService.GetManufPart((int)bom.BOMPartId);
                            if (assy != null)
                            {
                                workOrdersVM.Parentlevel = 'Y';
                            }
                            else
                            {
                                workOrdersVM.Parentlevel = 'N';
                            }
                        }
                    }
                    else
                    {
                        workOrdersVM.Parentlevel = 'Y';
                    }
                }
                postWO = await _baService.PostWO(workOrdersVM);
    
                return Ok(postWO);

        }
        [HttpPost]
        public async Task<IActionResult> RecombineWO(long parentWoId)
        {
            try
            {
                // GET ALL CHILD WOs
                var childWos = await _woService.AllParentChildWos(parentWoId);

                if (childWos == null || !childWos.Any())
                {
                    return Json(false);
                }
                var firstchildwo = childWos.First();
                // ==========================================
                // REACTIVATE PARENT WO
                // ==========================================

                var parentWo = (await _baService.AllWorkOrders()).Where(x=>x.WOID==parentWoId).FirstOrDefault();

                parentWo.Active = 1;
                //parentWo.ReloadOption = null;
                parentWo.RoutingId = firstchildwo.RoutingId;
                parentWo.StartingOpNo = firstchildwo.StartingOpNo;
                parentWo.EndingOpNo = firstchildwo.EndingOpNo;

                var postWO = await _baService.PostWO(parentWo); 

                // ==========================================
                // DEACTIVATE CHILD WOs
                // ==========================================
                
                foreach (var child in childWos)
                {
                    var resultList = await _woService.GetSoWoRel(child.WOID);
                    var first = resultList.FirstOrDefault();
                    var deletesowo = await _woService.DeleteWoSoRel(first.WOSOId);
                    var deletesplitwo= await _woService.DeleteWo(child.WOID);
                }

                // ==========================================
                // REMOVE SPLIT RELATIONS IF NEEDED
                // ==========================================

              

                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
        [HttpGet]
        private async Task EnsureTimeslotsForWO(List<WorkOrdersVM> workOrders)
        {
            if (workOrders == null || !workOrders.Any())
                return;

            // Only consider WOs having a planned completion date
            var validWOs = workOrders
                .Where(w => w.PlanCompletionDate.HasValue)
                .ToList();

            if (!validWOs.Any())
                return;

            DateTime maxWODate = validWOs.Max(x => x.PlanCompletionDate.Value).Date;

            var plants = await _plantService.GetPlants();

            // Load timeslots once
            var allTimeslots = await _woService.GetAllTimeslot_List();

            foreach (var plant in plants)
            {
                var wd = await _plantService.GetPlantWD(plant.PlantId);

                if (wd == null)
                    continue;

                var holidays = await _plantService.GetHolidays(plant.PlantId);
                var holidayList = holidays
                                    .Select(x => x.HolidayDate)
                                    .ToList();

                // Existing slots for this plant
                var plantSlots = allTimeslots
                    .Where(x => x.PlantId == plant.PlantId)
                    .OrderBy(x => x.Start_time)
                    .ToList();

                DateTime startDate;

                if (!plantSlots.Any())
                {
                    // First time generation
                    startDate = DateTime.Today;
                }
                else
                {
                    DateTime lastDate = plantSlots
                                        .Last()
                                        .Start_time
                                        .Date;

                    // Generate only after last existing date
                    startDate = lastDate.AddDays(1);
                }

                // Nothing to generate
                if (startDate > maxWODate)
                    continue;

                // Skip holidays/week offs
                DateTime? nextWorkingDate =
                    await GetNextWorkingDate(wd, startDate, holidayList);

                if (!nextWorkingDate.HasValue)
                    continue;

                await GenerateTimeslots(
                    plant,
                    wd,
                    nextWorkingDate.Value.Date,
                    maxWODate,
                    holidayList);
            }
        }
        private async Task GenerateTimeslots(Models.CoSettings.PlantVM plant, PlantWorkingDetailsVM wd, DateTime from, DateTime to, List<DateTime?> holidays)
        {
            for (var date = from; date <= to; date = date.AddDays(1))
            {
                if (holidays.Contains(date.Date) || date.DayOfWeek.ToString() == wd.WeeklyOff1 || date.DayOfWeek.ToString() == wd.WeeklyOff2)
                    continue;


                for (int shift = 1; shift <= wd.NoOfShifts; shift++)
                {
                    string shiftStartTime = shift == 1 ? wd.FirstShiftStartTime : shift == 2 ? wd.SecondShiftStartTime : wd.ThirdShiftStartTime;
                    string shiftDurationStr = shift == 1 ? wd.FirstShiftDuration : shift == 2 ? wd.SecondShiftDuration : wd.ThirdShiftDuration;
                    string breakStartStr = shift == 1 ? wd.First_Shift_Break_start_time : shift == 2 ? wd.Sec_Shift_Break_start_time : wd.Third_Shift_Break_start_time;
                    string breakDurationStr = shift == 1 ? wd.First_Shift_Break_duration : shift == 2 ? wd.Sec_Shift_Break_duration : wd.Third_Shift_Break_duration;

                    if (string.IsNullOrWhiteSpace(shiftStartTime) || string.IsNullOrWhiteSpace(shiftDurationStr)) continue;

                    DateTime shiftStart = DateTime.Parse($"{date:yyyy-MM-dd} {shiftStartTime}");
                    TimeSpan shiftDuration = TimeSpan.Parse(shiftDurationStr);
                    DateTime shiftEnd = shiftStart.Add(shiftDuration);

                    DateTime? breakStart = null;
                    DateTime? breakEnd = null;
                    if (!string.IsNullOrWhiteSpace(breakStartStr) && !string.IsNullOrWhiteSpace(breakDurationStr))
                    {
                        breakStart = DateTime.Parse($"{date:yyyy-MM-dd} {breakStartStr}");
                        if (breakDurationStr.Count(c => c == ':') == 1)
                        {
                            breakDurationStr = "00:" + breakDurationStr; // converts "30:00" → "00:30:00"
                        }
                        breakEnd = breakStart.Value.Add(TimeSpan.Parse(breakDurationStr));
                    }

                    DateTime currentSlotStart = shiftStart;
                    while (currentSlotStart < shiftEnd)
                    {
                        DateTime currentSlotEnd = currentSlotStart.AddMinutes(60);
                        if (currentSlotEnd > shiftEnd) break;

                        bool isBreak = breakStart.HasValue && breakEnd.HasValue &&
                                       currentSlotStart >= breakStart.Value && currentSlotStart < breakEnd.Value;

                        await _woService.PostTimeslot_List(new Timeslot_ListVM
                        {
                            PlantId = plant.PlantId,
                            Start_time = currentSlotStart,
                            End_time = currentSlotEnd,
                            Break_Slot = isBreak ? 'Y' : 'N'
                        });

                        currentSlotStart = currentSlotEnd;
                    }
                }
            }
        }
        private async Task<DateTime?> GetNextWorkingDate(PlantWorkingDetailsVM plant, DateTime startDate, List<DateTime?> holidays)
        {
            for (int i = 0; i < 30; i++)
            {
                var date = startDate.AddDays(i);
                if (!holidays.Contains(date.Date) && date.DayOfWeek.ToString() != plant.WeeklyOff1 && date.DayOfWeek.ToString() != plant.WeeklyOff2)
                {
                    return date;
                }
            }
            return null;
        }
        public async Task<IActionResult> AllWorkOrders()
        {
            var workOrders = await _baService.AllWorkOrders();
            var masterparts = await _masterService.MasterPartList();
            foreach (WorkOrdersVM item in workOrders)
            {
                foreach (ItemMasterPartVM imp in masterparts)
                {
                    if (item.PartId == imp.PartId)
                    {
                        item.PartNo = imp.PartNo;
                    }
                }
            }
            return Ok(workOrders);
        }

        [Route("~/C@S3t 2oP# ! ")]
        public IActionResult OrderEntry()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPOLogs(long customerOrderId)
        {
            var pologs = await _baService.GetPOLogs(customerOrderId);
            var masterparts = await _masterService.MasterPartList();
            ClaimsPrincipal userClaim = HttpContext.User;
            string fullName = AppUtil.GetFullName(userClaim);
            foreach (var item in pologs)
            {
                foreach (ItemMasterPartVM imp in masterparts)
                {
                    if (item.PartId == imp.PartId)
                    {
                        item.PartNo = imp.PartNo;
                    }
                }
                item.User = fullName;
            }
            return Ok(pologs);

        }
        [HttpGet]
        public async Task<IActionResult> GetWoPOLogs(long customerOrderId)
        {
            var pologs = await _baService.GetWoPOLogs(customerOrderId);
            return Ok(pologs);

        }
        [HttpGet]
        public async Task<IActionResult> GetBAAllStatus()
        {
            var pologs = await _baService.GetBAAllStatus();
            return Ok(pologs);

        }
        [HttpGet]
        public async Task<string> HelloWorld()
        {
            return await _baService.HelloWorld();
        }

        [HttpGet]
        public async Task<IActionResult> AllSalesOrders()
        {
            var salesorders = await _baService.AllSalesOrders();
            var masterparts = await _masterService.MasterPartList();
            var customer= await _baService.GetCustomerOrders();
           
            foreach (SalesOrderVM sovm in salesorders)
            {
                //var invpart = await _woService.GetAllInventory_MasterBypartid(sovm.PartId);
                //var inventorypart = invpart.First();
                //if (inventorypart != null)
                //{
                //    sovm.QntyOnHand = Convert.ToInt64(inventorypart.Current_QntOnHand);
                //}
                //else
                //{
                //    sovm.QntyOnHand = 0;
                //}
                sovm.BalanceSOQty = sovm.RequiredQuantity - sovm.ActQuantity;
                foreach (ItemMasterPartVM impvm in masterparts)
                {
                    if (sovm.PartId == impvm.PartId)
                    {
                        sovm.PartNo = impvm.PartNo;
                    }
                }
                foreach(CustomerOrderVM cu in customer)
                {
                    if (sovm.CustomerOrderId == cu.CustomerOrderId)
                    {
                        sovm.Customer = cu.CustomerName;
                    }
                }
            }
            return Ok(salesorders);
        }

            [HttpGet]
        public async Task<IActionResult> GetSalesOrders(long customerOrderId,long partId=0)
        {
            var salesorders = await _baService.GetSalesOrders(customerOrderId);
            var masterparts = await _masterService.MasterPartList();
            foreach(SalesOrderVM sovm in salesorders)
            {
                foreach(ItemMasterPartVM impvm in masterparts)
                {
                    if(sovm.PartId == impvm.PartId)
                    {
                        var bastatus = await _baService.GetBAStatus(sovm.Status);
                        sovm.StrStatus = bastatus.Status;
                        sovm.PartNo = impvm.PartNo;
                    }
                }
            }

            return Ok(salesorders);

        }

        [HttpGet]
        public async Task<IActionResult> GetCustOrders()
        {
            var custOrders = await _baService.GetCustomerOrders();
            var bastatus = await _baService.GetBAAllStatus();
            foreach (var item in custOrders)
            {
                var statusname = bastatus.FirstOrDefault(c => c.StatusId == item.Status);
                if (statusname != null)
                item.StrStatus = statusname.Status;
            }
            return Ok(custOrders);
        }


        [HttpGet]
        public async Task<IActionResult> GetPOLines(long customerOrderId)
        {
            var salesorders = await _baService.GetSalesOrders(customerOrderId);
            var masterparts = await _masterService.MasterPartList();
            List<POLineVM> pOLines = new List<POLineVM>();
            foreach (SalesOrderVM sovm in salesorders)
            {
                foreach (ItemMasterPartVM impvm in masterparts)
                {
                    if (sovm.PartId == impvm.PartId)
                    {
                        sovm.PartNo = impvm.PartNo;
                        sovm.PartId = impvm.PartId.GetValueOrDefault();
                    }
                }
            }
            foreach (SalesOrderVM sovm in salesorders)
            {
                if (pOLines.Count() == 0)
                {
                    POLineVM polineVM = new POLineVM();
                    polineVM.PartNo = sovm.PartNo;
                    polineVM.PartId = sovm.PartId;
                    polineVM.TotalQty = sovm.RequiredQuantity;
                    polineVM.Status = sovm.Status;
                    polineVM.Matl = sovm.Matl;
                    polineVM.Hold = sovm.Hold;
                    polineVM.Plan = sovm.Plan;
                    polineVM.Matl = sovm.Matl;
                    polineVM.WIP = sovm.WIP;
                    polineVM.NumSalesOrder = 1;
                    //polineVM.WONumber = sovm.WorkOrderNo;
                    polineVM.SONumber = sovm.SONumber;
                    polineVM.PoDateReqd = sovm.RequiredByDateStr;
                    polineVM.Comments = sovm.Comment;
                    polineVM.CustomerOrderId = sovm.CustomerOrderId;
                    polineVM.SalesOrderId = sovm.SalesOrderId;
                    //polineVM.WONumber = sovm.WorkOrderNo;
                    pOLines.Add(polineVM);
                }
                else
                {
                    bool foundPOLine = false;
                    foreach (POLineVM povm in pOLines)
                    {
                        if (sovm.PartNo == povm.PartNo)
                        {
                            povm.TotalQty += sovm.RequiredQuantity;
                            foundPOLine = true;
                            povm.NumSalesOrder += 1;
                            povm.SONumber = "Multiple";
                            povm.PoDateReqd = "Multiple";
                        }
                    }
                    if (foundPOLine) { }
                    else
                    {
                        POLineVM polineVM = new POLineVM();
                        polineVM.PartNo = sovm.PartNo;
                        polineVM.PartId = sovm.PartId;
                        polineVM.TotalQty = sovm.RequiredQuantity;
                        polineVM.Status = sovm.Status;
                        polineVM.Matl = sovm.Matl;
                        polineVM.Hold = sovm.Hold;
                        polineVM.Plan = sovm.Plan;
                        polineVM.Matl = sovm.Matl;
                        polineVM.WIP = sovm.WIP;
                        polineVM.NumSalesOrder = 1;
                        polineVM.SONumber = sovm.SONumber;
                        polineVM.Comments = sovm.Comment;
                        polineVM.CustomerOrderId = sovm.CustomerOrderId;
                        polineVM.SalesOrderId = sovm.SalesOrderId;
                        polineVM.PoDateReqd = sovm.RequiredByDateStr;
                        //polineVM.WONumber = sovm.WorkOrderNo;
                        pOLines.Add(polineVM);
                    }
                }
            }
            return Ok(pOLines);
        }

        [HttpGet]
        public async Task<IActionResult> GetSchedules(long customerOrderId)
        {
            var schedules = await _baService.GetSchedules(customerOrderId);
            var masterparts = await _masterService.MasterPartList();
            foreach (DeliveryScheduleVM sovm in schedules)
            {
                foreach (ItemMasterPartVM impvm in masterparts)
                {
                    if (sovm.DSPartId == impvm.PartId)
                    {
                        sovm.PartNo = impvm.PartNo;
                    }
                }
            }
            return Ok(schedules);
        }

        [HttpGet]
        public async Task<IActionResult> GetSOAggregate(long customerOrderId)
        {
            var pologs = await _baService.GetSOAggregate(customerOrderId);
            return Ok(pologs);

        }

        [HttpPost]
        public async Task<IActionResult> POLog(POLogVM pOLogVM)
        {
            var wo = await _baService.AllWorkOrders();
            var prodnwo = await _woService.AllProductionPlan_Wo();
            var findwo = wo.FirstOrDefault(w => w.SalesOrderId == pOLogVM.SalesOrderId);
            var findprodwo = prodnwo.FirstOrDefault(w => w.SalesOrderId == pOLogVM.SalesOrderId);
            if (findwo != null)
            {
                string msg = "This SaleOrder is already used in the WO: " + findwo.WONumber;
                return Ok(msg);
            }
            if (findprodwo != null)
            {
                string msg = "This SaleOrder is already used in the WO: " + findprodwo.WONumber;
                return Ok(msg);
            }
            var salesOrder = await _baService.PostPOLog(pOLogVM);
            return Ok(salesOrder);
        }
        [HttpPost]
        public async Task<IActionResult> SOLog(POLogVM pOLogVM)
        {
            var wo = await _baService.AllWorkOrders();
            var prodnwo = await _woService.AllProductionPlan_Wo();
            var findwo = wo.FirstOrDefault(w => w.SalesOrderId == pOLogVM.SalesOrderId);
            var findprodwo = prodnwo.FirstOrDefault(w => w.SalesOrderId == pOLogVM.SalesOrderId);
            if (findwo != null)
            {
                string msg = "This SaleOrder is already used in the WO: "+ findwo.WONumber;
                return Ok(msg);
            }
            if (findprodwo != null)
            {
                string msg = "This SaleOrder is already used in the WO: "+ findprodwo.WONumber;
                return Ok(msg);
            }
            var salesOrder = await _baService.PostSOLog(pOLogVM);
            return Ok(salesOrder);
        }

        [HttpPost]
        public async Task<IActionResult> SalesOrder(SalesOrderVM salesOrderVM)
        {
            var salesOrder = await _baService.PostSalesOrder(salesOrderVM);
            return Ok(salesOrder);
        }

        [HttpPost]
        public async Task<IActionResult> CustomerOrder(CustomerOrderVM customerOrderVM)
        {
            if(customerOrderVM.Comment == null)
            {
                customerOrderVM.Comment = ".";
            }
            var customerOrder = await _baService.PostCustomerOrder(customerOrderVM);
            return Ok(customerOrder);
        }

        [HttpPost]
        public async Task<IActionResult> DeliverySchedule(DeliveryScheduleVM deliveryScheduleVM)
        {
            var deliverySchedule = await _baService.PostDeliverySchedule(deliveryScheduleVM);
            return Ok(deliverySchedule);
        }

        [HttpPost]
        public async Task<IActionResult> SOAggregate(SOAggregateVM sOAggregateVM)
        {
            var deliverySchedule = await _baService.PostSOAggregate(sOAggregateVM);
            return Ok(deliverySchedule);
        }


        [HttpGet]
        public async Task<bool> RemoveCustomerOrder(long cutomerOrderId)
        {
            return await _baService.RemoveCustomerOder(cutomerOrderId);
        }
        [HttpGet]
        public async Task<bool> RemoveSalesOrder(long salesOrderId)
        {
            return await _baService.RemoveSalesOrder(salesOrderId);
        }
        [HttpGet]
        public async Task<bool> RemoveSchedule(long scheduleId)
        {
            return await _baService.RemoveDeliverySchedule(scheduleId);
        }

        [HttpGet]
        public async Task<bool> AddSalesOrders(long customerOrderId)
        {
            return await _baService.AddSalesOrders(customerOrderId);
        }

        [HttpGet]
        public async Task<JsonResult> CheckPartNo(long partId)
        {
            var result = await _baService.CheckPartNo(partId);
            return Json(!result);
        }

    }
}
