using CWB.App.AppUtils;
using CWB.App.Models.BusinessProcesses;
using CWB.App.Models.Contacts;
using CWB.App.Models.Departments;
using CWB.App.Models.DocumentManagement;
using CWB.App.Models.ItemMaster;
using CWB.App.Models.Machine;
using CWB.App.Models.Plants;
using CWB.App.Models.Routing;
using CWB.App.Models.WorkOrder;
using CWB.App.Services.BusinessProcesses;
using CWB.App.Services.CompanySettings;
using CWB.App.Services.DocumentMagement;
using CWB.App.Services.EmployeeMaster;
using CWB.App.Services.Masters;
using CWB.App.Services.ProductionPlanWo;
using CWB.App.Services.Routings;
using CWB.Constants.UserIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class WorkOrderController : Controller
    {
        private readonly ILogger<WorkOrderController> _logger;
        private readonly IWOService _woService;
        private readonly IDocMangService _docMangService;
        private readonly IBAService _baService;
        private readonly IRoutingService _routingService;
        private readonly IMastersServices _masterService;
        private readonly IPlantService _plantService;
        private readonly IMachineService _machineService;
        private readonly IDepartmentService _departmentService;
        private readonly IOperationService _operationService;
        private readonly IEmployeeService _employeeService;
        public static class SimulationState
        {
            public static bool IsPaused { get; set; } = false;
            public static bool IsStopped { get; set; } = false;
        }
        public WorkOrderController(ILogger<WorkOrderController> logger, IWOService wOService, 
            IBAService baService, IRoutingService routingService, IMastersServices masterServices, IPlantService plantService
            , IMachineService machineService, IDepartmentService departmentService, IEmployeeService employeeService, IDocMangService docMangService, IOperationService operationService)
        {
            _logger = logger;
            _woService = wOService;
            _baService = baService;
            _routingService = routingService;
            _employeeService = employeeService;
            _masterService = masterServices;
            _docMangService = docMangService;
            _plantService = plantService;
            _machineService = machineService;
            _departmentService = departmentService;
            _operationService = operationService;
        }
        public IActionResult Index()
        {
            _logger.LogTrace("WO--Index--Loading");
            return View();
        }

        [Route("~/C!O$N%1#23t! ")]
        public IActionResult SoToWo()
        {
            return View();
        }

        [Route("~/D#1@122P I%5$3T I")]
        public IActionResult DetailedProcPlan()
        {
            return View();
        }
        [Route("~/S@A!E0#% T%1P W ")]
        public IActionResult SalesOrderList(string soNumber)
        {
            ViewBag.SearchSoNumber = soNumber ?? string.Empty;
            return View();
        }

        [Route("~/W@A!E0#% U%1X#Q ")]
        public IActionResult WorkOrderList(string woNumber)
        {
            ViewBag.SearchWoNumber = woNumber ?? string.Empty;
            return View();
        } 
        [Route("~/P0!L!@T%2")]
        public IActionResult POLineList()
        {
            return View();
        }
        [Route("~/@PP0@T%2")]
        public IActionResult ApprovPoDetails()
        {
            return View();
        }
        [Route("~/W@IA!E0P#% U%1X#QC")]
        public IActionResult WipControl()
        {
            return View();
        }
        [Route("~/!NWDP@@T%2")]
        public IActionResult InwardPo()
        {
            return View();
        }
        [Route("~/!NSDP@T")]
        public IActionResult Inspection()
        {
            return View();
        }
        [Route("~/0OTSTS@!")]
        public IActionResult OperationsSettings()
        {
            return View();
        }
        [Route("~/N@C%!@!")]
        public IActionResult NcAwaitingDecision()
        {
            return View();
        }
        [Route("~/M@TR!C%!@!")]
        public IActionResult MaterialMovement()
        {
            return View();
        }
        [Route("~/S!M@l@T!L")]
        public IActionResult SimulationPage()
        {
            return View();
        }
        [Route("~/S!M@l@T!LO0")]
        public IActionResult SimulationOutPage()
        {
            return View();
        }
        [Route("~/M@H1NE!P")]
        public IActionResult MachinceListPage()
        {
            return View();
        }
        [Route("~/SYBC0!NK")]
        public IActionResult SubConListPage()
        {
            return View();
        }
        [Route("~/IS$SUE@!")]
        public IActionResult IssueMatl()
        {
            return View();
        }
        [Route("~/B00IK0u!@!")]
        public IActionResult BookOutPage()
        {
            return View();
        }
        [Route("~/S@D!S1O0")]
        public IActionResult SoDispatch()
        {
            return View();
        }
        [Route("~/!N2@O023A")]
        public IActionResult InvenControl()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AllSalesOrders()
        {
            var salesorders = await _baService.AllSalesOrders();
            var masterparts = await _masterService.ItemMasterParts();
            var customer = await _baService.GetCustomerOrders();
            foreach (SalesOrderVM sovm in salesorders)
            {
                foreach (ItemMasterPartVM impvm in masterparts)
                {
                    if (sovm.PartId == impvm.PartId)
                    {
                        var bastatus = await _baService.GetBAStatus(sovm.Status);
                        sovm.StrStatus = bastatus.Status;
                        sovm.PartNo = impvm.PartNo;
                        sovm.PartDesc = impvm.Description;
                    }
                }
                foreach (CustomerOrderVM cu in customer)
                {
                    if (sovm.CustomerOrderId == cu.CustomerOrderId)
                    {
                        sovm.Customer = cu.CustomerName;
                        sovm.PoNumber = cu.PONumber;
                    }
                }
            }
            return Ok(salesorders);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSOFinDisp(SalesOrderVM masterDocListVM)
        {
            var mc_Wait_Lists = await _baService.AllSalesOrders();
            var mcWait = mc_Wait_Lists.Where(m => m.SalesOrderId == masterDocListVM.SalesOrderId).FirstOrDefault();
            if (mcWait != null)
            {
                mcWait.FinalDispQnty = masterDocListVM.FinalDispQnty;
                var result = await _baService.PostSalesOrder(mcWait);
                return Ok(result);
            }
            return Ok("Not Found SalesOrder");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSOFinDispBulk([FromBody] List<SalesOrderVM> salesOrderUpdates)
        {
            // 1. Initialize a list to track successful updates or errors
            var results = new List<object>();

            if (salesOrderUpdates == null || !salesOrderUpdates.Any())
            {
                return BadRequest("No sales order data provided for bulk update.");
            }

            // 2. Loop through every item in the received list
            foreach (var updateItem in salesOrderUpdates)
            {
                // 3. Find the existing Sales Order (similar to your single-item logic)
                var mc_Wait_Lists = await _baService.AllSalesOrders();
                var mcWait = mc_Wait_Lists
                    .Where(m => m.SalesOrderId == updateItem.SalesOrderId)
                    .FirstOrDefault();

                if (mcWait != null)
                {
                    // 4. Apply the new dispatched quantity
                    mcWait.FinalDispQnty = updateItem.FinalDispQnty;

                    // 5. Save the change
                    var result = await _baService.PostSalesOrder(mcWait); // Assuming PostSalesOrder handles the update

                    results.Add(new { SalesOrderId = mcWait.SalesOrderId, Status = "Updated", ServiceResult = result });
                }
                else
                {
                    results.Add(new { SalesOrderId = updateItem.SalesOrderId, Status = "Not Found" });
                }
            }

            // 6. Return a summary of all operations
            return Ok(results);
        }
        [HttpGet]
        public async Task<IActionResult> AllSODispatch(int? take = null)
        {
            var salesorders = await _baService.AllSalesOrders();
            var masterparts = await _masterService.ItemMasterParts();
            var customer = await _baService.GetCustomerOrders();
            var trans = await _woService.GetAllInv_Trans_Log();
            var DispatchDetails = await _woService.GetAllDispatchDetails(); 
            if (take.HasValue)
                salesorders = salesorders.Take(take.Value).ToList();
            foreach (SalesOrderVM sovm in salesorders)
            {
                foreach (ItemMasterPartVM impvm in masterparts)
                {
                    if (sovm.PartId == impvm.PartId)
                    {
                        ManufacturedPartNoDetailVM mf = await _masterService.GetManufPart((int)sovm.PartId);
                        var bastatus = await _baService.GetBAStatus(sovm.Status);
                        sovm.StrStatus = bastatus.Status;
                        sovm.PartNo = impvm.PartNo;
                        sovm.PartDesc = impvm.Description;
                        double maxBuildableQty = double.MaxValue;
                        if (impvm.MasterPartType == "ManufacturedPart")
                        {
                            var mpmakefromlists = await _masterService.GetMPMakeFromListByPartId(mf.ManufacturedPartNoDetailId.ToString());

                            foreach (var component in mpmakefromlists)
                            {
                                // 1. Total Weight On Hand for the Component
                                decimal componentOnHandWeight = CalculateOnHand((int)component.MPPartId, trans);

                                // 2. Required Weight to build ONE unit of the final product
                                // ASSUMPTION: InputWeight is the property holding the required weight per unit
                                decimal requiredWeight = Convert.ToDecimal(component.InputWeight);

                                if (requiredWeight > 0)
                                {
                                    // 3. Calculate how many final products can be built: Floor(On Hand Weight / Required Weight)
                                    // Use Math.Floor to ensure we only count complete units
                                    double availableBasedOnComponent = Math.Floor((double)(componentOnHandWeight / requiredWeight));

                                    // 4. Update the bottleneck component (find the minimum buildable quantity)
                                    maxBuildableQty = Math.Min(maxBuildableQty, availableBasedOnComponent);
                                }
                            }
                        }
                        else // Handle the BOM case (assuming this is a purchased part that uses a BOM for assembly)
                        {
                            // ASSUMPTION: This returns a list of components and their required quantities
                            var bomlsts = await _masterService.BOMS(mf.ManufacturedPartNoDetailId.ToString());

                            foreach (var component in bomlsts)
                            {
                                decimal componentOnHandUnits = CalculateOnHand((int)component.BOMPartId, trans);

                                // ASSUMPTION: Quantity is the property holding the amount needed
                                decimal requiredUnits = component.Quantity;

                                if (requiredUnits > 0)
                                {
                                    double availableBasedOnComponent = Math.Floor((double)(componentOnHandUnits / requiredUnits));
                                    maxBuildableQty = Math.Min(maxBuildableQty, availableBasedOnComponent);
                                }
                            }
                        }
                        if (maxBuildableQty == double.MaxValue)
                        {
                            // No components defined
                            sovm.QntyOnHand = 0;
                        }
                        else
                        {
                            sovm.QntyOnHand = (long)maxBuildableQty;
                        }
                    }
                }
                foreach (CustomerOrderVM cu in customer)
                {
                    if (sovm.CustomerOrderId == cu.CustomerOrderId)
                    {
                        sovm.Customer = cu.CustomerName;
                        sovm.PoNumber = cu.PONumber;
                    }
                }
            }
            var partsToAllocate = salesorders
        .GroupBy(so => so.PartId)
        .ToList();

            foreach (var partGroup in partsToAllocate)
            {
                // 2a. Find the Total Buildable Stock for this PartId
                // The total resource is the maximum QntyOnHand calculated in Phase 1 (since all SOs for the same part share the same components/bottleneck).
                long totalBuildableStock = partGroup.Max(so => so.QntyOnHand);

                // 2b. Calculate the total quantity ALREADY RESERVED/DISPATCHED by ALL SOs of this part.
                long totalReservedQuantity = partGroup.Sum(so => so.FinalDispQnty);

                // 2c. Calculate the FINAL QOH (Available for Dispatch)
                // Available QOH = Total Buildable Stock - Total Reserved
                long finalAvailableQOH = totalBuildableStock - totalReservedQuantity;

                // Ensure the QOH doesn't go negative
                if (finalAvailableQOH < 0) finalAvailableQOH = 0;

                // ---------------------------------------------------------------------

                // --- PHASE 3: Apply FINAL QOH and Perform Suggested Allocation ---

                // 3a. Update the QntyOnHand for ALL SOs in this group to reflect the FINAL available stock.
                // This addresses your requirement: "if the FinalDispQnty of the so is 50 then the qntyonhand should be 50 [100-50] not 100"
                foreach (var so in partGroup)
                {
                    so.QntyOnHand = finalAvailableQOH;
                }

                // 3b. Use the FINAL QOH as the running stock for the Suggested Allocation.
                long runningStockForAllocation = finalAvailableQOH;

                // 3c. Order SOs by earliest RequiredByDate, and filter to only allocate NEW quantities.
                var sortedSOsForAllocation = partGroup
                    .Where(so => so.FinalDispQnty == 0) // Only allocate to SOs not yet dispatched
                    .OrderBy(so => so.RequiredByDate);

                // 3d. Perform the allocation for non-dispatched SOs
                foreach (var so in sortedSOsForAllocation)
                {
                    long soRequired = so.RequiredQuantity;

                    long suggestedQty = Math.Min(runningStockForAllocation, soRequired);

                    so.SuggestedDispQnty = (int)suggestedQty;

                    runningStockForAllocation -= suggestedQty;

                    if (runningStockForAllocation <= 0)
                    {
                        break;
                    }
                }
            }

            List<SalesOrderVM> ressultSos = new List<SalesOrderVM>();
            foreach (var sovm in salesorders)
            {
                var dispatchDetail = DispatchDetails.Where(d => d.SaleOrderId == sovm.SalesOrderId).ToList();
                
                if (dispatchDetail.Count() > 0 && sovm.QntyOnHand == 0)
                {
                    continue;
                }
                else
                {
                    if (dispatchDetail.Count() > 0)
                    {
                        sovm.DispatchId = dispatchDetail.First().DispatchDetailsId;
                        sovm.InvoiceNo= dispatchDetail.First().InvoiceNo;
                        sovm.InvoiceDate= dispatchDetail.First().InvoiceDate.ToString("dd-MM-yyyy");
                        sovm.DispatchDetail= dispatchDetail.First().DispatchDetail;
                    }
                    ressultSos.Add(sovm);
                }
            }
            return Ok(ressultSos);
        }
        // --- Start of logic inside AllSODispatch method ---

        // Helper function to calculate Qty On Hand for a single part ID
        decimal CalculateOnHand(int partId, IEnumerable<Inv_Trans_LogVM> trans)
        {
            decimal onHandWeight = 0;

            foreach (var tran in trans.Where(t => t.Input_Part_NoId == partId || t.Output_Part_No == partId))
            {
                // ASSUMPTION: tran.Quantity is the weight.
                if (tran.Output_Part_No == partId)
                {
                    onHandWeight += tran.Qnty;
                }
                else if (tran.Input_Part_NoId == partId)
                {
                    onHandWeight += tran.Qnty;
                }
            }
            return onHandWeight;
        }
        
        [HttpGet]
        public async Task<IActionResult> CustomerMis()
        {
            var salesorders = await _baService.AllSalesOrders();
            var masterparts = await _masterService.ItemMasterParts();
            var customers = await _baService.GetCustomerOrders();

            foreach (SalesOrderVM sovm in salesorders)
            {
                foreach (ItemMasterPartVM impvm in masterparts)
                {
                    if (sovm.PartId == impvm.PartId)
                    {
                        var bastatus = await _baService.GetBAStatus(sovm.Status);
                        sovm.StrStatus = bastatus.Status;
                        sovm.PartNo = impvm.PartNo;
                        sovm.PartDesc = impvm.Description;
                    }
                }
                foreach (CustomerOrderVM cu in customers)
                {
                    if (sovm.CustomerOrderId == cu.CustomerOrderId)
                    {
                        sovm.Customer = cu.CustomerName;
                        sovm.PoNumber = cu.PONumber;
                    }
                }
            }

            var summary = salesorders
                .GroupBy(s => s.Customer)
                .Select(g => new 
                {
                    CustomerName = g.Key,
                    SoOpenCount = g.Count(x => x.Status != 6),
                    SoOpenValue = 0,
                    SoWipCount = 0,
                    SoWipValue = 0
                })
                .ToList();

            return Ok(summary);
        }


        [HttpGet]
        public async Task<IActionResult> AllWorkOrders()
        {
            var workOrders = await _baService.AllWorkOrders();
            var masterparts = await _masterService.ItemMasterParts();
            foreach (WorkOrdersVM item in workOrders)
            {
                foreach (ItemMasterPartVM imp in masterparts)
                {
                    if (item.PartId == imp.PartId)
                    {
                        var wostatus = await _woService.GetWOStatus(item.Status);
                        item.StrStatus = wostatus.Status;
                        item.PartNo = imp.PartNo;
                        item.PartDesc = imp.Description;
                    }
                    if(item.ParentWoId != 0)
                    {
                        var partwos = workOrders.Where(w => w.WOID == item.ParentWoId).FirstOrDefault();
                        if(partwos != null)
                        {
                            item.BuildToStock = partwos.BuildToStock;
                        }
                    }
                }
            }
            return Ok(workOrders);
        }

        [HttpGet]
        public async Task<IActionResult> AllParentChildWos(long parentWoId)
        {
            var allparentwos = await _woService.AllParentChildWos(parentWoId);
            return Ok(allparentwos);
        }

        [HttpPost]
        public async Task<IActionResult> WOpost(WorkOrdersVM workOrdersVM)
        {
            var workOrders = await _baService.AllWorkOrders();
            var findWo = workOrders.Where(w => w.WOID == workOrdersVM.WOID).FirstOrDefault();
            findWo.Comment = workOrdersVM.Comment;
            findWo.Status = workOrdersVM.Status;
            var postWO = await _baService.PostWO(findWo);
            return Ok(postWO);
        }


        [HttpPost]
        public async Task<IActionResult> WoWaitlingpost(ProductionPlan_WoVM workOrdersVM)
        {
            var workOrders = await _woService.AllProductionPlan_Wo();
            var findWo = workOrders.Where(w => w.ProductionPlanId == workOrdersVM.ProductionPlanId).FirstOrDefault();
            findWo.Comment = workOrdersVM.Comment;
            findWo.Status = workOrdersVM.Status;
            var postWO = await _woService.UpdateHoldProductionPlan_Wo(findWo);
            return Ok(postWO);
        }
        [HttpPost]
        public async Task<IActionResult> BomListWait(BOMListVM workOrdersVM)
        {
            var workOrders = await _woService.AllProductionPlan_Wo();
            var findWo = workOrders.Where(w => w.WoId == workOrdersVM.ParentWoId).FirstOrDefault();
            findWo.Comment = workOrdersVM.Comment;
            findWo.Status = workOrdersVM.Status;
            var postWO = await _woService.UpdateHoldProductionPlan_Wo(findWo);
            return Ok(postWO);
        }

        [HttpGet]
        public async Task<IActionResult> ReloadWo(string reloadoption, long partid)
        {
            List<WorkOrdersVM> listwo = new List<WorkOrdersVM>();
            var workOrders = await _baService.AllWorkOrders();
            foreach (var item in workOrders)
            {
                if (item.ReloadOption == reloadoption && item.PartId == partid)
                {
                    listwo.Add(item);
                }
            }
            return Ok(listwo);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoutings(int manufPartId)
        {
            ManufacturedPartNoDetailVM mf = await _masterService.GetManufPart(manufPartId);
            var resultList = await _routingService.Routings(mf.ManufacturedPartNoDetailId); 
            var sortedList = resultList.OrderByDescending(x => x.PreferredRouting == 1).ThenBy(x => x.PreferredRouting).ToList();
            return Ok(sortedList);
        }
        [HttpGet]
        public async Task<IActionResult> GetSimRoutings(int manufPartId,int Qnty)
        {
            ManufacturedPartNoDetailVM mf = await _masterService.GetManufPart(manufPartId);
            var shops = await _departmentService.GetDepartments(1);
            var mcList = await _machineService.GetMachinesList();
            var resultList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
            var sortedList = resultList.OrderByDescending(x => x.PreferredRouting == 1).ThenBy(x => x.PreferredRouting).ToList();
            foreach (var item in sortedList)
            {
                var steps = await _routingService.RoutingSteps(item.RoutingId);

                if (item.PreferredRouting == 1)
                {
                    item.StrPreferredRouting = "Y";
                }
                else
                {
                    item.StrPreferredRouting = "N";
                }
                item.NoOprns = steps.Count();
                var steploc = steps.FirstOrDefault().StepLocation == "1";
                if(steploc)
                {

                var stepmcList = await _routingService.StepMachines((int)steps.FirstOrDefault().StepId);
                var preferredStepMc = stepmcList.FirstOrDefault(x => x.PreferredMachine == 1);
                var stepmc = preferredStepMc ?? stepmcList.FirstOrDefault();
                var machince = await _machineService.GetMachine(stepmc.MachineId);
                item.StartingOpr = machince.MachineMachineName + " - " + shops.FirstOrDefault(s=>s.DepartmentId == machince.MachineDepartmentId).Name;
                int oprQty = Qnty; // A = Quantity for the operation

                TimeSpan totalTpt = TimeSpan.Zero;

                foreach (var mc in stepmcList)
                {
                    TimeSpan setupTime = TimeSpan.TryParse(mc.SetupTime, out var st) ? st : TimeSpan.Zero;
                    TimeSpan cycleTime = TimeSpan.TryParse(mc.FloorToFloorTime, out var ct) ? ct : TimeSpan.Zero;

                    TimeSpan tpt = setupTime + TimeSpan.FromTicks(cycleTime.Ticks * oprQty);

                    totalTpt += tpt;
                }
                string totalTptStr = totalTpt.ToString(@"hh\:mm");
                item.Tpt = totalTptStr;
                }
                else
                {
                    var subconList = await _routingService.SubCons((int)steps.FirstOrDefault().StepId);
                    var subtransport = subconList.FirstOrDefault(s => s.PreferredSubcon == 1) ?? subconList.FirstOrDefault();
                    if(subtransport != null)
                    {

                        TimeSpan totalTpt = TimeSpan.Zero;
                        var subconwss = await _routingService.SubConWSS((int)steps.FirstOrDefault().StepId, subtransport.SubConDetailsId);
                        var subconwsf = subconwss.FirstOrDefault();
                        var machince = mcList.Where(m=>m.MachineTypeId == subconwsf.MachineType).FirstOrDefault();
                        item.StartingOpr = machince.Name + " - " + machince.Shop;
                        // Time calculations
                        foreach (var subconws in subconwss)
                        {
                            var trans = TimeSpan.Parse(subtransport.TransportTime); // "01:00:00"
                            var setupTime = TimeSpan.Parse(subconws.SetupTime); // "01:00:00"
                            var floorToFloorTime = TimeSpan.Parse(subconws.FloorToFloorTime);
                            TimeSpan tpt = trans + setupTime + TimeSpan.FromTicks(floorToFloorTime.Ticks * Qnty);

                            totalTpt += tpt;
                        }
                        string totalTptStr = totalTpt.ToString(@"hh\:mm");
                        item.Tpt = totalTptStr;
                    }


                }


            }
            return Ok(sortedList);
        }

        [HttpGet]
        public async Task<IActionResult> RoutingSteps(int routingId)
        {
            var result = await _routingService.RoutingSteps(routingId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> RoutingStepsMcSub(int routingId, int Qnty)
        {
            var result = await _routingService.RoutingSteps(routingId);
            var companies = await _masterService.GetCompanies();
            var shops = await _departmentService.GetDepartments(1);
            List<RoutingStepVM> steplist = new List<RoutingStepVM>();
            foreach (var item in result)
            {
                if(item.StepLocation == "1")
                {
                    item.LocationName = "Inhouse";
                    var stepmcs = await _routingService.StepMachines((int)item.StepId);
                    foreach (var stepmc in stepmcs)
                    {
                        var machince = await _machineService.GetMachine(stepmc.MachineId);
                        item.MachineName = machince.MachineMachineName + " - " + shops.FirstOrDefault(s => s.DepartmentId == machince.MachineDepartmentId).Name;
                        item.PreferredStr = stepmc.PreferredMachine == 1 ? "Y" : "N";
                        var cycleTime = TimeSpan.Parse(stepmc.FloorToFloorTime);
                        var setupTime = TimeSpan.Parse(stepmc.SetupTime);
                        item.CycleTime = cycleTime.TotalMinutes.ToString(); 
                        var residenceTime = setupTime + TimeSpan.FromTicks(cycleTime.Ticks * Qnty);
                        item.ResidenceTime = ((int)residenceTime.TotalHours).ToString("00") + ":" + residenceTime.Minutes.ToString("00");
                        steplist.Add(item);
                    }
                }
                else
                {
                    item.LocationName = "SubCon";
                    var subcons = await _routingService.SubCons((int)item.StepId);
                    foreach (var subcon in subcons)
                    {
                        var transport = subcon.TransportTime;
                        item.PreferredStr = subcon.PreferredSubcon == 1 ? "Y" : "N";
                        var subconwds = await _routingService.SubConWSS((int)item.StepId, subcon.SubConDetailsId);
                        var cycleTimes = subconwds.Select(s => TimeSpan.Parse(s.FloorToFloorTime)).ToList();
                        var setupTimes = subconwds.Select(s => TimeSpan.Parse(s.SetupTime)).ToList();

                        var totalCycleTime = new TimeSpan(cycleTimes.Sum(ct => ct.Ticks));
                        var totalSetupTime = new TimeSpan(setupTimes.Sum(st => st.Ticks));

                        var residenceTime = totalSetupTime + TimeSpan.FromTicks(totalCycleTime.Ticks * Qnty);

                        item.CycleTime = totalCycleTime.TotalMinutes.ToString();
                        item.ResidenceTime = ((int)residenceTime.TotalHours).ToString("00") + ":" + residenceTime.Minutes.ToString("00");
                        foreach (ContactsVM mobj in companies)
                        {
                            if (subcon.SupplierId == mobj.CompanyId)
                            {
                                subcon.Company = mobj.CompanyName;
                                item.MachineName = mobj.CompanyName;
                            }
                        }
                        steplist.Add(item);
                    }
                }
            }
            return Ok(steplist);
        }

        [HttpGet]
        public async Task<IActionResult> GetSoWo(long workOrderId)
        {
            var resultList = await _woService.GetSoWoRel(workOrderId);
            return Ok(resultList);
        }

        [HttpPost]
        public async Task<IActionResult> GetOneSO([FromBody] IEnumerable<WOSOVM> saleOrderId)
        {
            List<SalesOrderVM> setSO = new List<SalesOrderVM>();
            foreach (var item in saleOrderId)
            {
                //if(item.Active != 2)
                //{
                setSO.Add(await _baService.GetOneSO(item.SalesOrderId));
                //}

            }
            return Ok(setSO);
        }

        public async Task<IActionResult> GetSoNumber(long soid)
        {
            var so = await _baService.GetOneSO(soid);
            return Ok(so);
        }

        [HttpPost]
        public async Task<IActionResult> ProcPlan()
        {
            try
            {

                var workOrders = await _baService.AllWorkOrders();
                List<ProductionPlan_WoVM> productions = new List<ProductionPlan_WoVM>();
                foreach (var item in workOrders)
                {
                    if (item.Active != 2 && item.PPStatus != "PP")
                    {
                        ManufacturedPartNoDetailVM mf = await _masterService.GetManufPart((int)item.PartId);
                        DateTime planstartdt = DateTime.Now;
                        if (mf.ManufacturedPartType == 2)
                        {
                            var workdetails = await _plantService.GetPlantWD(1);
                            var holidaylist = await _plantService.GetHolidays(1);
                            string weekOff1 = workdetails.WeeklyOff1;
                            string weekOff2 = workdetails.WeeklyOff2;
                            var resultList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                            int minutes = 0;
                            foreach (var rote in resultList)
                            {
                                var result = await _routingService.RoutingSteps(rote.RoutingId);
                                foreach (var step in result)
                                {
                                    var stepdetails = await _routingService.StepMachines((int)step.StepId);
                                    var processingTimeSum = stepdetails
                                                    .GroupBy(sd => sd.RoutingStepId)
                                                    .Select(g => new
                                                    {
                                                        RoutingStepId = g.Key,
                                                        TotalProcessingTime = g.Sum(sd => TimeSpan.Parse(sd.FloorToFloorTime).TotalMinutes)
                                                    });
                                    foreach (var min in processingTimeSum)
                                    {
                                        minutes = (int)min.TotalProcessingTime;
                                    }
                                }
                            }
                            int noofhr = 0;
                            if (workdetails.NoOfShifts == 1)
                            {
                                noofhr = 420;
                            }
                            else
                            {
                                noofhr = 840;
                            }
                            int assyTime = (minutes * item.CalcWOQty) / noofhr;
                            int assyTimeInDays = assyTime / 1440;
                            planstartdt = item.PlanCompletionDate.Value.AddDays(-assyTimeInDays);
                        }
                        ProductionPlan_WoVM production = new ProductionPlan_WoVM
                        {
                            WONumber = item.WONumber,
                            WoId = item.WOID,
                            SalesOrderId = item.SalesOrderId,
                            PartId = item.PartId,
                            PartType = item.PartType,
                            Parentlevel = item.Parentlevel,
                            BuildToStock = item.BuildToStock,
                            TestData = item.TestData,
                            CalcWOQty = item.CalcWOQty,
                            PlanStartDate = planstartdt,
                            PlanCompletionDate = item.PlanCompletionDate,
                            SoComplDate = item.SoComplDate,
                            RoutingId = item.RoutingId,
                            StartingOpNo = item.StartingOpNo,
                            EndingOpNo = item.EndingOpNo,
                            ReloadOption = "",
                            TenantId = item.TenantId,
                        };
                        productions.Add(production);
                    }
                }

                var procdutionpost = await _woService.ProductionPlanWoPost(productions);
                try
                {
                    if (procdutionpost.Any())
                    {
                        List<ProcPlanVM> listprocplan = new List<ProcPlanVM>();
                        List<BOMListVM> listbom = new List<BOMListVM>();
                        List<ProductionPlan_WoVM> childwos = new List<ProductionPlan_WoVM>();
                        List<ChildWoRelVM> childWoRels = new List<ChildWoRelVM>();
                        List<McTimeListVM> mcTimeListVMs = new List<McTimeListVM>();

                        var updatewo = await _woService.UpdateMultipleWorkOrder(workOrders);
                        int totalLeadTime = 0;
                        foreach (var item in procdutionpost)
                        {
                            if (item.TestData == 'Y')
                            {
                                ManufacturedPartNoDetailVM mf = await _masterService.GetManufPart((int)item.PartId);
                                if (mf.ManufacturedPartType == 1)
                                {
                                    var mpmakefromlist = await _masterService.GetMPMakeFromListByPartId(mf.ManufacturedPartNoDetailId.ToString());
                                    foreach (var mpmakefrom in mpmakefromlist)
                                    {
                                        ChildWoRelVM cwo = new ChildWoRelVM()
                                        {
                                            WoId = item.WoId,
                                            PartId = mpmakefrom.MPPartId,
                                            Qnty = decimal.TryParse(mpmakefrom.InputWeight, out decimal quantity) ? quantity : 0,
                                            CameFrom = "MakeFromPart"
                                        };
                                        childWoRels.Add(cwo);
                                    }
                                    var groupedResults = mpmakefromlist.GroupBy(x => x.MPPartId)
                                           .Select(g => new
                                           {
                                               PartId = g.Key,
                                               TotalQuantity = g.Sum(x =>
                                               {
                                                   decimal quantity;
                                                   return decimal.TryParse(x.InputWeight, out quantity) ? quantity : 0;
                                               }),

                                           })
                                            .ToList();
                                    foreach (var grouped in groupedResults)
                                    {
                                        var mfpdList = await _masterService.PartPurchasesFor(grouped.PartId);
                                        var ptype = await _masterService.GetRMPart(grouped.PartId);

                                        if (ptype.MasterPartType == null)
                                        {
                                            var manufPart = await _masterService.GetManufPart(grouped.PartId);

                                            if (manufPart != null)
                                            {
                                                ptype = new RawMaterialDetailVM
                                                {
                                                    MasterPartType = manufPart.MasterPartType
                                                };
                                            }
                                        }

                                        totalLeadTime = mfpdList.Sum(x => x.LeadTimeInDays);
                                        DateTime nextworkdingdate = DateTime.Now;
                                        nextworkdingdate = nextworkdingdate.AddDays(totalLeadTime);
                                        decimal intermediateResult = grouped.TotalQuantity * item.CalcWOQty;
                                        if (groupedResults != null)
                                        {
                                            ProcPlanVM ppdata = new ProcPlanVM
                                            {
                                                PartId = grouped.PartId,
                                                PartType = ptype.MasterPartType,
                                                Calc_Proc_Qnty = (int)intermediateResult,
                                                UOMId = mf.UOMId,
                                                PlanReceiptDate = (DateTime)item.PlanCompletionDate,
                                                CalcReceiptDate = nextworkdingdate,
                                                WorkOrderId = item.WoId
                                            };
                                            listprocplan.Add(ppdata);
                                            BOMListVM bomdata = new BOMListVM
                                            {
                                                ParentWoId = item.WoId,
                                                Child_Part_No_ID = grouped.PartId,
                                                Child_Part_No_Type = ptype.MasterPartType.ToString(),
                                                Calc_Qnty = (int)intermediateResult,
                                                Plan_Qnty = item.CalcWOQty,
                                                //Plan_Start_Dt = planstartdt,
                                                Plan_Compl_Dt = item.PlanCompletionDate.GetValueOrDefault(),
                                                CalcReceiptDate = nextworkdingdate,
                                                //Manf_Days_Avl = manfDays,
                                                ProcPlanId = item.ProductionPlanId,
                                                //SaNestLevel = Sa_Nest_level
                                            };
                                            if (ptype.MasterPartType == "ManufacturedPart")
                                            {

                                                ProductionPlan_WoVM cwo = new ProductionPlan_WoVM()
                                                {
                                                    WoId = item.WoId,
                                                    ParentWoId = item.WoId,
                                                    SalesOrderId = item.SalesOrderId,
                                                    PartId = bomdata.Child_Part_No_ID,
                                                    PartType = 1,
                                                    Parentlevel = 'N',
                                                    BuildToStock = item.BuildToStock,
                                                    TestData = item.TestData,
                                                    CalcWOQty = bomdata.Calc_Qnty,
                                                    PlanStartDate = item.PlanCompletionDate.GetValueOrDefault(),
                                                    PlanCompletionDate = nextworkdingdate,
                                                    SoComplDate = item.SoComplDate,
                                                    RoutingId = item.RoutingId,
                                                    StartingOpNo = item.StartingOpNo,
                                                    EndingOpNo = item.EndingOpNo,
                                                    For_Ref = 'N',
                                                    ReloadOption = "",
                                                    TenantId = item.TenantId,
                                                };
                                                childwos.Add(cwo);
                                            }
                                            listbom.Add(bomdata);
                                        }
                                    }
                                }
                                else if (mf.ManufacturedPartType == 2)
                                {
                                    var bomlst = await _masterService.BOMS(mf.ManufacturedPartNoDetailId.ToString());
                                    foreach (var bomVM in bomlst)
                                    {
                                        ChildWoRelVM cwo = new ChildWoRelVM()
                                        {
                                            WoId = item.WoId,
                                            PartId = bomVM.BOMPartId,
                                            Qnty = Convert.ToInt32(bomVM.Quantity),
                                            CameFrom = "BOM"
                                        };
                                        childWoRels.Add(cwo);
                                    }
                                    var bomgroupedResults = bomlst.GroupBy(x => x.BOMPartId)
                                           .Select(g => new
                                           {
                                               PartId = g.Key,
                                               TotalQuantity = g.Sum(x => x.Quantity)
                                           })
                                            .ToList();
                                    foreach (var bomgrp in bomgroupedResults)
                                    {
                                        var mp = await _masterService.ItemMasterPartById(bomgrp.PartId);
                                        var workdetails = await _plantService.GetPlantWD(1);
                                        var holidaylist = await _plantService.GetHolidays(1);
                                        string weekOff1 = workdetails.WeeklyOff1;
                                        string weekOff2 = workdetails.WeeklyOff2;
                                        var resultList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                                        int minutes = 0;
                                        foreach (var rote in resultList)
                                        {
                                            var result = await _routingService.RoutingSteps(rote.RoutingId);
                                            foreach (var step in result)
                                            {
                                                var stepdetails = await _routingService.StepMachines((int)step.StepId);
                                                var processingTimeSum = stepdetails
                                                                .GroupBy(sd => sd.RoutingStepId)
                                                                .Select(g => new
                                                                {
                                                                    RoutingStepId = g.Key,
                                                                    TotalProcessingTime = g.Sum(sd => TimeSpan.Parse(sd.FloorToFloorTime).TotalMinutes)
                                                                });
                                                foreach (var min in processingTimeSum)
                                                {
                                                    minutes = (int)min.TotalProcessingTime;
                                                }
                                            }
                                        }
                                        int noofhr = 0;
                                        if (workdetails.NoOfShifts == 1)
                                        {
                                            noofhr = 420;
                                        }
                                        else
                                        {
                                            noofhr = 840;
                                        }
                                        int assyTime = (minutes * item.CalcWOQty) / noofhr;
                                        int assyTimeInDays = assyTime / 1440;
                                        DateTime planstartdt = item.PlanCompletionDate.Value.AddDays(-assyTimeInDays);
                                        switch (mp.MasterPartType)
                                        {
                                            case MasterPartType.ManufacturedPart:

                                                var manufchild = await _masterService.GetManufPart((int)bomgrp.PartId);
                                                var manfDays = 0;
                                                int Sa_Nest_level = 0;
                                                while (!IsWorkDay(planstartdt, holidaylist, weekOff1, weekOff2))
                                                {
                                                    planstartdt = planstartdt.AddDays(1);
                                                }
                                                if (manufchild.ManufacturedPartType == 1)
                                                {
                                                    DateTime planstdt = planstartdt;
                                                    DateTime plancpldt = item.PlanCompletionDate.GetValueOrDefault();
                                                    manfDays = Math.Max(0, (plancpldt - planstdt).Days);
                                                    var mpmakefromlist = await _masterService.GetMPMakeFromListByPartId(manufchild.ManufacturedPartNoDetailId.ToString());
                                                    foreach (var mpmakefrom in mpmakefromlist)
                                                    {
                                                        ChildWoRelVM subcwo = new ChildWoRelVM()
                                                        {
                                                            WoId = item.WoId,
                                                            PartId = mpmakefrom.MPPartId,
                                                            Qnty = decimal.TryParse(mpmakefrom.InputWeight, out decimal quantity) ? quantity : 0,
                                                            CameFrom = "BOM"
                                                        };
                                                        childWoRels.Add(subcwo);
                                                    }
                                                    var groupedResults = mpmakefromlist.GroupBy(x => x.MPPartId)
                                                        .Select(g => new
                                                        {
                                                            PartId = g.Key,
                                                            TotalQuantity = g.Sum(x =>
                                                            {
                                                                decimal quantity;
                                                                return decimal.TryParse(x.InputWeight, out quantity) ? quantity : 0;
                                                            })
                                                        })
                                                        .ToList();
                                                    foreach (var grouped in groupedResults)
                                                    {
                                                        var submfpdList = await _masterService.PartPurchasesFor(grouped.PartId);
                                                        var subptype = await _masterService.GetRMPart(grouped.PartId);
                                                        totalLeadTime = submfpdList.Sum(x => x.LeadTimeInDays);
                                                        DateTime subnextworkdingdate = DateTime.Now;
                                                        subnextworkdingdate = subnextworkdingdate.AddDays(totalLeadTime);
                                                        decimal intermediateResult = grouped.TotalQuantity * item.CalcWOQty;
                                                        if (groupedResults != null)
                                                        {
                                                            ProcPlanVM subppdata = new ProcPlanVM
                                                            {
                                                                PartId = grouped.PartId,
                                                                PartType = subptype.MasterPartType,
                                                                Calc_Proc_Qnty = (int)intermediateResult,
                                                                UOMId = manufchild.UOMId,
                                                                PlanReceiptDate = item.PlanStartDate,
                                                                CalcReceiptDate = subnextworkdingdate,
                                                                WorkOrderId = item.WoId
                                                            };
                                                            listprocplan.Add(subppdata);
                                                            BOMListVM subbomdata = new BOMListVM
                                                            {
                                                                ParentWoId = item.WoId,
                                                                Child_Part_No_ID = grouped.PartId,
                                                                Child_Part_No_Type = subptype.MasterPartType.ToString(),
                                                                Calc_Qnty = (int)intermediateResult,
                                                                Plan_Qnty = item.CalcWOQty,
                                                                Plan_Start_Dt = planstdt,
                                                                Plan_Compl_Dt = item.PlanCompletionDate.GetValueOrDefault(),
                                                                CalcReceiptDate = subnextworkdingdate,
                                                                //Manf_Days_Avl = manfDays,
                                                                ProcPlanId = item.ProductionPlanId,
                                                                //SaNestLevel = Sa_Nest_level
                                                            };
                                                            listbom.Add(subbomdata);

                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    var asyy = await _masterService.GetManufPart((int)bomgrp.PartId);
                                                    if (asyy.ManufacturedPartType == 2)
                                                    {
                                                        Sa_Nest_level = 1;

                                                    }
                                                    else
                                                    {
                                                        Sa_Nest_level = 2;
                                                    }

                                                }
                                                BOMListVM bomdata = new BOMListVM
                                                {
                                                    ParentWoId = item.WoId,
                                                    Child_Part_No_ID = bomgrp.PartId,
                                                    Child_Part_No_Type = mp.MasterPartType.ToString(),
                                                    Calc_Qnty = (int)bomgrp.TotalQuantity * item.CalcWOQty,
                                                    Plan_Qnty = item.CalcWOQty,
                                                    Plan_Start_Dt = planstartdt,
                                                    Plan_Compl_Dt = planstartdt,
                                                    CalcReceiptDate = planstartdt,
                                                    Manf_Days_Avl = manfDays,
                                                    ProcPlanId = item.ProductionPlanId,
                                                    SaNestLevel = Sa_Nest_level
                                                };
                                                listbom.Add(bomdata);
                                                ProductionPlan_WoVM cwo = new ProductionPlan_WoVM()
                                                {
                                                    WoId = item.WoId,
                                                    ParentWoId = item.WoId,
                                                    SalesOrderId = item.SalesOrderId,
                                                    PartId = bomdata.Child_Part_No_ID,
                                                    PartType = (int)manufchild.ManufacturedPartType,
                                                    Parentlevel = 'N',
                                                    BuildToStock = item.BuildToStock,
                                                    TestData = item.TestData,
                                                    CalcWOQty = bomdata.Calc_Qnty,
                                                    PlanStartDate = planstartdt,
                                                    PlanCompletionDate = item.PlanCompletionDate.Value.AddDays(-1),
                                                    SoComplDate = item.SoComplDate,
                                                    RoutingId = item.RoutingId,
                                                    StartingOpNo = item.StartingOpNo,
                                                    EndingOpNo = item.EndingOpNo,
                                                    For_Ref = 'N',
                                                    ReloadOption = "",
                                                    TenantId = item.TenantId,
                                                };
                                                childwos.Add(cwo);
                                                break;
                                            //case MasterPartType.BOM:

                                            //    break;
                                            case MasterPartType.BOF:
                                                var bofpdList = await _masterService.PartPurchasesFor(bomgrp.PartId);
                                                //var bofptype = await _masterService.GetRMPart(bomgrp.PartId);
                                                BoughtOutFinishDetailVM manuf = await _masterService.GetBOFPart(bomgrp.PartId);
                                                totalLeadTime = bofpdList.Sum(x => x.LeadTimeInDays);
                                                DateTime bofnextworkdingdate = DateTime.Now;
                                                bofnextworkdingdate = bofnextworkdingdate.AddDays(totalLeadTime);
                                                BOMListVM bofbomdata = new BOMListVM
                                                {
                                                    ParentWoId = item.WoId,
                                                    Child_Part_No_ID = bomgrp.PartId,
                                                    Child_Part_No_Type = mp.MasterPartType.ToString(),
                                                    Calc_Qnty = (int)bomgrp.TotalQuantity * item.CalcWOQty,
                                                    Plan_Qnty = item.CalcWOQty,
                                                    Plan_Compl_Dt = planstartdt,
                                                    CalcReceiptDate = bofnextworkdingdate,
                                                    ProcPlanId = item.ProductionPlanId
                                                };
                                                listbom.Add(bofbomdata);
                                                ProcPlanVM ppdata = new ProcPlanVM
                                                {
                                                    PartId = bomgrp.PartId,
                                                    PartType = mp.MasterPartType.ToString(),
                                                    Calc_Proc_Qnty = (int)bomgrp.TotalQuantity * item.CalcWOQty,
                                                    UOMId = manuf.UOMId,
                                                    PlanReceiptDate = item.PlanStartDate,
                                                    CalcReceiptDate = bofnextworkdingdate,
                                                    WorkOrderId = item.WoId
                                                };
                                                listprocplan.Add(ppdata);

                                                break;
                                            case MasterPartType.RawMaterial:
                                                var mfpdList = await _masterService.PartPurchasesFor(bomgrp.PartId);
                                                var ptype = await _masterService.GetRMPart(bomgrp.PartId);
                                                totalLeadTime = mfpdList.Sum(x => x.LeadTimeInDays);
                                                DateTime nextworkdingdate = DateTime.Now;
                                                nextworkdingdate = nextworkdingdate.AddDays(totalLeadTime);

                                                BOMListVM rmbomdata = new BOMListVM
                                                {
                                                    ParentWoId = item.WoId,
                                                    Child_Part_No_ID = bomgrp.PartId,
                                                    Child_Part_No_Type = mp.MasterPartType.ToString(),
                                                    Calc_Qnty = (int)bomgrp.TotalQuantity * item.CalcWOQty,
                                                    Plan_Qnty = item.CalcWOQty,
                                                    PlanReceiptDate = item.PlanStartDate,
                                                    CalcReceiptDate = nextworkdingdate,
                                                    ProcPlanId = item.ProductionPlanId
                                                };
                                                listbom.Add(rmbomdata);

                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                }

                            }

                            //McTimeList---
                            ManufacturedPartNoDetailVM mcmf = await _masterService.GetManufPart((int)item.PartId);
                            var mcworkdetails = await _plantService.GetPlantWD(1);
                            var mcholidaylist = await _plantService.GetHolidays(1);
                            string mcweekOff1 = mcworkdetails.WeeklyOff1;
                            string mcweekOff2 = mcworkdetails.WeeklyOff2;
                            var mcresultList = await _routingService.Routings(mcmf.ManufacturedPartNoDetailId);
                            int mcminutes = 0;
                            foreach (var rote in mcresultList)
                            {
                                var result = await _routingService.RoutingSteps(rote.RoutingId);
                                foreach (var step in result)
                                {
                                    var stepdetails = await _routingService.StepMachines((int)step.StepId);
                                    if (stepdetails.Count() != 0)
                                    {
                                        var processingTimeSum = stepdetails
                                                    .GroupBy(sd => sd.RoutingStepId)
                                                    .Select(g => new
                                                    {
                                                        RoutingStepId = g.Key,
                                                        TotalProcessingTime = g.Sum(sd => TimeSpan.Parse(sd.FloorToFloorTime).TotalMinutes)
                                                    });
                                        foreach (var min in processingTimeSum)
                                        {
                                            mcminutes = (int)min.TotalProcessingTime;
                                        }
                                    }
                                    else
                                    {
                                        var sub = await _routingService.SubCons((int)step.StepId);
                                        var subfirst = sub.FirstOrDefault();
                                        var subworks = await _routingService.SubConWSS((int)step.StepId, subfirst.SubConDetailsId);
                                        var processingTimeSum = subworks
                                                    .GroupBy(sd => sd.RoutingStepId)
                                                    .Select(g => new
                                                    {
                                                        RoutingStepId = g.Key,
                                                        TotalProcessingTime = g.Sum(sd => TimeSpan.Parse(sd.FloorToFloorTime).TotalMinutes)
                                                    });
                                        foreach (var min in processingTimeSum)
                                        {
                                            mcminutes = (int)min.TotalProcessingTime;
                                        }
                                    }
                                }
                            }
                            int mcassyTime = (mcminutes * item.CalcWOQty) / mcworkdetails.NoOfShifts;
                            int mcassyTimeInDays = mcassyTime / 1440;
                            DateTime mcplanstartdt = item.PlanCompletionDate.Value.AddDays(-mcassyTimeInDays);
                            var routingstep = await _routingService.RoutingSteps((int)item.RoutingId);
                            var oneroutingstep = routingstep.FirstOrDefault();
                            if (oneroutingstep != null)
                            {
                                var stepmachine = await _routingService.StepMachines((int)oneroutingstep.StepId);

                                var onestepmachine = stepmachine.FirstOrDefault();
                                if (onestepmachine != null)
                                {
                                    var machine = await _machineService.GetMachine((int)onestepmachine?.MachineId);
                                    // var result = await _departmentService.GetDepartments(1);
                                    //Total_Plan_time = Setup_time + (1st_Pc_Process_time+Cycle time  x (WO_Plan_Qnty-1))/No_of_parts_per_loading
                                    //TimeSpan.Parse(sd.FirstPieceProcessingTime).TotalMinutes

                                    var departments = await _departmentService.GetDepartments(1);
                                    var department = departments.FirstOrDefault(d => d.DepartmentId == machine.MachineDepartmentId);
                                    int setupTimeMinutes = (int)TimeSpan.Parse(onestepmachine.SetupTime).TotalMinutes;
                                    int floorToFloorTimeMinutes = (int)TimeSpan.Parse(onestepmachine.FloorToFloorTime).TotalMinutes;
                                    int noOfShifts = department.NoOfShifts;
                                    int calcWOQty = item.CalcWOQty;
                                    int noOfPartsPerLoading = onestepmachine.NoOfPartsPerLoading;

                                    if (noOfPartsPerLoading == 0)
                                    {
                                        // throw new DivideByZeroException("NoOfPartsPerLoading cannot be zero.");
                                        noOfPartsPerLoading = 1;
                                    }
                                    int totalPlanTime = setupTimeMinutes + ((floorToFloorTimeMinutes + noOfShifts) * (calcWOQty - 1)) / noOfPartsPerLoading;
                                    int totalPlanTimeInHoursRounded = (int)Math.Round(totalPlanTime / 60.0, MidpointRounding.AwayFromZero);
                                    Console.WriteLine($"Total Plan Time (Minutes): {totalPlanTime}");
                                    Console.WriteLine($"Total Plan Time (Rounded Hours): {totalPlanTimeInHoursRounded}");

                                    McTimeListVM mcTimeList = new McTimeListVM()
                                    {
                                        WoId = item.WoId,
                                        Routing_StepId = oneroutingstep.StepId,
                                        CompanyId = Convert.ToInt64(oneroutingstep.StepLocation),
                                        MachineId = onestepmachine.MachineId,
                                        MachineTypeId = machine.MachineMachineTypeId,
                                        PlanQnty = item.CalcWOQty,
                                        TotalPlanTime = totalPlanTimeInHoursRounded,
                                        McPlanStartTime = mcplanstartdt,
                                        McPlanEndTime = (DateTime)item.PlanCompletionDate,
                                    };
                                    mcTimeListVMs.Add(mcTimeList);
                                }
                                else
                                {
                                    var sub = await _routingService.SubCons((int)oneroutingstep.StepId);
                                    var subfirst = sub.FirstOrDefault();
                                    var subworks = await _routingService.SubConWSS((int)oneroutingstep.StepId, subfirst.SubConDetailsId);
                                    var onestepmach = subworks.FirstOrDefault();
                                    //var machine = await _machineService.GetMachine((int)onestepmach?.MachineType);
                                    // var result = await _departmentService.GetDepartments(1);
                                    //Total_Plan_time = Setup_time + (1st_Pc_Process_time+Cycle time  x (WO_Plan_Qnty-1))/No_of_parts_per_loading
                                    //TimeSpan.Parse(sd.FirstPieceProcessingTime).TotalMinutes

                                    var departments = await _departmentService.GetDepartments(1);
                                    var department = departments.FirstOrDefault();
                                    int setupTimeMinutes = (int)TimeSpan.Parse(onestepmach.SetupTime).TotalMinutes;
                                    int floorToFloorTimeMinutes = (int)TimeSpan.Parse(onestepmach.FloorToFloorTime).TotalMinutes;
                                    int noOfShifts = department.NoOfShifts;
                                    int calcWOQty = item.CalcWOQty;
                                    int noOfPartsPerLoading = onestepmach.NoOfPartsPerLoading;

                                    if (noOfPartsPerLoading == 0)
                                    {
                                        // throw new DivideByZeroException("NoOfPartsPerLoading cannot be zero.");
                                        noOfPartsPerLoading = 1;
                                    }
                                    int totalPlanTime = setupTimeMinutes + ((floorToFloorTimeMinutes + noOfShifts) * (calcWOQty - 1)) / noOfPartsPerLoading;
                                    int totalPlanTimeInHoursRounded = (int)Math.Round(totalPlanTime / 60.0, MidpointRounding.AwayFromZero);
                                    Console.WriteLine($"Total Plan Time (Minutes): {totalPlanTime}");
                                    Console.WriteLine($"Total Plan Time (Rounded Hours): {totalPlanTimeInHoursRounded}");

                                    McTimeListVM mcTimeList = new McTimeListVM()
                                    {
                                        WoId = item.WoId,
                                        Routing_StepId = oneroutingstep.StepId,
                                        CompanyId = Convert.ToInt64(oneroutingstep.StepLocation),
                                        MachineId = onestepmach.MachineType,
                                        MachineTypeId = onestepmach.MachineType,
                                        PlanQnty = item.CalcWOQty,
                                        TotalPlanTime = totalPlanTimeInHoursRounded,
                                        McPlanStartTime = mcplanstartdt,
                                        McPlanEndTime = (DateTime)item.PlanCompletionDate,
                                    };
                                    mcTimeListVMs.Add(mcTimeList);
                                }
                            }
                        }
                        if (listprocplan.Any())
                        {
                            var result = await _woService.ProcPlanPost(listprocplan);

                            List<ProcPlanPartPurChaseRelVM> purcList = new List<ProcPlanPartPurChaseRelVM>();
                            foreach (var item in result)
                            {
                                var submfpdList = await _masterService.PartPurchasesFor((int)item.PartId);
                                foreach (var pur in submfpdList)
                                {
                                    ProcPlanPartPurChaseRelVM subcwo = new ProcPlanPartPurChaseRelVM()
                                    {
                                        ProcPlanId = item.ProcPlanId,
                                        PartPurchaseId = pur.PartPurchaseId,
                                        LeadTime = pur.LeadTimeInDays.ToString(),
                                        Active = 1
                                    };
                                    purcList.Add(subcwo);
                                }
                            }
                            if (purcList.Any())
                            {
                                var bomresult = await _woService.ProcPurchasePost(purcList);
                            }

                        }
                        if (listbom.Any())
                        {
                            var bomresult = await _woService.BomListPost(listbom);
                        }
                        var childworels = await _woService.PostChildWoRel(childWoRels);
                        var machinetimepost = await _woService.PostMcTimeList(mcTimeListVMs);
                        var childproductionwopost = await _woService.ProductionPlanWoPost(childwos);
                        if (childproductionwopost.Any())
                        {
                            List<ProcPlanVM> sublistprocplan = new List<ProcPlanVM>();
                            List<BOMListVM> sublistbom = new List<BOMListVM>();
                            List<ProductionPlan_WoVM> subchildwos = new List<ProductionPlan_WoVM>();
                            List<ChildWoRelVM> subchildWoRels = new List<ChildWoRelVM>();
                            List<McTimeListVM> submcTimeListVMs = new List<McTimeListVM>();
                            int subtotalLeadTime = 0;
                            foreach (var item in childproductionwopost)
                            {
                                if (item.TestData == 'Y')
                                {
                                    ManufacturedPartNoDetailVM mf = await _masterService.GetManufPart((int)item.PartId);
                                    if (mf.ManufacturedPartType == 1)
                                    {
                                        var mpmakefromlist = await _masterService.GetMPMakeFromListByPartId(mf.ManufacturedPartNoDetailId.ToString());
                                        foreach (var mpmakefrom in mpmakefromlist)
                                        {
                                            ChildWoRelVM cwo = new ChildWoRelVM()
                                            {
                                                WoId = item.WoId,
                                                PartId = mpmakefrom.MPPartId,
                                                Qnty = decimal.TryParse(mpmakefrom.InputWeight, out decimal quantity) ? quantity : 0,
                                                CameFrom = "MakeFromPart"
                                            };

                                            subchildWoRels.Add(cwo);
                                        }
                                        var groupedResults = mpmakefromlist.GroupBy(x => x.MPPartId)
                                               .Select(g => new
                                               {
                                                   PartId = g.Key,
                                                   TotalQuantity = g.Sum(x =>
                                                   {
                                                       decimal quantity;
                                                       return decimal.TryParse(x.InputWeight, out quantity) ? quantity : 0;
                                                   })
                                               })
                                                .ToList();
                                        foreach (var grouped in groupedResults)
                                        {
                                            var mfpdList = await _masterService.PartPurchasesFor(grouped.PartId);
                                            var ptype = await _masterService.GetRMPart(grouped.PartId);
                                            subtotalLeadTime = mfpdList.Sum(x => x.LeadTimeInDays);
                                            DateTime nextworkdingdate = DateTime.Now;
                                            nextworkdingdate = nextworkdingdate.AddDays(subtotalLeadTime);
                                            decimal intermediateResult = grouped.TotalQuantity * item.CalcWOQty;
                                            if (groupedResults != null)
                                            {
                                                ProcPlanVM ppdata = new ProcPlanVM
                                                {
                                                    PartId = grouped.PartId,
                                                    PartType = ptype.MasterPartType,
                                                    Calc_Proc_Qnty = (int)intermediateResult,
                                                    UOMId = mf.UOMId,
                                                    PlanReceiptDate = item.PlanStartDate,
                                                    CalcReceiptDate = nextworkdingdate,
                                                    WorkOrderId = item.WoId
                                                };
                                                sublistprocplan.Add(ppdata);
                                                BOMListVM bomdata = new BOMListVM
                                                {
                                                    ParentWoId = item.WoId,
                                                    Child_Part_No_ID = grouped.PartId,
                                                    Child_Part_No_Type = ptype.MasterPartType.ToString(),
                                                    Calc_Qnty = (int)intermediateResult,
                                                    Plan_Qnty = item.CalcWOQty,
                                                    //Plan_Start_Dt = planstartdt,
                                                    Plan_Compl_Dt = item.PlanCompletionDate.Value.AddDays(-1),
                                                    CalcReceiptDate = nextworkdingdate,
                                                    //Manf_Days_Avl = manfDays,
                                                    ProcPlanId = item.ProductionPlanId,
                                                    //SaNestLevel = Sa_Nest_level
                                                };
                                                sublistbom.Add(bomdata);
                                            }
                                        }
                                    }
                                    else if (mf.ManufacturedPartType == 2)
                                    {
                                        var bomlst = await _masterService.BOMS(mf.ManufacturedPartNoDetailId.ToString());
                                        foreach (var bomVM in bomlst)
                                        {
                                            ChildWoRelVM cwo = new ChildWoRelVM()
                                            {
                                                WoId = item.WoId,
                                                PartId = bomVM.BOMPartId,
                                                Qnty = Convert.ToInt32(bomVM.Quantity),
                                                CameFrom = "BOM"
                                            };
                                            subchildWoRels.Add(cwo);
                                        }
                                        var bomgroupedResults = bomlst.GroupBy(x => x.BOMPartId)
                                               .Select(g => new
                                               {
                                                   PartId = g.Key,
                                                   TotalQuantity = g.Sum(x => x.Quantity)
                                               })
                                                .ToList();
                                        foreach (var bomgrp in bomgroupedResults)
                                        {
                                            var mp = await _masterService.ItemMasterPartById(bomgrp.PartId);
                                            var workdetails = await _plantService.GetPlantWD(1);
                                            var holidaylist = await _plantService.GetHolidays(1);
                                            string weekOff1 = workdetails.WeeklyOff1;
                                            string weekOff2 = workdetails.WeeklyOff2;
                                            var resultList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                                            int minutes = 0;
                                            foreach (var rote in resultList)
                                            {
                                                var result = await _routingService.RoutingSteps(rote.RoutingId);
                                                foreach (var step in result)
                                                {
                                                    var stepdetails = await _routingService.StepMachines((int)step.StepId);
                                                    var processingTimeSum = stepdetails
                                                                    .GroupBy(sd => sd.RoutingStepId)
                                                                    .Select(g => new
                                                                    {
                                                                        RoutingStepId = g.Key,
                                                                        TotalProcessingTime = g.Sum(sd => TimeSpan.Parse(sd.FloorToFloorTime).TotalMinutes)
                                                                    });
                                                    foreach (var min in processingTimeSum)
                                                    {
                                                        minutes = (int)min.TotalProcessingTime;
                                                    }
                                                }
                                            }
                                            int nohr = 0;
                                            if (workdetails.NoOfShifts == 1)
                                            {
                                                nohr = 420;
                                            }
                                            else
                                            {
                                                nohr = 840;
                                            }
                                            int assyTime = (minutes * item.CalcWOQty) / nohr;
                                            int assyTimeInDays = assyTime / 1440;
                                            DateTime planstartdt = item.PlanCompletionDate.Value.AddDays(-assyTimeInDays);
                                            switch (mp.MasterPartType)
                                            {
                                                case MasterPartType.ManufacturedPart:


                                                    var manufchild = await _masterService.GetManufPart((int)bomgrp.PartId);
                                                    var manfDays = 0;
                                                    int Sa_Nest_level = 0;
                                                    while (!IsWorkDay(planstartdt, holidaylist, weekOff1, weekOff2))
                                                    {
                                                        planstartdt = planstartdt.AddDays(1);
                                                    }
                                                    if (manufchild.ManufacturedPartType == 1)
                                                    {
                                                        DateTime planstdt = planstartdt;
                                                        DateTime plancpldt = item.PlanCompletionDate.GetValueOrDefault();
                                                        manfDays = Math.Max(0, (plancpldt - planstdt).Days);
                                                        //--changed RM 
                                                        var mpmakefromlist = await _masterService.GetMPMakeFromListByPartId(manufchild.ManufacturedPartNoDetailId.ToString());
                                                        foreach (var mpmakefrom in mpmakefromlist)
                                                        {
                                                            ChildWoRelVM subcwo = new ChildWoRelVM()
                                                            {
                                                                WoId = item.WoId,
                                                                PartId = mpmakefrom.MPPartId,
                                                                Qnty = decimal.TryParse(mpmakefrom.InputWeight, out decimal quantity) ? quantity : 0,
                                                                CameFrom = "BOM"
                                                            };
                                                            subchildWoRels.Add(subcwo);
                                                        }
                                                        var groupedResults = mpmakefromlist.GroupBy(x => x.MPPartId)
                                                   .Select(g => new
                                                   {
                                                       PartId = g.Key,
                                                       TotalQuantity = g.Sum(x =>
                                                       {
                                                           decimal quantity;
                                                           return decimal.TryParse(x.InputWeight, out quantity) ? quantity : 0;
                                                       })
                                                   })
                                                    .ToList();
                                                        foreach (var grouped in groupedResults)
                                                        {
                                                            var submfpdList = await _masterService.PartPurchasesFor(grouped.PartId);
                                                            var subptype = await _masterService.GetRMPart(grouped.PartId);
                                                            subtotalLeadTime = submfpdList.Sum(x => x.LeadTimeInDays);
                                                            DateTime subnextworkdingdate = DateTime.Now;
                                                            subnextworkdingdate = subnextworkdingdate.AddDays(subtotalLeadTime);
                                                            decimal intermediateResult = grouped.TotalQuantity * item.CalcWOQty;
                                                            if (groupedResults != null)
                                                            {
                                                                ProcPlanVM subppdata = new ProcPlanVM
                                                                {
                                                                    PartId = grouped.PartId,
                                                                    PartType = subptype.MasterPartType,
                                                                    Calc_Proc_Qnty = (int)intermediateResult,
                                                                    UOMId = manufchild.UOMId,
                                                                    PlanReceiptDate = item.PlanStartDate,
                                                                    CalcReceiptDate = subnextworkdingdate,
                                                                    WorkOrderId = item.WoId
                                                                };
                                                                sublistprocplan.Add(subppdata);
                                                                BOMListVM subbomdata = new BOMListVM
                                                                {
                                                                    ParentWoId = item.WoId,
                                                                    Child_Part_No_ID = grouped.PartId,
                                                                    Child_Part_No_Type = subptype.MasterPartType.ToString(),
                                                                    Calc_Qnty = (int)intermediateResult,
                                                                    Plan_Qnty = item.CalcWOQty,
                                                                    Plan_Start_Dt = planstartdt,
                                                                    Plan_Compl_Dt = item.PlanCompletionDate.GetValueOrDefault(),
                                                                    CalcReceiptDate = subnextworkdingdate,
                                                                    //Manf_Days_Avl = manfDays,
                                                                    ProcPlanId = item.ProductionPlanId,
                                                                    //SaNestLevel = Sa_Nest_level
                                                                };
                                                                sublistbom.Add(subbomdata);
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        var asyy = await _masterService.GetManufPart((int)bomgrp.PartId);
                                                        if (asyy.ManufacturedPartType == 2)
                                                        {
                                                            Sa_Nest_level = 1;
                                                        }
                                                        else
                                                        {
                                                            Sa_Nest_level = 2;
                                                        }
                                                    }

                                                    BOMListVM bomdata = new BOMListVM
                                                    {
                                                        ParentWoId = item.WoId,
                                                        Child_Part_No_ID = bomgrp.PartId,
                                                        Child_Part_No_Type = mp.MasterPartType.ToString(),
                                                        Calc_Qnty = (int)bomgrp.TotalQuantity * item.CalcWOQty,
                                                        Plan_Qnty = item.CalcWOQty,
                                                        Plan_Start_Dt = planstartdt,
                                                        Plan_Compl_Dt = planstartdt,
                                                        CalcReceiptDate = planstartdt,
                                                        Manf_Days_Avl = manfDays,
                                                        ProcPlanId = item.ProductionPlanId,
                                                        SaNestLevel = Sa_Nest_level
                                                    };
                                                    sublistbom.Add(bomdata);
                                                    ProductionPlan_WoVM cwo = new ProductionPlan_WoVM()
                                                    {
                                                        WoId = item.WoId,
                                                        ParentWoId = item.WoId,
                                                        SalesOrderId = item.SalesOrderId,
                                                        PartId = bomdata.Child_Part_No_ID,
                                                        PartType = (int)manufchild.ManufacturedPartType,
                                                        Parentlevel = 'N',
                                                        BuildToStock = item.BuildToStock,
                                                        TestData = item.TestData,
                                                        CalcWOQty = bomdata.Calc_Qnty,
                                                        PlanStartDate = planstartdt,
                                                        PlanCompletionDate = item.PlanCompletionDate.Value.AddDays(-1),
                                                        SoComplDate = item.SoComplDate,
                                                        RoutingId = item.RoutingId,
                                                        StartingOpNo = item.StartingOpNo,
                                                        EndingOpNo = item.EndingOpNo,
                                                        For_Ref = 'N',
                                                        ReloadOption = "",
                                                        TenantId = item.TenantId,
                                                    };
                                                    subchildwos.Add(cwo);
                                                    break;
                                                //case MasterPartType.BOM:

                                                //    break;
                                                case MasterPartType.BOF:
                                                    var bofpdList = await _masterService.PartPurchasesFor(bomgrp.PartId);
                                                    //var bofptype = await _masterService.GetRMPart(bomgrp.PartId);
                                                    BoughtOutFinishDetailVM manuf = await _masterService.GetBOFPart(bomgrp.PartId);
                                                    subtotalLeadTime = bofpdList.Sum(x => x.LeadTimeInDays);
                                                    DateTime bofnextworkdingdate = DateTime.Now;
                                                    bofnextworkdingdate = bofnextworkdingdate.AddDays(subtotalLeadTime);
                                                    BOMListVM bofbomdata = new BOMListVM
                                                    {
                                                        ParentWoId = item.WoId,
                                                        Child_Part_No_ID = bomgrp.PartId,
                                                        Child_Part_No_Type = mp.MasterPartType.ToString(),
                                                        Calc_Qnty = (int)bomgrp.TotalQuantity * item.CalcWOQty,
                                                        Plan_Qnty = item.CalcWOQty,
                                                        Plan_Compl_Dt = planstartdt,
                                                        CalcReceiptDate = bofnextworkdingdate,
                                                        ProcPlanId = item.ProductionPlanId
                                                    };
                                                    sublistbom.Add(bofbomdata);
                                                    ProcPlanVM ppdata = new ProcPlanVM
                                                    {
                                                        PartId = bomgrp.PartId,
                                                        PartType = mp.MasterPartType.ToString(),
                                                        Calc_Proc_Qnty = (int)bomgrp.TotalQuantity * item.CalcWOQty,
                                                        UOMId = manuf.UOMId,
                                                        PlanReceiptDate = (DateTime)item.PlanCompletionDate,
                                                        CalcReceiptDate = bofnextworkdingdate,
                                                        WorkOrderId = item.WoId
                                                    };
                                                    sublistprocplan.Add(ppdata);

                                                    break;
                                                case MasterPartType.RawMaterial:
                                                    var mfpdList = await _masterService.PartPurchasesFor(bomgrp.PartId);
                                                    var ptype = await _masterService.GetRMPart(bomgrp.PartId);
                                                    subtotalLeadTime = mfpdList.Sum(x => x.LeadTimeInDays);
                                                    DateTime nextworkdingdate = (DateTime)item.PlanCompletionDate;
                                                    nextworkdingdate = nextworkdingdate.AddDays(subtotalLeadTime);

                                                    BOMListVM rmbomdata = new BOMListVM
                                                    {
                                                        ParentWoId = item.WoId,
                                                        Child_Part_No_ID = bomgrp.PartId,
                                                        Child_Part_No_Type = mp.MasterPartType.ToString(),
                                                        Calc_Qnty = (int)bomgrp.TotalQuantity * item.CalcWOQty,
                                                        Plan_Qnty = item.CalcWOQty,
                                                        Plan_Compl_Dt = planstartdt,
                                                        CalcReceiptDate = nextworkdingdate,
                                                        ProcPlanId = item.ProductionPlanId
                                                    };
                                                    sublistbom.Add(rmbomdata);

                                                    break;
                                                default:
                                                    break;
                                            }
                                        }

                                    }

                                }

                                //McTimeList---
                                ManufacturedPartNoDetailVM mcmf = await _masterService.GetManufPart((int)item.PartId);
                                var mcworkdetails = await _plantService.GetPlantWD(1);
                                var mcholidaylist = await _plantService.GetHolidays(1);
                                string mcweekOff1 = mcworkdetails.WeeklyOff1;
                                string mcweekOff2 = mcworkdetails.WeeklyOff2;
                                var mcresultList = await _routingService.Routings(mcmf.ManufacturedPartNoDetailId);
                                int mcminutes = 0;
                                foreach (var rote in mcresultList)
                                {
                                    var result = await _routingService.RoutingSteps(rote.RoutingId);
                                    foreach (var step in result)
                                    {
                                        var stepdetails = await _routingService.StepMachines((int)step.StepId);
                                        if (stepdetails.Count() != 0)
                                        {
                                            var processingTimeSum = stepdetails
                                                        .GroupBy(sd => sd.RoutingStepId)
                                                        .Select(g => new
                                                        {
                                                            RoutingStepId = g.Key,
                                                            TotalProcessingTime = g.Sum(sd => TimeSpan.Parse(sd.FirstPieceProcessingTime).TotalMinutes)
                                                        });
                                            foreach (var min in processingTimeSum)
                                            {
                                                mcminutes = (int)min.TotalProcessingTime;
                                            }
                                        }
                                        else
                                        {
                                            var sub = await _routingService.SubCons((int)step.StepId);
                                            var subfirst = sub.FirstOrDefault();
                                            var subworks = await _routingService.SubConWSS((int)step.StepId, subfirst.SubConDetailsId);
                                            var processingTimeSum = subworks
                                                        .GroupBy(sd => sd.RoutingStepId)
                                                        .Select(g => new
                                                        {
                                                            RoutingStepId = g.Key,
                                                            TotalProcessingTime = g.Sum(sd => TimeSpan.Parse(sd.FloorToFloorTime).TotalMinutes)
                                                        });
                                            foreach (var min in processingTimeSum)
                                            {
                                                mcminutes = (int)min.TotalProcessingTime;
                                            }
                                        }
                                    }
                                }
                                int mcassyTime = (mcminutes * item.CalcWOQty) / mcworkdetails.NoOfShifts;
                                int mcassyTimeInDays = mcassyTime / 1440;
                                DateTime mcplanstartdt = item.PlanCompletionDate.Value.AddDays(-mcassyTimeInDays);
                                var routingstep = await _routingService.RoutingSteps((int)item.RoutingId);
                                var oneroutingstep = routingstep.FirstOrDefault();
                                if (oneroutingstep != null)
                                {
                                    var stepmachine = await _routingService.StepMachines((int)oneroutingstep.StepId);
                                    var onestepmachine = stepmachine.FirstOrDefault();
                                    if (onestepmachine != null)
                                    {
                                        var machine = await _machineService.GetMachine((int)onestepmachine?.MachineId);

                                        var departments = await _departmentService.GetDepartments(1);
                                        var department = departments.FirstOrDefault(d => d.DepartmentId == machine.MachineDepartmentId);
                                        int setupTimeMinutes = (int)TimeSpan.Parse(onestepmachine.SetupTime).TotalMinutes;
                                        int floorToFloorTimeMinutes = (int)TimeSpan.Parse(onestepmachine.FloorToFloorTime).TotalMinutes;
                                        int noOfShifts = department.NoOfShifts;
                                        int calcWOQty = item.CalcWOQty;
                                        int noOfPartsPerLoading = onestepmachine.NoOfPartsPerLoading;

                                        if (noOfPartsPerLoading == 0)
                                        {
                                            // throw new DivideByZeroException("NoOfPartsPerLoading cannot be zero.");
                                            noOfPartsPerLoading = 1;
                                        }
                                        int totalPlanTime = setupTimeMinutes + ((floorToFloorTimeMinutes + noOfShifts) * (calcWOQty - 1)) / noOfPartsPerLoading;
                                        int totalPlanTimeInHoursRounded = (int)Math.Round(totalPlanTime / 60.0, MidpointRounding.AwayFromZero);
                                        Console.WriteLine($"Total Plan Time (Minutes): {totalPlanTime}");
                                        Console.WriteLine($"Total Plan Time (Rounded Hours): {totalPlanTimeInHoursRounded}");

                                        McTimeListVM mcTimeList = new McTimeListVM()
                                        {
                                            WoId = item.WoId,
                                            Routing_StepId = oneroutingstep.StepId,
                                            CompanyId = Convert.ToInt64(oneroutingstep.StepLocation),
                                            MachineId = onestepmachine.MachineId,
                                            MachineTypeId = machine.MachineMachineTypeId,
                                            PlanQnty = item.CalcWOQty,
                                            TotalPlanTime = totalPlanTimeInHoursRounded,
                                            McPlanStartTime = mcplanstartdt,
                                            McPlanEndTime = (DateTime)item.PlanCompletionDate,
                                        };
                                        submcTimeListVMs.Add(mcTimeList);
                                    }
                                    else
                                    {
                                        var sub = await _routingService.SubCons((int)oneroutingstep.StepId);
                                        var subfirst = sub.FirstOrDefault();
                                        var subworks = await _routingService.SubConWSS((int)oneroutingstep.StepId, subfirst.SubConDetailsId);
                                        var onestepmach = subworks.FirstOrDefault();
                                        //var machine = await _machineService.GetMachine((int)onestepmach?.MachineType);
                                        // var result = await _departmentService.GetDepartments(1);
                                        //Total_Plan_time = Setup_time + (1st_Pc_Process_time+Cycle time  x (WO_Plan_Qnty-1))/No_of_parts_per_loading
                                        //TimeSpan.Parse(sd.FirstPieceProcessingTime).TotalMinutes

                                        var departments = await _departmentService.GetDepartments(1);
                                        var department = departments.FirstOrDefault();
                                        int setupTimeMinutes = (int)TimeSpan.Parse(onestepmach.SetupTime).TotalMinutes;
                                        int floorToFloorTimeMinutes = (int)TimeSpan.Parse(onestepmach.FloorToFloorTime).TotalMinutes;
                                        int noOfShifts = department.NoOfShifts;
                                        int calcWOQty = item.CalcWOQty;
                                        int noOfPartsPerLoading = onestepmach.NoOfPartsPerLoading;

                                        if (noOfPartsPerLoading == 0)
                                        {
                                            // throw new DivideByZeroException("NoOfPartsPerLoading cannot be zero.");
                                            noOfPartsPerLoading = 1;
                                        }
                                        int totalPlanTime = setupTimeMinutes + ((floorToFloorTimeMinutes + noOfShifts) * (calcWOQty - 1)) / noOfPartsPerLoading;
                                        int totalPlanTimeInHoursRounded = (int)Math.Round(totalPlanTime / 60.0, MidpointRounding.AwayFromZero);
                                        Console.WriteLine($"Total Plan Time (Minutes): {totalPlanTime}");
                                        Console.WriteLine($"Total Plan Time (Rounded Hours): {totalPlanTimeInHoursRounded}");

                                        McTimeListVM mcTimeList = new McTimeListVM()
                                        {
                                            WoId = item.WoId,
                                            Routing_StepId = oneroutingstep.StepId,
                                            CompanyId = Convert.ToInt64(oneroutingstep.StepLocation),
                                            MachineId = onestepmach.MachineType,
                                            MachineTypeId = onestepmach.MachineType,
                                            PlanQnty = item.CalcWOQty,
                                            TotalPlanTime = totalPlanTimeInHoursRounded,
                                            McPlanStartTime = mcplanstartdt,
                                            McPlanEndTime = (DateTime)item.PlanCompletionDate,
                                        };
                                        submcTimeListVMs.Add(mcTimeList);
                                    }
                                }
                            }
                            if (sublistprocplan.Any())
                            {
                                var result = await _woService.ProcPlanPost(sublistprocplan);
                                List<ProcPlanPartPurChaseRelVM> purcList = new List<ProcPlanPartPurChaseRelVM>();
                                foreach (var item in result)
                                {
                                    var submfpdList = await _masterService.PartPurchasesFor((int)item.PartId);
                                    foreach (var pur in submfpdList)
                                    {
                                        ProcPlanPartPurChaseRelVM subcwo = new ProcPlanPartPurChaseRelVM()
                                        {
                                            ProcPlanId = item.ProcPlanId,
                                            PartPurchaseId = pur.PartPurchaseId,
                                            LeadTime = pur.LeadTimeInDays.ToString(),
                                            Active = 1
                                        };
                                        purcList.Add(subcwo);
                                    }
                                }
                                if (purcList.Any())
                                {
                                    var bomresult = await _woService.ProcPurchasePost(purcList);
                                }
                            }
                            if (sublistbom.Any())
                            {
                                var bomresult = await _woService.BomListPost(sublistbom);
                            }
                            var subchildworels = await _woService.PostChildWoRel(subchildWoRels);
                            var submachinetimepost = await _woService.PostMcTimeList(submcTimeListVMs);
                            var combinedSubchildren = subchildwos
                                        .Where(sc => sc.PartType == 1)
                                        .GroupBy(sc => sc.PartId)
                                        .Select(g => new ProductionPlan_WoVM
                                        {
                                            PartId = g.Key,
                                            CalcWOQty = g.Sum(sc => sc.CalcWOQty),
                                            PlanCompletionDate = g.Min(sc => sc.PlanCompletionDate)
                                        })
                                        .ToList();
                            var subchildproductionwopost = await _woService.ProductionPlanWoPost(subchildwos);


                            if (subchildproductionwopost.Any())
                            {
                                List<ProcPlanVM> sublistprocplan2 = new List<ProcPlanVM>();
                                List<BOMListVM> sublistbom2 = new List<BOMListVM>();
                                List<ProductionPlan_WoVM> subchildwos2 = new List<ProductionPlan_WoVM>();
                                List<ChildWoRelVM> subchildWoRels2 = new List<ChildWoRelVM>();
                                List<McTimeListVM> submcTimeListVMs2 = new List<McTimeListVM>();
                                int subtotalLeadTime2 = 0;
                                foreach (var item in subchildproductionwopost)
                                {
                                    if (item.TestData == 'Y')
                                    {
                                        ManufacturedPartNoDetailVM mf = await _masterService.GetManufPart((int)item.PartId);
                                        if (mf.ManufacturedPartType == 1)
                                        {
                                            var mpmakefromlist = await _masterService.GetMPMakeFromListByPartId(mf.ManufacturedPartNoDetailId.ToString());
                                            foreach (var mpmakefrom in mpmakefromlist)
                                            {
                                                ChildWoRelVM cwo = new ChildWoRelVM()
                                                {
                                                    WoId = item.WoId,
                                                    PartId = mpmakefrom.MPPartId,
                                                    Qnty = decimal.TryParse(mpmakefrom.InputWeight, out decimal quantity) ? quantity : 0,
                                                    CameFrom = "MakeFromPart"
                                                };

                                                subchildWoRels2.Add(cwo);
                                            }
                                            var groupedResults = mpmakefromlist.GroupBy(x => x.MPPartId)
                                                   .Select(g => new
                                                   {
                                                       PartId = g.Key,
                                                       TotalQuantity = g.Sum(x =>
                                                       {
                                                           decimal quantity;
                                                           return decimal.TryParse(x.InputWeight, out quantity) ? quantity : 0;
                                                       })
                                                   })
                                                    .ToList();
                                            foreach (var grouped in groupedResults)
                                            {
                                                var mfpdList = await _masterService.PartPurchasesFor(grouped.PartId);
                                                var ptype = await _masterService.GetRMPart(grouped.PartId);
                                                subtotalLeadTime2 = mfpdList.Sum(x => x.LeadTimeInDays);
                                                DateTime nextworkdingdate = DateTime.Now;
                                                nextworkdingdate = nextworkdingdate.AddDays(subtotalLeadTime2);
                                                decimal intermediateResult = grouped.TotalQuantity * item.CalcWOQty;
                                                if (groupedResults != null)
                                                {
                                                    ProcPlanVM ppdata = new ProcPlanVM
                                                    {
                                                        PartId = grouped.PartId,
                                                        PartType = ptype.MasterPartType,
                                                        Calc_Proc_Qnty = (int)intermediateResult,
                                                        UOMId = mf.UOMId,
                                                        PlanReceiptDate = item.PlanStartDate,
                                                        CalcReceiptDate = nextworkdingdate,
                                                        WorkOrderId = item.WoId
                                                    };
                                                    sublistprocplan2.Add(ppdata);
                                                    BOMListVM bomdata = new BOMListVM
                                                    {
                                                        ParentWoId = item.WoId,
                                                        Child_Part_No_ID = grouped.PartId,
                                                        Child_Part_No_Type = ptype.MasterPartType.ToString(),
                                                        Calc_Qnty = (int)intermediateResult,
                                                        Plan_Qnty = item.CalcWOQty,
                                                        //Plan_Start_Dt = planstartdt,
                                                        Plan_Compl_Dt = item.PlanCompletionDate.Value.AddDays(-1),
                                                        CalcReceiptDate = nextworkdingdate,
                                                        //Manf_Days_Avl = manfDays,
                                                        ProcPlanId = item.ProductionPlanId,
                                                        //SaNestLevel = Sa_Nest_level
                                                    };
                                                    sublistbom2.Add(bomdata);
                                                }
                                            }
                                        }
                                        else if (mf.ManufacturedPartType == 2)
                                        {
                                            var bomlst = await _masterService.BOMS(mf.ManufacturedPartNoDetailId.ToString());
                                            foreach (var bomVM in bomlst)
                                            {
                                                ChildWoRelVM cwo = new ChildWoRelVM()
                                                {
                                                    WoId = item.WoId,
                                                    PartId = bomVM.BOMPartId,
                                                    Qnty = Convert.ToInt32(bomVM.Quantity),
                                                    CameFrom = "BOM"
                                                };
                                                subchildWoRels2.Add(cwo);
                                            }
                                            var bomgroupedResults = bomlst.GroupBy(x => x.BOMPartId)
                                                   .Select(g => new
                                                   {
                                                       PartId = g.Key,
                                                       TotalQuantity = g.Sum(x => x.Quantity)
                                                   })
                                                    .ToList();
                                            foreach (var bomgrp in bomgroupedResults)
                                            {
                                                var mp = await _masterService.ItemMasterPartById(bomgrp.PartId);
                                                var workdetails = await _plantService.GetPlantWD(1);
                                                var holidaylist = await _plantService.GetHolidays(1);
                                                string weekOff1 = workdetails.WeeklyOff1;
                                                string weekOff2 = workdetails.WeeklyOff2;
                                                var resultList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                                                int minutes = 0;
                                                foreach (var rote in resultList)
                                                {
                                                    var result = await _routingService.RoutingSteps(rote.RoutingId);
                                                    foreach (var step in result)
                                                    {
                                                        var stepdetails = await _routingService.StepMachines((int)step.StepId);
                                                        var processingTimeSum = stepdetails
                                                                        .GroupBy(sd => sd.RoutingStepId)
                                                                        .Select(g => new
                                                                        {
                                                                            RoutingStepId = g.Key,
                                                                            TotalProcessingTime = g.Sum(sd => TimeSpan.Parse(sd.FloorToFloorTime).TotalMinutes)
                                                                        });
                                                        foreach (var min in processingTimeSum)
                                                        {
                                                            minutes = (int)min.TotalProcessingTime;
                                                        }
                                                    }
                                                }
                                                int nohr = 0;
                                                if (workdetails.NoOfShifts == 1)
                                                {
                                                    nohr = 420;
                                                }
                                                else
                                                {
                                                    nohr = 840;
                                                }
                                                int assyTime = (minutes * item.CalcWOQty) / nohr;
                                                int assyTimeInDays = assyTime / 1440;
                                                DateTime planstartdt = item.PlanCompletionDate.Value.AddDays(-assyTimeInDays);
                                                switch (mp.MasterPartType)
                                                {
                                                    case MasterPartType.ManufacturedPart:


                                                        var manufchild = await _masterService.GetManufPart((int)bomgrp.PartId);
                                                        var manfDays = 0;
                                                        int Sa_Nest_level = 0;
                                                        while (!IsWorkDay(planstartdt, holidaylist, weekOff1, weekOff2))
                                                        {
                                                            planstartdt = planstartdt.AddDays(1);
                                                        }
                                                        if (manufchild.ManufacturedPartType == 1)
                                                        {
                                                            DateTime planstdt = planstartdt;
                                                            DateTime plancpldt = item.PlanCompletionDate.GetValueOrDefault();
                                                            manfDays = Math.Max(0, (plancpldt - planstdt).Days);
                                                            //--changed RM 
                                                            var mpmakefromlist = await _masterService.GetMPMakeFromListByPartId(manufchild.ManufacturedPartNoDetailId.ToString());
                                                            foreach (var mpmakefrom in mpmakefromlist)
                                                            {
                                                                ChildWoRelVM subcwo = new ChildWoRelVM()
                                                                {
                                                                    WoId = item.WoId,
                                                                    PartId = mpmakefrom.MPPartId,
                                                                    Qnty = decimal.TryParse(mpmakefrom.InputWeight, out decimal quantity) ? quantity : 0,
                                                                    CameFrom = "BOM"
                                                                };
                                                                subchildWoRels2.Add(subcwo);
                                                            }
                                                            var groupedResults = mpmakefromlist.GroupBy(x => x.MPPartId)
                                                       .Select(g => new
                                                       {
                                                           PartId = g.Key,
                                                           TotalQuantity = g.Sum(x =>
                                                           {
                                                               decimal quantity;
                                                               return decimal.TryParse(x.InputWeight, out quantity) ? quantity : 0;
                                                           })
                                                       })
                                                        .ToList();
                                                            foreach (var grouped in groupedResults)
                                                            {
                                                                var submfpdList = await _masterService.PartPurchasesFor(grouped.PartId);
                                                                var subptype = await _masterService.GetRMPart(grouped.PartId);
                                                                subtotalLeadTime2 = submfpdList.Sum(x => x.LeadTimeInDays);
                                                                DateTime subnextworkdingdate = DateTime.Now;
                                                                subnextworkdingdate = subnextworkdingdate.AddDays(subtotalLeadTime2);
                                                                decimal intermediateResult = grouped.TotalQuantity * item.CalcWOQty;
                                                                if (groupedResults != null)
                                                                {
                                                                    ProcPlanVM subppdata = new ProcPlanVM
                                                                    {
                                                                        PartId = grouped.PartId,
                                                                        PartType = subptype.MasterPartType,
                                                                        Calc_Proc_Qnty = (int)intermediateResult,
                                                                        UOMId = manufchild.UOMId,
                                                                        PlanReceiptDate = item.PlanStartDate,
                                                                        CalcReceiptDate = subnextworkdingdate,
                                                                        WorkOrderId = item.WoId
                                                                    };
                                                                    sublistprocplan2.Add(subppdata);
                                                                    BOMListVM subbomdata = new BOMListVM
                                                                    {
                                                                        ParentWoId = item.WoId,
                                                                        Child_Part_No_ID = grouped.PartId,
                                                                        Child_Part_No_Type = subptype.MasterPartType.ToString(),
                                                                        Calc_Qnty = (int)intermediateResult,
                                                                        Plan_Qnty = item.CalcWOQty,
                                                                        Plan_Start_Dt = planstartdt,
                                                                        Plan_Compl_Dt = item.PlanCompletionDate.GetValueOrDefault(),
                                                                        CalcReceiptDate = subnextworkdingdate,
                                                                        //Manf_Days_Avl = manfDays,
                                                                        ProcPlanId = item.ProductionPlanId,
                                                                        //SaNestLevel = Sa_Nest_level
                                                                    };
                                                                    sublistbom2.Add(subbomdata);
                                                                }
                                                            }

                                                        }
                                                        else
                                                        {
                                                            var asyy = await _masterService.GetManufPart((int)bomgrp.PartId);
                                                            if (asyy.ManufacturedPartType == 2)
                                                            {
                                                                Sa_Nest_level = 1;
                                                            }
                                                            else
                                                            {
                                                                Sa_Nest_level = 2;
                                                            }
                                                        }

                                                        BOMListVM bomdata = new BOMListVM
                                                        {
                                                            ParentWoId = item.WoId,
                                                            Child_Part_No_ID = bomgrp.PartId,
                                                            Child_Part_No_Type = mp.MasterPartType.ToString(),
                                                            Calc_Qnty = (int)bomgrp.TotalQuantity * item.CalcWOQty,
                                                            Plan_Qnty = item.CalcWOQty,
                                                            Plan_Start_Dt = planstartdt,
                                                            Plan_Compl_Dt = planstartdt,
                                                            CalcReceiptDate = planstartdt,
                                                            Manf_Days_Avl = manfDays,
                                                            ProcPlanId = item.ProductionPlanId,
                                                            SaNestLevel = Sa_Nest_level
                                                        };
                                                        sublistbom2.Add(bomdata);
                                                        ProductionPlan_WoVM cwo = new ProductionPlan_WoVM()
                                                        {
                                                            WoId = item.WoId,
                                                            ParentWoId = item.WoId,
                                                            SalesOrderId = item.SalesOrderId,
                                                            PartId = bomdata.Child_Part_No_ID,
                                                            PartType = (int)manufchild.ManufacturedPartType,
                                                            Parentlevel = 'N',
                                                            BuildToStock = item.BuildToStock,
                                                            TestData = item.TestData,
                                                            CalcWOQty = bomdata.Calc_Qnty,
                                                            PlanStartDate = planstartdt,
                                                            PlanCompletionDate = item.PlanCompletionDate.Value.AddDays(-1),
                                                            SoComplDate = item.SoComplDate,
                                                            RoutingId = item.RoutingId,
                                                            StartingOpNo = item.StartingOpNo,
                                                            EndingOpNo = item.EndingOpNo,
                                                            For_Ref = 'N',
                                                            ReloadOption = "",
                                                            TenantId = item.TenantId,
                                                        };
                                                        subchildwos2.Add(cwo);
                                                        break;
                                                    //case MasterPartType.BOM:

                                                    //    break;
                                                    case MasterPartType.BOF:
                                                        var bofpdList = await _masterService.PartPurchasesFor(bomgrp.PartId);
                                                        //var bofptype = await _masterService.GetRMPart(bomgrp.PartId);
                                                        BoughtOutFinishDetailVM manuf = await _masterService.GetBOFPart(bomgrp.PartId);
                                                        subtotalLeadTime2 = bofpdList.Sum(x => x.LeadTimeInDays);
                                                        DateTime bofnextworkdingdate = DateTime.Now;
                                                        bofnextworkdingdate = bofnextworkdingdate.AddDays(subtotalLeadTime2);
                                                        BOMListVM bofbomdata = new BOMListVM
                                                        {
                                                            ParentWoId = item.WoId,
                                                            Child_Part_No_ID = bomgrp.PartId,
                                                            Child_Part_No_Type = mp.MasterPartType.ToString(),
                                                            Calc_Qnty = (int)bomgrp.TotalQuantity * item.CalcWOQty,
                                                            Plan_Qnty = item.CalcWOQty,
                                                            Plan_Compl_Dt = planstartdt,
                                                            CalcReceiptDate = bofnextworkdingdate,
                                                            ProcPlanId = item.ProductionPlanId
                                                        };
                                                        sublistbom2.Add(bofbomdata);
                                                        ProcPlanVM ppdata = new ProcPlanVM
                                                        {
                                                            PartId = bomgrp.PartId,
                                                            PartType = mp.MasterPartType.ToString(),
                                                            Calc_Proc_Qnty = (int)bomgrp.TotalQuantity * item.CalcWOQty,
                                                            UOMId = manuf.UOMId,
                                                            PlanReceiptDate = (DateTime)item.PlanCompletionDate,
                                                            CalcReceiptDate = bofnextworkdingdate,
                                                            WorkOrderId = item.WoId
                                                        };
                                                        sublistprocplan2.Add(ppdata);

                                                        break;
                                                    case MasterPartType.RawMaterial:
                                                        var mfpdList = await _masterService.PartPurchasesFor(bomgrp.PartId);
                                                        var ptype = await _masterService.GetRMPart(bomgrp.PartId);
                                                        subtotalLeadTime2 = mfpdList.Sum(x => x.LeadTimeInDays);
                                                        DateTime nextworkdingdate = (DateTime)item.PlanCompletionDate;
                                                        nextworkdingdate = nextworkdingdate.AddDays(subtotalLeadTime2);

                                                        BOMListVM rmbomdata = new BOMListVM
                                                        {
                                                            ParentWoId = item.WoId,
                                                            Child_Part_No_ID = bomgrp.PartId,
                                                            Child_Part_No_Type = mp.MasterPartType.ToString(),
                                                            Calc_Qnty = (int)bomgrp.TotalQuantity * item.CalcWOQty,
                                                            Plan_Qnty = item.CalcWOQty,
                                                            Plan_Compl_Dt = planstartdt,
                                                            CalcReceiptDate = nextworkdingdate,
                                                            ProcPlanId = item.ProductionPlanId
                                                        };
                                                        sublistbom2.Add(rmbomdata);

                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }

                                        }

                                    }

                                    //McTimeList---
                                    ManufacturedPartNoDetailVM mcmf = await _masterService.GetManufPart((int)item.PartId);
                                    var mcworkdetails = await _plantService.GetPlantWD(1);
                                    var mcholidaylist = await _plantService.GetHolidays(1);
                                    string mcweekOff1 = mcworkdetails.WeeklyOff1;
                                    string mcweekOff2 = mcworkdetails.WeeklyOff2;
                                    var mcresultList = await _routingService.Routings(mcmf.ManufacturedPartNoDetailId);
                                    int mcminutes = 0;
                                    foreach (var rote in mcresultList)
                                    {
                                        var result = await _routingService.RoutingSteps(rote.RoutingId);
                                        foreach (var step in result)
                                        {
                                            var stepdetails = await _routingService.StepMachines((int)step.StepId);
                                            if (stepdetails.Count() != 0)
                                            {
                                                var processingTimeSum = stepdetails
                                                            .GroupBy(sd => sd.RoutingStepId)
                                                            .Select(g => new
                                                            {
                                                                RoutingStepId = g.Key,
                                                                TotalProcessingTime = g.Sum(sd => TimeSpan.Parse(sd.FirstPieceProcessingTime).TotalMinutes)
                                                            });
                                                foreach (var min in processingTimeSum)
                                                {
                                                    mcminutes = (int)min.TotalProcessingTime;
                                                }
                                            }
                                            else
                                            {
                                                var sub = await _routingService.SubCons((int)step.StepId);
                                                var subfirst = sub.FirstOrDefault();
                                                var subworks = await _routingService.SubConWSS((int)step.StepId, subfirst.SubConDetailsId);
                                                var processingTimeSum = subworks
                                                            .GroupBy(sd => sd.RoutingStepId)
                                                            .Select(g => new
                                                            {
                                                                RoutingStepId = g.Key,
                                                                TotalProcessingTime = g.Sum(sd => TimeSpan.Parse(sd.FloorToFloorTime).TotalMinutes)
                                                            });
                                                foreach (var min in processingTimeSum)
                                                {
                                                    mcminutes = (int)min.TotalProcessingTime;
                                                }
                                            }
                                        }
                                    }
                                    int mcassyTime = (mcminutes * item.CalcWOQty) / mcworkdetails.NoOfShifts;
                                    int mcassyTimeInDays = mcassyTime / 1440;
                                    DateTime mcplanstartdt = item.PlanCompletionDate.Value.AddDays(-mcassyTimeInDays);
                                    var routingstep = await _routingService.RoutingSteps((int)item.RoutingId);
                                    var oneroutingstep = routingstep.FirstOrDefault();
                                    if (oneroutingstep != null)
                                    {
                                        var stepmachine = await _routingService.StepMachines((int)oneroutingstep.StepId);
                                        var onestepmachine = stepmachine.FirstOrDefault();
                                        if (onestepmachine != null)
                                        {
                                            var machine = await _machineService.GetMachine((int)onestepmachine?.MachineId);

                                            var departments = await _departmentService.GetDepartments(1);
                                            var department = departments.FirstOrDefault(d => d.DepartmentId == machine.MachineDepartmentId);
                                            int setupTimeMinutes = (int)TimeSpan.Parse(onestepmachine.SetupTime).TotalMinutes;
                                            int floorToFloorTimeMinutes = (int)TimeSpan.Parse(onestepmachine.FloorToFloorTime).TotalMinutes;
                                            int noOfShifts = department.NoOfShifts;
                                            int calcWOQty = item.CalcWOQty;
                                            int noOfPartsPerLoading = onestepmachine.NoOfPartsPerLoading;

                                            if (noOfPartsPerLoading == 0)
                                            {
                                                // throw new DivideByZeroException("NoOfPartsPerLoading cannot be zero.");
                                                noOfPartsPerLoading = 1;
                                            }
                                            int totalPlanTime = setupTimeMinutes + ((floorToFloorTimeMinutes + noOfShifts) * (calcWOQty - 1)) / noOfPartsPerLoading;
                                            int totalPlanTimeInHoursRounded = (int)Math.Round(totalPlanTime / 60.0, MidpointRounding.AwayFromZero);
                                            Console.WriteLine($"Total Plan Time (Minutes): {totalPlanTime}");
                                            Console.WriteLine($"Total Plan Time (Rounded Hours): {totalPlanTimeInHoursRounded}");

                                            McTimeListVM mcTimeList = new McTimeListVM()
                                            {
                                                WoId = item.WoId,
                                                Routing_StepId = oneroutingstep.StepId,
                                                CompanyId = Convert.ToInt64(oneroutingstep.StepLocation),
                                                MachineId = onestepmachine.MachineId,
                                                MachineTypeId = machine.MachineMachineTypeId,
                                                PlanQnty = item.CalcWOQty,
                                                TotalPlanTime = totalPlanTimeInHoursRounded,
                                                McPlanStartTime = mcplanstartdt,
                                                McPlanEndTime = (DateTime)item.PlanCompletionDate,
                                            };
                                            submcTimeListVMs2.Add(mcTimeList);
                                        }
                                        else
                                        {
                                            var sub = await _routingService.SubCons((int)oneroutingstep.StepId);
                                            var subfirst = sub.FirstOrDefault();
                                            var subworks = await _routingService.SubConWSS((int)oneroutingstep.StepId, subfirst.SubConDetailsId);
                                            var onestepmach = subworks.FirstOrDefault();
                                            var departments = await _departmentService.GetDepartments(1);
                                            var department = departments.FirstOrDefault();
                                            int setupTimeMinutes = (int)TimeSpan.Parse(onestepmach.SetupTime).TotalMinutes;
                                            int floorToFloorTimeMinutes = (int)TimeSpan.Parse(onestepmach.FloorToFloorTime).TotalMinutes;
                                            int noOfShifts = department.NoOfShifts;
                                            int calcWOQty = item.CalcWOQty;
                                            int noOfPartsPerLoading = onestepmach.NoOfPartsPerLoading;

                                            if (noOfPartsPerLoading == 0)
                                            {
                                                // throw new DivideByZeroException("NoOfPartsPerLoading cannot be zero.");
                                                noOfPartsPerLoading = 1;
                                            }
                                            int totalPlanTime = setupTimeMinutes + ((floorToFloorTimeMinutes + noOfShifts) * (calcWOQty - 1)) / noOfPartsPerLoading;
                                            int totalPlanTimeInHoursRounded = (int)Math.Round(totalPlanTime / 60.0, MidpointRounding.AwayFromZero);
                                            Console.WriteLine($"Total Plan Time (Minutes): {totalPlanTime}");
                                            Console.WriteLine($"Total Plan Time (Rounded Hours): {totalPlanTimeInHoursRounded}");

                                            McTimeListVM mcTimeList = new McTimeListVM()
                                            {
                                                WoId = item.WoId,
                                                Routing_StepId = oneroutingstep.StepId,
                                                CompanyId = Convert.ToInt64(oneroutingstep.StepLocation),
                                                MachineId = onestepmach.MachineType,
                                                MachineTypeId = onestepmach.MachineType,
                                                PlanQnty = item.CalcWOQty,
                                                TotalPlanTime = totalPlanTimeInHoursRounded,
                                                McPlanStartTime = mcplanstartdt,
                                                McPlanEndTime = (DateTime)item.PlanCompletionDate,
                                            };
                                            submcTimeListVMs2.Add(mcTimeList);
                                        }
                                    }
                                }
                                if (sublistprocplan2.Any())
                                {
                                    var result = await _woService.ProcPlanPost(sublistprocplan2);
                                    List<ProcPlanPartPurChaseRelVM> purcList = new List<ProcPlanPartPurChaseRelVM>();
                                    foreach (var item in result)
                                    {
                                        var submfpdList = await _masterService.PartPurchasesFor((int)item.PartId);
                                        foreach (var pur in submfpdList)
                                        {
                                            ProcPlanPartPurChaseRelVM subcwo = new ProcPlanPartPurChaseRelVM()
                                            {
                                                ProcPlanId = item.ProcPlanId,
                                                PartPurchaseId = pur.PartPurchaseId,
                                                LeadTime = pur.LeadTimeInDays.ToString(),
                                                Active = 1
                                            };
                                            purcList.Add(subcwo);
                                        }
                                    }
                                    if (purcList.Any())
                                    {
                                        var bomresult = await _woService.ProcPurchasePost(purcList);
                                    }
                                }
                                if (sublistbom2.Any())
                                {
                                    var bomresult = await _woService.BomListPost(sublistbom2);
                                }
                                var subchildworels2 = await _woService.PostChildWoRel(subchildWoRels2);
                                var submachinetimepost2 = await _woService.PostMcTimeList(submcTimeListVMs2);
                                var combinedSubchildren2 = subchildwos2
                                            .Where(sc => sc.PartType == 1)
                                            .GroupBy(sc => sc.PartId)
                                            .Select(g => new ProductionPlan_WoVM
                                            {
                                                PartId = g.Key,
                                                CalcWOQty = g.Sum(sc => sc.CalcWOQty),
                                                PlanCompletionDate = g.Min(sc => sc.PlanCompletionDate)
                                            })
                                            .ToList();
                                var subchildproductionwopost2 = await _woService.ProductionPlanWoPost(subchildwos2);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            //return RedirectToAction("DetailedProcPlan");
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateInwardPOdetails([FromBody] IEnumerable<PODetailsVM> pODetails)
        {
            List<PODetailsVM> pODetailsVMs = new List<PODetailsVM>();
            var allPoDetails = await _woService.GetAllPodetails();
            foreach (var item in allPoDetails)
            {
                foreach (var po in pODetails)
                {
                    if (item.PoDetailsId == po.PoDetailsId)
                    {
                        item.Status = 3;
                        pODetailsVMs.Add(item);
                    }
                }
            }
            var postPODetails = await _woService.PODetails(pODetailsVMs);
            return Ok(postPODetails);
        }
        [HttpPost]
        public async Task<IActionResult> PostProcPlan([FromBody] IEnumerable<ProcPlanVM> procPlanVMs)
        {
            var result = await _woService.ProcPlanPost(procPlanVMs);
            return Ok(result);
        } 
        
        [HttpGet]
        public async Task<IActionResult> AllProductionWos()  // AllProductionWoReadForProd
        {
            var productions = await _woService.AllProductionPlan_Wo();
            var masterparts = await _masterService.ItemMasterParts();
            var procplan = await _woService.GetAllProcPlan();
            var customer = await _baService.GetCustomerOrders();
            var nclogs = await _woService.GetAllNcLog();
            var trans = await _woService.GetAllInv_Trans_Log();
            foreach (ProductionPlan_WoVM item in productions)
            {
                if(item.PartType == 2)
                {
                    item.PlanStartDateStr = item.PlanStartDate.ToString("dd-MM-yyyy");
                    item.PartTypeName = "Assembly";
                }
                else
                {
                    var findpp = procplan.Where(p => p.WorkOrderId == item.WoId).FirstOrDefault().CalcReceiptDate.ToString("dd-MM-yyyy");
                    item.PlanStartDateStr = findpp;
                    item.PartTypeName = "Child Part";
                }
                item.SoComplDateStr = item.SoComplDate.Value.ToString("dd-MM-yyyy");
                foreach (ItemMasterPartVM imp in masterparts)
                {
                    if (item.PartId == imp.PartId)
                    {
                        item.PartNo = imp.PartNo;
                        item.PartDesc = imp.Description;
                    }
                }
                var so = await _baService.GetOneSO(item.SalesOrderId);
                if (so != null)
                {
                    if (item.PlanCompletionDate >= so.RequiredByDate && item.CalcWOQty >= so.RequiredQuantity)
                    {
                        item.WoRelease = "Y";
                    }
                    else
                    {
                        item.WoRelease = "N";
                    }
                    foreach (CustomerOrderVM cu in customer)
                    {
                        if (so.CustomerOrderId == cu.CustomerOrderId)
                        {
                            item.Customer = cu.CustomerName;
                        }
                    }
                }
                else
                {
                    var wosos = await _woService.GetSoWoRel(item.WoId);
                    foreach (var woso in wosos)
                    {
                        var sos = await _baService.GetOneSO(woso.SalesOrderId);
                        if (sos != null)
                        {
                            item.SoComplDateStr = sos.RequiredByDateStr;
                        }
                        if (sos.RequiredByDate > item.PlanCompletionDate)
                        {
                            item.WoRelease = "Y";
                        }
                        else
                        {
                            item.WoRelease = "N";
                        }
                        foreach (CustomerOrderVM cu in customer)
                        {
                            if (sos.CustomerOrderId == cu.CustomerOrderId)
                            {
                                item.Customer = cu.CustomerName;
                            }
                        }
                    }
                }
                item.NoOfOpenNc = nclogs.Where(n => n.Inw_Recpt_Part_No_Id == item.PartId).Count();
                var procplanwo = procplan.Where(p => p.WorkOrderId == item.WoId).FirstOrDefault();
                var tran = trans.Where(n => (n.Input_Part_NoId == procplanwo.PartId || n.Output_Part_No == procplanwo.PartId) && n.Qnty >= procplanwo.Calc_Proc_Qnty).FirstOrDefault();
                if (tran != null)
                {
                item.ReadyForProd = "Y";
                }
                else
                {
                    item.ReadyForProd = "N";
                }
                var oprnos = await _routingService.RoutingSteps((int)item.RoutingId);
                int docPendingApprovalCount = 0;
                foreach (var op in oprnos)
                {
                    var docmand = await _operationService.GetOperationalDocTypesByOptId(Convert.ToInt64(op.StepOperation));

                    if (docmand.Any())
                    {
                        var docListVMs = await _docMangService.GetAllDocList();
                        docPendingApprovalCount += docListVMs.Count(doc =>
                            doc.AppvStatus == 1 &&
                            docmand.Any(docMand => doc.DocumentTypeId == docMand.DocumentTypeId &&
                                                   doc.RoutingId == item.RoutingId));
                    }
                }
                item.NoOfDocWf = docPendingApprovalCount;
            }
            return Ok(productions);
        }
        [HttpGet]
        public async Task<IActionResult> AllProductionWo()
        {
            // 1. Bulk Fetch Data (Parallelize Backend & External Service Calls)
            var productionTask = _woService.AllProductionWoReadForProd();
            var masterPartsTask = _masterService.ItemMasterParts();
            var customerTask = _baService.GetCustomerOrders();
            var allDocsTask = _docMangService.GetAllDocList();
            var allSalesOrdersTask = _baService.AllSalesOrders();
            var allRoutingStepsTask = _routingService.AllRoutingSteps();

            await Task.WhenAll(productionTask, masterPartsTask, customerTask, allDocsTask, allSalesOrdersTask, allRoutingStepsTask);

            // 2. Prepare Data Structures (Dictionaries & Lookups for O(1) access)
            var productions = productionTask.Result.ToList();
            var masterparts = masterPartsTask.Result.ToDictionary(p => p.PartId);
            var customers = customerTask.Result.ToList();

            // Dictionary for fast Sales Order lookup
            var salesOrdersDict = allSalesOrdersTask.Result.ToDictionary(s => s.SalesOrderId);

            // Lookup for Docs grouped by RoutingId (Drastically speeds up the count logic)
            var docsByRouting = allDocsTask.Result.ToLookup(d => d.RoutingId);

            // Lookup for Routing Steps grouped by RoutingId
            var stepsByRouting = allRoutingStepsTask.Result.ToLookup(r => r.RoutingId);

            // 3. Pre-fetch Operational Doc Requirements (Optimization)
            // Identify all unique Operation IDs used across these WOs to batch-fetch their doc types
            var uniqueOpIds = productions
                .SelectMany(p => stepsByRouting[(int)p.RoutingId])
                .Select(step => Convert.ToInt64(step.StepOperation))
                .Distinct()
                .ToList();

            // Create tasks to fetch doc types for each unique operation in parallel
            var opDocTasks = uniqueOpIds.ToDictionary(
                id => id,
                id => _operationService.GetOperationalDocTypesByOptId(id)
            );

            await Task.WhenAll(opDocTasks.Values);

            // Dictionary: OperationId -> List of Mandatory Doc Types
            var opDocsMap = opDocTasks.ToDictionary(k => k.Key, k => k.Value.Result.ToList());

            // 4. Main Loop for Data Enrichment
            foreach (var item in productions)
            {
                // A. Populate Part Details
                if (masterparts.TryGetValue(item.PartId, out var imp))
                {
                    item.PartNo = imp.PartNo;
                    item.PartDesc = imp.Description;
                }

                // B. Sales Order Logic
                // OPTIMIZATION: Use dictionary lookup instead of await _baService.GetOneSO(...)
                SalesOrderVM so = null;
                salesOrdersDict.TryGetValue(item.SalesOrderId, out so);

                if (so != null)
                {
                    // Logic for Direct SO
                    if (item.PlanCompletionDate >= so.RequiredByDate && item.CalcWOQty >= so.RequiredQuantity)
                        item.WoRelease = "Y";
                    else
                        item.WoRelease = "N";

                    var cust = customers.FirstOrDefault(c => c.CustomerOrderId == so.CustomerOrderId);
                    if (cust != null) item.Customer = cust.CustomerName;
                }
                else
                {
                    // Logic for SO via Relations
                    // NOTE: Kept internal API call as requested ("except GetSoWoRel")
                    var wosos = await _woService.GetSoWoRel(item.WoId);

                    foreach (var woso in wosos)
                    {
                        // OPTIMIZATION: Use dictionary lookup for the related SO
                        SalesOrderVM sos = null;
                        salesOrdersDict.TryGetValue(woso.SalesOrderId, out sos);

                        if (sos != null)
                        {
                            item.SoComplDateStr = sos.RequiredByDateStr;

                            if (sos.RequiredByDate > item.PlanCompletionDate)
                                item.WoRelease = "Y";
                            else
                                item.WoRelease = "N";

                            var cust = customers.FirstOrDefault(c => c.CustomerOrderId == sos.CustomerOrderId);
                            if (cust != null) item.Customer = cust.CustomerName;
                        }
                    }
                }

                // C. Document Workflow Logic
                // OPTIMIZATION: Use pre-fetched steps and pre-fetched doc requirements
                var oprnos = stepsByRouting[(int)item.RoutingId];
                int docPendingApprovalCount = 0;

                // Retrieve all docs for this Routing once (from Lookup)
                var wODocs = docsByRouting[item.RoutingId];

                foreach (var op in oprnos)
                {
                    long opId = Convert.ToInt64(op.StepOperation);

                    // Retrieve pre-fetched requirements for this operation
                    if (opDocsMap.TryGetValue(opId, out var docmand) && docmand.Any())
                    {
                        // Count matching docs from the pre-filtered wODocs list
                        var pendingDocs = wODocs.Count(doc =>
                            doc.AppvStatus == 1 &&
                            docmand.Any(docMand => doc.DocumentTypeId == docMand.DocumentTypeId));

                        docPendingApprovalCount += pendingDocs;
                    }
                }
                item.NoOfDocWf = docPendingApprovalCount;
            }

            return Ok(productions);
        }
        [HttpGet]
        public async Task<IActionResult> AllRMWo(int rmpartids)
        {
            var productions = await _woService.AllProductionPlan_Wo();
            var masterparts = await _masterService.ItemMasterParts();
            List<ProductionPlan_WoVM> ppwos = new List<ProductionPlan_WoVM>();
            foreach (ProductionPlan_WoVM item in productions)
            {
                foreach (ItemMasterPartVM imp in masterparts)
                {
                    if (item.PartId == imp.PartId)
                    {
                        item.PartNo = imp.PartNo;
                        item.PartDesc = imp.Description;
                    }
                }
                var so = await _baService.GetOneSO(item.SalesOrderId);
                if (so != null)
                {
                    item.SoComplDateStr = so.RequiredByDateStr;
                    if(so.RequiredByDate > item.PlanCompletionDate)
                    {
                        item.WoRelease = "Y";
                    }
                    else
                    {
                        item.WoRelease = "N";
                    }
                }
                else
                {
                    var wosos = await _woService.GetSoWoRel(item.WoId);
                    foreach (var woso in wosos)
                    {
                        var sos = await _baService.GetOneSO(woso.SalesOrderId);
                        if (sos != null)
                        {
                            item.SoComplDateStr = sos.RequiredByDateStr;
                        }
                        if (sos.RequiredByDate > item.PlanCompletionDate)
                        {
                            item.WoRelease = "Y";
                        }
                        else
                        {
                            item.WoRelease = "N";
                        }
                    }
                }

                ManufacturedPartNoDetailVM mf = await _masterService.GetManufPart((int)item.PartId);

                if (mf.ManufacturedPartType == 1)
                {
                    var mpmakefromlist = await _masterService.GetMPMakeFromListByPartId(mf.ManufacturedPartNoDetailId.ToString());
                    if(mpmakefromlist.Any(r=>r.MPPartId == rmpartids))
                    {
                        ppwos.Add(item);
                    }
                }
            }
            return Ok(ppwos);
        }
        public async Task<ActionResult> DownloadProductionWoGridData()
        {
            var productions = await _woService.AllProductionPlan_Wo();
            var masterparts = await _masterService.ItemMasterParts();
            foreach (ProductionPlan_WoVM item in productions)
            {
                foreach (ItemMasterPartVM imp in masterparts)
                {
                    if (item.PartId == imp.PartId)
                    {
                        item.PartNo = imp.PartNo;
                        item.PartDesc = imp.Description;
                    }
                }
            }
            // Get the grid data from your database or data source
            //var gridData =productions;

            // Create a CSV string from the grid data   Addn Qnty from User	Plan WO Qnty	Input Matl Rcpt Date	Plan Start Date	Plan Compl Dt
            string csv = string.Empty;
            csv += "WO Number,Build To Stock,Part No/Description,Calc WO Qty,Qty On Hand,Addn Qnty from User,Plan WO Qnty,Plan Start Date,Plan Compl Dt\r\n";
            foreach (var item in productions)
            {
                csv += $"{item.WONumber},{item.BuildToStock},{item.PartNo}/{item.PartDesc},{item.CalcWOQty},{item.QtyOnHand},{item.AddnOtyUser},{item.PlanWOQnty},{""},{item.PlanCompletionDateStr}\r\n";
            }

            // Return the CSV file as a FileContentResult
            return File(Encoding.UTF8.GetBytes(csv), "text/csv", "ProductionPlanWoData.csv");
        }

        public async Task<ActionResult> DownloadMaterialProcPlanGridData()
        {
            var resultList = await _woService.GetAllProcPlan();
            foreach (ProcPlanVM item in resultList)
            {
                var mp = await _masterService.ItemMasterPartById((int)item.PartId);
                var mfpdList = await _masterService.PartPurchasesFor((int)item.PartId);
                var uoms = await _masterService.GetUOMs();
                foreach (var uom in uoms)
                {
                    if (item.UOMId == uom.UOMId)
                    {
                        item.UomName = uom.Name;
                    }
                }

                foreach (var purs in mfpdList)
                {
                    if (item.PartId == purs.PPartId)
                    {
                        item.Supplier = purs.PSupplier;
                        int subtotalLeadTime = mfpdList.Sum(x => x.LeadTimeInDays);
                        item.LeadTimeInDays = subtotalLeadTime.ToString();
                        int MOQ = mfpdList.Sum(x => x.MinimumOrderQuantity);
                        item.Moq = MOQ;
                    }
                }
                item.PartNo = mp.PartNo;
                item.PartDesc = mp.PartDescription;
            }
            string csv = string.Empty;
            csv += "Part No / Desc,Part Type,Supplier,PO Ref,Calculated Requirement,UOM,Qnty on Hand,Planned Purchase Qnty,MOQ,Date Reqd,Lead Time Days,Calculated Receipt Date\r\n";
            foreach (var item in resultList)
            {
                csv += $"{item.PartNo}/{item.PartDesc},{item.PartType},{item.Supplier},{""},{item.Calc_Proc_Qnty},{item.UomName},{item.OtyOnHand},{item.Plan_Proc_Qnty},{item.Moq},{""},{item.LeadTimeInDays},{item.CalcReceiptDateStr}\r\n";
            }

            return File(Encoding.UTF8.GetBytes(csv), "text/csv", "MaterialProcPlanData.csv");
        }

        public async Task<ActionResult> DownloadBomListGridData()
        {
            var resultList = await _woService.GetAllBomlist();
            var workOrders = await _baService.AllWorkOrders();
            foreach (BOMListVM item in resultList)
            {
                var mp = await _masterService.ItemMasterPartById((int)item.Child_Part_No_ID);
                item.PartNo = mp.PartNo;
                item.PartDesc = mp.PartDescription;
                foreach (WorkOrdersVM wo in workOrders)
                {
                    if (item.ParentWoId == wo.WOID)
                    {
                        item.WoNumber = wo.WONumber;
                    }
                }
            }
            string csv = string.Empty;
            csv += "Child Part / Desc,Part Type,WO / PO Ref,Plan Qnty,Plan Compl / Rcpt Date,Act Qnty,Act Compl / Recpt Dt,Status,Critical Part\r\n";
            foreach (var item in resultList)
            {
                csv += $"{item.PartNo}/{item.PartDesc},{item.Child_Part_No_Type},{item.WoNumber},{item.Calc_Qnty},{item.CalcReceiptDateStr},{""},{""},{""},{""}\r\n";
            }

            return File(Encoding.UTF8.GetBytes(csv), "text/csv", "BOMListData.csv");
        }

        public async Task<ActionResult> DownloadMachineListDetailGridData()
        {
            var mctimelist = await _woService.GetAllMcTimeList();
            var productions = await _woService.AllProductionPlan_Wo();
            var masterparts = await _masterService.ItemMasterParts();
            foreach (var mctime in mctimelist)
            {
                var machine = await _machineService.GetMachine((int)mctime.MachineId);
                var machineTypes = await _machineService.GetMachineTypes();
                foreach (ProductionPlan_WoVM item in productions)
                {
                    if (item.WoId == mctime.WoId && item.ParentWoId == 0)
                    {
                        foreach (ItemMasterPartVM imp in masterparts)
                        {
                            if (item.PartId == imp.PartId)
                            {
                                mctime.PartNo = imp.PartNo;
                                mctime.PartDesc = imp.Description;
                                mctime.PartType = imp.MasterPartType;
                                mctime.WoNumber = item.WONumber;
                            }
                        }
                    }
                }
                if (mctime.MachineId == machine.MachineMachineId)
                {
                    mctime.MachineName = machine.MachineMachineName;

                    var departments = await _departmentService.GetDepartments(1);
                    var department = departments.FirstOrDefault(d => d.DepartmentId == machine.MachineDepartmentId);
                    mctime.Location = department.PlantName;
                    var operation = await _operationService.Operation(machine.MachineOperationListId);
                    mctime.OprationNo = operation.Operation;
                    foreach (var machinetype in machineTypes)
                    {
                        if (mctime.MachineTypeId == machinetype.MachineTypeTypeId)
                        {
                            mctime.MachineTypeName = machinetype.MachineTypeName;
                        }
                    }
                }
            }
            string csv = string.Empty;
            csv += "Part No / Desc,Part Type,WO Ref,Location,Opr No,Machine Type,Machine,Plan Qnty,Start Date,End Date,Mc Hrs,Status,Critical Part\r\n";
            foreach (var item in mctimelist)
            {
                csv += $"{item.PartNo}/{item.PartDesc},{item.PartType},{item.WoNumber},{item.Location},{item.OprationNo},{item.MachineTypeName},{item.MachineName},{item.PlanQnty},{item.McPlanStartTimeStr},{item.McPlanEndTimeStr},{item.TotalPlanTime},{""},{""}\r\n";
            }

            return File(Encoding.UTF8.GetBytes(csv), "text/csv", "MachineListDetailData.csv");
        }


        [HttpGet]
        public async Task<IActionResult> ReloadProductionWo(string reloadoption, long partid)
        {
            List<ProductionPlan_WoVM> listwo = new List<ProductionPlan_WoVM>();
            var productions = await _woService.AllProductionPlan_Wo();
            foreach (var item in productions)
            {
                if (item.ReloadOption == reloadoption && item.PartId == partid)
                {
                    listwo.Add(item);
                }
            }
            return Ok(listwo);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProcPlan()
        {
            var resultList = await _woService.GetAllProcPlan();
            var uoms = await _masterService.GetUOMs();
            var workOrders = await _woService.AllProductionPlan_Wo();

            // Build set of work order IDs to exclude (those with status == 8)
            var excludedWoIds = workOrders
                .Where(wo => wo.Status == 8)
                .Select(wo => wo.WoId) // adjust if the ID property is named differently
                .ToHashSet();
            resultList = resultList
                .Where(item => !excludedWoIds.Contains(item.WorkOrderId))
                .ToList();
            var partIds = resultList.Select(item => (int)item.PartId).Distinct().ToList();

            var masterPartsTasks = partIds.ToDictionary(partId => partId, partId => _masterService.ItemMasterPartById(partId));
            var partPurchasesTasks = partIds.ToDictionary(partId => partId, partId => _masterService.PartPurchasesFor(partId));

            await Task.WhenAll(masterPartsTasks.Values);
            await Task.WhenAll(partPurchasesTasks.Values);
            var uomDict = uoms.ToDictionary(uom => uom.UOMId, uom => uom.Name);

            foreach (var item in resultList)
            {
                item.PlanStartDateStr = item.PlanReceiptDate.ToString("dd-MM-yyyy");
                var mp = masterPartsTasks[(int)item.PartId].Result;
                item.PartNo = mp.PartNo;
                item.PartDesc = mp.PartDescription;
                if (uomDict.TryGetValue(item.UOMId, out var uomName))
                {
                    item.UomName = uomName;
                }
                var mfpdList = partPurchasesTasks[(int)item.PartId].Result;
                var relevantPurchases = mfpdList.Where(purs => item.PartId == purs.PPartId).ToList();

                if (relevantPurchases.Any())
                {
                    item.Supplier = relevantPurchases.First().PSupplier;
                    item.SupplierId = relevantPurchases.First().PSupplierId;
                    item.LeadTimeInDays = relevantPurchases.Sum(x => x.LeadTimeInDays).ToString();
                    item.Moq = relevantPurchases.Sum(x => x.MinimumOrderQuantity);
                    item.Price = relevantPurchases
                                .Where(x => long.TryParse(x.Price, out _)) 
                                .Sum(x => Convert.ToInt64(x.Price ?? "0")) 
                                .ToString();
                }
            }

            return Ok(resultList);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllBomlist()
        {
            // Fetch all BOM list and Work Orders in parallel
            var resultListTask = _woService.GetAllBomlist();
            var workOrdersTask = _baService.AllWorkOrders();
            var prodnWosTask = _woService.AllProductionPlan_Wo();

            await Task.WhenAll(resultListTask, workOrdersTask, prodnWosTask);

            var prodnWos = prodnWosTask.Result;
            var resultList = resultListTask.Result;
            var workOrders = workOrdersTask.Result;

            // Prepare a dictionary for fast lookup of Work Orders by ID
            var workOrdersDict = workOrders.ToDictionary(wo => wo.WOID, wo => wo.WONumber);

            var prodnWosStatusDict = prodnWos
      .GroupBy(p => p.WoId) // or appropriate key matching ParentWoId
      .ToDictionary(
          g => g.Key,
          g =>
          {
              var p = g.First(); // or use a more specific selector, e.g., latest by timestamp
                return new
              {
                  Status = p.Status  
              };
          });

            // Get all unique Child_Part_No_IDs from the result list
            var partIds = resultList.Select(item => (int)item.Child_Part_No_ID).Distinct().ToList();

            // Fetch all necessary ItemMasterParts in parallel
            var masterPartsTasks = partIds.ToDictionary(partId => partId, partId => _masterService.ItemMasterPartById(partId));
            await Task.WhenAll(masterPartsTasks.Values);

            foreach (var item in resultList)
            {
                // Get master part details
                var mp = masterPartsTasks[(int)item.Child_Part_No_ID].Result;
                item.PartNo = mp.PartNo;
                item.PartDesc = mp.PartDescription;

                // Assign Work Order number if it exists in the dictionary
                if (workOrdersDict.TryGetValue(item.ParentWoId, out var woNumber))
                {
                    item.WoNumber = woNumber;
                }

                if (prodnWosStatusDict.TryGetValue(item.ParentWoId, out var statusInfo))
                {
                    item.Status = statusInfo.Status;
                }
                else
                {
                    // Optional: default/fallback if no matching prodnwos entry
                    item.StatusStr ??= "Unknown";
                }
            }

            return Ok(resultList);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllMcTimeList()
        {
            var mctimelist = await _woService.GetAllMcTimeList();
            var productions = await _woService.AllProductionPlan_Wo();
            var masterparts = await _masterService.ItemMasterParts();
            foreach (var mctime in mctimelist)
            {
                var machineTypes = await _machineService.GetMachineTypes();
                foreach (ProductionPlan_WoVM item in productions)
                {
                    if (item.WoId == mctime.WoId && item.ParentWoId == 0)
                    {
                        foreach (ItemMasterPartVM imp in masterparts)
                        {
                            if (item.PartId == imp.PartId)
                            {
                                mctime.PartNo = imp.PartNo;
                                mctime.PartDesc = imp.Description;
                                mctime.PartType = imp.MasterPartType;
                                mctime.WoNumber = item.WONumber;
                            }
                        }
                    }
                }
                var machine = await _machineService.GetMachine(mctime.MachineId);
                if (mctime.MachineId == machine.MachineMachineId)
                {
                    mctime.MachineName = machine.MachineMachineName;

                    var departments = await _departmentService.GetDepartments(1);
                    var department = departments.FirstOrDefault(d => d.DepartmentId == machine.MachineDepartmentId);
                    mctime.Location = department.PlantName;
                    var operation = await _operationService.Operation(machine.MachineOperationListId);
                    mctime.OprationNo = operation.Operation;
                    if (operation.Inhouse == 1)
                    {
                        mctime.Inhouse = "Y";
                    }
                    else if (operation.Subcon == 1)
                    {
                        mctime.Inhouse = "N";
                    }
                    foreach (var machinetype in machineTypes)
                    {
                        if (mctime.MachineTypeId == machinetype.MachineTypeTypeId)
                        {
                            mctime.MachineTypeName = machinetype.MachineTypeName;
                        }
                    }
                }
            }
            return Ok(mctimelist);
        }


        [HttpPost]
        public async Task<IActionResult> MultipleProductionUpdateWOPost([FromBody] IEnumerable<ProductionPlan_WoVM> ppwos)
        {
            List<ProductionPlan_WoVM> listprod = new List<ProductionPlan_WoVM>();
            var productions = await _woService.AllProductionPlan_Wo();
            foreach (var item in productions)
            {
                var matchingPpwo = ppwos.FirstOrDefault(ppwo => ppwo.ProductionPlanId == item.ProductionPlanId);
                if (matchingPpwo != null)
                {
                    item.Status = matchingPpwo.Status;
                    listprod.Add(item);
                }
            }
            var procdutionpost = await _woService.ProductionPlanWoPost(listprod);
            return Ok(procdutionpost);
        }


        [HttpGet]
        public async Task<IActionResult> GetPOLogs(long customerOrderId)
        {
            var pologs = await _baService.GetProcLogs(customerOrderId);
            return Ok(pologs);
        }

        [HttpPost]
        public async Task<IActionResult> MulitplePOdetails([FromBody] IEnumerable<PODetailsVM> pODetails)
        {
            var postPODetails = await _woService.PODetails(pODetails);
            if (postPODetails.Any())
            {
                var groupedData = postPODetails.GroupBy(x => x.CompanyId)
                                .Select(grp => new POHeaderVM
                                {
                                    SupplierId = grp.Key,
                                    PoHeaderId = 0,
                                    PoDetailsId = grp.Select(x => x.PoDetailsId).FirstOrDefault(),
                                    PartId = grp.Select(x => x.PartId).FirstOrDefault(),
                                })
                                .ToList();
                var postPOHeader = await _woService.POHeader(groupedData);
            }
            return Ok(postPODetails);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePOdetails([FromBody] IEnumerable<PODetailsVM> pODetails)
        {
            List<PODetailsVM> pODetailsVMs = new List<PODetailsVM>();
            var allPoDetails = await _woService.GetAllPodetails();
            foreach (var item in allPoDetails)
            {
                foreach (var po in pODetails)
                {
                    if (item.PoDetailsId == po.PoDetailsId)
                    {
                        item.Status = 2;
                        pODetailsVMs.Add(item);
                    }
                }
            }
            var postPODetails = await _woService.PODetails(pODetailsVMs);
            return Ok(postPODetails);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInv_Mismatch_List()
        {
            var inv_Mismatch_ListVMs = await _woService.GetAllInv_Mismatch_List();
            var masterparts = await _masterService.ItemMasterParts();
            var companies = await _masterService.GetCompanies();
            foreach (var item in inv_Mismatch_ListVMs)
            {
                foreach (ItemMasterPartVM imp in masterparts)
                {
                    if (item.Part_No == imp.PartId)
                    {
                        item.PartNoStr = imp.PartNo + " / " + imp.Description;
                    }
                }
                ManufacturedPartNoDetailVM mf = await _masterService.GetManufPart(Convert.ToInt32(item.Part_No));
                var resultList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                var sortedList = resultList.OrderByDescending(x => x.PreferredRouting == 1).ThenBy(x => x.PreferredRouting).ToList();
                var result = await _routingService.RoutingSteps(sortedList[0].RoutingId);
                var routname = sortedList[sortedList.Count() - 1].RoutingName;
                var steps = result.ToList();
                var opname = steps[steps.Count() - 1].StepNumber;
                if (routname != null)
                {
                    item.RoutName = routname;
                }
                if (opname != null)
                {
                    item.OpNoName = opname;
                }
                var subc = await _routingService.SubCons((int)steps[steps.Count() - 1].StepId);
                var subcs = subc.ToList();
                var companie = companies.Where(c => c.CompanyId == subcs[0].SupplierId).FirstOrDefault();
                if(companie != null)
                {
                    item.LocationStr = companie.CompanyName;
                }
                ClaimsPrincipal userClaim = HttpContext.User;
                string fullName = AppUtil.GetFullName(userClaim);
                item.TransactionDone = "N";
                item.Reported_ByName = fullName;
                item.Report_dateStr = item.Report_date.ToString("dd-MM-yyyy hh:mm:ss tt");
            }
            return Ok(inv_Mismatch_ListVMs);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPodetails()
        {
            var procdutionpost = await _woService.GetAllPodetails();
            var resultList = await _woService.GetAllProcPlan();
            var inv_Mismatch_Lists = await _woService.GetAllInv_Mismatch_List();
            var doclist = await _woService.GetAllInWardDocList();
            List<PODetailsVM> woSubs = new List<PODetailsVM>();
            var companies = await _masterService.GetCompanies();
            var masterparts = await _masterService.ItemMasterParts();
            var uoms = await _masterService.GetUOMs();
            var uomDict = uoms.ToDictionary(uom => uom.UOMId, uom => uom.Name);
            foreach (var item in procdutionpost)
            {
                foreach (ContactsVM mobj in companies)
                {
                    if (item.CompanyId == mobj.CompanyId)
                    {
                        item.Supplier = mobj.CompanyName;
                    }
                }
                item.DateStr = item.PlanPoReceiptDate.ToString("dd-MM-yyyy");
                item.NoOfLine = "1";
                item.NoOfOpenLine = "1";
                item.NoOfPastLine = "0";
                if(item.Status == 1)
                {
                    item.StatusStr = "Not Approved";
                }
                else if(item.Status == 2)
                {
                    item.StatusStr = "PO Approved";
                }
                else if(item.Status == 3)
                {
                    item.StatusStr = "Completed";
                }
                item.PoType = "Prodn";
                foreach (ItemMasterPartVM imp in masterparts)
                {
                    if (item.PartId == imp.PartId)
                    {
                        item.PartNo = imp.PartNo + " / " + imp.Description;
                        //item.PartType = imp.MasterPartType;
                        var mp = await _masterService.PartPurchasesFor((int)item.PartId);
                        if (mp != null && mp.Count()>0)
                        {
                            item.ProcPrice = mp.FirstOrDefault().Price;
                        }
                        if (imp.MasterPartType == "ManufacturedPart")
                        {
                            item.PartType = "SubCon";
                        }
                        else
                        {
                            item.PartType = imp.MasterPartType;
                        }
                    }
                }
                if (uomDict.TryGetValue(1, out var uomName))
                {
                    item.Unit = uomName;
                }
                foreach (var doc in doclist)
                {
                    item.DocStatus = doc.Mandatory.ToString();
                }
                foreach (var proc in resultList)
                {
                    if (item.ProcPlanId == proc.ProcPlanId)
                    {
                        if (item.PoQnty < proc.Plan_Proc_Qnty)
                        {
                            item.QntyRed = "Y";
                        }
                        if(item.PlanPoReceiptDate < proc.PlanReceiptDate)
                        {
                            item.DateRed = "Y";
                        }
                    }
                }
                var invmistach = inv_Mismatch_Lists.Where(i => i.PO_Ref == item.PoDetailsId).FirstOrDefault();
                if(invmistach != null)
                {
                    item.Mismatch_Resolved = invmistach.Resolved;
                }
                else
                {
                    item.Mismatch_Resolved = '-';
                }
            }
            return Ok(procdutionpost);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInvMaster()
        {
            var result = await _woService.GetAllInventory_Master();
            var masterparts = await _masterService.ItemMasterParts();
            var companies = await _masterService.GetCompanies();
            var inv_Trans = await _woService.GetAllInv_Trans_Log();
            foreach (var item in result)
            {
                foreach (ItemMasterPartVM imp in masterparts)
                {
                    if (item.Part_NoId == imp.PartId)
                    {
                        item.PartNoStr = imp.PartNo;
                        item.PartDescStr = imp.Description;
                        item.PartTypeStr = imp.MasterPartType;
                        item.CompanyStr = imp.Company;
                    }
                }
                ManufacturedPartNoDetailVM mf = await _masterService.GetManufPart(Convert.ToInt32(item.Part_NoId));
                var resultList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                var sortedList = resultList.Where(x => x.RoutingId == item.Routing_Id).FirstOrDefault();
                item.LocationStr = "Inhouse";
                if (sortedList != null)
                {
                    item.RoutName = sortedList.RoutingName + " / ";
                    var routingSteps = await _routingService.RoutingSteps(sortedList.RoutingId);
                    var steps = routingSteps.Where(s=>s.StepId== item.Opr_No_Id).FirstOrDefault();
                    if(steps != null)
                    {
                        item.OpName = steps.StepNumber;
                        if (steps.StepLocation == 1.ToString())
                        {
                            item.LocationStr = "Inhouse";
                        }
                        else if (steps.StepLocation == 2.ToString())
                        {
                            item.LocationStr = "SubCon";
                        }
                        else
                        {
                            item.LocationStr = "Company";
                        }
                    }
                }
                var inv_Tran = inv_Trans.Where(i => i.Input_Part_NoId == item.Part_NoId || i.Output_Part_No == item.Part_NoId).FirstOrDefault();
                if(inv_Tran != null)
                {
                    if (inv_Tran.Part_Status == 1)
                    {
                        item.PartStatus = "Accepted";
                    }
                    else if (inv_Tran.Part_Status == 2)
                    {
                        item.PartStatus = "Rework";
                    }
                    else
                    {
                        item.PartStatus = "Rejected";
                    }
                }
                item.DateStr = item.Dt_time.ToString("dd-MM-yyyy");
                item.Qnty = Convert.ToInt64(item.Current_QntOnHand);
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateInvMasterFirst(Inventory_MasterVM masterDocListVM)
        {
            var findInv = await _woService.GetAllInventory_Master();
            var update = findInv.Where(i => i.Inventory_MasterId == masterDocListVM.Inventory_MasterId).FirstOrDefault();
            if (update != null)
            {
                update.Current_QntOnHand = masterDocListVM.Current_QntOnHand;
                update.ReasonDesc = masterDocListVM.ReasonDesc;
                update.Location_Id = masterDocListVM.Location_Id;
                var result = await _woService.PostInventory_Master(update);
                return Ok(result);
            }
            return Ok(masterDocListVM);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInvMasterCon(Inventory_MasterVM masterDocListVM)
        {
            var findInv = await _woService.GetAllInventory_Master();
            var update = findInv.Where(i => i.Inventory_MasterId == masterDocListVM.Inventory_MasterId).FirstOrDefault();
            if (update != null)
            {
                update.Current_QntOnHand = masterDocListVM.Current_QntOnHand;
                update.ReasonDesc = masterDocListVM.ReasonDesc;
                var result = await _woService.PostInventory_Master(update);
                return Ok(result);
            }
            return Ok(masterDocListVM);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInv_Master_Log(long invmasterid)
        {
            var results = await _woService.GetAllInv_Master_Log();
            var result = results.Where(i => i.Inv_mast_ID == invmasterid).ToList();
            foreach (var item in result)
            {
                item.DateStr = item.Dt_time.ToString("dd-MM-yyyy");
                ClaimsPrincipal userClaim = HttpContext.User;
                string fullName = AppUtil.GetFullName(userClaim);
                item.UserName = fullName;
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInvTranLog()
        {
            var result = await _woService.GetAllInv_Trans_Log();
            var masterparts = await _masterService.ItemMasterParts();
            var prodwos = await _woService.AllProductionPlan_Wo();
            foreach (var item in result)
            {
                foreach (var m in masterparts)
                {
                    if (item.Output_Part_No == m.PartId)
                    {
                        item.OutPutPartNo = m.PartNo + " / " + m.Description;
                    }
                    if (item.Input_Part_NoId == m.PartId)
                    {
                        item.InputPartNo = m.PartNo + " / " + m.Description;
                    }
                }
                ManufacturedPartNoDetailVM mf = await _masterService.GetManufPart(Convert.ToInt32(item.Output_Part_No));
                var resultList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                if (resultList.Count() > 0)
                {

                    if (item.Output_Routing_Id != 0)
                    {
                        item.OutPutRoutingName = resultList.Where(r => r.RoutingId == item.Output_Routing_Id).FirstOrDefault().RoutingName;
                        var step = await _routingService.RoutingSteps((int)item.Output_Routing_Id);
                        item.OutputOprNo = step.FirstOrDefault().StepNumber;
                        if (step.FirstOrDefault().StepLocation == 1.ToString())
                        {
                            item.FromSender = "Inhouse";
                        }
                        else if (step.FirstOrDefault().StepLocation == 2.ToString())
                        {
                            item.FromSender = "SubCon";
                        }
                        else
                        {
                            item.FromSender = "Company";
                        }
                    }
                    if (item.Input_Routing_Id != 0)
                    {
                        item.InPutRoutingName = resultList.Where(r => r.RoutingId == item.Input_Routing_Id).FirstOrDefault().RoutingName;
                        var step = await _routingService.RoutingSteps((int)item.Input_Routing_Id);
                        item.InputOprNo = step.FirstOrDefault().StepNumber;
                        if (step.FirstOrDefault().StepLocation == 1.ToString())
                        {
                            item.FromSender = "Inhouse";
                        }
                        else if (step.FirstOrDefault().StepLocation == 2.ToString())
                        {
                            item.FromSender = "SubCon";
                        }
                        else
                        {
                            item.FromSender = "Company";
                        }
                    }
                }
                if (item.Part_Status == 1)
                {
                    item.PartStatus = "Accepted";
                }
                else if (item.Part_Status == 2)
                {
                    item.PartStatus = "Rework";
                }
                else
                {
                    item.PartStatus = "Rejected";
                }
                if (item.Qnty_Mismatch > 0)
                {
                    item.MisMatchStatus = "Less";
                }
                else if (item.Qnty_Mismatch < 0)
                {
                    item.MisMatchStatus = "Extra";
                }
                else
                {
                    item.MisMatchStatus = "-";
                }
                ClaimsPrincipal userClaim = HttpContext.User;
                string fullName = AppUtil.GetFullName(userClaim);
                item.UserName = fullName;
                item.ToSender = "Stores";
                item.Dt_timeStr = item.Dt_time.ToString("dd-MM-yyyy");
                item.QntyStr = Convert.ToInt64(item.Qnty);
                var prodwo = prodwos.Where(wo => wo.WoId == item.Wo_Id).FirstOrDefault();
                if(prodwo != null)
                {
                    item.WoNumber = prodwo.WONumber;
                }
                switch (item.Transaction_Id)
                {
                    case 1:
                        item.TransactionName = "Inward RM / BOF";
                        break;
                    case 2:
                        item.TransactionName = "Inward SubCon (Fin Part)";
                        break;
                    case 3:
                        item.TransactionName = "Return Unprocessed Parts to Stores from Subcon";
                        break;
                    case 4:
                        item.TransactionName = "Return Unprocessed Parts to Stores from Shop";
                        break;
                    case 5:
                        item.TransactionName = "Issue Shop";
                        break;
                    case 6:
                        item.TransactionName = "Issue SubCon";
                        break;
                    case 7:
                        item.TransactionName = "Within Shop Bookout";
                        break;
                    case 8:
                        item.TransactionName = "Bookout from Shop";
                        break;
                    case 9:
                        item.TransactionName = "Dispatch";
                        break;
                    case 10:
                        item.TransactionName = "Move to Scrap";
                        break;
                    default:
                        item.TransactionName = "-";
                        break;
                }
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetChildManfWIP()
        {
            try
            {
                // --- 1. Fast Concurrent Data Fetching ---
                var productionsTask = _woService.AllProductionPlan_Wo();
                var allTransLogsTask = _woService.GetAllInv_Trans_Log();
                var allMcWaitListsTask = _woService.GetAllMc_Wait_List();
                var masterpartsTask = _masterService.ItemMasterParts();
                var allTimeslotsTask = _woService.GetAllTimeslot_List();
                var allRwkListTask = _woService.GetAllRwk_List();
                var allNcLogsTask = _woService.GetAllNcLog();
                var procplanTask = _woService.GetAllProcPlan();
                var shopInspLogsTask = _woService.GetAllShop_Insp_Log();

                await Task.WhenAll(productionsTask, allTransLogsTask, allMcWaitListsTask,
                                   masterpartsTask, allTimeslotsTask, allRwkListTask, allNcLogsTask, procplanTask, shopInspLogsTask);

                var productions = productionsTask.Result;
                var allTransLogs = allTransLogsTask.Result;
                var allMcWaitLists = allMcWaitListsTask.Result;
                var masterparts = masterpartsTask.Result;
                var allTimeslots = allTimeslotsTask.Result;
                var allRwkList = allRwkListTask.Result;
                var allNcLogs = allNcLogsTask.Result;
                var procplan = procplanTask.Result;
                var shopInspLogs = shopInspLogsTask.Result;

                var cmpsInProgress = productions
                    .Where(p => p.PartType == 1 && p.Status != 8)
                    .ToList();

                // Create Dictionaries for O(1) access
                var partDict = masterparts.ToDictionary(p => p.PartId, p => p);// Fast SO Number lookup
                var allTimeslotDict = allTimeslots.ToDictionary(t => t.Timeslot_ListId, t => t);

                // Identify NC Logs that are already moved to Rework (to avoid double counting in Scrap)
                var rwkNcLogIds = allRwkList.Select(r => r.NC_Log_Id).ToHashSet();

                var gridData = new List<dynamic>();

                // --- 3. Process Data per Work Order ---
                foreach (var wo in cmpsInProgress)
                {
                    partDict.TryGetValue(wo.PartId, out var part);
                    string partNo = part?.PartNo ?? "N/A";
                    string partDesc = part?.Description ?? "N/A";

                    var procplanwo = procplan.Where(p => p.WorkOrderId == wo.ProductionPlanId).FirstOrDefault();
                    var woTransactions = allTransLogs.Where(t => t.Wo_Id == wo.ProductionPlanId || (t.Input_Part_NoId == procplanwo?.PartId || t.Output_Part_No == procplanwo?.PartId)).ToList();

                    string inputPartNo = "";
                    string inputPartDesc = "";
                    var firstIssue = woTransactions.FirstOrDefault(t => t.Transaction_Id == 1|| t.Transaction_Id == 2);
                    //if (firstIssue != null)
                    //{
                    //    partDict.TryGetValue(firstIssue.Input_Part_NoId, out var inPart);
                    //    inputPartNo = inPart?.PartNo ?? "N/A";
                    //    inputPartDesc = inPart?.Description ?? "";
                    //}
                    string routingName = " ";
                    string inputOprNo = " ";
                    string outputOprNo = " ";
                    var mf = await _masterService.GetManufPart((int)wo.PartId);
                    var routingList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                    var route = routingList.FirstOrDefault(r => r.RoutingId == wo.RoutingId);
                    if (route != null)
                    {
                        routingName = route.RoutingName;
                        var routingSteps = await _routingService.RoutingSteps(route.RoutingId);

                        partDict.TryGetValue(route.MKPartId, out var inpart);
                        inputPartNo = inpart?.PartNo ?? "N/A";
                        inputPartDesc = inpart?.Description ?? "N/A";
                    }

                    var inputIssue = woTransactions
                        .Where(t => t.Transaction_Id == 1 || t.Transaction_Id == 2)
                        .Sum(t => t.Qnty);

                    // 2. Output (A): Sum of Bookout Transactions (Id 7=Within Shop, 8=Bookout, or Status=1 Accepted)
                    var outputBooked = woTransactions
                        .Where(t => (t.Transaction_Id == 7 || t.Transaction_Id == 8) && t.Part_Status == 1)
                        .Sum(t => t.Qnty);
                    if(outputBooked == 0)
                    {
                        outputBooked = shopInspLogs
                             .Where(s => s.Input_Part_No == wo.PartId)
                             .Sum(s => s.Qnty_OK_finished);
                    }

                    var woNcLogs = allNcLogs.Where(n => n.Inw_Recpt_Part_No_Id == wo.PartId).ToList(); // Filter NCs for this Part
                    var woNcLogIds = woNcLogs.Select(n => n.Insp_Outcome_Details_Id).ToHashSet();

                    var reworkQty = allRwkList
                        .Where(r => woNcLogIds.Contains(r.NC_Log_Id)) // Reworks linked to this WO's NCs
                        .Join(allNcLogs, r => r.NC_Log_Id, n => n.Insp_Outcome_Details_Id, (r, n) => n.NC_Qnty)
                        .Sum();

                    var scrapFromNc = woNcLogs
                        .Where(n => !rwkNcLogIds.Contains(n.Insp_Outcome_Details_Id)) // Only NCs NOT in rework
                        .Sum(n => n.NC_Qnty);

                    var manualScrap = woTransactions
                        .Where(t => (t.Transaction_Id == 10 || t.Part_Status == 3) && t.NC_Log_Id == 0) // Manual moves to scrap
                        .Sum(t => t.Qnty);

                    var totalScrap = scrapFromNc + manualScrap;

                    var wipQnty = inputIssue - (outputBooked + totalScrap + reworkQty);
                    if (wipQnty < 0) wipQnty = 0; // Safety check

                    decimal inputCoverage = wo.CalcWOQty > 0 ? (inputIssue / (decimal)wo.CalcWOQty) * 100 : 0;
                    string inputCoverageStr = $"{inputCoverage:0.00}%";

                    decimal woCompletion = wo.CalcWOQty > 0 ? (outputBooked / (decimal)wo.CalcWOQty) * 100 : 0;
                    string woCompletionStr = $"{woCompletion:0.00}%";

                    int daysElapsed = 0;
                    if (wo.WODate.HasValue)
                    {
                        daysElapsed = (DateTime.Now - wo.WODate.Value).Days;
                    }
                    string outputBookoutDate = (wo.ActCompletionDate != DateTime.MinValue)
                        ? wo.ActCompletionDate.ToString("dd-MM-yyyy")
                        : " ";

                    string inputIssueDate = wo.WODate.HasValue
                        ? wo.WODate.Value.ToString("dd-MM-yyyy")
                        : " ";

                    gridData.Add(new
                    {
                        WoId = (wo.ProductionPlanId),
                        WoNumber = wo.WONumber,
                        WoQnty = wo.CalcWOQty,
                        PlanStDate = wo.PlanStartDate.ToString("dd-MM-yyyy"),
                        PlanEndDate = wo.PlanCompletionDate?.ToString("dd-MM-yyyy"),
                        PartNo = partNo,
                        PartDesc = partDesc,
                        RoutingName = routingName,
                        InputPartNo = inputPartNo,
                        InputPartDesc = inputPartDesc,

                        // Column E (Input)
                        Input_Issue_Date = inputIssueDate,
                        Input_Qnty = inputIssue,

                        // Column A (Output)
                        Output_Bookout_Date = outputBookoutDate,
                        Output_Qnty = outputBooked,

                        // Column B (WIP)
                        WIP_Qnty = wipQnty,

                        // Column C (Scrap)
                        Scrap_Qnty = totalScrap,

                        // Column D (Rework)
                        Rework_Qnty = reworkQty,

                        // Percentages & Days
                        Input_Coverage_Pct = inputCoverageStr,
                        WO_Completion_Pct = woCompletionStr,
                        Elapsed_Days = daysElapsed
                    });
                }

                return Ok(gridData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetChildManfWIP: {ex.Message}");
                return BadRequest("An error occurred while fetching WIP data.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetWIPFlowReport(long woId)
        {
            try
            {
                // 1. Fetch Work Order Details
                var allWos = await _woService.AllProductionPlan_Wo();
                var shopInspLogs = (await _woService.GetAllShop_Insp_Log()).ToList();
                var wo = allWos.FirstOrDefault(w => w.ProductionPlanId == woId);
                if (wo == null) return NotFound("Work Order not found");

                // 2. Fetch Routing Steps (to build the 10 -> 20 -> 30 structure)
                var routingSteps = (await _routingService.RoutingSteps((int)wo.RoutingId))
                                   .OrderBy(r => r.StepSequence)
                                   .ToList();

                if (!routingSteps.Any()) return Ok(new List<dynamic>());

                // 3. Fetch all Logs and Data needed for calculation
                var allTransLogs = await _woService.GetAllInv_Trans_Log();
                var woLogs = allTransLogs.Where(t => t.Wo_Id == woId).ToList();

                var allNcLogs = await _woService.GetAllNcLog();
                var woNcLogs = allNcLogs.Where(n => n.Inw_Recpt_Part_No_Id == wo.PartId).ToList(); // Assuming NC links via Part/WO context logic

                var allRwkList = await _woService.GetAllRwk_List();
                var departments = await _departmentService.GetDepartments(1);
                var companies = await _masterService.GetCompanies();

                var resultList = new List<dynamic>();

                // 4. Iterate through steps to calculate flow
                for (int i = 0; i < routingSteps.Count; i++)
                {
                    var currentStep = routingSteps[i];
                    var nextStep = (i + 1 < routingSteps.Count) ? routingSteps[i + 1] : null;

                    // --- A. Identify Operations ---
                    string startOp = currentStep.StepNumber ?? "0";
                    string endOp = nextStep != null ? nextStep.StepNumber : "Stores"; // Last op goes to Stores

                    // --- B. Identify Locations ---
                    string fromLocation = "-";
                    if (currentStep.StepLocation == "1") // Inhouse
                    {
                        // Assuming you have logic to find which shop matches the Machine/Step
                        // For now, defaulting to "Shop" based on your existing controllers
                        fromLocation = "Shop";
                    }
                    else if (currentStep.StepLocation == "2") // SubCon
                    {
                        fromLocation = "SubCon";
                    }

                    string toLocation = "-";
                    if (nextStep != null)
                    {
                        toLocation = nextStep.StepLocation == "1" ? "Shop" : "SubCon";
                    }
                    else
                    {
                        toLocation = "Stores"; // Final destination
                    }

                    // --- C. Calculate Quantities ---

                    // 1. INPUT (E): 
                    // If First Step: Input is Material Issue (Trans Id 5 or 6)
                    // If Middle Step: Input is the Bookout of the PREVIOUS step
                    long inputQty = 0;
                    if (i == 0)
                    {
                        inputQty = (long)woLogs
                            .Where(t => t.Transaction_Id == 1 || t.Transaction_Id == 2) // Issue to Shop/Subcon
                            .Sum(t => t.Qnty);
                    }
                    else
                    {
                        var prevStep = routingSteps[i - 1];
                        inputQty = (long)woLogs
                            .Where(t => t.Output_Opr_No == prevStep.StepId && (t.Transaction_Id == 8 || t.Transaction_Id == 3 || t.Part_Status == 1))
                            .Sum(t => t.Qnty);
                    }

                    // 2. BOOKOUT (A): Successfully completed items from CURRENT step
                    // Looking for transactions where Output_Opr_No is current step
                    //long bookoutQty = (long)woLogs
                    //    .Where(t => t.Output_Opr_No == currentStep.StepId && (t.Transaction_Id == 7 || t.Transaction_Id == 8 || t.Part_Status == 1))
                    //    .Sum(t => t.Qnty); 
                    long bookoutQty = (long)shopInspLogs
                         .Where(s => s.Input_Opr_NoId == currentStep.StepId)
                         .Sum(s => s.Qnty_OK_finished);

                    // 3. REWORK (D) & SCRAP (C) specific to this Operation
                    // Need to filter NC/Rework logs by the Operation ID (StepId)

                    // Filter NCs for this specific step
                    var stepNcLogs = woNcLogs.Where(n => n.Opr_No_Id == currentStep.StepId).ToList();
                    var stepNcIds = stepNcLogs.Select(n => n.Insp_Outcome_Details_Id).ToList();

                    // Rework = NCs that exist in Rwk_List
                    long reworkQty = (long)allRwkList
                        .Where(r => stepNcIds.Contains(r.NC_Log_Id))
                        .Join(stepNcLogs, r => r.NC_Log_Id, n => n.Insp_Outcome_Details_Id, (r, n) => n.NC_Qnty)
                        .Sum();

                    // Scrap = NCs NOT in Rwk_List + Manual Scrap (Trans Id 10) for this step
                    long ncScrapQty = (long)stepNcLogs
                        .Where(n => !allRwkList.Any(r => r.NC_Log_Id == n.Insp_Outcome_Details_Id))
                        .Sum(n => n.NC_Qnty);

                    long manualScrapQty = (long)woLogs
                        .Where(t => t.Input_Opr_No == currentStep.StepId && t.Transaction_Id == 10)
                        .Sum(t => t.Qnty);

                    long scrapQty = ncScrapQty + manualScrapQty;

                    // 4. WIP (B): Balance sitting in this operation
                    // Formula: Input - (Good Output + Scrap + Rework)
                    long wipQty = inputQty - (bookoutQty + scrapQty + reworkQty);
                    if (wipQty < 0) wipQty = 0; // Prevent negatives due to data sync issues

                    // --- D. Build Object ---
                    resultList.Add(new
                    {
                        WoId = (wo.ProductionPlanId),
                        StartingOprNoId = currentStep.StepId,
                        StartingOprNo = startOp,
                        EndingOprNo = endOp,
                        FromLocation = fromLocation,
                        ToLocation = toLocation,
                        InputQnty = inputQty,      // E10, E20
                        BookoutQnty = bookoutQty,  // A10, A20
                        WIPQnty = wipQty,          // B10
                        Scrap = scrapQty,          // C10
                        WfRework = reworkQty       // D10
                    });
                }

                return Ok(resultList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetWIPFlowReport: {ex.Message}");
                return BadRequest("Error generating report");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetInventoryTransactionLog(long woId, long stepId)
        {
            try
            {
                // 1. Fetch WO and Basic Details
                var allWos = await _woService.AllProductionPlan_Wo();
                var wo = allWos.FirstOrDefault(w => w.ProductionPlanId == woId);
                if (wo == null) return NotFound("Work Order not found");

                var masterParts = await _masterService.ItemMasterParts();
                var part = masterParts.FirstOrDefault(p => p.PartId == wo.PartId);

                // 2. Fetch Routing & Operations to Determine Context (Previous/Current)
                var routingSteps = (await _routingService.RoutingSteps((int)wo.RoutingId))
                                   .OrderBy(r => r.StepSequence)
                                   .ToList();

                var currentStep = routingSteps.FirstOrDefault(r => r.StepId == stepId);
                if (currentStep == null) return BadRequest("Invalid Operation Step ID");

                // Find Previous Step (to calculate Input from Previous Bookout)
                var currentIndex = routingSteps.IndexOf(currentStep);
                var prevStep = currentIndex > 0 ? routingSteps[currentIndex - 1] : null;

                // 3. Fetch Logs & Employees
                var allTransLogs = await _woService.GetAllInv_Trans_Log();
                var employees = await _employeeService.GetAllEmployee();

                // Filter logs relevant to this WO context
                // We need: 
                // A. Input Logs (Issue to this Op OR Bookout from Prev Op)
                // B. Output Logs (Bookout/Scrap/Rework from this Op)

                var woLogs = allTransLogs.Where(t => t.Input_Opr_No == stepId).ToList();
                var gridRows = new List<dynamic>();

                long totalInput = 0;
                long totalBookout = 0;
                long totalScrap = 0;
                long totalRework = 0;

                // --- 4. Logic to Identify "Input" Transactions ---
                List<Inv_Trans_LogVM> inputTransactions;
                if (prevStep == null)
                {
                    // First Operation: Input comes from Stores Issue (Trans Id 5 or 6)
                    inputTransactions = woLogs.Where(t => (t.Transaction_Id == 1 || t.Transaction_Id == 2) && t.Input_Opr_No == stepId).ToList();
                }
                else
                {
                    // Subsequent Operation: Input comes from Previous Op's Bookout (Trans Id 7, 8)
                    // Note: When Op 10 Books out (Output_Opr_No = 10), it effectively becomes input for Op 20.
                    inputTransactions = woLogs.Where(t => (t.Transaction_Id == 7 || t.Transaction_Id == 8) && t.Output_Opr_No == prevStep.StepId && t.Part_Status == 1).ToList();
                }

                // --- 5. Logic to Identify "Output" Transactions (Current Step) ---
                var outputTransactions = woLogs.Where(t =>
                    (t.Input_Opr_No == stepId || t.Output_Opr_No == stepId) && // Involves current step
                    (t.Transaction_Id == 7 || t.Transaction_Id == 8 || t.Transaction_Id == 10 || t.Part_Status == 2 || t.Part_Status == 3) // Bookout, Scrap, Rework
                ).ToList();

                // Merge and Sort by Date for the Grid
                var allRelevantLogs = inputTransactions.Concat(outputTransactions)
                                                       .OrderBy(t => t.Dt_time)
                                                       .ToList();

                // --- 6. Process Grid Rows ---
                foreach (var log in allRelevantLogs)
                {
                    string transType = "";
                    string inputVal = "0";
                    string bookoutVal = "0";
                    string scrapVal = "0";
                    string reworkVal = "0";

                    // Detect Type
                    bool isInput = inputTransactions.Any(x => x.Inv_Trans_LogId == log.Inv_Trans_LogId);

                    if (isInput)
                    {
                        transType = prevStep == null ? "Material Issue" : "Input from Prev Opr";
                        inputVal = log.Qnty.ToString();
                        totalInput += (long)log.Qnty;
                    }
                    else if ((log.Transaction_Id == 7 || log.Transaction_Id == 8) && log.Part_Status == 1)
                    {
                        transType = "Bookout from Shop";
                        bookoutVal = log.Qnty.ToString();
                        totalBookout += (long)log.Qnty;
                    }
                    else if (log.Part_Status == 3 || log.Transaction_Id == 10) // Scrap
                    {
                        transType = "Scrap";
                        scrapVal = log.Qnty.ToString();
                        totalScrap += (long)log.Qnty;
                    }
                    else if (log.Part_Status == 2) // Rework
                    {
                        transType = "Move to Rework";
                        reworkVal = log.Qnty.ToString();
                        totalRework += (long)log.Qnty;
                    }
                    else
                    {
                        continue; 
                    }

                    //var user = employees.FirstOrDefault(e => e.Employee_ID == log.PersonId)?.Employee_name ?? "System";
                    ClaimsPrincipal userClaim = HttpContext.User;
                    string fullName = AppUtil.GetFullName(userClaim);
                    var user = fullName;

                    gridRows.Add(new
                    {
                        TransactionDate = log.Dt_time.ToString("dd-MM-yyyy"),
                        TransactionType = transType,
                        User = user,
                        InputQnty = inputVal,
                        BookoutQnty = bookoutVal,
                        WipQnty = "-", // Per image: "We show WIP Only in the Total Row only"
                        Scrap = scrapVal,
                        WfRework = reworkVal
                    });
                }

                // --- 7. Calculate WIP (C = A - (B + D + E)) ---
                long totalWip = totalInput - (totalBookout + totalScrap + totalRework);
                if (totalWip < 0) totalWip = 0;

                // --- 8. Build Header Data ---
                // Locations
                string fromLoc = currentStep.StepLocation == "1" ? "Shop" : "SubCon"; // Simplified logic
                string toLoc = "Next Opr / Stores"; // Simplified

                // Percentages
                double planQty = wo.CalcWOQty > 0 ? wo.CalcWOQty : 1;
                string inputCoverage = ((totalInput / planQty) * 100).ToString("0.##") + "%";
                string bookoutCompl = ((totalBookout / planQty) * 100).ToString("0.##") + "%";

                // Input Part Info
                string inputPartStr = "";
                if (prevStep == null)
                {
                    // If Step 1, find MakeFrom Part
                    var mf = await _masterService.GetManufPart((int)wo.PartId);
                    if (mf.ManufacturedPartType == 1) // Child Part
                    {
                        var mkList = await _masterService.GetMPMakeFromListByPartId(mf.ManufacturedPartNoDetailId.ToString());
                        var mk = mkList.FirstOrDefault(); // Taking first RM/Input
                        if (mk != null)
                        {
                            var mkPart = masterParts.FirstOrDefault(p => p.PartId == mk.MPPartId);
                            inputPartStr = mkPart != null ? $"{mkPart.PartNo} / {mkPart.Description}" : "";
                        }
                    }
                }
                else
                {
                    // If Step > 1, Input is WIP of current part
                    inputPartStr = $"{part.PartNo} (WIP) / {part.Description}";
                }

                var headerData = new
                {
                    WoNo = wo.WONumber,
                    ChildManfPart = $"{part.PartNo} / {part.Description}",
                    RoutingNo = wo.RoutingId.ToString(), // Or RoutingName if available
                    InputPart = inputPartStr,
                    StartingOprNo = currentStep.StepNumber,
                    EndingOprNo = currentStep.StepNumber,
                    FromLocation = fromLoc,
                    ToLocation = toLoc,
                    InputCoverage = inputCoverage,
                    BookoutCompl = bookoutCompl,
                    // Totals for Footer
                    TotalInput = totalInput,
                    TotalBookout = totalBookout,
                    TotalWIP = totalWip,
                    TotalScrap = totalScrap,
                    TotalRework = totalRework
                };

                return Ok(new { Header = headerData, GridData = gridRows });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetInventoryTransactionLog: {ex.Message}");
                return BadRequest("Error generating transaction log");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAssemblyWOInProgress()
        {
            try
            {
                // 1. Fetch all necessary data in parallel
                var productionsTask = _woService.AllProductionPlan_Wo();
                var allTransLogsTask = _woService.GetAllInv_Trans_Log();
                var masterpartsTask = _masterService.ItemMasterParts();
                var allNcLogsTask = _woService.GetAllNcLog();
                var allRwkListTask = _woService.GetAllRwk_List();
                var procplanTask = _woService.GetAllProcPlan();

                await Task.WhenAll(productionsTask, allTransLogsTask, masterpartsTask, allNcLogsTask, allRwkListTask, procplanTask);

                var productions = productionsTask.Result;
                var allTransLogs = allTransLogsTask.Result;
                var masterparts = masterpartsTask.Result;
                var allNcLogs = allNcLogsTask.Result;
                var allRwkList = allRwkListTask.Result;
                var procplan = procplanTask.Result;

                // 2. Filter for Active Assembly Work Orders (Status != 8 i.e., not closed)
                // Assuming PartType == 2 indicates Assembly based on your existing ProcPlan logic
                var assemblyWOs = productions
                    .Where(p => p.PartType == 2 && p.Status != 8)
                    .ToList();

                // 3. Create Lookups for performance
                var partDict = masterparts.ToDictionary(p => p.PartId, p => p);

                // Identify NC Logs already moved to Rework to avoid double counting in Scrap
                var rwkNcLogIds = allRwkList.Select(r => r.NC_Log_Id).ToHashSet();

                var gridData = new List<dynamic>();

                foreach (var wo in assemblyWOs)
                {
                    // --- Part Details ---
                    partDict.TryGetValue(wo.PartId, out var part);
                    string partNoStr = part != null ? $"{part.PartNo} / {part.Description}" : "Unknown";

                    // --- Transactions for this WO ---
                    //var woTransactions = allTransLogs.Where(t => t.Wo_Id == wo.ProductionPlanId).ToList();
                    var procplanwo = procplan.Where(p => p.WorkOrderId == wo.ProductionPlanId).FirstOrDefault();
                    var woTransactions = allTransLogs.Where(n => (n.Input_Part_NoId == procplanwo.PartId || n.Output_Part_No == procplanwo.PartId)).ToList();

                    // --- Calculate Data Points based on Image Logic ---

                    // E: Assy Kits Issue Qnty (Sum of Issue Transactions: ID 5 or 6)
                    var inputTrans = woTransactions.Where(t => t.Transaction_Id == 1 || t.Transaction_Id == 2).ToList();
                    long inputIssueQty = (long)inputTrans.Sum(t => t.Qnty);

                    // 1st Assy Kit Issue Date
                    var firstIssue = inputTrans.OrderBy(t => t.Dt_time).FirstOrDefault();
                    string firstIssueDateStr = firstIssue != null ? firstIssue.Dt_time.ToString("dd-MM-yyyy") : "-";

                    // A: Assy Bookout Qnty (Sum of Bookout Trans: ID 7, 8 or Status=1 Accepted)
                    var outputTrans = woTransactions.Where(t => t.Transaction_Id == 7 || t.Transaction_Id == 8 || t.Part_Status == 1).ToList();
                    long bookoutQty = (long)outputTrans.Sum(t => t.Qnty);

                    // 1st Assy B/O Date
                    var firstBookout = outputTrans.OrderBy(t => t.Dt_time).FirstOrDefault();
                    string firstBookoutDateStr = firstBookout != null ? firstBookout.Dt_time.ToString("dd-MM-yyyy") : "-";

                    // D: Waiting for Rework
                    // Logic: Find NCs for this Part, match with Rework List
                    var woNcLogs = allNcLogs.Where(n => n.Inw_Recpt_Part_No_Id == wo.PartId).ToList();
                    var woNcLogIds = woNcLogs.Select(n => n.Insp_Outcome_Details_Id).ToHashSet();

                    long reworkQty = (long)allRwkList
                        .Where(r => woNcLogIds.Contains(r.NC_Log_Id))
                        .Join(allNcLogs, r => r.NC_Log_Id, n => n.Insp_Outcome_Details_Id, (r, n) => n.NC_Qnty)
                        .Sum();

                    // C: Scrap
                    // Logic: NCs not in Rework + Manual Scrap Transactions (ID 10)
                    long ncScrapQty = (long)woNcLogs
                        .Where(n => !rwkNcLogIds.Contains(n.Insp_Outcome_Details_Id))
                        .Sum(n => n.NC_Qnty);

                    long manualScrapQty = (long)woTransactions
                        .Where(t => (t.Transaction_Id == 10 || t.Part_Status == 3) && t.NC_Log_Id == 0)
                        .Sum(t => t.Qnty);

                    long totalScrap = ncScrapQty + manualScrapQty;

                    // B: WIP Kit Qnty = E - (A + C + D)
                    long wipQty = inputIssueQty - (bookoutQty + totalScrap + reworkQty);
                    if (wipQty < 0) wipQty = 0;

                    // --- Calculations ---

                    // % Input Coverage (E / Plan Qty)
                    double inputCovPct = wo.CalcWOQty > 0 ? ((double)inputIssueQty / wo.CalcWOQty) * 100 : 0;

                    // % BO Completion (A / Plan Qty)
                    double boCompPct = wo.CalcWOQty > 0 ? ((double)bookoutQty / wo.CalcWOQty) * 100 : 0;

                    // Elapsed Days (Today - 1st Input Date)
                    int elapsedDays = 0;
                    if (firstIssue != null)
                    {
                        elapsedDays = (DateTime.Now - firstIssue.Dt_time).Days;
                    }

                    // --- Construct "WO Details" String ---
                    // Format: WO ID: xxxx, Pln Qnty: xxx, Pln St Dt: ..., Pln End Dt: ...
                    string woDetails = $"WO ID: {wo.WONumber}, Pln Qnty: {wo.CalcWOQty}\n" +
                                       $"Pln St Dt: {wo.PlanStartDate:dd-MM-yyyy}, Pln End Dt: {wo.PlanCompletionDate:dd-MM-yyyy}";

                    // Add to List
                    gridData.Add(new 
                    {
                        WoId = wo.ProductionPlanId,
                        WoDetails = woDetails,
                        AssyPartNoDesc = partNoStr,
                        FirstKitIssueDate = firstIssueDateStr,
                        KitIssueQnty = inputIssueQty, // E
                        FirstBookoutDate = firstBookoutDateStr,
                        BookoutQnty = bookoutQty, // A
                        WipKitQnty = wipQty,      // B
                        ScrapQnty = totalScrap,   // C
                        ReworkQnty = reworkQty,   // D
                        InputCoverage = $"{inputCovPct:0.00}%",
                        BoCompletion = $"{boCompPct:0.00}%",
                        ElapsedDays = elapsedDays
                    });
                }

                return Ok(gridData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAssemblyWOInProgress: {ex.Message}");
                return BadRequest("Error retrieving Assembly WO Progress data.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAssemblyBOMCoverage(long woId)
        {
            try
            {
                // 1. Fetch all necessary data in parallel
                var productionsTask = _woService.AllProductionPlan_Wo();
                var allTransLogsTask = _woService.GetAllInv_Trans_Log();
                var masterpartsTask = _masterService.ItemMasterParts();
                var routingsTask = _routingService.GetRoutingListItems(); // Assuming this gets routing list headers
                var procplanTask = _woService.GetAllProcPlan();

                await Task.WhenAll(productionsTask, allTransLogsTask, masterpartsTask, routingsTask, procplanTask);

                var productions = productionsTask.Result;
                var allTransLogs = allTransLogsTask.Result;
                var masterparts = masterpartsTask.Result;
                var routings = routingsTask.Result;
                var procplan = procplanTask.Result;

                // 2. Get Specific Work Order
                var wo = productions.FirstOrDefault(w => w.ProductionPlanId == woId);
                if (wo == null) return NotFound("Work Order not found");

                // 3. Get Master Part Details
                var mainPart = masterparts.FirstOrDefault(m => m.PartId == wo.PartId);
                var manufPart = await _masterService.GetManufPart((int)wo.PartId);

                // 4. Determine Type (Assembly vs Sub-Assembly logic)
                // Logic: If PartType is 2 (Assembly) and it has a ParentWoId, it's likely a Sub-Assembly in this context
                string assemblyType = (wo.PartType == 2) ? "Assembly" : "Sub-Assembly";
                if (wo.ParentWoId > 0 && wo.PartType == 2) assemblyType = "Sub-Assembly";

                // 5. Get Routing Info
                //var routing = ;
                var routingList = await _routingService.Routings(manufPart.ManufacturedPartNoDetailId);
                var route = routingList.FirstOrDefault(r => r.RoutingId == wo.RoutingId);
                string routingName = "";
                if (route != null)
                {
                    routingName = route.RoutingName;
                }

                // 6. Get BOM List
                var bomList = await _masterService.BOMS(manufPart.ManufacturedPartNoDetailId.ToString());

                var gridData = new List<dynamic>();
                double minAssemblyCoverage = double.MaxValue; // Used to calculate the header bottleneck count

                // 7. Process BOM Items
                foreach (var bomItem in bomList)
                {
                    var childPart = masterparts.FirstOrDefault(m => m.PartId == bomItem.BOMPartId);

                    // A. BOM Qnty (Per Unit)
                    decimal bomQtyPerUnit = bomItem.Quantity;

                    // B. Transaction Qnty (Total Issued to this WO)
                    // Filtering for Transaction_Id 5 (Issue Shop) or 6 (Issue SubCon)
                    var procplanwo = procplan.Where(p => p.WorkOrderId == wo.ProductionPlanId).FirstOrDefault();
                    var issuedQty = allTransLogs
                        .Where(t =>  (t.Transaction_Id == 1 || t.Transaction_Id == 2)
                                    && (t.Input_Part_NoId == procplanwo.PartId || t.Output_Part_No == procplanwo.PartId))
                        .Sum(t => t.Qnty);

                    // C. # Assy Coverage (Floor calculation)
                    // How many assemblies can we make with this specific child part?
                    double assyCoverage = 0;
                    if (bomQtyPerUnit > 0)
                    {
                        assyCoverage = Math.Floor((double)(issuedQty / bomQtyPerUnit));
                    }

                    // Update the global minimum (Bottleneck calculation)
                    if (assyCoverage < minAssemblyCoverage)
                    {
                        minAssemblyCoverage = assyCoverage;
                    }

                    // D. % Coverage
                    double percentCoverage = 0;
                    if (wo.CalcWOQty > 0)
                    {
                        percentCoverage = (assyCoverage / wo.CalcWOQty) * 100;
                    }

                    // Locations (Simplified logic based on typical flow)
                    string fromLocation = "Stores";
                    string toLocation = "Shop"; // Defaulting to Shop, could be derived from Routing Step

                    gridData.Add(new
                    {
                        PartId = bomItem.BOMPartId,
                        PartNo = childPart?.PartNo ?? "Unknown",
                        PartDesc = childPart?.Description ?? "Unknown",
                        PartType = childPart?.MasterPartType ?? "", // CMF, BOF, etc.
                        FromLocation = fromLocation,
                        ToLocation = toLocation,
                        BomQnty = bomQtyPerUnit,
                        TransactionQnty = issuedQty,
                        AssyCoverage = assyCoverage,
                        PercentCoverage = Math.Round(percentCoverage, 2)
                    });
                }

                // Handle case where BOM is empty
                if (!bomList.Any()) minAssemblyCoverage = 0;

                // 8. Construct Final Response Object
                var response = new
                {
                    Header = new
                    {
                        WoId = wo.ProductionPlanId,
                        WoNumber = wo.WONumber,
                        AssyPartNo = mainPart?.PartNo,
                        AssyDesc = mainPart?.Description,
                        Type = assemblyType, // "Sub-Assembly / Assembly"
                        PlannedWOQnty = wo.CalcWOQty,
                        RoutingNo = routingName,
                        PlanStartDate = wo.PlanStartDate.ToString("dd-MM-yyyy"),
                        PlanEndDate = wo.PlanCompletionDate?.ToString("dd-MM-yyyy"),
                        // The calculated bottleneck value
                        BuildableAssemblies = minAssemblyCoverage
                    },
                    GridData = gridData
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAssemblyBOMCoverage: {ex.Message}");
                return BadRequest("Error fetching BOM Coverage data.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAssemblyPartInventoryLog(long woId, long bomPartId)
        {
            try
            {
                // 1. Fetch Work Order Details
                var allWos = await _woService.AllProductionPlan_Wo();
                var wo = allWos.FirstOrDefault(w => w.ProductionPlanId == woId);
                if (wo == null) return NotFound("Work Order not found");

                // 2. Fetch Part Details (The Child Part from BOM)
                var masterParts = await _masterService.ItemMasterParts();
                var childPart = masterParts.FirstOrDefault(p => p.PartId == bomPartId);
                var assyPart = masterParts.FirstOrDefault(p => p.PartId == wo.PartId); // The parent Assembly

                // 3. Fetch BOM Details to calculate "Total Qnty Reqd"
                // We need to know how many of this child part are needed per 1 Assembly
                var manufPart = await _masterService.GetManufPart((int)wo.PartId);
                var bomList = await _masterService.BOMS(manufPart.ManufacturedPartNoDetailId.ToString());
                var bomItem = bomList.FirstOrDefault(b => b.BOMPartId == bomPartId);

                decimal qtyPerUnit = bomItem?.Quantity ?? 0;
                long totalQtyReqd = (long)(qtyPerUnit * wo.CalcWOQty);

                // 4. Fetch Transactions
                var allTransLogs = await _woService.GetAllInv_Trans_Log();

                // Filter Logic:
                // We want transactions where:
                // a. The WO matches
                // b. The Input Part ID matches the BOM Part ID
                // c. It is an Issue transaction (Transaction_Id 1 or 2 usually for Issue, or specific Issue Shop types)
                var procplan = await _woService.GetAllProcPlan();
                var procplanwo = procplan.Where(p => p.WorkOrderId == wo.ProductionPlanId).FirstOrDefault();
                var partLogs = allTransLogs.Where(t =>
                    (t.Wo_Id == woId ||
                    (t.Input_Part_NoId == procplanwo?.PartId || t.Output_Part_No == procplanwo?.PartId) )&&
                    (t.Transaction_Id == 1 || t.Transaction_Id == 2 || t.Transaction_Id == 5 || t.Transaction_Id == 6) // Include 5/6 for specific Issue types
                ).OrderBy(t => t.Dt_time).ToList();

                var gridRows = new List<dynamic>();
                long totalIssued = 0;

                // 5. Build Grid Rows
                foreach (var log in partLogs)
                {
                    totalIssued += (long)log.Qnty;

                    // Determine Transaction Type Name
                    string transType = log.Transaction_Id switch
                    {
                        1 => "Inward RM / Bof",
                        2 => "Inward SubCon",
                        7 => "Within Shop Bookout",
                        8 => "Shop Bookout",
                        _ => "Material Issue"
                    };

                    // Get User Name (Assuming current context or fetch from EmployeeService based on PersonId)
                    // For now, using the logged-in user logic found in your other methods or "System"
                    ClaimsPrincipal userClaim = HttpContext.User;
                    string fullName = AppUtil.GetFullName(userClaim); // Or fetch via log.PersonId if specific user tracking is needed

                    gridRows.Add(new
                    {
                        TransactionDate = log.Dt_time.ToString("dd-MM-yyyy"),
                        TransactionType = transType,
                        User = fullName, // Or resolve log.PersonId using _employeeService
                        TransactionQnty = log.Qnty
                    });
                }

                // 6. Calculate Header Metrics
                string percentCoverage = totalQtyReqd > 0
                    ? $"{((double)totalIssued / totalQtyReqd * 100):0.00}%"
                    : "0.00%";

                // Determine Locations (Logic based on your existing controllers)
                // Usually from Stores to Shop
                string fromLoc = "Stores";
                string toLoc = "Shop";

                // 7. Construct Final Response
                var response = new
                {
                    Header = new
                    {
                        WoNo = wo.WONumber,
                        PartNo = childPart?.PartNo ?? "Unknown",
                        PartDesc = childPart?.Description ?? "Unknown",
                        FromLocation = fromLoc,
                        ToLocation = toLoc,
                        TotalQntyReqd = totalQtyReqd,
                        PercentCoverage = percentCoverage,
                        TotalTransactionQnty = totalIssued // The "A xxx" from the footer
                    },
                    GridData = gridRows
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAssemblyPartInventoryLog: {ex.Message}");
                return BadRequest("Error retrieving inventory log");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSubAssemblyBOMCoverage(long woId,long BomPartId)
        {
            try
            {
                // 1. Fetch all necessary data in parallel
                var productionsTask = _woService.AllProductionPlan_Wo();
                var allTransLogsTask = _woService.GetAllInv_Trans_Log();
                var masterpartsTask = _masterService.ItemMasterParts();
                var routingsTask = _routingService.GetRoutingListItems(); // Assuming this gets routing list headers
                var procplanTask = _woService.GetAllProcPlan();

                await Task.WhenAll(productionsTask, allTransLogsTask, masterpartsTask, routingsTask, procplanTask);

                var productions = productionsTask.Result;
                var allTransLogs = allTransLogsTask.Result;
                var masterparts = masterpartsTask.Result;
                var routings = routingsTask.Result;
                var procplan = procplanTask.Result;

                // 2. Get Specific Work Order
                var wo = productions.FirstOrDefault(w => w.ProductionPlanId == woId);
                if (wo == null) return NotFound("Work Order not found");

                // 3. Get Master Part Details
                var mainPart = masterparts.FirstOrDefault(m => m.PartId == wo.PartId);
                var manufPart = await _masterService.GetManufPart((int)BomPartId);

                // 4. Determine Type (Assembly vs Sub-Assembly logic)
                // Logic: If PartType is 2 (Assembly) and it has a ParentWoId, it's likely a Sub-Assembly in this context
                string assemblyType = (wo.PartType == 2) ? "Assembly" : "Sub-Assembly";
                if (wo.ParentWoId > 0 && wo.PartType == 2) assemblyType = "Sub-Assembly";

                // 5. Get Routing Info
                //var routing = ;
                var routingList = await _routingService.Routings(manufPart.ManufacturedPartNoDetailId);
                var route = routingList.FirstOrDefault();
                string routingName = "";
                if (route != null)
                {
                    routingName = route.RoutingName;
                }

                // 6. Get BOM List
                var bomList = await _masterService.BOMS(manufPart.ManufacturedPartNoDetailId.ToString());

                var gridData = new List<dynamic>();
                double minAssemblyCoverage = double.MaxValue; // Used to calculate the header bottleneck count

                // 7. Process BOM Items
                foreach (var bomItem in bomList)
                {
                    var childPart = masterparts.FirstOrDefault(m => m.PartId == bomItem.BOMPartId);

                    // A. BOM Qnty (Per Unit)
                    decimal bomQtyPerUnit = bomItem.Quantity;

                    // B. Transaction Qnty (Total Issued to this WO)
                    // Filtering for Transaction_Id 5 (Issue Shop) or 6 (Issue SubCon)
                    var procplanwo = procplan.Where(p => p.WorkOrderId == wo.ProductionPlanId).FirstOrDefault();
                    var issuedQty = allTransLogs
                        .Where(t => (t.Transaction_Id == 1 || t.Transaction_Id == 2)
                                    && (t.Input_Part_NoId == procplanwo.PartId || t.Output_Part_No == procplanwo.PartId))
                        .Sum(t => t.Qnty);

                    // C. # Assy Coverage (Floor calculation)
                    // How many assemblies can we make with this specific child part?
                    double assyCoverage = 0;
                    if (bomQtyPerUnit > 0)
                    {
                        assyCoverage = Math.Floor((double)(issuedQty / bomQtyPerUnit));
                    }

                    // Update the global minimum (Bottleneck calculation)
                    if (assyCoverage < minAssemblyCoverage)
                    {
                        minAssemblyCoverage = assyCoverage;
                    }

                    // D. % Coverage
                    double percentCoverage = 0;
                    if (wo.CalcWOQty > 0)
                    {
                        percentCoverage = (assyCoverage / wo.CalcWOQty) * 100;
                    }

                    // Locations (Simplified logic based on typical flow)
                    string fromLocation = "Stores";
                    string toLocation = "Shop"; // Defaulting to Shop, could be derived from Routing Step

                    gridData.Add(new
                    {
                        PartId = bomItem.BOMPartId,
                        PartNo = childPart?.PartNo ?? "Unknown",
                        PartDesc = childPart?.Description ?? "Unknown",
                        PartType = childPart?.MasterPartType ?? "", // CMF, BOF, etc.
                        FromLocation = fromLocation,
                        ToLocation = toLocation,
                        BomQnty = bomQtyPerUnit,
                        TransactionQnty = issuedQty,
                        AssyCoverage = assyCoverage,
                        PercentCoverage = Math.Round(percentCoverage, 2)
                    });
                }

                // Handle case where BOM is empty
                if (!bomList.Any()) minAssemblyCoverage = 0;

                // 8. Construct Final Response Object
                var response = new
                {
                    Header = new
                    {
                        WoId = wo.ProductionPlanId,
                        WoNumber = wo.WONumber,
                        AssyPartNo = mainPart?.PartNo,
                        AssyDesc = mainPart?.Description,
                        Type = assemblyType,
                        PlannedWOQnty = wo.CalcWOQty,
                        RoutingNo = routingName,
                        PlanStartDate = wo.PlanStartDate.ToString("dd-MM-yyyy"),
                        PlanEndDate = wo.PlanCompletionDate?.ToString("dd-MM-yyyy"),
                        // The calculated bottleneck value
                        BuildableAssemblies = minAssemblyCoverage
                    },
                    GridData = gridData
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAssemblyBOMCoverage: {ex.Message}");
                return BadRequest("Error fetching BOM Coverage data.");
            }
        }
        //[HttpGet]
        //public async Task<IActionResult> GetInvTranLogForWO(long woId)
        //{
        //    try
        //    {
        //        // --- 1. Fetch Core Data ---
        //        // Fetch specific WO transactions directly if possible, otherwise fetch all and filter (optimized for in-memory)
        //        var allTransLogsTask = _woService.GetAllInv_Trans_Log();
        //        var masterpartsTask = _masterService.ItemMasterParts();
        //        var employeesTask = _employeeService.GetAllEmployee(); // For "Transacted By"
        //        var departmentsTask = _departmentService.GetDepartments(1); // For Shop Names
        //        var companiesTask = _masterService.GetCompanies(); // For Supplier Names

        //        await Task.WhenAll(allTransLogsTask, masterpartsTask, employeesTask, departmentsTask, companiesTask);

        //        var allTransLogs = allTransLogsTask.Result;
        //        var masterparts = masterpartsTask.Result;
        //        var employees = employeesTask.Result;
        //        var departments = departmentsTask.Result;
        //        var companies = companiesTask.Result;

        //        // --- 2. Filter & Prepare Lookups ---

        //        // Filter logs for the specific Work Order
        //        var woLogs = allTransLogs.Where(t => t.Wo_Id == woId).ToList();

        //        if (!woLogs.Any())
        //        {
        //            return Ok(new List<dynamic>()); // Return empty list if no logs found
        //        }

        //        // Dictionaries for O(1) Fast Lookups
        //        var partDict = masterparts.ToDictionary(p => p.PartId, p => p);
        //        var empDict = employees.ToDictionary(e => e.Employee_ID, e => e.Employee_name);
        //        var deptDict = departments.ToDictionary(d => d.DepartmentId, d => d.Name);
        //        var compDict = companies.ToDictionary(c => c.CompanyId, c => c.CompanyName);

        //        // Identify unique Routing IDs involved to fetch Step details efficiently
        //        var routingIds = woLogs.Select(l => l.Input_Routing_Id)
        //                               .Union(woLogs.Select(l => l.Output_Routing_Id))
        //                               .Where(id => id > 0)
        //                               .Distinct()
        //                               .ToList();

        //        // Fetch Routing Steps for all involved routings (Optimization: Batch fetch if possible, else loop)
        //        // Since we don't have a "GetStepsByRoutingIds" method, we fetch individually but cache them.
        //        var routingStepsCache = new Dictionary<long, IEnumerable<RoutingStepVM>>();
        //        foreach (var rid in routingIds)
        //        {
        //            if (!routingStepsCache.ContainsKey(rid))
        //            {
        //                var steps = await _routingService.RoutingSteps((int)rid);
        //                routingStepsCache[rid] = steps;
        //            }
        //        }

        //        var gridData = new List<dynamic>();

        //        // --- 3. Process Logs ---
        //        foreach (var item in woLogs)
        //        {
        //            // A. Transaction Type & Name
        //            string transName = item.Transaction_Id switch
        //            {
        //                1 => "Inward RM / BOF",
        //                2 => "Inward SubCon",
        //                3 => "Return to Stores (Subcon)",
        //                4 => "Return to Stores (Shop)",
        //                5 => "Issue to Shop",
        //                6 => "Issue to SubCon",
        //                7 => "Within Shop Movement",
        //                8 => "Bookout from Shop",
        //                9 => "Dispatch",
        //                10 => "Scrap",
        //                _ => "-"
        //            };

        //            // B. Part Information
        //            // Logic: If it's an Issue (Input), show Input Part. If Bookout (Output), show Output Part.
        //            long relevantPartId = (item.Transaction_Id == 5 || item.Transaction_Id == 6)
        //                                  ? item.Input_Part_NoId
        //                                  : (item.Output_Part_No > 0 ? item.Output_Part_No : item.Input_Part_NoId);

        //            partDict.TryGetValue(relevantPartId, out var part);
        //            string partNo = part?.PartNo ?? "N/A";
        //            string partDesc = part?.Description ?? "N/A";

        //            // C. Operation / Location Resolving
        //            string fromLoc = "-";
        //            string toLoc = "-";

        //            // Helper to get Op Number from Routing Cache
        //            string GetOpNum(long routingId, long opId)
        //            {
        //                if (routingId > 0 && routingStepsCache.TryGetValue(routingId, out var steps))
        //                {
        //                    return steps.FirstOrDefault(s => s.StepId == opId)?.StepNumber ?? "N/A";
        //                }
        //                return "N/A";
        //            }
        //            if (item.Transaction_Id == 1) 
        //            {
        //                fromLoc = "-";
        //                toLoc = "Stores";
        //            }
        //            if (item.Transaction_Id == 2) 
        //            {
        //                fromLoc = "Subcon";
        //                toLoc = "Stores";
        //            }

        //            // Logic for "From" and "To" based on Transaction Type
        //            if (item.Transaction_Id == 5) // Issue Shop
        //            {
        //                fromLoc = "Inhouse";
        //                toLoc = "Shop";
        //            }
        //            else if (item.Transaction_Id == 6) // Issue SubCon
        //            {
        //                fromLoc = "Stores";
        //                string supplier = compDict.ContainsKey(item.To_Location_Id) ? compDict[item.To_Location_Id] : "SubCon";
        //                toLoc = "SubCon";
        //            }
        //            else if (item.Transaction_Id == 7) // Within Shop
        //            {
        //                // From Input Op -> To Output Op (or Next Op logic if different)
        //                fromLoc = "Shop";
        //                // Assuming Output_Opr_No is the destination for within-shop movement logic, 
        //                // or if it's booking out of an op, 'To' is the next logical step. 
        //                // For simplicity in logs:
        //                toLoc = "Stores";
        //            }
        //            else if (item.Transaction_Id == 8) // Bookout Shop
        //            {
        //                fromLoc = $"Op: {GetOpNum(item.Output_Routing_Id, item.Output_Opr_No)}";
        //                toLoc = "Stores";
        //            }
        //            else if (item.Transaction_Id == 10 || item.Part_Status == 3) // Scrap
        //            {
        //                fromLoc = $"Op: {GetOpNum(item.Input_Routing_Id, item.Input_Opr_No)}";
        //                toLoc = "Scrap Yard";
        //            }

        //            // D. Status
        //            string status = item.Part_Status switch
        //            {
        //                1 => "Accepted",
        //                2 => "Rework",
        //                3 => "Rejected",
        //                _ => "Pending"
        //            };

        //            // E. User
        //            string transBy = empDict.ContainsKey(item.PersonId) ? empDict[item.PersonId] : "System";
        //            ClaimsPrincipal userClaim = HttpContext.User;
        //            string fullName = AppUtil.GetFullName(userClaim);
        //            transBy = fullName;
        //            // F. Date Formatting
        //            string dateStr = item.Dt_time.ToString("dd-MM-yyyy");
        //            string timeStr = item.Dt_time.ToString("hh:mm tt");

        //            // --- G. Build Grid Object ---
        //            gridData.Add(new
        //            {
        //                TransId = item.Inv_Trans_LogId,
        //                TransDate = dateStr,
        //                TransTime = timeStr,
        //                TransactionType = transName,
        //                PartNo = partNo,
        //                PartDesc = partDesc,
        //                Quantity = item.Qnty,
        //                FromLocation = fromLoc,
        //                ToLocation = toLoc,
        //                Status = status,
        //                TransactedBy = transBy,
        //                Remarks = item.Qnty_Mismatch_Comment ?? ""
        //            });
        //        }

        //        // Return ordered by latest first
        //        return Ok(gridData.OrderByDescending(x => x.TransDate).ThenByDescending(x => x.TransTime));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error in GetInvTranLogForWO: {ex.Message}");
        //        return BadRequest("An error occurred while fetching the transaction log.");
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> MultipleProductionWOPost([FromBody] IEnumerable<ProductionPlan_WoVM> ppwos)
        {
            var procdutionpost = await _woService.ProductionPlanWoPost(ppwos);
            return Ok(procdutionpost);
        }

        [HttpGet]
        public async Task<IActionResult> SplitWo(long woid, string initialDate, int numDays, int quantity, long salersorderId, int partId, int partType)
        {
            var inltialDt = DateTime.Parse(initialDate);
            List<ProductionPlan_WoVM> previousWorkdays = new List<ProductionPlan_WoVM>();
            DateTime currentDate = inltialDt;
            var workdetails = await _plantService.GetPlantWD(1);
            var holidaylist = await _plantService.GetHolidays(1);
            string weekOff1 = workdetails.WeeklyOff1;
            string weekOff2 = workdetails.WeeklyOff2;
            int quantityPerDay = quantity / numDays;

            for (int i = 0; i < numDays; i++)
            {
                do
                {
                    currentDate = currentDate.AddDays(-1);
                } while (!IsWorkDay(currentDate, holidaylist, weekOff1, weekOff2));

                ProductionPlan_WoVM dailywo = new ProductionPlan_WoVM
                {
                    ParentWoId = woid,
                    SalesOrderId = salersorderId,
                    CalcWOQty = quantityPerDay,
                    PlanCompletionDate = currentDate,
                    PartId = partId,
                    PartType = partType,
                    //For_Ref = 'N',
                    ReloadOption = "Split"
                };
                previousWorkdays.Add(dailywo);
            }
            //var postWO = await _baService.MultiplePostWO(previousWorkdays);
            var procdutionpost = await _woService.ProductionPlanWoPost(previousWorkdays);
            return Ok(procdutionpost);
        }

        [HttpPost]
        public async Task<IActionResult> ProductionPlanPost(ProductionPlan_WoVM production)
        {
            List<ProductionPlan_WoVM> productions = new List<ProductionPlan_WoVM>();
            if (production.PartType == 1)
            {
                production.Parentlevel = 'N';
            }
            else
            {
                var mpBOM = await _masterService.BOMS(production.PartId.ToString());
                if (mpBOM != null && mpBOM.Any())
                {
                    foreach (var bom in mpBOM)
                    {
                        var assy = await _masterService.GetManufPart((int)bom.BOMPartId);
                        if (assy != null)
                        {
                            production.Parentlevel = 'Y';
                        }
                        else
                        {
                            production.Parentlevel = 'N';
                        }
                    }
                }
                else
                {
                    production.Parentlevel = 'Y';
                }
            }
            productions.Add(production);
            var procdutionpost = await _woService.ProductionPlanWoPost(productions);
            return Ok(procdutionpost);
        }

        [HttpGet]
        public async Task<IActionResult> CalculateWOQuantity(string dispatchStartDate, string soCompletionDate, int balanceToManufacture, string dispatchOption)
        {
            var dispatchStartDt = DateTime.Parse(dispatchStartDate);
            var soCompletionDt = DateTime.Parse(soCompletionDate);

            var workdetails = await _plantService.GetPlantWD(1);
            var holidaylist = await _plantService.GetHolidays(1);
            string weekOff1 = workdetails.WeeklyOff1;
            string weekOff2 = workdetails.WeeklyOff2;

            int workDays = 0;

            for (var date = dispatchStartDt; date <= soCompletionDt; date = date.AddDays(1))
            {
                // Check if the day is a work day
                if (IsWorkDay(date, holidaylist, weekOff1, weekOff2))
                {
                    workDays++;
                }
            }

            int noOfWeeks = (int)Math.Ceiling((double)workDays / 7); // calculate number of weeks
            int noOfMonths = (int)Math.Ceiling((double)workDays / 30); // calculate number of months

            List<WorkOrdersVM> woDetails = new List<WorkOrdersVM>();

            switch (dispatchOption)
            {
                case "Daily":
                    int workDaysAdjusted = 0;
                    DateTime tempDate = dispatchStartDt.AddDays(1);
                    while (workDaysAdjusted < workDays)
                    {
                        if (!IsNonWorkingDay(tempDate, weekOff1, weekOff2, holidaylist))
                        {
                            workDaysAdjusted++;
                            WorkOrdersVM dailywo = new WorkOrdersVM
                            {
                                WONumber ="",
                                CalcWOQty = balanceToManufacture / workDays,
                                PlanCompletionDate = tempDate
                            };
                            woDetails.Add(dailywo);
                        }
                        tempDate = tempDate.AddDays(1);
                    }
                    break;
                case "Weekly":
                    //if (workDays > 22)
                    //{
                    int noOfWeeksAdjusted = 0;
                    tempDate = dispatchStartDt.AddDays(7);
                    while (noOfWeeksAdjusted < noOfWeeks)
                    {
                        int weeklyWoQuantity = balanceToManufacture / noOfWeeks;
                        while (!IsWorkDay(tempDate, holidaylist, weekOff1, weekOff2))
                        {
                            tempDate = tempDate.AddDays(1);
                        }
                        WorkOrdersVM wo = new WorkOrdersVM
                        {
                            WONumber = "",
                            CalcWOQty = weeklyWoQuantity,
                            PlanCompletionDate = tempDate
                        };
                        woDetails.Add(wo);
                        tempDate = tempDate.AddDays(7);
                        noOfWeeksAdjusted++;
                    }
                    //}
                    //else
                    //{
                    //    return BadRequest("Error: Number of work days is less than 22. Please use Manual Multiple selection.");
                    //}
                    break;
                case "Monthly":
                    if (workDays > 95)
                    {
                        int noOfMonthsAdjusted = 0;
                        tempDate = dispatchStartDt.AddMonths(1);
                        while (noOfMonthsAdjusted < noOfMonths)
                        {
                            int monthlyWoQuantity = balanceToManufacture / noOfMonths;

                            while (!IsWorkDay(tempDate, holidaylist, weekOff1, weekOff2))
                            {
                                tempDate = tempDate.AddDays(1);
                            }
                            WorkOrdersVM monthlywo = new WorkOrdersVM
                            {
                                WONumber = "",
                                CalcWOQty = monthlyWoQuantity,
                                PlanCompletionDate = tempDate
                            };
                            woDetails.Add(monthlywo);
                            tempDate = tempDate.AddMonths(1);
                            noOfMonthsAdjusted++;
                        }
                    }
                    else
                    {
                        return BadRequest("Error: Number of work days is less than 95. Please use Manual Multiple selection.");
                    }
                    break;
                default:
                    return BadRequest("Error: Invalid dispatch option.");
            }

            return Ok(woDetails);
        }

        private bool IsWorkDay(DateTime date, IEnumerable<HolidayVM> holidaylist, string weekOff1, string weekOff2)
        {
            // Check if the day is a holiday
            if (holidaylist.Any(h => h.HolidayDate == date))
            {
                return false;
            }

            // Check if the day is a weekly off
            var dayOfWeek = date.DayOfWeek.ToString();
            if (dayOfWeek == weekOff1 || dayOfWeek == weekOff2)
            {
                return false;
            }

            // If none of the above conditions are true, it's a work day
            return true;
        }

        bool IsNonWorkingDay(DateTime date, string weekOff1, string weekOff2, IEnumerable<HolidayVM> holidaylist)
        {
            DayOfWeek day = date.DayOfWeek;
            string dayName = date.DayOfWeek.ToString();

            //if (day == DayOfWeek.Saturday || day == DayOfWeek.Sunday)
            //{
            //    return true; // Weekend
            //}
            if (dayName == weekOff1 || dayName == weekOff2)
            {
                return true; // Weekoff1 or Weekoff2
            }
            else if (holidaylist.Any(h => h.HolidayDate == date.Date))
            {
                return true; // Holiday
            }
            return false; // Working day
        }

        [HttpGet]
        public async Task<IActionResult> Subcon(int routingId)
        {
            var result = await _routingService.RoutingSteps(routingId);

            var companies = await _masterService.GetCompanies();
            List<SubConDetailsVM> listsubcon = new List<SubConDetailsVM>();
            foreach (var item in result)
            {
                var subconwss = await _routingService.SubConWSS((int)item.StepId, -1);
                var subcons = await _routingService.SubCons((int)item.StepId);
                foreach (SubConDetailsVM obj in subcons)
                {
                    obj.StrPreferredSubCon = (obj.PreferredSubcon == 1) ? "Yes" : "";
                    foreach (ContactsVM mobj in companies)
                    {
                        if (obj.SupplierId == mobj.CompanyId)
                        {
                            obj.Company = mobj.CompanyName;
                        }
                    }
                    foreach (SubConWorkStepDetailsVM mobj in subconwss)
                    {
                        if (obj.SubConDetailsId == mobj.SubConDetailsId)
                        {
                            obj.NoOfOperations++;
                        }
                    }
                    listsubcon.Add(obj);
                }
            }
            return Ok(listsubcon);
        }
        [HttpGet]
        public async Task<IActionResult> GetManufPart(int routingId)
        {
            var manuf = await _masterService.GetManufPart(routingId);
            return Ok(manuf);
        }

        [HttpPost]
        public async Task<IActionResult> WoSubConSupplier(WoSubConSupplierVM production)
        {
            var procdutionpost = await _woService.PostSubConSupplier(production);
            return Ok(procdutionpost);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSubCons(int woid)
        {
            var procdutionpost = await _woService.GetAllSubCOnSupp();
            List<WoSubConSupplierVM> woSubs = new List<WoSubConSupplierVM>();
            var companies = await _masterService.GetCompanies();
            foreach (var item in procdutionpost)
            {
                foreach (ContactsVM mobj in companies)
                {
                    if (item.SupplierId == mobj.CompanyId)
                    {
                        item.Company = mobj.CompanyName;
                    }
                }
                item.AgreedDate = item.RecieptDate.ToString("dd-MM-yyyy");
                item.ProcPriceQnty = (Convert.ToDouble(item.ProcPrice) * Convert.ToDouble(item.Qnty)).ToString();
                if (item.WoId == woid)
                {
                    woSubs.Add(item);
                }
            }
            return Ok(woSubs);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPoSubCons(int woid)
        {
            var procdutionpost = await _woService.GetAllSubCOnSupp();
            List<WoSubConSupplierVM> woSubs = new List<WoSubConSupplierVM>();
            var companies = await _masterService.GetCompanies();
            foreach (var item in procdutionpost)
            {
                foreach (ContactsVM mobj in companies)
                {
                    if (item.SupplierId == mobj.CompanyId)
                    {
                        item.Company = mobj.CompanyName;
                    }
                }
                item.AgreedDate = item.RecieptDate.ToString("dd-MM-yyyy");
                item.ProcPriceQnty = (Convert.ToDouble(item.ProcPrice) * Convert.ToDouble(item.Qnty)).ToString();
                if (item.ProcPlanId == woid)
                {
                    woSubs.Add(item);
                }
            }
            return Ok(woSubs);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSubCon(long id)
        {
            var procdutionpost = await _woService.GetAllSubCOnSupp();
            var result = await _woService.DeleteSubCon(id);
            if(result == true)
            {
                var findsubcom = procdutionpost.Where(s => s.WoSubConSupplierId == id).FirstOrDefault();
                var wosubcon = procdutionpost.Where(s => s.WoId == findsubcom.WoId).ToList();
                var qnty = Convert.ToDouble(findsubcom.Qnty) / wosubcon.Count();
                foreach (var item in wosubcon)
                {
                    item.Qnty = (Convert.ToDouble(item.Qnty) + qnty).ToString();
                    var postsubcon = await _woService.PostSubConSupplier(item);
                }
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteWo(long id)
        {
            var result = await _woService.DeleteWo(id);
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetPartPurchase(long partId)
        {
            var submfpdList = await _masterService.PartPurchasesFor(Convert.ToInt32(partId));
            return Ok(submfpdList);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInWardDocLists()
        {
            var result = await _woService.GetAllInWardDocList();
            var doctype = await _docMangService.GetAllDocumentType();
            foreach (var item in result)
            {
                foreach (var doc in doctype)
                {
                    if (item.DocumentTypeId == doc.DocumentTypeId)
                    {
                        item.DocumentTypeName = doc.DocumentName;
                    }
                }
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostInw_Recpt_Part_No(Inw_Recpt_Part_NoVM masterDocListVM)
        {
            var result = await _woService.PostInw_Recpt_Part_No(masterDocListVM);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostInWardDocList(InwardDocTypeVM masterDocListVM)
        {
            var result = await _woService.PostInWardDocList(masterDocListVM);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteInWardDocList(long itemMasterDocListId)
        {
            var result = await _woService.DeleteInWardDocList(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInspectDocLists()
        {
            var result = await _woService.GetAllInspectDocList();
            var doctype = await _docMangService.GetAllDocumentType();
            foreach (var item in result)
            {
                foreach (var doc in doctype)
                {
                    if (item.DocumentTypeId == doc.DocumentTypeId)
                    {
                        item.DocumentTypeName = doc.DocumentName;
                    }
                }
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostInspectDocList(InspectDocTypeVM masterDocListVM)
        {
            var result = await _woService.PostInspectDocList(masterDocListVM);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteInspectDocList(long itemMasterDocListId)
        {
            var result = await _woService.DeleteInspectDocList(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLineDocLists()
        {
            var result = await _woService.GetAllLineDocList();
            var doctype = await _docMangService.GetAllDocumentType();
            foreach (var item in result)
            {
                foreach (var doc in doctype)
                {
                    if (item.DocumentTypeId == doc.DocumentTypeId)
                    {
                        item.DocumentTypeName = doc.DocumentName;
                    }
                }
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostLineDocList(LineInspectDocTypeVM masterDocListVM)
        {
            var result = await _woService.PostLineDocList(masterDocListVM);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteLineDocList(long itemMasterDocListId)
        {
            var result = await _woService.DeleteLineDocList(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllFinalDocLists()
        {
            var result = await _woService.GetAllFinalDocList();
            var doctype = await _docMangService.GetAllDocumentType();
            foreach (var item in result)
            {
                foreach (var doc in doctype)
                {
                    if (item.DocumentTypeId == doc.DocumentTypeId)
                    {
                        item.DocumentTypeName = doc.DocumentName;
                    }
                }
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostFinalDocList(FinalInspectDocTypeVM masterDocListVM)
        {
            var result = await _woService.PostFinalDocList(masterDocListVM);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteFinalDocList(long itemMasterDocListId)
        {
            var result = await _woService.DeleteFinalDocList(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCust_NC_Decs_Matrix()
        {
            var result = await _woService.GetAllCust_NC_Decs_Matrix();
            var desc = await _woService.GetAllNC_Disp_Decision_List();
            var opt = await _woService.GetAllCust_NC_Decs_Matrix_Opt();
            foreach (var item in result)
            {
                foreach (var o in opt)
                {
                    if(o.Cust_NC_Decs_Matrix_Id == item.Cust_NC_Decs_MatrixId)
                    {
                        foreach (var d in desc)
                        {
                            if (Convert.ToInt64(o.NC_Disp_Decision_Id) == d.NC_Disp_Decision_ListId)
                            {
                                item.NcDecision = item.NcDecision + " , " + d.NC_Disp_Decision_Desc;
                            }
                        }
                    }
                }
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostCust_NC_Decs_Matrix(Cust_NC_Decs_MatrixVM masterDocListVM)
        {
            var result = await _woService.PostCust_NC_Decs_Matrix(masterDocListVM);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteCust_NC_Decs_Matrix(long itemMasterDocListId)
        {
            var result = await _woService.DeleteCust_NC_Decs_Matrix(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCust_NC_Decs_Matrix_Opt()
        {
            var result = await _woService.GetAllCust_NC_Decs_Matrix_Opt();
            var desc = await _woService.GetAllNC_Disp_Decision_List();
            foreach (var item in result)
            {

                foreach (var d in desc)
                {
                    if (Convert.ToInt64(item.NC_Disp_Decision_Id) == d.NC_Disp_Decision_ListId)
                    {
                        item.NcDecision = item.NcDecision + " " + d.NC_Disp_Decision_Desc;
                    }
                }
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostCust_NC_Decs_Matrix_Opt(Cust_NC_Decs_Matrix_OptVM masterDocListVM)
        {
            var result = await _woService.PostCust_NC_Decs_Matrix_Opt(masterDocListVM);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteCust_NC_Decs_Matrix_Opt(long itemMasterDocListId)
        {
            var result = await _woService.DeleteCust_NC_Decs_Matrix_Opt(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRcaCaDocLists()
        {
            var result = await _woService.GetAllRcaCaDocList();
            var doctype = await _docMangService.GetAllDocumentType();
            foreach (var item in result)
            {
                foreach (var doc in doctype)
                {
                    if (item.DocumentTypeId == doc.DocumentTypeId)
                    {
                        item.DocumentTypeName = doc.DocumentName;
                    }
                }
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostRcaCaDocList(RcCaDocTypeVM masterDocListVM)
        {
            var result = await _woService.PostRcaCaDocList(masterDocListVM);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostNC_Decision_Log(NC_Decision_LogVM masterDocListVM)
        {
            var result = await _woService.PostNC_Decision_Log(masterDocListVM);
            if(result.NC_Disp_Deci_Lvl2_Id == 4 || result.NC_Disp_Deci_Lvl1_Id == 4)
            {
                var rwkadd = new Rwk_ListVM();
                rwkadd.NC_Log_Id = result.NcLogId;
                var postrwk = await _woService.PostRwk_List(rwkadd);
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNC_Decision_Log()
        {
            var result = await _woService.GetAllNC_Decision_Log();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCont_RCA_CA_log(Cont_RCA_CA_LogVM masterDocListVM)
        {
            var find = await _woService.GetAllCont_RCA_CA_log();
            var cont = find.Where(c => c.Cont_RCA_CA_LogId == masterDocListVM.Cont_RCA_CA_LogId).FirstOrDefault();
            cont.Cont_RCA_CA_Status_Id = masterDocListVM.Cont_RCA_CA_Status_Id;
            var result = await _woService.PostCont_RCA_CA_Log(cont);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateNclog(Insp_Outcome_DetailsVM masterDocListVM)
        {
            var find = await _woService.GetAllNcLog();
            var cont = find.Where(c => c.Insp_Outcome_Details_Id == masterDocListVM.Insp_Outcome_Details_Id).FirstOrDefault();
            cont.NC_Log_status_Id = masterDocListVM.NC_Log_status_Id;
            var result = await _woService.PostNcLog(cont);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostCont_RCA_CA_log(Cont_RCA_CA_LogVM masterDocListVM)
        {
            var result = await _woService.PostCont_RCA_CA_Log(masterDocListVM);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCont_RCA_CA_log()
        {
            var result = await _woService.GetAllCont_RCA_CA_log();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostInv_Trans_Log(Inv_Trans_LogVM masterDocListVM)
        {
            var result = await _woService.PostInv_Trans_Log(masterDocListVM);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateInv_Trans_Log(Inv_Trans_LogVM masterDocListVM)
        {
            var result = await _woService.GetAllInv_Trans_Log();
            var inv = result.Where(i => i.Inv_Trans_LogId == masterDocListVM.Inv_Trans_LogId).FirstOrDefault();
            inv.Movement_Compl = masterDocListVM.Movement_Compl;
            inv.Qnty_Mismatch = masterDocListVM.Qnty_Mismatch;
            inv.Qnty_mismatch_status = masterDocListVM.Qnty_mismatch_status;
            inv.Qnty_Mismatch_Comment = masterDocListVM.Qnty_Mismatch_Comment;
            var post = await _woService.PostInv_Trans_Log(inv);
            return Ok(post);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateMultipleInv_Trans_Log([FromBody] IEnumerable<Inv_Trans_LogVM>  masterDocListVM)
        {
            var result = await _woService.GetAllInv_Trans_Log();
            foreach (var item in masterDocListVM)
            {
                var inv = result.Where(i => i.Inv_Trans_LogId == item.Inv_Trans_LogId).FirstOrDefault();
                inv.Movement_Compl = item.Movement_Compl;
                inv.Qnty_Mismatch = item.Qnty_Mismatch;
                inv.Qnty_mismatch_status = item.Qnty_mismatch_status;
                inv.Qnty_Mismatch_Comment = item.Qnty_Mismatch_Comment;
                var post = await _woService.PostInv_Trans_Log(inv);

            }
            return Ok(masterDocListVM);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInv_Trans_Log()
        {
            var result = await _woService.GetAllInv_Trans_Log();
            var masterparts = await _masterService.ItemMasterParts();
            foreach (var item in result)
            {
                if(item.Output_Part_No == 0)
                {
                    item.Output_Part_No = item.Input_Part_NoId;
                }
                if(item.Output_Routing_Id == 0)
                {
                    item.Output_Routing_Id = item.Input_Routing_Id;
                }
                foreach (var m in masterparts)
                {
                    if (item.Output_Part_No == m.PartId)
                    {
                        item.PartNo = m.PartNo + " / " + m.Description;
                    }
                }
                ManufacturedPartNoDetailVM mf = await _masterService.GetManufPart(Convert.ToInt32(item.Output_Part_No));
                var resultList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                if(item.Output_Routing_Id != 0)
                {
                item.RoutingName = resultList.Where(r=>r.RoutingId == item.Output_Routing_Id).FirstOrDefault().RoutingName;
                var step = await _routingService.RoutingSteps((int)item.Output_Routing_Id);
                item.OprNo = step.FirstOrDefault().StepNumber;
                    if(step.FirstOrDefault().StepLocation == 1.ToString())
                    {
                        item.FromSender = "Inhouse";
                    }
                    else if (step.FirstOrDefault().StepLocation == 2.ToString())
                    {
                        item.FromSender = "SubCon";
                    }else
                    {
                        item.FromSender = "Company";
                    }
                }
                if(item.Part_Status == 1)
                {
                    item.PartStatus = "Accepted";
                }
                else if(item.Part_Status == 2)
                {
                    item.PartStatus = "Rework";
                }
                else
                {
                    item.PartStatus = "Rejected";
                }
                if(item.Qnty_mismatch_status == 1)
                {
                    item.MisMatchStatus = "None";
                }
                else if(item.Qnty_mismatch_status == 2)
                {
                    item.MisMatchStatus = "Open";
                }
                else if (item.Qnty_mismatch_status == 3)
                {
                    item.MisMatchStatus = "Corrected";
                }
                item.ToSender = "Stores";
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostInventory_Master(Inventory_MasterVM masterDocListVM)
        {
            var result = await _woService.PostInventory_Master(masterDocListVM);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInventory_Master()
        {
            var result = await _woService.GetAllInventory_Master();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostNC_Wk_List_Tmpl_Head(NC_Wk_List_Tmpl_HeadVM masterDocListVM)
        {
            var result = await _woService.PostNC_Wk_List_Tmpl_Head(masterDocListVM);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostNC_Wk_List_Tmpl_Det(NC_Wk_List_Tmpl_DetVM masterDocListVM)
        {
            var result = await _woService.PostNC_Wk_List_Tmpl_Det(masterDocListVM);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostNC_Disp_Decs_Appl_List(NC_Disp_Decs_Appl_ListVM masterDocListVM)
        {
            var result = await _woService.PostNC_Disp_Decs_Appl_List(masterDocListVM);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteNC_Wk_List_Tmpl_Head(long itemMasterDocListId)
        {
            var result = await _woService.DeleteNC_Wk_List_Tmpl_Head(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteNC_Wk_List_Tmpl_Det(long itemMasterDocListId)
        {
            var result = await _woService.DeleteNC_Wk_List_Tmpl_Det(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteRcaCaDocList(long itemMasterDocListId)
        {
            var result = await _woService.DeleteRcaCaDocList(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteInvMismatch(long itemMasterDocListId)
        {
            var result = await _woService.DeleteInvMismatch(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteNC_Disp_Decs_Appl_List(long itemMasterDocListId)
        {
            var result = await _woService.DeleteNC_Disp_Decs_Appl_List(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOperationSettings()
        {
            var result = await _woService.GetAllOperationSettings();
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNC_Wk_List_Tmpl_Head()
        {
            var result = await _woService.GetAllNC_Wk_List_Tmpl_Head();
            var desc = await _woService.GetAllNC_Disp_Decision_List();
            var ncdet = await _woService.GetAllNC_Wk_List_Tmpl_Det();
            foreach (var item in result)
            {
                foreach (var d in desc)
                {
                    if(item.NC_Disp_Decision_Id == d.NC_Disp_Decision_ListId)
                    {
                    item.NC_Disp_Decision_Name = d.NC_Disp_Decision_Desc;
                    }
                }
                item.No_of_steps = ncdet.Where(nc => nc.NC_Wk_List_Tmpl_Appl_Id == item.NC_Wk_List_Tmpl_HeadId).Count();
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNC_Wk_List_Tmpl_Det()
        {
            var result = await _woService.GetAllNC_Wk_List_Tmpl_Det();
            var dept = await _departmentService.GetDepartments(1);
            var uilist = await _employeeService.GetAllUilist();
            foreach (var item in result)
            {
                foreach (var dep in dept)
                {
                    if(item.Resp_Dept == dep.PlantId)
                    {
                        item.Resp_DeptName = dep.Name;
                    }
                }
                foreach (var ui in uilist)
                {
                    if(item.UI_ID == ui.UiListId)
                    {
                        item.UI_Name = ui.UI_Name_Label;
                    }
                }
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNC_Disp_Decision_List()
        {
            var result = await _woService.GetAllNC_Disp_Decision_List();
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNC_Disp_Decs_Appl_List()
        {
            var desc = await _woService.GetAllNC_Disp_Decision_List();
            var result = await _woService.GetAllNC_Disp_Decs_Appl_List();
            foreach (var item in result)
            {
                foreach (var d in desc)
                {
                    if (item.NC_Disp_Decision_Id == d.NC_Disp_Decision_ListId)
                    {
                        item.NC_Disp_Decision_Name = d.NC_Disp_Decision_Desc;
                    }
                }
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostOperationsSettings([FromBody] IEnumerable<OperationSettingsVM> masterDocListVM)
        {
            foreach (var item in masterDocListVM)
            {
                var result = await _woService.PostOperationsSettings(item);
            }
            return Ok(masterDocListVM);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInw_Recpt_HeaderInsp()
        {
            // Fetch all data in parallel to reduce I/O wait time
            var podetailsTask = _woService.GetAllPodetails();
            var procPlanTask = _woService.GetAllProcPlan();
            var inwHeaderTask = _woService.GetAllInw_Recpt_Header();
            var companiesTask = _masterService.GetCompanies();
            var masterPartsTask = _masterService.ItemMasterParts();
            var uomsTask = _masterService.GetUOMs();
            var wosTask = _woService.AllProductionPlan_Wo();

            await Task.WhenAll(podetailsTask, procPlanTask, inwHeaderTask,
                               companiesTask, masterPartsTask, uomsTask, wosTask);

            var podetails = podetailsTask.Result;
            var procPlans = procPlanTask.Result;
            var inwHeaders = inwHeaderTask.Result;
            var companies = companiesTask.Result;
            var masterParts = masterPartsTask.Result;
            var uoms = uomsTask.Result;
            var wos = wosTask.Result;

            // Create dictionaries for O(1) lookups instead of nested loops
            var companyDict = companies.ToDictionary(c => c.CompanyId, c => c.CompanyName);
            var partDict = masterParts.ToDictionary(p => p.PartId, p => p);
            var uomDict = uoms.ToDictionary(u => u.UOMId, u => u.Name);
            var procPlanDict = procPlans.ToDictionary(p => p.ProcPlanId, p => p);
            var woDict = wos.ToDictionary(w => w.ProductionPlanId, w => w.WoId);
            var inwDict = inwHeaders.ToDictionary(i => i.PoHeaderId, i => i);

            List<PODetailsVM> woSubs = new List<PODetailsVM>();

            foreach (var item in podetails)
            {
                // Supplier
                if (companyDict.TryGetValue(item.CompanyId, out var companyName))
                    item.Supplier = companyName;

                // Date formatting
                item.DateStr = item.PlanPoReceiptDate.ToString("dd-MM-yyyy");
                item.NoOfLine = "1";
                item.NoOfOpenLine = "1";
                item.NoOfPastLine = "0";

                // Status
                item.StatusStr = item.Status switch
                {
                    1 => "Not Approved",
                    2 => "PO Approved",
                    _ => "Complete"
                };

                item.PoType = "Prodn";

                // Part Info
                if (partDict.TryGetValue(item.PartId, out var part))
                {
                    item.PartNo = $"{part.PartNo} / {part.Description}";
                    item.PartType = part.MasterPartType == "ManufacturedPart" ? "SubCon" : part.MasterPartType;

                    // ⚡ Instead of await per item, batch-load part purchases (if possible)
                    // var mp = await _masterService.PartPurchasesFor((int)item.PartId);
                    // item.ProcPrice = mp.FirstOrDefault()?.Price;
                }

                // UOM (hardcoded to 1 in your code)
                if (uomDict.TryGetValue(1, out var uomName))
                    item.Unit = uomName;

                // ProcPlan check
                if (procPlanDict.TryGetValue(item.ProcPlanId, out var proc))
                {
                    if (item.PoQnty < proc.Plan_Proc_Qnty)
                        item.QntyRed = "Y";

                    if (item.PlanPoReceiptDate < proc.PlanReceiptDate)
                        item.DateRed = "Y";

                    if (woDict.TryGetValue(proc.WorkOrderId, out var woId))
                        item.WoId = woId;
                }

                // Inward Receipt check
                if (inwDict.TryGetValue(item.PoDetailsId, out var inw))
                {
                    item.InwHeaderId = inw.Inw_Recpt_HeaderId;
                    item.InwDate = inw.Inw_Date_time.ToString("dd/MM/yyyy");
                    woSubs.Add(item);
                }
            }

            return Ok(woSubs);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInward_Condn_list()
        {
            var result = await _woService.GetAllInward_Condn_list();
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSetupVariationReason()
        {
            var result = await _woService.GetAllSetupVariationReason();
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCust_NC_Decision()
        {
            var result = await _woService.GetAllCust_NC_Decision();
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInw_Recpt_Header()
        {
            var result = await _woService.GetAllInw_Recpt_Header();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostInw_Recpt_Header([FromBody] Inw_Recpt_HeaderVM procPlanVMs)
        {
            var result = await _woService.PostInw_Recpt_Header(procPlanVMs);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostInv_Mismatch_List([FromBody] Inv_Mismatch_ListVM procPlanVMs)
        {
            var result = await _woService.PostInv_Mismatch_List(procPlanVMs);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostSetupVariationReason(SetupVariationReasonVM procPlanVMs)
        {
            var result = await _woService.PostSetupVariationReason(procPlanVMs);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInw_Recpt_Details()
        {
            var result = await _woService.GetAllInw_Recpt_Details();
            var masterparts = await _masterService.ItemMasterParts();
            var condition = await _woService.GetAllInward_Condn_list();
            foreach (var item in result)
            {
                foreach (var m in masterparts)
                {
                    if (item.Inw_Recpt_Part_No_Id == m.PartId)
                    {
                        item.Inw_Recpt_Part_No_Name = m.PartNo + " / " + m.Description;
                    }
                }
                foreach (var con in condition)
                {
                    if (item.Inward_Condition == con.Inward_Condn_listId)
                    {
                        item.Inward_ConditionName = con.Inward_Condn_desc;
                    }
                }
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNcLogInsp()
        {
            var result = await _woService.GetAllNcLog();
            List<Insp_Outcome_DetailsVM> listnc = new List<Insp_Outcome_DetailsVM>();
            var masterparts = await _masterService.ItemMasterParts();
            var rcclog = await _woService.GetAllCont_RCA_CA_log();
            var ncdisplog = await _woService.GetAllNC_Decision_Log();
            var inw = await _woService.GetAllInw_Recpt_Details();
            foreach (var item in result)
            {
                foreach (var m in masterparts)
                {
                    if (item.Inw_Recpt_Part_No_Id == m.PartId)
                    {
                        item.Inw_Recpt_Part_No_Name = m.PartNo + " / " + m.Description;
                        if (m.MasterPartType == "ManufacturedPart")
                        {
                            item.PartType = "SubCon";
                        }
                        else
                        {
                            item.PartType = m.MasterPartType;
                        }
                    }
                }
                item.Unit = "Nos";
                item.NcDateStr = item.NcDate.ToString("dd/MM/yyyy");

                if (Convert.ToInt32(item.Storage_Location) == 1)
                {
                    item.LocationName = "Shop";
                }
                else
                {
                    item.LocationName = "Stores";
                }
                DateTime today = DateTime.Today;
                int daysElapsed = (today - item.NcDate).Days;
                item.NoOfDays = daysElapsed;
                foreach (var nc in rcclog)
                {
                    if (rcclog.Any(r => r.NcLogId == item.Insp_Outcome_Details_Id))
                    {
                        item.WfCustFeedBack = "N";
                        item.RcaStatus = "Uploaded";
                        foreach (var displog in ncdisplog)
                        {
                            if (nc.Cont_RCA_CA_Status_Id == 3 && nc.NcLogId == item.Insp_Outcome_Details_Id)
                            {
                                item.LvlAppro = "Y";
                                item.Lvldesc = "Done";
                            }
                            else
                            {
                                item.LvlAppro = "-";
                                item.Lvldesc = "Y";
                            }
                        }
                    }
                    else
                    {
                        item.RcaStatus = "Not Uploaded";
                        item.WfCustFeedBack = "Y";
                        item.LvlAppro = "-";
                        item.Lvldesc = "-";
                    }
                }
                //foreach (var inward in inw)
                //{
                //    if (inward.Inw_Recpt_DetailsId == item.Inw_Recpt_Header_Id && inward.Inward_Condition == 2)
                //    {
                        listnc.Add(item);
                //    }
                //}
            }
            return Ok(listnc);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNcLog()
        {
            var result = await _woService.GetAllNcLog();
            List<Insp_Outcome_DetailsVM> listnc = new List<Insp_Outcome_DetailsVM>();
            var masterparts = await _masterService.ItemMasterParts();
            var loc = await _departmentService.GetDepartments(1);
            var rcclog = await _woService.GetAllCont_RCA_CA_log();
            var ncdisplog = await _woService.GetAllNC_Decision_Log();
            var inw = await _woService.GetAllInw_Recpt_Details();
            foreach (var item in result)
            {
                foreach (var m in masterparts)
                {
                    if (item.Inw_Recpt_Part_No_Id == m.PartId)
                    {
                        item.Inw_Recpt_Part_No_Name = m.PartNo+ " / " +m.Description;
                        if (m.MasterPartType == "ManufacturedPart")
                        {
                            item.PartType = "SubCon";
                        }
                        else
                        {
                            item.PartType = m.MasterPartType;
                        }
                    }
                }
                item.Unit = "Nos";
                item.NcDateStr = item.NcDate.ToString("dd/MM/yyyy");
                
                if (Convert.ToInt32(item.Storage_Location) == 1)
                {
                    item.LocationName = "Shop";
                }
                else if (Convert.ToInt32(item.Storage_Location) == 3)
                {
                    item.LocationName = "Customers";
                }
                else
                {
                    item.LocationName = "Stores";
                }
                DateTime today = DateTime.Today;
                int daysElapsed = (today - item.NcDate).Days;
                item.NoOfDays = daysElapsed;
                foreach (var nc in rcclog)
                {
                    if (rcclog.Any(r=>r.NcLogId == item.Insp_Outcome_Details_Id))
                    {
                        item.WfCustFeedBack = "N";
                        item.RcaStatus = "Uploaded";
                        foreach (var displog in ncdisplog)
                        {
                            if (nc.Cont_RCA_CA_Status_Id == 3 && nc.NcLogId == item.Insp_Outcome_Details_Id)
                            {
                                item.LvlAppro = "Y";
                                item.Lvldesc = "Done";
                            }
                            else
                            {
                                item.LvlAppro = "-";
                                item.Lvldesc = "Y";
                            }
                        }
                    }
                    else
                    {
                        item.RcaStatus = "Not Uploaded";
                        item.WfCustFeedBack = "Y";
                        item.LvlAppro = "-";
                        item.Lvldesc = "-";
                    }
                }
                foreach (var inward in inw)
                {
                    if(inward.Inw_Recpt_DetailsId == item.Inw_Recpt_Header_Id && inward.Inward_Condition == 2)
                    {
                    }
                }
                listnc.Add(item);
            }
            return Ok(listnc);
        }
        [HttpPost]
        public async Task<IActionResult> PostInw_Recpt_Details([FromBody] Inw_Recpt_DetailsVM procPlanVMs)
        {
            var result = await _woService.PostInw_Recpt_Details(procPlanVMs);
            return Ok(result);
        }  
        [HttpPost]
        public async Task<IActionResult> PostInspNcLog([FromBody]Insp_Outcome_DetailsVM procPlanVMs)
        {
            var result = await _woService.PostNcLog(procPlanVMs);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostNcLog(Insp_Outcome_DetailsVM procPlanVMs)
        {
            var result = await _woService.PostNcLog(procPlanVMs);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> InwardDoclist(long poheaderid)
        {
            var result = await _woService.GetAllInWardDocList();
            var docListVMs = await _docMangService.GetAllDocList();
            var doctype = await _docMangService.GetAllDocumentType();
            var custRetnDataVMs = await _docMangService.GetAllCustRet();
            var companies = await _masterService.GetCompanies();
            var extns = await _docMangService.GetAllFileExtn();

            List<DocListVM> docListVM = new List<DocListVM>();
            List<DocListVM> empdocLists = new List<DocListVM>();

            if (poheaderid == 0)
            {
                foreach (var master in result)
                {
                    DocListVM empDoc = new DocListVM();
                    PopulateItemFields(empDoc, master, doctype, extns, custRetnDataVMs, companies);
                    empDoc.Comments = string.Empty;
                    empDoc.DeletionDate = DateTime.Now;
                    empdocLists.Add(empDoc);
                }
                return Ok(empdocLists);
            }
            else
            {
                bool hasEntries = false;
                foreach (var item in docListVMs.Where(d => d.Inw_Recpt_HeaderId == poheaderid && result.Any()))
                {
                    PopulateItemFields(item, result, doctype, extns, custRetnDataVMs, companies);
                    ClaimsPrincipal userClaim = HttpContext.User;
                    string fullName = AppUtil.GetFullName(userClaim);
                    item.UpdatedOnStr = item.CreationDt.ToString("MM-dd-yyyy");
                    item.UploadedBy = fullName;
                    var getdoc = await _docMangService.GetDoc_Status_List(item.AppvStatus);
                    item.DocStatus = getdoc.Doc_Status_Desc;
                    if (item.DocCat == 1)
                    {
                        if (item.Approved_by > 0)
                        {
                            item.ApprovedOnStr = item.Appv_Date_time.ToString("MM-dd-yyyy");
                            item.ApprovedByStr = fullName;
                        }
                    }
                    else
                    {
                        item.ApprovedOnStr = "";
                        item.ApprovedByStr = "";
                    }
                    docListVM.Add(item);
                    hasEntries = true;
                }

                // If there are document types related to contentid without entries in docListVMs, add to empdocLists
                if (!hasEntries || docListVM.Count < result.Count())
                {
                    foreach (var master in result.Where(m => !docListVM.Any(d => d.DocumentTypeId == m.DocumentTypeId)))
                    {
                        DocListVM empDoc = new DocListVM();
                        PopulateItemFields(empDoc, master, doctype, extns, custRetnDataVMs, companies);
                        empDoc.DeletionDate = DateTime.Now;
                        empDoc.Comments = string.Empty;
                        empdocLists.Add(empDoc);
                    }
                }

                // Return combination of docListVM and empdocLists when poheaderid != 0
                return Ok(docListVM.Concat(empdocLists).ToList());
            }
        }

        void PopulateItemFields(DocListVM item, dynamic master, IEnumerable<DocumentTypeVM> doctype,
                         IEnumerable<ExtnInfoVM> extns, IEnumerable<CustRetnDataVM> custRetnDataVMs,
                         IEnumerable<ContactsVM> companies)
        {
            // Determine if master is an array or a single object
            var masterObj = (master is IEnumerable<dynamic> && master.Count > 0) ? master[0] : master;

            if (masterObj?.DocumentTypeId != null)
            {
                var doc = doctype.FirstOrDefault(d => d.DocumentTypeId == masterObj.DocumentTypeId);
                if (doc != null)
                {
                    item.DocumentTypeName = doc.DocumentName;
                    item.DocumentTypeId = doc.DocumentTypeId;
                    item.DataReqdByCust = doc.DataReqdByCust;
                    item.DocCat = doc.DocuCategory;
                    item.Mandatory = masterObj.Mandatory;
                    var extn = extns.FirstOrDefault(e => e.ExtnId == doc.ExtnId);
                    item.FileExtnName = extn?.ExtnName;

                    // Calculate DeletionDate if required
                    if (doc.DefaultRetPerYear != 0 && doc.DefaultRetPerMon != 0)
                    {
                        DateTime currentDate = DateTime.Now;
                        DateTime futureDate = currentDate.AddYears(doc.DefaultRetPerYear).AddMonths(doc.DefaultRetPerMon);
                        item.DeletionDate = futureDate;
                    }
                }

                var cust = custRetnDataVMs.FirstOrDefault(c => c.DocumentTypeId == masterObj.DocumentTypeId);
                if (cust != null)
                {
                    var comp = companies.FirstOrDefault(c => c.CompanyId == cust.ComapanyId);
                    item.CompanyName = comp?.CompanyName;
                }
            }
        }


        [HttpGet]
        public async Task<IActionResult> InwardDetailsDoclist(long podetailsId)
        {
            var result = await _woService.GetAllInWardDocList();
            var docListVMs = await _docMangService.GetAllDocList();
            var doctype = await _docMangService.GetAllDocumentType();
            var custRetnDataVMs = await _docMangService.GetAllCustRet();
            var companies = await _masterService.GetCompanies();
            var extns = await _docMangService.GetAllFileExtn();

            List<DocListVM> docListVM = new List<DocListVM>();
            List<DocListVM> empdocLists = new List<DocListVM>();

            if (podetailsId == 0)
            {
                foreach (var master in result)
                {
                    DocListVM empDoc = new DocListVM();
                    PopulateItemFieldsInwardDetails(empDoc, master, doctype, extns, custRetnDataVMs, companies);
                    empDoc.Comments = string.Empty;
                    empDoc.DeletionDate = DateTime.Now;
                    empdocLists.Add(empDoc);
                }
                return Ok(empdocLists);
            }
            else
            {
                bool hasEntries = false;
                foreach (var item in docListVMs.Where(d => d.Inw_Recpt_DetailsId == podetailsId && result.Any()))
                {
                    PopulateItemFieldsInwardDetails(item, result, doctype, extns, custRetnDataVMs, companies);
                    ClaimsPrincipal userClaim = HttpContext.User;
                    string fullName = AppUtil.GetFullName(userClaim);
                    item.UpdatedOnStr = item.CreationDt.ToString("MM-dd-yyyy");
                    item.UploadedBy = fullName;
                    var getdoc = await _docMangService.GetDoc_Status_List(item.AppvStatus);
                    item.DocStatus = getdoc.Doc_Status_Desc;
                    if (item.DocCat == 1)
                    {
                        if (item.Approved_by > 0)
                        {
                            item.ApprovedOnStr = item.Appv_Date_time.ToString("MM-dd-yyyy");
                            item.ApprovedByStr = fullName;
                        }
                    }
                    else
                    {
                        item.ApprovedOnStr = "";
                        item.ApprovedByStr = "";
                    }
                    docListVM.Add(item);
                    hasEntries = true;
                }

                // If there are document types related to contentid without entries in docListVMs, add to empdocLists
                if (!hasEntries || docListVM.Count < result.Count())
                {
                    foreach (var master in result.Where(m => !docListVM.Any(d => d.DocumentTypeId == m.DocumentTypeId)))
                    {
                        DocListVM empDoc = new DocListVM();
                        PopulateItemFieldsInwardDetails(empDoc, master, doctype, extns, custRetnDataVMs, companies);
                        empDoc.DeletionDate = DateTime.Now;
                        empDoc.Comments = string.Empty;
                        empdocLists.Add(empDoc);
                    }
                }

                return Ok(docListVM.Concat(empdocLists).ToList());
            }
        }
        void PopulateItemFieldsInwardDetails(DocListVM item, dynamic master, IEnumerable<DocumentTypeVM> doctype,
                                IEnumerable<ExtnInfoVM> extns, IEnumerable<CustRetnDataVM> custRetnDataVMs,
                                IEnumerable<ContactsVM> companies)
        {
            var masterObj = (master is IEnumerable<dynamic> && master.Count > 0) ? master[0] : master;

            if (masterObj?.DocumentTypeId != null)
            {
                var doc = doctype.FirstOrDefault(d => d.DocumentTypeId == masterObj.DocumentTypeId);
                if (doc != null)
                {
                    item.DocumentTypeName = doc.DocumentName;
                    item.DocumentTypeId = doc.DocumentTypeId;
                    item.DataReqdByCust = doc.DataReqdByCust;
                    item.DocCat = doc.DocuCategory;
                    item.Mandatory = masterObj.Mandatory;
                    var extn = extns.FirstOrDefault(e => e.ExtnId == doc.ExtnId);
                    item.FileExtnName = extn?.ExtnName;

                    // Calculate DeletionDate if required
                    if (doc.DefaultRetPerYear != 0 && doc.DefaultRetPerMon != 0)
                    {
                        DateTime currentDate = DateTime.Now;
                        DateTime futureDate = currentDate.AddYears(doc.DefaultRetPerYear).AddMonths(doc.DefaultRetPerMon);
                        item.DeletionDate = futureDate;
                    }
                }

                var cust = custRetnDataVMs.FirstOrDefault(c => c.DocumentTypeId == masterObj.DocumentTypeId);
                if (cust != null)
                {
                    var comp = companies.FirstOrDefault(c => c.CompanyId == cust.ComapanyId);
                    item.CompanyName = comp?.CompanyName;
                }
            }
        }


        [HttpGet]
        public async Task<IActionResult> NcDoclist(long nclogId)
        {
            var result = await _woService.GetAllInspectDocList();
            var docListVMs = await _docMangService.GetAllDocList();
            var doctype = await _docMangService.GetAllDocumentType();
            var custRetnDataVMs = await _docMangService.GetAllCustRet();
            var companies = await _masterService.GetCompanies();
            var extns = await _docMangService.GetAllFileExtn();

            List<DocListVM> docListVM = new List<DocListVM>();
            List<DocListVM> empdocLists = new List<DocListVM>();

            if (nclogId == 0)
            {
                foreach (var master in result)
                {
                    DocListVM empDoc = new DocListVM();
                    PopulateItemFieldsInspectionDetails(empDoc, master, doctype, extns, custRetnDataVMs, companies);
                    empDoc.Comments = string.Empty;
                    empDoc.DeletionDate = DateTime.Now;
                    empdocLists.Add(empDoc);
                }
                return Ok(empdocLists);
            }
            else
            {
                bool hasEntries = false;
                foreach (var item in docListVMs.Where(d => d.NcLogIdId == nclogId && result.Any()))
                {
                    PopulateItemFieldsInspectionDetails(item, result, doctype, extns, custRetnDataVMs, companies);
                    ClaimsPrincipal userClaim = HttpContext.User;
                    string fullName = AppUtil.GetFullName(userClaim);
                    item.UpdatedOnStr = item.CreationDt.ToString("MM-dd-yyyy");
                    item.UploadedBy = fullName;
                    var getdoc = await _docMangService.GetDoc_Status_List(item.AppvStatus);
                    item.DocStatus = getdoc.Doc_Status_Desc;
                    if (item.DocCat == 1)
                    {
                        if (item.Approved_by > 0)
                        {
                            item.ApprovedOnStr = item.Appv_Date_time.ToString("MM-dd-yyyy");
                            item.ApprovedByStr = fullName;
                        }
                    }
                    else
                    {
                        item.ApprovedOnStr = "";
                        item.ApprovedByStr = "";
                    }
                    docListVM.Add(item);
                    hasEntries = true;
                }

                // If there are document types related to contentid without entries in docListVMs, add to empdocLists
                if (!hasEntries || docListVM.Count < result.Count())
                {
                    foreach (var master in result.Where(m => !docListVM.Any(d => d.DocumentTypeId == m.DocumentTypeId)))
                    {
                        DocListVM empDoc = new DocListVM();
                        PopulateItemFieldsInspectionDetails(empDoc, master, doctype, extns, custRetnDataVMs, companies);
                        empDoc.DeletionDate = DateTime.Now;
                        empDoc.Comments = string.Empty;
                        empdocLists.Add(empDoc);
                    }
                }

                return Ok(docListVM.Concat(empdocLists).ToList());
            }
        }
        [HttpGet]
        public async Task<IActionResult> InspectionDetailsDoclist(long podetailsId)
        {
            var result = await _woService.GetAllInspectDocList();
            var docListVMs = await _docMangService.GetAllDocList();
            var doctype = await _docMangService.GetAllDocumentType();
            var custRetnDataVMs = await _docMangService.GetAllCustRet();
            var companies = await _masterService.GetCompanies();
            var extns = await _docMangService.GetAllFileExtn();

            List<DocListVM> docListVM = new List<DocListVM>();
            List<DocListVM> empdocLists = new List<DocListVM>();

            if (podetailsId == 0)
            {
                foreach (var master in result)
                {
                    DocListVM empDoc = new DocListVM();
                    PopulateItemFieldsInspectionDetails(empDoc, master, doctype, extns, custRetnDataVMs, companies);
                    empDoc.Comments = string.Empty;
                    empDoc.DeletionDate = DateTime.Now;
                    empdocLists.Add(empDoc);
                }
                return Ok(empdocLists);
            }
            else
            {
                bool hasEntries = false;
                foreach (var item in docListVMs.Where(d => d.Inw_Recpt_HeaderId == podetailsId && result.Any()))
                {
                    PopulateItemFieldsInspectionDetails(item, result, doctype, extns, custRetnDataVMs, companies);
                    ClaimsPrincipal userClaim = HttpContext.User;
                    string fullName = AppUtil.GetFullName(userClaim);
                    item.UpdatedOnStr = item.CreationDt.ToString("MM-dd-yyyy");
                    item.UploadedBy = fullName;
                    var getdoc = await _docMangService.GetDoc_Status_List(item.AppvStatus);
                    item.DocStatus = getdoc.Doc_Status_Desc;
                    if (item.DocCat == 1)
                    {
                        if (item.Approved_by > 0)
                        {
                            item.ApprovedOnStr = item.Appv_Date_time.ToString("MM-dd-yyyy");
                            item.ApprovedByStr = fullName;
                        }
                    }
                    else
                    {
                        item.ApprovedOnStr = "";
                        item.ApprovedByStr = "";
                    }
                    docListVM.Add(item);
                    hasEntries = true;
                }

                // If there are document types related to contentid without entries in docListVMs, add to empdocLists
                if (!hasEntries || docListVM.Count < result.Count())
                {
                    foreach (var master in result.Where(m => !docListVM.Any(d => d.DocumentTypeId == m.DocumentTypeId)))
                    {
                        DocListVM empDoc = new DocListVM();
                        PopulateItemFieldsInspectionDetails(empDoc, master, doctype, extns, custRetnDataVMs, companies);
                        empDoc.DeletionDate = DateTime.Now;
                        empDoc.Comments = string.Empty;
                        empdocLists.Add(empDoc);
                    }
                }

                return Ok(docListVM.Concat(empdocLists).ToList());
            }
        }
        void PopulateItemFieldsInspectionDetails(DocListVM item, dynamic master, IEnumerable<DocumentTypeVM> doctype,
                                IEnumerable<ExtnInfoVM> extns, IEnumerable<CustRetnDataVM> custRetnDataVMs,
                                IEnumerable<ContactsVM> companies)
        {
            var masterObj = (master is IEnumerable<dynamic> && master.Count > 0) ? master[0] : master;

            if (masterObj?.DocumentTypeId != null)
            {
                var doc = doctype.FirstOrDefault(d => d.DocumentTypeId == masterObj.DocumentTypeId);
                if (doc != null)
                {
                    item.DocumentTypeName = doc.DocumentName;
                    item.DocumentTypeId = doc.DocumentTypeId;
                    item.DataReqdByCust = doc.DataReqdByCust;
                    item.DocCat = doc.DocuCategory;
                    item.Mandatory = masterObj.Mandatory;
                    var extn = extns.FirstOrDefault(e => e.ExtnId == doc.ExtnId);
                    item.FileExtnName = extn?.ExtnName;

                    if (doc.DefaultRetPerYear != 0 && doc.DefaultRetPerMon != 0)
                    {
                        DateTime currentDate = DateTime.Now;
                        DateTime futureDate = currentDate.AddYears(doc.DefaultRetPerYear).AddMonths(doc.DefaultRetPerMon);
                        item.DeletionDate = futureDate;
                    }
                }

                var cust = custRetnDataVMs.FirstOrDefault(c => c.DocumentTypeId == masterObj.DocumentTypeId);
                if (cust != null)
                {
                    var comp = companies.FirstOrDefault(c => c.CompanyId == cust.ComapanyId);
                    item.CompanyName = comp?.CompanyName;
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWO_Wait_List()
        {
            var result = await _woService.GetAllWO_Wait_List();
            var prodnWos = await _woService.AllProductionPlan_Wo();
            var masterparts = await _masterService.ItemMasterParts();
            var customer = await _baService.GetCustomerOrders();
            var nclogs = await _woService.GetAllNcLog();
            foreach (var item in result)
            {
                var prodwo = prodnWos.Where(w => w.ProductionPlanId == item.Wo_Id).FirstOrDefault();
                item.WoNumber = prodwo?.WONumber;
                foreach (ItemMasterPartVM imp in masterparts)
                {
                    if (prodwo.PartId == imp.PartId)
                    {
                        item.Part = imp.PartNo + " / "+ imp.Description;
                    }
                    if(prodwo.PartId == 2)
                    {
                        item.PartType = "Assembly";
                    }
                    else
                    {
                        item.PartType = "Child Part";
                    }
                }
                var so = await _baService.GetOneSO(prodwo.SalesOrderId);
                if (so != null)
                {
                    foreach (CustomerOrderVM cu in customer)
                    {
                        if (so.CustomerOrderId == cu.CustomerOrderId)
                        {
                            item.Customer = cu.CustomerName;
                        }
                    }
                }
                else
                {
                    var wosos = await _woService.GetSoWoRel(prodwo.WoId);
                    foreach (var woso in wosos)
                    {
                        var sos = await _baService.GetOneSO(woso.SalesOrderId);
                        foreach (CustomerOrderVM cu in customer)
                        {
                            if (sos.CustomerOrderId == cu.CustomerOrderId)
                            {
                                item.Customer = cu.CustomerName;
                            }
                        }
                    }
                }
                item.ModeName = "Test";
                item.Tpt = item.Total_TPT.ToString();
                item.PlanStartStr = item.Plan_Start_Date.ToString("dd-MM-yyyy hh:mm tt"); ;
                item.PlanEndStr = item.Plan_End_Date.ToString("dd-MM-yyyy hh:mm tt"); ;
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostWO_Wait_List(WO_Wait_ListVM masterDocListVM)
        {
            var result = await _woService.PostWO_Wait_List(masterDocListVM);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteWO_Wait_List(long itemMasterDocListId)
        {
            var result = await _woService.DeleteWO_Wait_List(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMc_not_avl_reason()
        {
            var result = await _woService.GetAllMc_not_avl_reason();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostMc_not_avl_reason(Mc_Not_Avl_ReasonVM masterDocListVM)
        {
            var result = await _woService.PostMc_not_avl_reason(masterDocListVM);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTimeslot_Setting()
        {
            var result = await _woService.GetAllTimeslot_Setting();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostTimeslot_Setting(Timeslot_SettingVM masterDocListVM)
        {
            var result = await _woService.PostTimeslot_Setting(masterDocListVM);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteTimeslot_Setting(long itemMasterDocListId)
        {
            var result = await _woService.DeleteTimeslot_Setting(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTimeslot_List()
        {
            var result = await _woService.GetAllTimeslot_List();
            return Ok(result);
        }
        //[HttpPost]
        //public async Task<IActionResult> PostTimeslot_List()
        //{
        //    var result = await _plantService.GetPlants();
        //    var alltimeslotsettings = await _woService.GetAllTimeslot_Setting();
        //    var timeslotsettings = alltimeslotsettings.LastOrDefault(); // get latest config
        //    int totalDays = timeslotsettings.No_of_span_days;
        //    int slotDurationMins = timeslotsettings.Timeslot_duration;

        //    foreach (var plant in result)
        //    {
        //        var holidayList = await _plantService.GetHolidays(plant.PlantId); // List<DateTime>
        //        var wd = await _plantService.GetPlantWD(plant.PlantId);

        //        plant.NoOfShifts = wd.NoOfShifts;
        //        plant.WeeklyOff1 = wd.WeeklyOff1 ?? " ";
        //        plant.FirstShiftStartTime = wd.FirstShiftStartTime ?? " ";

        //        DateTime currentDate = DateTime.Today;

        //        for (int d = 0; d < totalDays; d++)
        //        {
        //            var day = currentDate.AddDays(d);

        //            // Skip weekly offs
        //            if (wd.WeeklyOff1 != null && day.DayOfWeek.ToString() == wd.WeeklyOff1)
        //                continue;

        //            // Skip holidays
        //            if (holidayList.Any(h => h.HolidayDate == day.Date))
        //                continue;

        //            if (!TimeSpan.TryParse(wd.FirstShiftStartTime, out TimeSpan shiftStart))
        //                continue;

        //            DateTime shiftStartTime = day.Date.Add(shiftStart);
        //            int totalMinutes = wd.NoOfShifts * 8 * 60; 
        //            int slotCount = totalMinutes / slotDurationMins;

        //            for (int s = 0; s < slotCount; s++)
        //            {
        //                DateTime startTime = shiftStartTime.AddMinutes(s * slotDurationMins);
        //                DateTime endTime = startTime.AddMinutes(slotDurationMins);

        //                // Check if this time falls in break (pseudo example)
        //                bool isBreak = IsBreakTime(startTime);

        //                var timeslot = new Timeslot_ListVM
        //                {
        //                    Start_time = startTime,
        //                    End_time = endTime,
        //                    Break_Slot = isBreak ? 'Y' : 'N'
        //                };

        //                await _woService.PostTimeslot_List(timeslot);
        //            }
        //        }
        //    }
        //    return Ok(result);
        //}
        [HttpPost]
        public async Task<IActionResult> PostTimeslot_List()
        {
            var result = await _plantService.GetPlants();

            foreach (var plant in result)
            {
                var holiday = await _plantService.GetHolidays(plant.PlantId); 
                var holidayList = holiday.Select(h=>h.HolidayDate).ToList(); // List<DateTime>
                var wd = await _plantService.GetPlantWD(plant.PlantId);

                plant.NoOfShifts = wd.NoOfShifts;
                plant.WeeklyOff1 = wd.WeeklyOff1 ?? " ";
                plant.FirstShiftStartTime = wd.FirstShiftStartTime ?? " ";

                var changeFlag = plant.Change_flag == 'Y' || wd.Change_flag == 'Y';
                var spanDays = wd.No_of_span_days;
                var slotDuration = wd.Timeslot_duration; // in minutes
                var retentionDays = wd.Retention_Days;

                var existingSlots = (await _woService.GetAllTimeslot_List())
                    .Where(t => t.PlantId == plant.PlantId)
                    .ToList();
                var today = DateTime.Today;
                var lastDate = existingSlots.OrderByDescending(t => t.Start_time).FirstOrDefault()?.Start_time ?? today;

                if (changeFlag)
                {
                    // Remove future timeslots
                    //await RemoveTimeslotsBefore(plant.PlantId, today);

                    // Recreate timeslots from today for spanDays
                    await GenerateTimeslots(plant, wd, today, today.AddDays(spanDays - 1), holidayList);
                    plant.Change_flag = 'N';
                    wd.Change_flag = 'N';
                    var plantupdate =await _plantService.PostPlant(plant);
                    var plantdetailsUpdate =await _plantService.PostPlantWD(wd);
                }
                else
                {
                    // Add 1 extra future working day
                    var nextWorking = await GetNextWorkingDate(wd, lastDate.AddDays(1), holidayList);
                    if (nextWorking != null)
                    {
                        await GenerateTimeslots(plant, wd, nextWorking.Value, nextWorking.Value, holidayList);
                    }

                    // Remove timeslots older than retention
                    var removeBefore = today.AddDays(-retentionDays);
                    //await RemoveTimeslotsBefore(plant.PlantId, removeBefore);
                }
            }

            return Ok("Timeslot list updated successfully.");
        }
        private async Task RemoveTimeslotsBefore(long PlantId, DateTime startDate)
        {
            var alltimeslots = await _woService.GetAllTimeslot_List();
            var timeslotsPlant = alltimeslots
    .Where(t => t.PlantId == PlantId && t.Start_time >= startDate)
    .ToList();
            foreach (var item in timeslotsPlant)
            {
                var result = await _woService.DeleteTimeslot_List(item.Timeslot_ListId);
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
                        DateTime currentSlotEnd = currentSlotStart.AddMinutes(wd.Timeslot_duration);
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

        private bool IsBreakTime(DateTime startTime)
        {
            // Example: hardcode or fetch break times from settings
            var breakPeriods = new List<(TimeSpan Start, TimeSpan End)>
            {
                (new TimeSpan(10, 0, 0), new TimeSpan(10, 30, 0)), // 10:00 to 10:30
                (new TimeSpan(14, 0, 0), new TimeSpan(14, 30, 0))  // 2:00 to 2:30
            };

            var timeOnly = startTime.TimeOfDay;
            return breakPeriods.Any(b => timeOnly >= b.Start && timeOnly < b.End);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTimeslot_List(long itemMasterDocListId)
        {
            var result = await _woService.DeleteTimeslot_List(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMc_Timeslot_List()
        {
            var allMachines = await _woService.GetAllMc_Timeslot_List();
            var timeresult = await _woService.GetAllTimeslot_List();
            var machineGroups = allMachines
                .GroupBy(mc => mc.Mc_Id);
            var machineResidenceTimes = new List<(long MachineId, TimeSpan ResidenceTime)>();

            foreach (var group in machineGroups)
            {
                long machineId = group.Key;
                var timeSlots = group
                    .SelectMany(mc => timeresult
                        .Where(ts => ts.Timeslot_ListId == mc.Timeslot_List_Id))
                    .ToList();

                if (timeSlots.Count > 0)
                {
                    var startTimes = timeSlots.Select(ts => ts.Start_time).ToList();
                    var endTimes = timeSlots.Select(ts => ts.End_time).ToList();
                    DateTime earliestStart = startTimes.Min();
                    DateTime latestEnd = endTimes.Max();

                    TimeSpan residenceTime = latestEnd - earliestStart;

                    machineResidenceTimes.Add((machineId, residenceTime));
                }
            }
            return Ok(machineResidenceTimes);
        }
        [HttpPost]
        public async Task<IActionResult> PostMc_Timeslot_List()
        {
            var machineslist = await _machineService.GetMachinesList();
            var allTimeslots = await _woService.GetAllTimeslot_List();

            foreach (var machine in machineslist)
            {
                var plantId = machine.PlantId;
                var holidays = await _plantService.GetHolidays(plantId);
                var holidayDates = holidays.Select(h => h.HolidayDate).ToList();
                var wd = await _plantService.GetPlantWD(plantId);

                foreach (var slot in allTimeslots.Where(t => t.PlantId == plantId))
                {
                    if (holidayDates.Contains(slot.Start_time.Date) ||
                        slot.Start_time.DayOfWeek.ToString() == wd.WeeklyOff1 ||
                        slot.Start_time.DayOfWeek.ToString() == wd.WeeklyOff2)
                        continue;


                    var mcSlot = new Mc_Timeslot_ListVM
                    {
                        Mc_Id = machine.MachineId,
                        Timeslot_List_Id = slot.Timeslot_ListId,
                        Allocation = 1,
                        Slot_Not_Avl = 'N', 
                        Not_Avl_reason = 0
                    };

                    await _woService.PostMc_Timeslot_List(mcSlot);
                }
            }

            return Ok("Mc_Timeslot_List created for all machines.");
        }

        [HttpPost]
        public async Task<IActionResult> PostTempMc_Timeslot_List()
        {
            var machineslist = await _machineService.GetMachinesList();
            var allTimeslots = await _woService.GetAllTimeslot_List();

            foreach (var machine in machineslist)
            {
                var plantId = machine.PlantId;
                var holidays = await _plantService.GetHolidays(plantId);
                var holidayDates = holidays.Select(h => h.HolidayDate).ToList();
                var wd = await _plantService.GetPlantWD(plantId);

                foreach (var slot in allTimeslots.Where(t => t.PlantId == plantId))
                {
                    if (holidayDates.Contains(slot.Start_time.Date) ||
                        slot.Start_time.DayOfWeek.ToString() == wd.WeeklyOff1 ||
                        slot.Start_time.DayOfWeek.ToString() == wd.WeeklyOff2)
                        continue;

                    var tempMcSlot = new TempMc_Timeslot_ListVM
                    {
                        Mc_Id = machine.MachineId,
                        Timeslot_List_Id = slot.Timeslot_ListId,
                        Allocation = 1,
                        Slot_Not_Avl = 'N',
                        Not_Avl_reason = 0
                    };

                    await _woService.PostTempMc_Timeslot_List(tempMcSlot);
                }
            }

            return Ok("TempMc_Timeslot_List created for all machines.");
        }
        [HttpPost]
        public async Task<IActionResult> SimulateMc_Wait_List()
        {
            var _allocatedSlotIds = new HashSet<long>();
            var woList = (await _woService.GetAllWO_Wait_List())
                .ToList();
            var prodnWos = await _woService.AllProductionPlan_Wo();

            var allOprs = await _woService.GetAllOpr_List();

            // Sort WO: Rwk > NonPlan > Normal → Then by Plan Completion Date → Then TPT
            var orderedWOList = woList
                .OrderBy(wo => wo.Rework_Wo == 'Y' ? 0 : wo.NC_Log_Ref > 0 ? 1 : 2)
                .ThenBy(wo => wo.Plan_End_Date)
                .ThenBy(wo => wo.Total_TPT)
                .ToList();

            foreach (var wo in orderedWOList)
            {
                var oprList = allOprs
                    .Where(o => o.Wo_Id == wo.Wo_Id)
                    .OrderBy(o => o.Opr_No)
                    .ToList();

                foreach (var opr in oprList)
                {

                    var oprMachines = await _routingService.StepMachines((int)opr.Opr_No);
                    var prodwo = prodnWos.Where(w => w.ProductionPlanId == wo.Wo_Id).FirstOrDefault();
                    var routingsteps = await _routingService.RoutingSteps((int)prodwo.RoutingId);
                    var routingstep = routingsteps.FirstOrDefault(r => r.StepId == opr.Opr_No);
                    int noOfSimultMcs = routingstep.NumberOfSimMachines;
                    var allEligibleMachines = await GetMachinesForOpr(opr.Opr_No);

                    int machineAllocCount = 0;

                    foreach (var oprMc in oprMachines)
                    {
                        var mc = allEligibleMachines.FirstOrDefault(m => m.MachineId == oprMc.MachineId);
                        if (mc == null) continue;

                        var timeslot = await GetEarliestFreeTimeslot(mc.MachineId, _allocatedSlotIds);
                        if (timeslot == null) continue;

                        var waitSeq = await CalculateNextWaitSeqNo(mc.MachineId);
                        TimeSpan setupTime = TimeSpan.Parse(oprMc.SetupTime);
                        TimeSpan firstPieceTime = TimeSpan.Parse(oprMc.FirstPieceProcessingTime);
                        TimeSpan floorToFloorTime = TimeSpan.Parse(oprMc.FloorToFloorTime);

                        int planQty = Math.Max(0, opr.Plan_Qnty - 1);
                        TimeSpan totalTPT = setupTime + firstPieceTime + (floorToFloorTime * planQty);

                        var mcWait = new Mc_Wait_ListVM
                        {
                            Wo_Id = wo.Wo_Id,
                            Opr_No_Id = opr.Opr_No,
                            Mc_Id = mc.MachineId,
                            Mode = wo.Mode,
                            Wait_Seq_No = waitSeq,
                            Plan_Qnty = opr.Plan_Qnty,
                            Act_Qnty = 0,
                            Bal_Qnty = opr.Plan_Qnty,
                            Rework_Wo = wo.Rework_Wo,
                            Non_Plan_Wk = wo.NC_Log_Ref > 0 ? 'Y' : 'N',
                            Non_Plan_wk_Id = wo.NC_Log_Ref,
                            Plan_start_time_Id = timeslot.Mc_Timeslot_List_Id,
                            Plan_end_time_Id = timeslot.Mc_Timeslot_List_Id,
                            Next_Opr_Start_time_Id = 0,
                            Setup_Start_time = null,
                            Setup_Apprvl_time = null,
                            Act_End_time = null,
                            Mc_TPT = (decimal)totalTPT.TotalMinutes
                        };

                        await _woService.PostMc_Wait_List(mcWait);
                        machineAllocCount++;

                        if (machineAllocCount >= noOfSimultMcs)
                            break;
                    }
                }
            }

            return Ok("Mc_Wait_List simulation completed.");
        }

        private HashSet<long> _allocatedSlotIds = new HashSet<long>();

        public async Task<Mc_Timeslot_ListVM?> GetEarliestFreeTimeslot(long machineId, HashSet<long> _allocatedSlotIds)
        {
            var allMachineSlots = await _woService.GetAllMc_Timeslot_List();
            var allTimeSlots = await _woService.GetAllTimeslot_List();
            var now = DateTime.Now;

            var query = from mcSlot in allMachineSlots
                        join ts in allTimeSlots on mcSlot.Timeslot_List_Id equals ts.Timeslot_ListId
                        where mcSlot.Mc_Id == machineId
                              && mcSlot.Allocation == 1
                              && !_allocatedSlotIds.Contains(mcSlot.Mc_Timeslot_List_Id)
                              && ts.Start_time > now
                        orderby ts.Start_time
                        select new { MachineSlot = mcSlot, SlotStartTime = ts.Start_time };

            var earliest = query.FirstOrDefault();

            if (earliest != null)
            {
                lock (_allocatedSlotIds)
                {
                    _allocatedSlotIds.Add(earliest.MachineSlot.Mc_Timeslot_List_Id);
                }
                return earliest.MachineSlot;
            }

            return null;
        }
        [HttpPost]
        public async Task<IActionResult> SimulateTempMc_Wait_List()
        {
            var _allocatedTempSlotIds = new HashSet<long>();
            var woList = (await _woService.GetAllTempWo_Wait_List())
                .ToList();

            var prodnWos = await _woService.AllProductionPlan_Wo();
            var allOprs = await _woService.GetAllTempOpr_List();

            // Sort WOs by Rework > NonPlan > Normal > Plan Date > TPT
            var orderedWOList = woList
                .OrderBy(wo => wo.Rework_Wo == 'Y' ? 0 : wo.NC_Log_Ref > 0 ? 1 : 2)
                .ThenBy(wo => wo.Plan_End_Date)
                .ThenBy(wo => wo.Total_TPT)
                .ToList();

            foreach (var wo in orderedWOList)
            {
                var oprList = allOprs
                    .Where(o => o.Wo_Id == wo.Wo_Id)
                    .OrderBy(o => o.Opr_No)
                    .ToList();

                foreach (var opr in oprList)
                {
                    var oprMachines = await _routingService.StepMachines((int)opr.Opr_No);
                    var prodwo = prodnWos.Where(w => w.ProductionPlanId == wo.Wo_Id).FirstOrDefault();
                    var routingsteps = await _routingService.RoutingSteps((int)prodwo.RoutingId);
                    var routingstep = routingsteps.FirstOrDefault(r => r.StepId == opr.Opr_No);
                    int noOfSimultMcs = routingstep.NumberOfSimMachines;

                    var allEligibleMachines = await GetMachinesForOpr(opr.Opr_No);

                    int machineAllocCount = 0;

                    foreach (var oprMc in oprMachines)
                    {
                        var mc = allEligibleMachines.FirstOrDefault(m => m.MachineId == oprMc.MachineId);
                        if (mc == null) continue;

                        var timeslot = await GetEarliestFreeTempTimeslot(mc.MachineId, _allocatedTempSlotIds);
                        if (timeslot == null) continue;

                        var waitSeq = await CalculateNextTempWaitSeqNo(mc.MachineId);

                        TimeSpan setupTime = TimeSpan.Parse(oprMc.SetupTime);
                        TimeSpan firstPieceTime = TimeSpan.Parse(oprMc.FirstPieceProcessingTime);
                        TimeSpan floorToFloorTime = TimeSpan.Parse(oprMc.FloorToFloorTime);

                        int planQty = Math.Max(0, opr.Plan_Qnty - 1);
                        TimeSpan totalTPT = setupTime + firstPieceTime + (floorToFloorTime * planQty);

                        var tempMcWait = new TempMc_Wait_ListVM
                        {
                            Wo_Id = wo.Wo_Id,
                            Opr_No_Id = opr.Opr_No,
                            Mc_Id = mc.MachineId,
                            Mode = wo.Mode,
                            Wait_Seq_No = waitSeq,
                            Plan_Qnty = opr.Plan_Qnty,
                            Act_Qnty = 0,
                            Bal_Qnty = opr.Plan_Qnty,
                            Rework_Wo = wo.Rework_Wo,
                            Non_Plan_Wk = wo.NC_Log_Ref > 0 ?'Y':'N',
                            Non_Plan_wk_Id = wo.NC_Log_Ref,
                            Plan_start_time_Id = timeslot.TempMc_Timeslot_List_Id,
                            Plan_end_time_Id = timeslot.TempMc_Timeslot_List_Id,
                            Next_Opr_Start_time_Id = 0,
                            Setup_Start_time = null,
                            Setup_Apprvl_time = null,
                            Act_End_time = null,
                            Mc_TPT = (decimal)totalTPT.TotalMinutes
                        };

                        await _woService.PostTempMc_Wait_List(tempMcWait);
                        machineAllocCount++;

                        if (machineAllocCount >= noOfSimultMcs)
                            break;
                    }
                }
            }

            return Ok("TempMc_Wait_List simulation completed.");
        }

        private HashSet<long> _allocatedTempSlotIds = new HashSet<long>();
        public async Task<TempMc_Timeslot_ListVM?> GetEarliestFreeTempTimeslot(long machineId, HashSet<long> _allocatedTempSlotIds)
        {
            var allMachineSlots = await _woService.GetAllTempMc_Timeslot_List();
            var allTimeSlots = await _woService.GetAllTimeslot_List();

            var now = DateTime.Now;

            var query = from mcSlot in allMachineSlots
                        join ts in allTimeSlots on mcSlot.Timeslot_List_Id equals ts.Timeslot_ListId
                        where mcSlot.Mc_Id == machineId
                              && mcSlot.Allocation == 1
                              && !_allocatedTempSlotIds.Contains(mcSlot.TempMc_Timeslot_List_Id)
                              && ts.Start_time > now
                        orderby ts.Start_time
                        select new { MachineSlot = mcSlot, SlotStartTime = ts.Start_time };

            var earliest = query.FirstOrDefault();

            if (earliest != null)
            {
                lock (_allocatedTempSlotIds)
                {
                    _allocatedTempSlotIds.Add(earliest.MachineSlot.TempMc_Timeslot_List_Id);
                }
                return earliest.MachineSlot;
            }

            return null;
        }

        public async Task<int> CalculateNextTempWaitSeqNo(long mcId)
        {
            var mcWaits = await _woService.GetAllTempMc_Wait_List();
            var maxSeq = mcWaits
                .Where(m => m.Mc_Id == mcId)
                .Select(m => m.Wait_Seq_No)
                .DefaultIfEmpty(-1)
                .Max();
            return (int)(maxSeq + 1);
        }
        public async Task<List<MachineListVM>> GetMachinesForOpr(long oprId)
        {
            var allMachines = await _machineService.GetMachinesList();
            var oprMachines = await _routingService.StepMachines((int)oprId);
            var oprMachineIds = oprMachines.Select(rsm => rsm.MachineId).ToHashSet();
            return allMachines.Where(m => oprMachineIds.Contains(m.MachineId)).ToList();
        }
        public async Task<int> CalculateNextWaitSeqNo(long mcId)
        {
            var mcWaits = await _woService.GetAllMc_Wait_List();
            var maxSeq = mcWaits
                .Where(m => m.Mc_Id == mcId)
                .Select(m => m.Wait_Seq_No)
                .DefaultIfEmpty(0)
                .Max();
            return (int)(maxSeq + 1);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMc_Timeslot_List(long itemMasterDocListId)
        {
            var result = await _woService.DeleteMc_Timeslot_List(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetMachines()
        {
            var machinesList = await _machineService.GetMachinesList();
            var machineType = await _machineService.GetMachineTypes();
            var machineWaitList = await _woService.GetAllMc_Wait_List();
            var allTimeslots = await _woService.GetAllTimeslot_List();

            foreach (var item in machinesList)
            {
                // Set Machine Type Name
                foreach (var mctype in machineType)
                {
                    if (item.MachineTypeId == mctype.MachineTypeTypeId)
                    {
                        item.MachineType = mctype.MachineTypeName;
                        break;
                    }
                }

                // Find corresponding wait list entry
                var waitEntry = machineWaitList.FirstOrDefault(w => w.Mc_Id == item.MachineId);
                if (waitEntry != null && waitEntry.Next_Opr_Start_time_Id != null)
                {
                    var nextSlot = allTimeslots.FirstOrDefault(t => t.Timeslot_ListId == waitEntry.Next_Opr_Start_time_Id);
                    if (nextSlot != null)
                    {
                        item.NextOprTime = nextSlot.Start_time.ToString("dd-MM-yyyy hh:mm tt"); 
                    }
                    else
                    {
                        item.NextOprTime = "";
                    }
                }
                else
                {
                    item.NextOprTime = "";
                }
            }

            return Ok(machinesList);

        }
        [HttpGet]
        public async Task<IActionResult> GetAllMc_Wait_List()
        {
            var result = new List<TempMc_Wait_ListVM>();
            var wowaitList = await _woService.GetAllTempWo_Wait_List();
            var waitList = await _woService.GetAllTempMc_Wait_List();
            var timeslots = await _woService.GetAllTempMc_Timeslot_List();
            var mcactive = await _woService.GetAllMc_Timeslot_List();
            var allTimeSlots = await _woService.GetAllTimeslot_List();
            var machineTypes = await _machineService.GetMachineTypes();
            var getshop = await _departmentService.GetDepartments(1);
            var getsection = await _departmentService.GetSections();
            var masterparts = await _masterService.ItemMasterParts();
            var alProductionWOs = await _woService.AllProductionPlan_Wo();
            var partinQ = 0;
            foreach (var item in waitList)
            {
                var mcId = item.Mc_Id;
                var machine = await _machineService.GetMachine(mcId);
                var mcName = machine?.MachineMachineName ?? " ";
                var mcTypeName = machineTypes.FirstOrDefault(m => m.MachineTypeTypeId == machine.MachineMachineTypeId)?.MachineTypeName ?? "Unknown";
                var shopName = getshop.First(s => s.DepartmentId == machine.MachineDepartmentId).Name ?? " ";
                var section = getsection.FirstOrDefault(s => s.SectionsId == machine.SectionId);
                var secName = section != null ? section.Name : " ";

                var plantwd = await _plantService.GetPlantWD(machine.MachinePlantId);
                var duration = plantwd.Timeslot_duration;
                var mcSlots = timeslots.Where(t => t.Mc_Wait_List_Id == item.TempMc_Wait_ListId).ToList();
                double hrsBooked = 0;
                if (mcSlots.Any())
                {
                    var startSlotId = mcSlots.First().Timeslot_List_Id;
                    var endSlotId = mcSlots.First().EndTimeslot_List_Id;

                    var startSlot = allTimeSlots.FirstOrDefault(t => t.Timeslot_ListId == startSlotId);
                    var endSlot = allTimeSlots.FirstOrDefault(t => t.Timeslot_ListId == endSlotId);

                    if (startSlot != null && endSlot != null)
                    {
                        var timeDiff = (endSlot.End_time - startSlot.Start_time).TotalMinutes;
                        hrsBooked = timeDiff / 60.0;
                    }
                }
                item.McName = mcName;
                item.McTypeName = mcTypeName;
                item.ShopName = shopName;
                item.SectionName = secName;
                item.PartInQueue = partinQ++;
                item.HrsBooked = hrsBooked.ToString("0.00");
                var mcNotAvlSlots = mcactive
                    .Where(t => t.Mc_Id == mcId && t.Slot_Not_Avl == 'Y')
                    .ToList();
                double mcNotAvlHrs = 0;
                foreach (var slot in mcNotAvlSlots)
                {
                    var start = allTimeSlots.FirstOrDefault(t => t.Timeslot_ListId == slot.Timeslot_List_Id)?.Start_time;
                    var end = allTimeSlots.FirstOrDefault(t => t.Timeslot_ListId == slot.EndTimeslot_List_Id)?.End_time;

                    if (start != null && end != null)
                    {
                        mcNotAvlHrs += (end.Value - start.Value).TotalMinutes / 60.0;
                    }
                }
                item.McNotAvlHrs = mcNotAvlHrs.ToString("0.00");
                double simulationDurationHrs = 0;
                if (mcSlots.Any())
                {
                    var simStart = allTimeSlots.FirstOrDefault(t => t.Timeslot_ListId == mcSlots.First().Timeslot_List_Id)?.Start_time;
                    var simEnd = allTimeSlots.FirstOrDefault(t => t.Timeslot_ListId == mcSlots.First().EndTimeslot_List_Id)?.End_time;

                    if (simStart != null && simEnd != null)
                    {
                        simulationDurationHrs = (simEnd.Value - simStart.Value).TotalMinutes / 60.0;
                    }
                }
                var freeHrs = simulationDurationHrs - hrsBooked - mcNotAvlHrs;
                if (freeHrs < 0) freeHrs = 0;
                var percentUsed = simulationDurationHrs > 0 ? (hrsBooked * 100.0) / simulationDurationHrs : 0;
                item.FreeHrs = freeHrs.ToString("0.00");
                item.SimulationDurationHrs = simulationDurationHrs.ToString("0.00");
                item.PercentHrsUsed = percentUsed.ToString("0.00");
                var lastWoEndSlotId = wowaitList
                    .Where(w => w.Wo_Id == item.Wo_Id)
                    .OrderByDescending(w => w.Plan_End_Date)
                    .FirstOrDefault()?.Plan_End_Date;
                var pp = alProductionWOs.Where(p => p.ProductionPlanId == item.Wo_Id).FirstOrDefault();
                if (lastWoEndSlotId != null)
                {
                    item.DateMcNotLoaded = lastWoEndSlotId?.ToString("dd-MM-yyyy hh:mm tt");
                }
                else
                {
                    var csend = allTimeSlots.Where(t => t.Timeslot_ListId == item.Plan_end_time_Id).FirstOrDefault();
                    item.DateMcNotLoaded = csend.Start_time.ToString("dd-MM-yyyy hh:mm tt");
                }
                var routste = await _routingService.RoutingSteps((int)pp.RoutingId);
                item.PartNo = masterparts.FirstOrDefault(m=>m.PartId == pp.PartId).PartNo + " / " + routste.FirstOrDefault().StepNumber;
                
                if (item.Rework_Wo == 'Y')
                {
                    // Store rework hours on the item (we'll sum it later)
                    item.ReworkHrs = hrsBooked.ToString("0.00");
                    item.IsReworkCount = 1;
                }
                else
                {
                    item.ReworkHrs = "0.00";
                }
                bool isNonPlan = item.Non_Plan_Wk == 'Y';
                item.NonPlanHrs = isNonPlan ? hrsBooked.ToString("0.00") : "0.00";
                item.IsNonPlanCount = isNonPlan ? 1 : 0;
            }
            var groupedResult = waitList
      .GroupBy(w => w.Mc_Id)
      .Select(g =>
      {
          var first = g.First(); // Take common fields like names from the first item
          
        double totalHrsBooked = g.Sum(x => double.TryParse(x.HrsBooked, out var val) ? val : 0);
          double totalMcNotAvlHrs = double.TryParse(g.First().McNotAvlHrs, out var val) ? val : 0;
          double totalSimulationDuration = g.Sum(x => double.TryParse(x.SimulationDurationHrs, out var val) ? val : 0);
          double totalFreeHrs = g.Sum(x => double.TryParse(x.FreeHrs, out var val) ? val : 0);
          double totalReworkHrs = g.Sum(x => double.TryParse(x.ReworkHrs, out var val) ? val : 0);
          double percentUsed = totalSimulationDuration > 0 ? (totalHrsBooked * 100.0) / totalSimulationDuration : 0;
          int totalReworkCount = g.Sum(x => x.IsReworkCount);
          double totalNonPlanHrs = g.Sum(x => double.TryParse(x.NonPlanHrs, out var val) ? val : 0);
          int totalNonPlanCount = g.Sum(x => x.IsNonPlanCount);
          var datenot = first.DateMcNotLoaded;
          string FormatHours(double hours)
          {
              var time = TimeSpan.FromHours(hours);
              return $"{(int)time.TotalHours:D2}:{time.Minutes:D2}";
          }
          return new TempMc_Wait_ListVM
          {
              Mc_Id = g.Key,
              McName = first.McName,
              McTypeName = first.McTypeName,
              ShopName = first.ShopName,
              SectionName = first.SectionName,
              HrsBooked = FormatHours(totalHrsBooked),
              McNotAvlHrs = FormatHours(totalMcNotAvlHrs),
              SimulationDurationHrs = FormatHours(totalSimulationDuration),
              FreeHrs = FormatHours(totalFreeHrs),
              ReworkHrs = FormatHours(totalReworkHrs),
              ReworkCount = totalReworkCount,
              NonPlanHrs = FormatHours(totalNonPlanHrs),
              NonPlanCount = totalNonPlanCount,
              PercentHrsUsed = percentUsed.ToString("0.00"),
              DateMcNotLoaded = datenot,
              PartNo = g.Select(x => x.PartNo).FirstOrDefault()
          };
      })
      .ToList();

            return Ok(groupedResult);
        }
        [HttpPost]
        public async Task<IActionResult> PostMc_Wait_List(Mc_Wait_ListVM masterDocListVM)
        {
            var result = await _woService.PostMc_Wait_List(masterDocListVM);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetNotAvlMcTimeslot()
        {
            var getMCList = await _woService.GetAllMc_Timeslot_List();
            var result = getMCList.Where(m => m.Slot_Not_Avl == 'Y').ToList();

            if (!result.Any())
                return Ok(result);

            var getmachince = await _machineService.GetMachinesList();
            var nonavlRess = await _woService.GetAllMc_not_avl_reason();
            var alltimeSlotList = await _woService.GetAllTimeslot_List();

            // Pre-index the lists to avoid repeated searching
            var machineDict = getmachince.ToDictionary(m => m.MachineId);
            var reasonDict = nonavlRess.ToDictionary(r => r.Mc_Not_Avl_ReasonId);
            var timeslotDict = alltimeSlotList.ToDictionary(t => t.Timeslot_ListId);

            foreach (var item in result)
            {
                if (machineDict.TryGetValue(item.Mc_Id, out var mc))
                {
                    item.McName = mc.Name;
                    item.ShopName = mc.Shop;
                    item.ShopId = mc.ShopId;
                }

                if (reasonDict.TryGetValue(item.Not_Avl_reason, out var reason))
                {
                    item.NonAvlReas = reason.Reason_Desc;
                }

                if (timeslotDict.TryGetValue(item.Timeslot_List_Id, out var startSlot))
                {
                    item.StartTime = startSlot.Start_time.ToString("dd-MM-yyyy hh:mm tt");
                }

                if (timeslotDict.TryGetValue(item.EndTimeslot_List_Id, out var endSlot))
                {
                    item.EndTime = endSlot.End_time.ToString("dd-MM-yyyy hh:mm tt");
                }
            }

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostNotAvlMcTimeslot(Mc_Timeslot_ListVM masterDocListVM)
        {
            var getTimeSlotLists = await _woService.GetAllTimeslot_List();
            var getStTimeSlotList = getTimeSlotLists
                .Where(t => t.Start_time <= masterDocListVM.GetStartTime)
                .OrderByDescending(t => t.Start_time)
                .FirstOrDefault();
            var getEndTimeSlotList = getTimeSlotLists
                .Where(t => t.End_time >= masterDocListVM.GetEndTime)
                .OrderBy(t => t.End_time)
                .FirstOrDefault();
            masterDocListVM.Timeslot_List_Id = getStTimeSlotList.Timeslot_ListId;
            masterDocListVM.EndTimeslot_List_Id = getEndTimeSlotList.Timeslot_ListId;
            var result = await _woService.PostMc_Timeslot_List(masterDocListVM);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSetupMc_Wait_List(Mc_Wait_ListVM masterDocListVM)
        {
            var mc_Wait_Lists = await _woService.GetAllMc_Wait_List();
            var mcWait = mc_Wait_Lists.Where(m => m.Mc_Wait_ListId == masterDocListVM.Mc_Wait_ListId).FirstOrDefault();
            if(mcWait!= null)
            {
                mcWait.Setup_Start_time = masterDocListVM.Setup_Start_time;
                ClaimsPrincipal userClaim = HttpContext.User;
                long LoggedId = long.Parse(AppUtil.GetTenantId(userClaim));
                mcWait.Setup_start_time_entry = LoggedId;
                var result = await _woService.PostMc_Wait_List(mcWait);
                return Ok(result);
            }
            return Ok("Not Found McWait");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSetupAplMc_Wait_List(Mc_Wait_ListVM masterDocListVM)
        {
            var mc_Wait_Lists = await _woService.GetAllMc_Wait_List();
            var mcWait = mc_Wait_Lists.Where(m => m.Mc_Wait_ListId == masterDocListVM.Mc_Wait_ListId).FirstOrDefault();
            if (mcWait != null)
            {
                mcWait.Setup_Appvl_Doc_Ref = masterDocListVM.Setup_Appvl_Doc_Ref;
                mcWait.Setup_Apprvl_time = DateTime.Now;
                mcWait.Setup_FTR = masterDocListVM.Setup_FTR;
                mcWait.Setup_comments = masterDocListVM.Setup_comments;
                mcWait.OperatorId = masterDocListVM.OperatorId;
                mcWait.ReasonfornotachievingFTRId = masterDocListVM.ReasonfornotachievingFTRId;
                mcWait.ReasonforAddnSetupTimeId = masterDocListVM.ReasonforAddnSetupTimeId;
                mcWait.SetupTimeTaken = masterDocListVM.SetupTimeTaken;
                mcWait.AddntimeforSetup = masterDocListVM.AddntimeforSetup;
                var result = await _woService.PostMc_Wait_List(mcWait);
                return Ok(result);
            }
            return Ok("Not Found McWait");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteMc_Wait_List(long itemMasterDocListId)
        {
            var result = await _woService.DeleteMc_Wait_List(itemMasterDocListId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostMatl_Issue_Settings(Matl_Issue_SettingsVM masterDocListVM)
        {
                var result = await _woService.PostMatl_Issue_Settings(masterDocListVM);
                return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMatl_Issue_Settings()
        {
            var matl_Issue_Settings = await _woService.GetAllMatl_Issue_Settings();
            var shops = await _departmentService.GetDepartments(1);
            var result = new List<Matl_Issue_SettingsVM>();
            foreach (var item in shops)
            {
                var setting = matl_Issue_Settings.FirstOrDefault(x => x.Shop_Id == item.DepartmentId);
                result.Add(new Matl_Issue_SettingsVM
                {
                    Shop = item.Name,
                    NoOfShifts = Convert.ToString(item?.NoOfShifts ?? 0),
                    Shop_Id = item?.DepartmentId ?? 0,
                    Matl_Issue_SettingsId = setting?.Matl_Issue_SettingsId ?? 0,
                    No_days_coverage = setting?.No_days_coverage ?? 0,
                    IssueDay = setting?.IssueDay ?? 0,
                    IssueDayStr = setting?.IssueDay == 1 ? "Day of Prodn"
                 : setting?.IssueDay == 2 ? "Prev Day"
                 : "-",
                    LastIssueTime = setting?.LastIssueTime ?? DateTime.MinValue,
                    LastIssueTimeStr = setting?.IssueDay == 1 ? setting.LastIssueTime.ToString("hh:mm tt") : "-"

            });
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteMatl_Issue_Settings(long itemMasterDocListId)
        {
            var result = await _woService.DeleteMatl_Issue_Settings(itemMasterDocListId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostDispatchQnty(DispatchQntyVM masterDocListVM)
        {
                var result = await _woService.PostDispatchQnty(masterDocListVM);
                return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDispatchQnty()
        {
            var DispatchQnty = await _woService.GetAllDispatchQnty();
            return Ok(DispatchQnty);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteDispatchQnty(long itemMasterDocListId)
        {
            var result = await _woService.DeleteDispatchQnty(itemMasterDocListId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostDispatchDetails(DispatchDetailsVM masterDocListVM)
        {
                var result = await _woService.PostDispatchDetails(masterDocListVM);
                return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDispatchDetails()
        {
            var DispatchDetails = await _woService.GetAllDispatchDetails();
            return Ok(DispatchDetails);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteDispatchDetails(long itemMasterDocListId)
        {
            var result = await _woService.DeleteDispatchDetails(itemMasterDocListId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostMatl_Issue_List(Matl_Issue_ListVM masterDocListVM)
        {
                var result = await _woService.PostMatl_Issue_List(masterDocListVM);
                return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMatl_Issue_Lists()
        {
            var result = await _woService.GetAllMatl_Issue_List();
            var tempoprs = await _woService.GetAllTempOpr_List();
            var depts = await _departmentService.GetDepartments(1);
            var prodns = await _woService.AllProductionPlan_Wo();
            var translog = await _woService.GetAllInv_Trans_Log();
            var masterparts = await _masterService.ItemMasterParts();
            var finalresult = new List<Matl_Issue_ListVM>();
            foreach (var item in result)
            {
                var tempopr = tempoprs.Where(o => o.TempOpr_ListId == item.Part_Ref).FirstOrDefault();
                var translogs = translog.Where(t => t.Input_Opr_No == tempopr.Opr_No && t.Movement_Compl == 'N').FirstOrDefault();
                if(translogs != null)
                {
                    continue;
                }
                var pp= prodns.Where(p => p.ProductionPlanId == tempopr.Wo_Id).FirstOrDefault();
                if (pp == null || pp.Status == 8)
                    continue;
                var todept = depts.FirstOrDefault(d => d.DepartmentId == item.To_Location)?.Name ?? "Stores"; 
                var fromdept = depts.FirstOrDefault(d => d.DepartmentId == item.From_Location)?.Name ?? "Stores";
                item.To_LocationStr = todept;
                item.From_LocationStr = fromdept;
                if(todept != "Stores")
                {
                    item.Shop = todept;
                }
                else if (fromdept != "Stores")
                {
                    item.Shop = fromdept;
                }
                item.WoNumber = pp.WONumber ?? "";
                var masterpart = masterparts.Where(m => m.PartId == pp.PartId).FirstOrDefault();
                item.PartNo = masterpart.PartNo ?? "" + " / " + masterpart.Description;
                var mf = await _masterService.GetManufPart((int)pp.PartId);
                var routingList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                var routstep = await _routingService.RoutingSteps((int)pp.RoutingId);
                item.RoutingName = routingList.Where(r => r.RoutingId == pp.RoutingId).FirstOrDefault().RoutingName;
                item.OpNo = routstep.FirstOrDefault(s => s.StepId == tempopr.Opr_No).StepNumber ?? "";
                item.BalWoQnty = pp.CalcWOQty.ToString() ?? "0";
                item.QntyAvl = 0;
                item.BookOutQnty = 0;
                item.QntyRecdCnf = "Not Confirmed";
                item.IssueMovDtStr = item.Issue_Mov_date.ToString("dd-MM-yyyy");
                finalresult.Add(item);
            }

            var groupedResult = finalresult
    .GroupBy(x => new { x.PartNo, x.Issue_Qnty, x.IssueMovDtStr, x.OpNo, x.WoNumber })
    .Select(g => g.First()) // Take the first full original object
    .ToList();
            return Ok(groupedResult);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMatl_Issue_List()
        {
            // 1. Parallel Fetch: Transaction Data + Master Data + Lookups
            // We add bulk fetches for Manufactured Parts, Routings, and Routing Steps here.
            var matlIssueTask = _woService.GetAllMatlIssueListForShop();
            var tempOprTask = _woService.GetAllTempOpr_List();
            var deptTask = _departmentService.GetDepartments(1);
            var masterPartsTask = _masterService.ItemMasterParts();

            // New Bulk Tasks
            var allManufPartsTask = _masterService.GetAllManufacturedPartNoDetailList();
            var allRoutingsTask = _routingService.AllRoutings();
            var allRoutingStepsTask = _routingService.AllRoutingSteps();

            await Task.WhenAll(
                matlIssueTask,
                tempOprTask,
                deptTask,
                masterPartsTask,
                allManufPartsTask,
                allRoutingsTask,
                allRoutingStepsTask
            );

            var resultList = matlIssueTask.Result.ToList();
            if (!resultList.Any()) return Ok(resultList);

            // 2. Prepare Efficient Lookups (O(1) Access)
            var tempOprs = tempOprTask.Result.ToDictionary(t => t.TempOpr_ListId);
            var depts = deptTask.Result.ToDictionary(d => d.DepartmentId);
            var masterParts = masterPartsTask.Result.ToDictionary(m => m.PartId);

            // Lookup: PartId -> ManufacturedPartNoDetailVM
            // Used to replace _masterService.GetManufPart((int)item.PartId)
            var manufPartLookup = allManufPartsTask.Result
                                    .GroupBy(m => m.PartId)
                                    .ToDictionary(g => g.Key, g => g.First());

            // Lookup: ManufacturedPartNoDetailId -> List of Routings
            // Used to replace _routingService.Routings(mf.ManufacturedPartNoDetailId)
            var routingsByManufId = allRoutingsTask.Result.ToLookup(r => r.ManufacturedPartId);

            // Lookup: RoutingId -> List of RoutingSteps
            // Used to replace _routingService.RoutingSteps((int)item.RoutingId)
            var stepsByRoutingId = allRoutingStepsTask.Result.ToLookup(s => s.RoutingId);

            // 3. Enrich Data (Memory Access Only)
            foreach (var item in resultList)
            {
                // A. Departments & Shop Name
                var todept = depts.ContainsKey(item.To_Location) ? depts[item.To_Location].Name : "Stores";
                var fromdept = depts.ContainsKey(item.From_Location) ? depts[item.From_Location].Name : "Stores";

                item.To_LocationStr = todept;
                item.From_LocationStr = fromdept;

                if (todept != "Stores") item.Shop = todept;
                else if (fromdept != "Stores") item.Shop = fromdept;

                // B. Part No
                if (masterParts.TryGetValue(item.PartId, out var part))
                {
                    item.PartNo = (part.PartNo ?? "") + " / " + part.Description;
                }

                // C. Routing & OpNo
                if (tempOprs.TryGetValue(item.Part_Ref, out var tempOpr))
                {
                    // OPTIMIZATION: Retrieve ManufPart from dictionary
                    if (manufPartLookup.TryGetValue((int)item.PartId, out var mf))
                    {
                        // OPTIMIZATION: Retrieve Routings from Lookup (No DB Call)
                        var routingList = routingsByManufId[mf.ManufacturedPartNoDetailId];
                        var routing = routingList.FirstOrDefault(r => r.RoutingId == item.RoutingId);
                        item.RoutingName = routing?.RoutingName;

                        // OPTIMIZATION: Retrieve RoutingSteps from Lookup (No DB Call)
                        // Note: Using Lookup key prevents iterating through all steps in the system
                        var routSteps = stepsByRoutingId[(int)item.RoutingId];
                        var step = routSteps.FirstOrDefault(s => s.StepId == tempOpr.Opr_No);
                        item.OpNo = step?.StepNumber ?? "";
                    }
                }
            }

            // 4. Grouping Logic
            var groupedResult = resultList
                .GroupBy(x => new { x.PartNo, x.Issue_Qnty, x.IssueMovDtStr, x.OpNo, x.WoNumber })
                .Select(g => g.First())
                .ToList();

            return Ok(groupedResult);
        }
        [HttpPost]
        public async Task<IActionResult> PostIssueInv_Trans_Log([FromBody] List<int> selectedMatlIds)
        {
            try
            {
                var tempoprs = await _woService.GetAllTempOpr_List();
                var depts = await _departmentService.GetDepartments(1);
                var prodns = await _woService.AllProductionPlan_Wo();
                var masterparts = await _masterService.ItemMasterParts();
                var matl_Issue_Lists = await _woService.GetAllMatl_Issue_List();
                foreach (var item in selectedMatlIds)
                {
                    var invdata = new Inv_Trans_LogVM();
                    var matl_Issue_List = matl_Issue_Lists.Where(m => m.Matl_Issue_ListId == item).FirstOrDefault();
                    var tempopr = tempoprs.Where(o => o.TempOpr_ListId == matl_Issue_List.Part_Ref).FirstOrDefault();
                    var pp = prodns.Where(p => p.ProductionPlanId == tempopr.Wo_Id).FirstOrDefault();
                    invdata.Wo_Id = pp.WoId;
                    invdata.Qnty = tempopr.Plan_Qnty; //matl_Issue_List.Issue_Qnty
                    invdata.Part_Status = 1;
                    invdata.From_Location_Id = 0;
                    invdata.To_Location_Id = matl_Issue_List.To_Location;
                    invdata.Input_Part_NoId = pp.PartId;
                    invdata.Input_Routing_Id = pp.RoutingId;
                    invdata.Input_Opr_No = tempopr.Opr_No;
                    invdata.Movement_Started = 'Y';
                    invdata.Movement_Compl = 'Y';
                    invdata.Transaction_Id = 5;
                    var result = await _woService.PostInv_Trans_Log(invdata);
                    var invMaster = new Inventory_MasterVM();
                    invMaster.Part_NoId = result.Input_Part_NoId;
                    invMaster.Routing_Id = result.Input_Routing_Id;
                    invMaster.Opr_No_Id = result.Input_Opr_No;
                    invMaster.Current_QntOnHand = result.Qnty;
                    invMaster.Location_Id = result.To_Location_Id;
                    var inmaster = await _woService.PostInventory_Master(invMaster);
                }
                return Json(new { message = "Material Issued." });
            }
            catch (Exception)
            {

                return Json(new { message = "Material Issue Failed ." });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllIssueSubCon()
        {
            var subconOps = await _woService.GetAllTempSubCon_List();
            var productions = await _woService.AllProductionPlan_Wo();
            var masterparts = await _masterService.ItemMasterParts();
            var compaines = await _masterService.GetCompanies();
            foreach (var item in subconOps)
            {
                var pwo = productions.Where(p => p.ProductionPlanId == item.Wo_Id).FirstOrDefault();
                if (pwo == null || pwo.Status == 8)
                    continue;
                item.WoNumber = pwo.WONumber;
                item.IssueQnty = pwo.CalcWOQty;
                item.Bal_Qnty = pwo.CalcWOQty;
                item.Supplier = compaines.FirstOrDefault(c => c.CompanyId == item.Supplier_Id).CompanyName;
                item.QntyAvl = 0;
                var imp = masterparts.Where(im => im.PartId == pwo.PartId).FirstOrDefault();
                item.PartNo = imp.PartNo + " / " + imp.Description;
                var mf = await _masterService.GetManufPart((int)pwo.PartId);
                var routingList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                var routingStep = await _routingService.RoutingSteps((int)pwo.RoutingId);
                item.RoutingName = routingList.First(r => r.RoutingId == pwo.RoutingId).RoutingName;
                item.OprNoName = routingStep.First(r => r.StepId == item.Opr_No).StepNumber;
            }
            return Ok(subconOps);
        }

        [HttpPost]
        public async Task<IActionResult> PostIssueInvSubCon([FromBody] List<int> selectedMatlIds)
        {
            try
            {
                var tempoprs = await _woService.GetAllTempOpr_List();
                var depts = await _departmentService.GetDepartments(1);
                var prodns = await _woService.AllProductionPlan_Wo();
                var masterparts = await _masterService.ItemMasterParts();
                var tempSubCon_Lists = await _woService.GetAllTempSubCon_List();
                var procplans = await _woService.GetAllProcPlan();
                var podetails = await _woService.GetAllPodetails();
                foreach (var item in selectedMatlIds)
                {
                    var invdata = new Inv_Trans_LogVM();
                    var tempSubCon_List = tempSubCon_Lists.Where(m => m.TempSubCon_ListId == item).FirstOrDefault();
                    var tempopr = tempoprs.Where(o => o.TempOpr_ListId == tempSubCon_List.Opr_No).FirstOrDefault();
                    var pp = prodns.Where(p => p.ProductionPlanId == tempopr.Wo_Id).FirstOrDefault();
                    var procplan = procplans.Where(p => p.WorkOrderId == pp.WoId).LastOrDefault();
                    var podetail = podetails.Where(p => p.ProcPlanId == procplan.ProcPlanId).LastOrDefault();
                    invdata.Input_Part_NoId = pp.PartId;
                    invdata.Input_Routing_Id = pp.RoutingId;
                    invdata.Input_Opr_No = tempSubCon_List.Opr_No;
                    invdata.Wo_Id = pp.WoId;
                    invdata.Part_Status = 1;
                    invdata.PO_No_Id = podetail.PoDetailsId;
                    invdata.Qnty = pp.CalcWOQty;
                    invdata.From_Location_Id = 0;
                    invdata.To_Location_Id = tempSubCon_List.Supplier_Id;
                    invdata.Movement_Started = 'Y';
                    invdata.Movement_Compl = 'Y';
                    var result = await _woService.PostInv_Trans_Log(invdata);
                    var invMaster = new Inventory_MasterVM();
                    invMaster.Part_NoId = result.Input_Part_NoId;
                    invMaster.Routing_Id = result.Input_Routing_Id;
                    invMaster.Opr_No_Id = result.Input_Opr_No;
                    invMaster.Current_QntOnHand = result.Qnty;
                    invMaster.Location_Id = result.To_Location_Id;
                    var inmaster = await _woService.PostInventory_Master(invMaster);
                }
                return Json(new { message = "SubCon Issued." });
            }
            catch (Exception ex)
            {

                return Json(new { message = "SubCon Issue Failed." });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetPartsLoadedInShop()
        {
            var result = new List<TempMc_Wait_ListVM>();

            var waitList = await _woService.GetAllTempMc_Wait_List();
            var timeslots = await _woService.GetAllTempMc_Timeslot_List();
            var allTimeslots = await _woService.GetAllTimeslot_List();
            var parts = await _masterService.ItemMasterParts();
            var machines = await _machineService.GetMachinesList();
            var shops = await _departmentService.GetDepartments(1);
            var allWO = await _woService.AllProductionPlan_Wo();

            foreach (var mcWait in waitList)
            {
                var wo = allWO.FirstOrDefault(p => p.ProductionPlanId == mcWait.Wo_Id);
                if (wo == null || wo.Status == 8)
                    continue;
                var part = parts.FirstOrDefault(p => p.PartId == wo.PartId);
                var machine = machines.FirstOrDefault(m => m.MachineId == mcWait.Mc_Id);
                var shop = shops.FirstOrDefault(s => s.DepartmentId == machine.ShopId);

                var routing = await _routingService.RoutingSteps((int)wo.RoutingId);
                var routingstep = routing.FirstOrDefault(r => r.StepId == mcWait.Opr_No_Id);
                var stepMachines = await _routingService.StepMachines((int)routingstep.StepId);
                var step = stepMachines.FirstOrDefault(sm => sm.PreferredMachine == 1)
                               ?? stepMachines.FirstOrDefault();

                // --- Calculate Bookout & Issue ---
                var matlIssue = await _woService.GetAllMatl_Issue_List();
                long issuedQty = matlIssue.FirstOrDefault(m => m.Part_Ref == mcWait.Opr_No_Id)?.Issue_Qnty ?? wo.CalcWOQty;
                int bookedQty = 0;//await _woService.GetTotalBookoutQty(mcWait.Wo_Id, mcWait.Opr_No_Id) 
                int balanceToBookout = Math.Max(wo.CalcWOQty - bookedQty, 0);
                int wipQty = Math.Max((int)issuedQty - bookedQty, 0);

                TimeSpan cycleTime = TimeSpan.TryParse(step?.FloorToFloorTime, out var ct) ? ct : TimeSpan.Zero;
                TimeSpan setupTime = TimeSpan.TryParse(step?.SetupTime, out var st) ? st : TimeSpan.Zero;
                TimeSpan timePerPart = cycleTime + setupTime;
                TimeSpan totalTimeRequired = TimeSpan.FromTicks(timePerPart.Ticks * wipQty);

                bool isPartInMc = timeslots.Any(t => t.Mc_Wait_List_Id == mcWait.TempMc_Wait_ListId);

                result.Add(new TempMc_Wait_ListVM
                {
                    ShopName = shop?.Name ?? "",
                    WoNumber = wo?.WONumber ?? "",
                    McName = machine?.Name ?? "",
                    PartNo = part?.PartNo + " / " + part?.Description,
                    Wait_Seq_No = mcWait.Wait_Seq_No,
                    WoQnty = wo?.CalcWOQty.ToString() ?? "0",
                    Bal_Qnty = balanceToBookout,
                    QtyWfProcessing = wipQty,
                    BalTimeForAvlQtyHrs = $"{(int)totalTimeRequired.TotalHours:D2}:{totalTimeRequired.Minutes:D2}",
                    PartInMc ="-"
                });
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetPartsLoadedInSubCon()
        {
            var subconWos = await _woService.GetAllTempSubCon_List();
            var woList = await _woService.AllProductionPlan_Wo();
            var companies = await _masterService.GetCompanies();
            var parts = await _masterService.ItemMasterParts();
            var transactions = await _woService.GetAllInv_Trans_Log();

            var result = new List<TempSubCon_ListVM>();

            foreach (var item in subconWos)
            {
                var wo = woList.FirstOrDefault(p => p.ProductionPlanId == item.Wo_Id);

                if (wo == null || wo.Status == 8)
                    continue;
                var part = parts.FirstOrDefault(p => p.PartId == wo.PartId);
                var company = companies.FirstOrDefault(c => c.CompanyId == item.Supplier_Id);

                // Sum qty sent and qty received from Inv_Transaction_Log
                int qtySent = (int)transactions
                    .Where(t => t.Wo_Id == item.Wo_Id && t.Input_Opr_No == item.Opr_No)
                    .Sum(t => t.Qnty);

                int qtyRecd = (int)transactions
                    .Where(t => t.Wo_Id == item.Wo_Id && t.Input_Opr_No == item.Opr_No)
                    .Sum(t => t.Qnty);

                string oprNoName = "-";

                if (wo != null)
                {
                    var routingSteps = await _routingService.RoutingSteps((int)wo.RoutingId);
                    oprNoName = routingSteps.FirstOrDefault(r => r.StepId == item.Opr_No)?.StepNumber ?? "-";
                }
                var partno = part?.PartNo ?? "" + " / " + part?.Description ?? "";
                result.Add(new TempSubCon_ListVM
                {
                    Supplier = company?.CompanyName ?? "Unknown",
                    WoNumber = wo?.WONumber ?? item.Wo_Id.ToString(),
                    PartNo = partno ?? "Unknown",
                    OprNoName = oprNoName,
                    Plan_Qnty = wo?.CalcWOQty ?? 0,
                    QntySent = qtySent,
                    QntyRecd = qtyRecd,
                    BalToReceive = qtySent - qtyRecd
                });
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSetUpCnfList()
        {
            // 2. Start all parallel tasks
            var cnfListTask = _woService.GetAllSetUpCnfList();
            var partsTask = _masterService.ItemMasterParts();
            var machinesTask = _machineService.GetMachinesList();
            // var shopsTask = _departmentService.GetDepartments(1); // Not strictly needed if MachineVM has ShopName, but kept if needed
            var allWOTask = _woService.AllProductionPlan_Wo();
            var allMfTask = _masterService.GetAllManufacturedPartNoDetailList();
            var allRoutingTask = _routingService.AllRoutings();
            var allRoutingStepsTask = _routingService.AllRoutingSteps();

            await Task.WhenAll(cnfListTask, partsTask, machinesTask, allWOTask, allMfTask, allRoutingTask, allRoutingStepsTask);

            var resultList = cnfListTask.Result.ToList();
            if (!resultList.Any()) return Ok(resultList);

            // 3. Create Lookups
            var parts = partsTask.Result.ToDictionary(x => x.PartId);
            var machines = machinesTask.Result.ToDictionary(x => x.MachineId);
            var allWO = allWOTask.Result.ToDictionary(x => x.ProductionPlanId);

            // Efficient Routing Lookups
            var mfDict = allMfTask.Result.GroupBy(x => x.PartId).ToDictionary(g => g.Key, g => g.First());
            var routingDict = allRoutingTask.Result.ToLookup(x => x.ManufacturedPartId);
            var routingStepDict = allRoutingStepsTask.Result.ToLookup(x => x.RoutingId);

            // 4. Enrich Data
            foreach (var item in resultList)
            {
                // A. Machine & Shop
                if (machines.TryGetValue(item.Mc_Id, out var machine))
                {
                    item.McName = machine.Name;
                    item.ShopName = machine.Shop;
                }

                // B. Routing & Part Info
                if (allWO.TryGetValue(item.Wo_Id, out var wo))
                {
                    // Part Name
                    if (parts.TryGetValue(wo.PartId, out var part))
                    {
                        item.PartNo = $"{part.PartNo} / {part.Description}";
                    }

                    // Routing Name
                    if (mfDict.TryGetValue((int)wo.PartId, out var mf))
                    {
                        var routing = routingDict[mf.ManufacturedPartNoDetailId]
                                        .FirstOrDefault(r => r.RoutingId == wo.RoutingId);
                        item.RoutingName = routing?.RoutingName ?? "";
                    }

                    // Operation/Step Name
                    var step = routingStepDict[wo.RoutingId].FirstOrDefault(r => r.StepId == item.Opr_No_Id);
                    item.OprNoName = step?.StepNumber ?? "";
                }
            }

            return Ok(resultList);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSetUpCnfLists()
        {
            var result = new List<TempMc_Wait_ListVM>();

            // ---- PARALLEL LOAD ALL REQUIRED MASTER DATA ----
            var waitListTask = _woService.GetAllMc_Wait_List();
            var timeslotTask = _woService.GetAllTimeslot_List();
            var itemPartsTask = _masterService.ItemMasterParts();
            var machinesTask = _machineService.GetMachinesList();
            var shopsTask = _departmentService.GetDepartments(1);
            var allWoTask = _woService.AllProductionPlan_Wo();
            var translogTask = _woService.GetAllInv_Trans_Log();
            var allMfTask = _masterService.GetAllManufacturedPartNoDetailList();
            var allRoutingTask = _routingService.AllRoutings();
            var allRoutingStepsTask = _routingService.AllRoutingSteps();

            await Task.WhenAll(waitListTask, timeslotTask, itemPartsTask, machinesTask, shopsTask, allWoTask, translogTask,
                allMfTask, allRoutingTask, allRoutingStepsTask
                );

            var waitList = waitListTask.Result; // .Where(x=>x.Wait_Seq_No == 0).ToList()
            var timeSlots = timeslotTask.Result;
            var parts = itemPartsTask.Result.ToDictionary(x => x.PartId);
            var machines = machinesTask.Result.ToDictionary(x => x.MachineId);
            var shops = shopsTask.Result.ToDictionary(x => x.DepartmentId);
            var allWO = allWoTask.Result.ToDictionary(x => x.ProductionPlanId);
            var translogs = translogTask.Result;
            var mfDict = allMfTask.Result.ToDictionary(x => x.PartId);
            var routingDict = allRoutingTask.Result.GroupBy(x => x.ManufacturedPartId)
                                      .ToDictionary(g => g.Key, g => g.ToList());
            var routingStepDict = allRoutingStepsTask.Result.GroupBy(x => x.RoutingId)
                                                           .ToDictionary(g => g.Key, g => g.ToList());

            // Pre-group translogs to avoid repeated Where()
            var translogLookup = translogs
                .GroupBy(x => x.Input_Part_NoId)
                .ToDictionary(g => g.Key, g => g.First());

            foreach (var mcWait in waitList)
            {
                //if (mcWait.Wait_Seq_No != 0)
                //    continue;

                if (!allWO.TryGetValue(mcWait.Wo_Id, out var wo))
                    continue;

                Inv_Trans_LogVM translog = null;

                // Try Input Part first
                if (translogLookup.TryGetValue(wo.PartId, out translog) == false)
                {
                    // fallback Output_Part search (rare case)
                    translog = translogs.FirstOrDefault(t => t.Output_Part_No == wo.PartId);
                    if (translog == null)
                        continue;
                }

                var part = parts.GetValueOrDefault(wo.PartId);
                if (!machines.TryGetValue(mcWait.Mc_Id, out var machine))
                    continue;

                var shop = shops.GetValueOrDefault(machine.ShopId);

                // Preload routing cache (avoid repeated DB hits)
                // (CACHE KEY: PartId or RoutingId)
                var mf = mfDict[(int)wo.PartId];
                var routingList = routingDict[mf.ManufacturedPartNoDetailId];
                var routingSteps = routingStepDict[wo.RoutingId];

                var routingstep = routingSteps.FirstOrDefault(r => r.StepId == mcWait.Opr_No_Id);
                var currentStart = timeSlots.FirstOrDefault(t => t.Timeslot_ListId == mcWait.Plan_start_time_Id)?.Start_time;

                result.Add(new TempMc_Wait_ListVM
                {
                    ShopName = shop?.Name ?? "",
                    McName = machine?.Name ?? "",
                    WoNumber = wo?.WONumber ?? "",
                    PartNo = $"{part?.PartNo} / {part?.Description}",
                    RoutingName = routingList.First(r => r.RoutingId == wo.RoutingId).RoutingName ?? "",
                    OprNoName = routingstep?.StepNumber ?? "",
                    WoQnty = wo?.CalcWOQty.ToString() ?? "0",
                    Plan_Qnty = mcWait.Plan_Qnty,
                    ActiveId = mcWait.Mc_Wait_ListId,
                    MatlIssued = Convert.ToInt32(translog.Qnty).ToString(),
                    PlanStartStr = currentStart?.ToString("hh:mm tt") ?? "",
                    MatlReceptTime = translog.Dt_time.ToString("hh:mm tt"),
                    Rework_Wo = 'N'
                });
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSetUpApprolList()
        {
            // . Start all data fetch tasks in parallel (Faster Performance)
            //    We fetch the API result and all necessary master data at the same time.
            var setupListTask = _woService.GetAllSetUpApprolList();
            var machinesTask = _machineService.GetMachinesList();
            var partsTask = _masterService.ItemMasterParts();
            var allWOTask = _woService.AllProductionPlan_Wo();

            // Routing Data
            var allRoutingsTask = _routingService.AllRoutings();
            var allRoutingStepsTask = _routingService.AllRoutingSteps();
            var allStepMachinesTask = _routingService.AllStepMachines();

            // Await all tasks
            await Task.WhenAll(setupListTask, machinesTask, partsTask, allWOTask, allRoutingsTask, allRoutingStepsTask, allStepMachinesTask);

            var resultList = setupListTask.Result.ToList();
            if (!resultList.Any()) return Ok(resultList);

            // 3. Create Dictionaries/Lookups for O(1) Access
            var machines = machinesTask.Result.ToDictionary(m => m.MachineId);
            var parts = partsTask.Result.ToDictionary(p => p.PartId);
            var workOrders = allWOTask.Result.ToDictionary(w => w.ProductionPlanId);

            var routings = allRoutingsTask.Result.ToDictionary(r => r.RoutingId);
            // Use ToLookup for one-to-many relationships (Routing -> Steps)
            var routingSteps = allRoutingStepsTask.Result.ToLookup(rs => rs.RoutingId);
            var stepMachines = allStepMachinesTask.Result.ToLookup(sm => sm.RoutingStepId);

            // 4. Populate the missing fields using the local master data
            foreach (var item in resultList)
            {
                // A. Populate Machine & Shop Name
                if (machines.TryGetValue(item.Mc_Id, out var machine))
                {
                    item.McName = machine.Name;
                    item.ShopName = machine.Shop; // Assuming MachineListVM contains Shop Name
                }

                // B. Get Work Order Details
                if (workOrders.TryGetValue(item.Wo_Id, out var wo))
                {
                    // C. Populate Part No
                    if (parts.TryGetValue(wo.PartId, out var part))
                    {
                        item.PartNo = $"{part.PartNo} / {part.Description}";
                    }

                    // D. Populate Routing Name
                    if (routings.TryGetValue((int)wo.RoutingId, out var routing))
                    {
                        item.RoutingName = routing.RoutingName;
                    }

                    // E. Populate Operation Name & Planned Setup Time
                    var steps = routingSteps[wo.RoutingId];
                    var step = steps.FirstOrDefault(s => s.StepId == item.Opr_No_Id);

                    if (step != null)
                    {
                        item.OprNoName = step.StepNumber;

                        // Find the setup time for the specific machine on this step
                        var stepMcs = stepMachines[step.StepId];
                        var specificMc = stepMcs.FirstOrDefault(sm => sm.MachineId == item.Mc_Id);

                        if (specificMc != null)
                        {
                            item.PlannedSetupTime = specificMc.SetupTime;
                        }
                    }
                }
            }

            return Ok(resultList);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSetUpApprolLists()
        {
            var result = new List<TempMc_Wait_ListVM>();

            // 1. Load all master data in parallel (Network I/O - Unchanged)
            var waitListTask = _woService.GetAllMc_Wait_List();
            var timeslotsTask = _woService.GetAllTimeslot_List();
            var partsTask = _masterService.ItemMasterParts();
            var machinesTask = _machineService.GetMachinesList();
            var shopsTask = _departmentService.GetDepartments(1);
            var allWOTask = _woService.AllProductionPlan_Wo();
            var translogsTask = _woService.GetAllInv_Trans_Log();
            var allMfTask = _masterService.GetAllManufacturedPartNoDetailList();
            var allRoutingTask = _routingService.AllRoutings();
            var allRoutingStepsTask = _routingService.AllRoutingSteps();
            var allStepMachinesTask = _routingService.AllStepMachines();

            await Task.WhenAll(waitListTask, timeslotsTask, partsTask, machinesTask, shopsTask,
                               allWOTask, translogsTask, allMfTask, allRoutingTask,
                               allRoutingStepsTask, allStepMachinesTask);

            // 2. Filter the Main List immediately to reduce loop count
            var rawWaitList = waitListTask.Result;
            var waitList = rawWaitList
                .Where(x => x.Setup_Start_time != null && x.Setup_Apprvl_time == null)
                .ToList();

            if (waitList.Count == 0) return Ok(result);

            // 3. Convert Lists to Dictionaries (O(1) Lookup)
            // We assume IDs are unique. If duplicates exist in DB, GroupBy().First() is safer.

            var allWO = allWOTask.Result
                .GroupBy(x => x.ProductionPlanId).ToDictionary(g => g.Key, g => g.First());

            var parts = partsTask.Result
                .GroupBy(x => x.PartId).ToDictionary(g => g.Key, g => g.First());

            var machines = machinesTask.Result
                .GroupBy(x => x.MachineId).ToDictionary(g => g.Key, g => g.First());

            var shops = shopsTask.Result
                .GroupBy(x => x.DepartmentId).ToDictionary(g => g.Key, g => g.First());

            // Convert TimeSlots to Dictionary for instant access by ID
            var timeSlots = timeslotsTask.Result
                .GroupBy(x => x.Timeslot_ListId).ToDictionary(g => g.Key, g => g.First());

            var mfDict = allMfTask.Result
                .GroupBy(x => x.PartId).ToDictionary(g => g.Key, g => g.First());

            // 4. Handle One-To-Many Relationships (Routings) using Lookups
            // GroupBy is expensive inside a loop, so we do it ONCE here.
            var routingDict = allRoutingTask.Result
                .ToLookup(x => x.ManufacturedPartId);

            var routingStepDict = allRoutingStepsTask.Result
                .ToLookup(x => x.RoutingId);

            var stepMachinesDict = allStepMachinesTask.Result
                .ToLookup(x => x.RoutingStepId);

            // 5. OPTIMIZE TRANSLOGS (The CPU Killer)
            // We create TWO indexes: One for Input_Part, one for Output_Part
            var translogs = translogsTask.Result;

            // Index A: Lookup by Input_Part_NoId
            var translogInputIndex = translogs
                .GroupBy(t => t.Input_Part_NoId)
                .ToDictionary(g => g.Key, g => g.First());

            // Index B: Lookup by Output_Part_No (Filter nulls first)
            var translogOutputIndex = translogs
                .Where(t => t.Output_Part_No != null)
                .GroupBy(t => t.Output_Part_No)
                .ToDictionary(g => g.Key, g => g.First());

            // 6. The Loop (Now highly efficient)
            foreach (var mcWait in waitList)
            {
                // A. Fast Fail lookups
                if (!allWO.TryGetValue(mcWait.Wo_Id, out var wo)) continue;
                if (!machines.TryGetValue(mcWait.Mc_Id, out var machine)) continue;

                // B. Translog Logic (Replaces the slow FirstOrDefault)
                Inv_Trans_LogVM translog = null;

                // Try Input Part ID
                if (!translogInputIndex.TryGetValue(wo.PartId, out translog))
                {
                    // Fallback: Try Output Part ID
                    translogOutputIndex.TryGetValue(wo.PartId, out translog);
                }

                // If still null, skip
                if (translog == null) continue;

                // C. Standard Data Mapping
                var part = parts.GetValueOrDefault(wo.PartId);
                var shop = shops.GetValueOrDefault(machine.ShopId);
                var currentStart = timeSlots.GetValueOrDefault(mcWait.Plan_start_time_Id)?.Start_time;

                // D. Routing Logic (Using Lookups)
                if (!mfDict.TryGetValue((int)wo.PartId, out var mf)) continue;

                // Find Routing Name
                var routingName = "";
                var routingList = routingDict[mf.ManufacturedPartNoDetailId]; // Returns IEnumerable (Instant)
                var matchedRouting = routingList.FirstOrDefault(r => r.RoutingId == wo.RoutingId);
                if (matchedRouting != null) routingName = matchedRouting.RoutingName;

                // Find Routing Step
                var routingSteps = routingStepDict[wo.RoutingId];
                var routingStep = routingSteps.FirstOrDefault(r => r.StepId == mcWait.Opr_No_Id);
                if (routingStep == null) continue;

                // Find Machine Step
                var stepMachines = stepMachinesDict[routingStep.StepId];
                var plannedSetupTime = stepMachines.FirstOrDefault()?.SetupTime ?? "";

                // E. Add to Result
                result.Add(new TempMc_Wait_ListVM
                {
                    ShopName = shop?.Name ?? "",
                    McName = machine?.Name ?? "",
                    WoNumber = wo?.WONumber ?? "",
                    PartNo = $"{part?.PartNo} / {part?.Description}",
                    RoutingName = routingName,
                    OprNoName = routingStep?.StepNumber ?? "",
                    WoQnty = wo?.CalcWOQty.ToString() ?? "0",
                    Plan_Qnty = mcWait.Plan_Qnty,
                    ActiveId = mcWait.Mc_Wait_ListId,
                    MatlIssued = Convert.ToInt32(translog.Qnty).ToString(),
                    PlanStartStr = currentStart?.ToString("hh:mm tt") ?? "",
                    SetUpTimeStr = mcWait.Setup_Start_time?.ToString("hh:mm tt") ?? "",
                    MatlReceptTime = translog.Dt_time.ToString("hh:mm tt"),
                    PlannedSetupTime = plannedSetupTime,
                    Rework_Wo = 'N'
                });
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBookOutLists()
        {
            var result = new List<TempMc_Wait_ListVM>();

            // ---- Load everything in parallel ----
            var waitListTask = _woService.GetAllMc_Wait_List();
            var timeslotsTask = _woService.GetAllTimeslot_List();
            var partsTask = _masterService.ItemMasterParts();
            var machinesTask = _machineService.GetMachinesList();
            var shopsTask = _departmentService.GetDepartments(1);
            var allWOTask = _woService.AllProductionPlan_Wo();
            var translogsTask = _woService.GetAllInv_Trans_Log();
            var uomTask = _masterService.GetUOMs();
            var allMfTask = _masterService.GetAllManufacturedPartNoDetailList();
            var allRoutingTask = _routingService.AllRoutings();
            var allRoutingStepsTask = _routingService.AllRoutingSteps();
            //var allStepMachinesTask = _routingService.AllStepMachines();

            await Task.WhenAll(
                waitListTask, timeslotsTask, partsTask, machinesTask,
                shopsTask, allWOTask, translogsTask, uomTask, allMfTask,
                allRoutingTask, allRoutingStepsTask
            );

            // ---- Convert lists to Dictionary for fast lookup ----
            var waitList = waitListTask.Result.Where(x=>x.Setup_Apprvl_time != null).ToList();
            var timeslots = timeslotsTask.Result
                .ToDictionary(t => t.Timeslot_ListId, t => t);

            var parts = partsTask.Result
                .ToDictionary(p => p.PartId, p => p);

            var machines = machinesTask.Result
                .ToDictionary(m => m.MachineId, m => m);

            var shops = shopsTask.Result
                .ToDictionary(s => s.DepartmentId, s => s);

            var allWO = allWOTask.Result
                .ToDictionary(w => w.ProductionPlanId, w => w);

            var translogs = translogsTask.Result;
            var uoms = uomTask.Result.ToDictionary(u => u.UOMId, u => u);
            var mfDict = allMfTask.Result.ToDictionary(x => x.PartId);
            var routingDict = allRoutingTask.Result.GroupBy(x => x.ManufacturedPartId)
                                      .ToDictionary(g => g.Key, g => g.ToList());
            var routingStepDict = allRoutingStepsTask.Result.GroupBy(x => x.RoutingId)
                                                           .ToDictionary(g => g.Key, g => g.ToList());
    //        var stepMachinesDict = allStepMachinesTask.Result
    //.GroupBy(x => x.RoutingStepId)
    //.ToDictionary(g => g.Key, g => g.ToList());



            // ---- Pre-group translogs for fast part lookup ----
            var translogLookup = translogs
                .GroupBy(t => t.Input_Part_NoId)
                .ToDictionary(g => g.Key, g => g.First());

            foreach (var mcWait in waitList)
            {
                // Only items where:
                // Wait_Seq_No == 0 AND setup approval exists
                //if (mcWait.Wait_Seq_No != 0 || mcWait.Setup_Apprvl_time == null)
                //    continue;

                // WO lookup
                if (!allWO.TryGetValue(mcWait.Wo_Id, out var wo))
                    continue;

                // Translog lookup
                Inv_Trans_LogVM translog = null;

                if (!translogLookup.TryGetValue(wo.PartId, out translog))
                {
                    translog = translogs.FirstOrDefault(t => t.Output_Part_No == wo.PartId);
                    if (translog == null)
                        continue;
                }

                var part = parts.GetValueOrDefault(wo.PartId);
                if (!machines.TryGetValue(mcWait.Mc_Id, out var machine))
                    continue;

                var shop = shops.GetValueOrDefault(machine.ShopId);

                // ---- Routing-related calls (still needed per WO) ----
                var mf = mfDict[(int)wo.PartId];
                var routingList = routingDict[mf.ManufacturedPartNoDetailId];
                var routingSteps = routingStepDict[wo.RoutingId];

                var routingStep = routingSteps.FirstOrDefault(r => r.StepId == mcWait.Opr_No_Id);
                if (routingStep == null)
                    continue;

                timeslots.TryGetValue(mcWait.Plan_start_time_Id, out var startTS);

                string uomName = uoms.TryGetValue(mf.UOMId, out var u)
                    ? u.Name
                    : "";

                result.Add(new TempMc_Wait_ListVM
                {
                    ShopName = shop?.Name ?? "",
                    McName = machine?.Name ?? "",
                    WoNumber = wo?.WONumber ?? "",
                    Wo_Id = wo?.ProductionPlanId ?? 0,
                    PartId = wo?.PartId ?? 0,
                    PartNo = $"{part?.PartNo} / {part?.Description}",
                    RoutingName = routingList.First(r => r.RoutingId == wo.RoutingId).RoutingName,
                    OprNoName = routingStep?.StepNumber,
                    WoQnty = wo?.CalcWOQty.ToString(),
                    Opr_No_Id = mcWait.Opr_No_Id,
                    Plan_Qnty = mcWait.Plan_Qnty,
                    ActiveId = mcWait.Mc_Wait_ListId,
                    QntyOffered = mcWait.QntyOffered,
                    Accepted = mcWait.Accepted,
                    NonConQnty = mcWait.NonConQnty,
                    MatlIssued = Convert.ToInt32(translog.Qnty).ToString(),
                    PlanStartStr = startTS?.Start_time.ToString("hh:mm tt") ?? "",
                    SetUpTimeStr = mcWait.Setup_Apprvl_time.Value
                        .ToLocalTime()
                        .ToString("hh:mm tt"),
                    MatlReceptTime = translog.Dt_time.ToString("hh:mm tt"),
                    UomName = uomName,
                    Rework_Wo = 'N'
                });
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBookOutList()
        {
            // 2. Start all parallel tasks
            var bookOutListTask = _woService.GetAllBookOutList();

            var partsTask = _masterService.ItemMasterParts();
            var machinesTask = _machineService.GetMachinesList();
            // var shopsTask = _departmentService.GetDepartments(1); // Optional if MachineVM has ShopName
            var uomTask = _masterService.GetUOMs();

            // Routing Data
            var allWOTask = _woService.AllProductionPlan_Wo(); // Needed to map WO_Id -> PartId/RoutingId
            var allMfTask = _masterService.GetAllManufacturedPartNoDetailList();
            var allRoutingTask = _routingService.AllRoutings();
            var allRoutingStepsTask = _routingService.AllRoutingSteps();

            await Task.WhenAll(
                bookOutListTask, partsTask, machinesTask, uomTask,
                allWOTask, allMfTask, allRoutingTask, allRoutingStepsTask
            );

            var resultList = bookOutListTask.Result.ToList();
            if (!resultList.Any()) return Ok(resultList);

            // 3. Create Lookups
            var parts = partsTask.Result.ToDictionary(p => p.PartId);
            var machines = machinesTask.Result.ToDictionary(m => m.MachineId);
            var uoms = uomTask.Result.ToDictionary(u => u.UOMId);
            var allWO = allWOTask.Result.ToDictionary(w => w.ProductionPlanId);

            var mfDict = allMfTask.Result.GroupBy(x => x.PartId).ToDictionary(g => g.Key, g => g.First());
            var routingDict = allRoutingTask.Result.ToLookup(x => x.ManufacturedPartId);
            var routingStepDict = allRoutingStepsTask.Result.ToLookup(x => x.RoutingId);

            // 4. Enrich Data
            foreach (var item in resultList)
            {
                // A. Machine & Shop
                if (machines.TryGetValue(item.Mc_Id, out var machine))
                {
                    item.McName = machine.Name;
                    item.ShopName = machine.Shop;
                }

                // B. Context via WO
                if (allWO.TryGetValue(item.Wo_Id, out var wo))
                {
                    // Part Details
                    if (parts.TryGetValue(wo.PartId, out var part))
                    {
                        item.PartNo = $"{part.PartNo} / {part.Description}";
                    }

                    // Routing Name & UOM
                    if (mfDict.TryGetValue((int)wo.PartId, out var mf))
                    {
                        // UOM
                        if (uoms.TryGetValue(mf.UOMId, out var u))
                        {
                            item.UomName = u.Name;
                        }

                        // Routing Name
                        var routing = routingDict[mf.ManufacturedPartNoDetailId]
                                        .FirstOrDefault(r => r.RoutingId == wo.RoutingId);
                        item.RoutingName = routing?.RoutingName ?? "";
                    }

                    // Operation/Step Name
                    var step = routingStepDict[wo.RoutingId].FirstOrDefault(r => r.StepId == item.Opr_No_Id);
                    item.OprNoName = step?.StepNumber ?? "";
                }
            }

            return Ok(resultList);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateQntyMc_Wait_List(Mc_Wait_ListVM masterDocListVM)
        {
            var mc_Wait_Lists = await _woService.GetAllMc_Wait_List();
            var mcWait = mc_Wait_Lists.Where(m => m.Mc_Wait_ListId == masterDocListVM.Mc_Wait_ListId).FirstOrDefault();
            if (mcWait != null)
            {
                mcWait.NonConQnty = masterDocListVM.NonConQnty;
                mcWait.Accepted = masterDocListVM.Accepted;
                mcWait.QntyOffered = masterDocListVM.QntyOffered;
                var result = await _woService.PostMc_Wait_List(mcWait);
                return Ok(result);
            }
            return Ok("Not Found McWait");
        }

        [HttpGet]
        public async Task<IActionResult> GetSetupSummaryByShop()
        {

            // 2. Start all Data Fetching Tasks in Parallel (Optimized)
            //    We call the Services directly to avoid the overhead of fetching Parts/Routings in the Controller actions
            var cnfTask = _woService.GetAllSetUpCnfList();       // Waiting
            var approvalTask = _woService.GetAllSetUpApprolList(); // Approval
            var bookoutTask = _woService.GetAllBookOutList();    // Bookout
            var machinesTask = _machineService.GetMachinesList();        // Needed for Shop Name mapping

            await Task.WhenAll(cnfTask, approvalTask, bookoutTask, machinesTask);

            // 3. Extract Results
            var waitingList = cnfTask.Result.ToList();
            var approvalList = approvalTask.Result.ToList();
            var bookoutList = bookoutTask.Result.ToList();

            // 4. Create Machine Lookup for Shop Names (O(1) access)
            var machines = machinesTask.Result.ToDictionary(m => m.MachineId, m => m.Shop);

            // 5. Consolidate all unique Shop Names from the 3 lists
            //    We map Mc_Id to ShopName here on the fly
            var allShops = waitingList.Select(x => machines.GetValueOrDefault(x.Mc_Id, "Unknown"))
                .Union(approvalList.Select(x => machines.GetValueOrDefault(x.Mc_Id, "Unknown")))
                .Union(bookoutList.Select(x => machines.GetValueOrDefault(x.Mc_Id, "Unknown")))
                .Where(s => !string.IsNullOrEmpty(s) && s != "Unknown")
                .Distinct()
                .OrderBy(s => s)
                .ToList();

            // 6. Calculate Counts
            var summaryList = allShops.Select(shop => new
            {
                Shop = shop,
                WaitingForSetupStart = waitingList.Count(x => machines.GetValueOrDefault(x.Mc_Id) == shop),
                ReadyForSetupApproval = approvalList.Count(x => machines.GetValueOrDefault(x.Mc_Id) == shop),
                Bookout = bookoutList.Count(x => machines.GetValueOrDefault(x.Mc_Id) == shop)
            }).ToList();

            return Ok(summaryList);
        }
        [HttpGet]
        public async Task<IActionResult> GetSetupSummaryByShops()
        {
            // Fetch and process ALL data in a single, optimized method
            var fullDataSet = await GetFullWaitListDataSet();

            // The data is already in a structure ready for filtering
            var waitingData = fullDataSet
                //.Where(x => x.Wait_Seq_No == 0) // Equivalent logic from GetAllSetUpCnfList
                .ToList();

            var approvalData = fullDataSet
                .Where(x => x.Setup_Start_time.HasValue && !x.Setup_Apprvl_time.HasValue) // Equivalent logic from GetAllSetUpApprolList
                .ToList();

            var bookoutData = fullDataSet
                .Where(x => x.Wait_Seq_No == 0 && x.Setup_Apprvl_time.HasValue) // Equivalent logic from GetAllBookOutList
                .ToList();

            // Combine all shop names
            var allShops = waitingData.Select(x => x.ShopName)
                                       .Union(approvalData.Select(x => x.ShopName))
                                       .Union(bookoutData.Select(x => x.ShopName))
                                       .Distinct()
                                       .ToList();

            // Prepare final summary result (This remains efficient)
            var summaryList = allShops.Select(shop => new
            {
                Shop = shop,
                WaitingForSetupStart = waitingData.Count(x => x.ShopName == shop),
                ReadyForSetupApproval = approvalData.Count(x => x.ShopName == shop),
                Bookout = bookoutData.Count(x => x.ShopName == shop)
            }).ToList();

            return Ok(summaryList);
        }
        private async Task<List<TempMc_Wait_ListVM>> GetFullWaitListDataSet()
        {
            // 1. Concurrent Fetching: Use Task.WhenAll to execute all service calls in parallel.
            var tasks = new List<Task>
    {
        _woService.GetAllMc_Wait_List(),
        _masterService.ItemMasterParts(),
        _machineService.GetMachinesList(),
        _departmentService.GetDepartments(1),
        _woService.AllProductionPlan_Wo(),
        _woService.GetAllInv_Trans_Log(),
        _woService.GetAllTimeslot_List(),
        _masterService.GetUOMs() // Assuming GetAllBookOutList needs this
        // Note: GetAllTempMc_Timeslot_List is unused in the final loop logic
    };

            await Task.WhenAll(tasks);

            // 2. Extract Results
            var waitList = (await (Task<IEnumerable<Mc_Wait_ListVM>>)tasks[0]).ToList();
            var parts = (await (Task<IEnumerable<ItemMasterPartVM>>)tasks[1]).ToList();
            var machines = (await (Task<IEnumerable<MachineListVM>>)tasks[2]).ToList();
            var shops = (await (Task<IEnumerable<ShopDepartmentVM>>)tasks[3]).ToList();
            var allWO = (await (Task<IEnumerable<ProductionPlan_WoVM>>)tasks[4]).ToList();
            var translogs = (await (Task<IEnumerable<Inv_Trans_LogVM>>)tasks[5]).ToList();
            var allTimeSlots = (await (Task<IEnumerable<Timeslot_ListVM>>)tasks[6]).ToList();
            var uoms = (await (Task<IEnumerable<UOMVM>>)tasks[7]).ToList();

            // 3. Pre-Caching for Fast Lookups (O(1) access inside the loop)
            // Key-value lookups are much faster than FirstOrDefault on a list
            var partCache = parts.ToDictionary(p => p.PartId, p => p);
            var machineCache = machines.ToDictionary(m => m.MachineId, m => m);
            var shopCache = shops.ToDictionary(s => s.DepartmentId, s => s);
            var woCache = allWO.ToDictionary(w => w.ProductionPlanId, w => w);
            var timeslotCache = allTimeSlots.ToDictionary(t => t.Timeslot_ListId, t => t);
            var uomCache = uoms.ToDictionary(u => u.UOMId, u => u.Name);

            // Grouping translogs by PartId for faster lookup (since a part may have multiple translogs)
            // We only need the first one found for the material receipt time/quantity.
            var translogCache = translogs
                //.Where(t => t.Input_Part_NoId || t.Output_Part_No)
                .GroupBy(t => t.Input_Part_NoId > 0 ? t.Input_Part_NoId : t.Output_Part_No)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault());

            var result = new List<TempMc_Wait_ListVM>();

            // 4. Single Pass Processing
            foreach (var mcWait in waitList)
            {
                // Avoid slow, repeated async calls inside the loop.
                // Lookups replaced with dictionary access.

                if (!woCache.TryGetValue(mcWait.Wo_Id, out var wo) || wo == null) continue;

                // Use the PartId from WO for translog lookup
                if (!translogCache.TryGetValue(wo.PartId, out var translog) || translog == null) continue;

                // Simplified, safe lookups
                partCache.TryGetValue(wo.PartId, out var part);
                machineCache.TryGetValue(mcWait.Mc_Id, out var machine);

                // Nested lookups
                shopCache.TryGetValue(machine?.ShopId ?? 0, out var shop);
                timeslotCache.TryGetValue(mcWait.Plan_start_time_Id, out var currentTimeslot);

                var matltime = translog.Dt_time.ToString("hh:mm tt");
                var planStartStr =currentTimeslot.Start_time.ToString("hh:mm tt")
                    ?? "";

                // *** AWAITABLE CALLS IN LOOP ***
                // These are the last remaining blocking calls that need to be addressed.
                // It's likely better to fetch ALL routing data outside the loop if possible.
                // For now, they remain as they depend on wo.PartId and wo.RoutingId, 
                // which would require a major refactor (e.g., getting ALL routing steps/lists).

                // If you can't pre-fetch all routing data:
                var mf = await _masterService.GetManufPart((int)wo.PartId);
                var routingList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                var routing = await _routingService.RoutingSteps((int)wo.RoutingId);
                var routingstep = routing.FirstOrDefault(r => r.StepId == mcWait.Opr_No_Id);
                var stepMachines = routingstep != null ? await _routingService.StepMachines((int)routingstep.StepId) : new List<RoutingStepMachineVM>();
                // *******************************

                var routingName = routingList.FirstOrDefault(r => r.RoutingId == wo.RoutingId)?.RoutingName ?? "";
                var plannedSetupTime = stepMachines.FirstOrDefault()?.SetupTime ?? "";
                var uomName = mf != null && uomCache.TryGetValue(mf.UOMId, out var name) ? name : "";


                // Build the result object (includes all fields used across the 3 original methods)
                result.Add(new TempMc_Wait_ListVM
                {
                    ShopName = shop?.Name ?? "",
                    McName = machine?.Name ?? "",
                    WoNumber = wo?.WONumber ?? "",
                    Wo_Id = wo?.WoId ?? 0,
                    PartId = (long)(wo?.PartId),
                    PartNo = part?.PartNo + (part != null ? " / " + part.Description : ""),
                    RoutingName = routingName,
                    OprNoName = routingstep?.StepNumber ?? "",
                    WoQnty = wo?.CalcWOQty.ToString() ?? "0",
                    Opr_No_Id = mcWait.Opr_No_Id,
                    Plan_Qnty = mcWait.Plan_Qnty,
                    ActiveId = mcWait.Mc_Wait_ListId,
                    QntyOffered = mcWait.QntyOffered,
                    Accepted = mcWait.Accepted,
                    NonConQnty = mcWait.NonConQnty,
                    MatlIssued = Convert.ToInt32(translog.Qnty).ToString() ?? "0",
                    PlanStartStr = planStartStr,
                    MatlReceptTime = matltime,
                    UomName = uomName,
                    PlannedSetupTime = plannedSetupTime,
                    Rework_Wo = 'N',
                    // Crucially, include the fields needed for final filtering in GetSetupSummaryByShop
                    Wait_Seq_No = mcWait.Wait_Seq_No,
                    Setup_Start_time = mcWait.Setup_Start_time,
                    Setup_Apprvl_time = mcWait.Setup_Apprvl_time,
                    // Add SetUpTimeStr for approval/bookout lists
                    SetUpTimeStr = mcWait.Setup_Apprvl_time.HasValue
                        ? TimeZoneInfo.ConvertTimeFromUtc(mcWait.Setup_Apprvl_time.Value, TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata"))
                              .ToString("hh:mm tt")
                        : (mcWait.Setup_Start_time.HasValue ? mcWait.Setup_Start_time.Value.ToString("hh:mm tt") : "")
                });
            }

            return result;
        }
        [HttpGet]
        public async Task<IActionResult> DeleteMatl_Issue_List(long itemMasterDocListId)
        {
            var result = await _woService.DeleteMatl_Issue_List(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNon_Plan_Wk_ListData()
        {
            var result = await _woService.GetAllNon_Plan_Wk_List();
            var machines = await _machineService.GetMachinesList();
            var mcWaitList = await _woService.GetAllMc_Wait_List();
            foreach (var item in result)
            {
                var machine = machines.Where(m => m.MachineId == item.Mc_Id).FirstOrDefault();
                item.MachineName = machine?.Name ?? "Unknown";
                item.ShopName = machine?.Shop ?? "Unknown";
                item.ReqDateStr = item.Plan_start_time.ToString("dd-MM-yyyy hh:mm tt");
                item.PlannedStr = item.PlannedDate.ToString("dd-MM-yyyy");
                if(mcWaitList.Any(m=>m.Non_Plan_wk_Id == item.Non_Plan_Wk_ListId))
                {
                    item.InProcess = "Y";
                }
                else
                {
                    item.InProcess = "N";
                }

            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNon_Plan_Wk_List()
        {
            var result = await _woService.GetAllNon_Plan_Wk_List();
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNon_Plan_Wk_type_List()
        {
            var result = await _woService.GetAllNon_Plan_Wk_type_List();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostNon_Plan_Wk_List(Non_Plan_Wk_ListVM masterDocListVM)
        {
            var result = await _woService.PostNon_Plan_Wk_List(masterDocListVM);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteNon_Plan_Wk_List(long itemMasterDocListId)
        {
            var result = await _woService.DeleteNon_Plan_Wk_List(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRwk_List()
        {
            var result = await _woService.GetAllRwk_List();
            var nclogs = await _woService.GetAllNcLog();
            var recpts = await _woService.GetAllInw_Recpt_Header();
            var podetails = await _woService.GetAllPodetails();
            var procplans = await _woService.GetAllProcPlan();
            var prodnwos = await _woService.AllProductionPlan_Wo();
            var masterparts = await _masterService.ItemMasterParts();
            var machines = await _machineService.GetMachinesList();
            var mcWaitList = await _woService.GetAllTempMc_Wait_List();

            foreach (var item in result)
            {
                // ---- NC Log ----
                var nclog = nclogs.FirstOrDefault(n => n.Insp_Outcome_Details_Id == item.NC_Log_Id);
                if (nclog == null) continue;

                // ---- Receipt ----
                var recpt = recpts.FirstOrDefault(r => r.Inw_Recpt_HeaderId == nclog.Inw_Recpt_Header_Id);
                if (recpt == null) continue;

                // ---- PO Detail ----
                var podetail = podetails.FirstOrDefault(p => p.PoDetailsId == recpt.PoHeaderId);
                if (podetail == null) continue;

                // ---- Process Plan ----
                var procplan = procplans.FirstOrDefault(pp => pp.ProcPlanId == podetail.ProcPlanId);
                if (procplan == null) continue;

                // ---- Work Order ----
                var prodnwo = prodnwos.FirstOrDefault(w => w.WoId == procplan.WorkOrderId);
                if (prodnwo == null) continue;

                // ---- Master Part ----
                var masterprt = masterparts.FirstOrDefault(m => m.PartId == prodnwo.PartId);

                item.WoNumber = prodnwo.WONumber ?? "";
                item.PartNo = masterprt != null
                    ? $"{masterprt.PartNo} / {masterprt.Description}"
                    : "";

                item.RwkQnty = (int)(nclog.NC_Qnty);
                item.Planndt = prodnwo.WODate?.ToString("dd-MM-yyyy") ?? "";
                item.RecdComplDt = prodnwo.PlanCompletionDate?.ToString("dd-MM-yyyy") ?? "";

                // ---- Machine ----
                var machine = machines.FirstOrDefault(m => m.MachineId == item.Mc_Id);
                item.MachineName = machine?.Name ?? "";
                item.ShopName = machine?.Shop ?? "";

                // ---- In Process ----
                item.InProcess = mcWaitList.Any(m => m.Wo_Id == prodnwo.ProductionPlanId) ? "Y" : "N";

                // ---- Plan Duration ----
                item.Plan_Duration ??= string.Empty;

                // ---- Manufactured Part ----
                var mf = await _masterService.GetManufPart((int)prodnwo.PartId);
                if (mf != null)
                {
                    item.PartType = mf.ManufacturedPartType;

                    var routingList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                    var routing = routingList
                        .FirstOrDefault(r => r.RoutingId == prodnwo.RoutingId);

                    item.RoutingName = routing?.RoutingName ?? "";
                }
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostRwk_List(Rwk_ListVM masterDocListVM)
        {
            var rwk_Lists = await _woService.GetAllRwk_List();
            var rwkList = rwk_Lists.Where(r => r.Rwk_ListId == masterDocListVM.Rwk_ListId).FirstOrDefault();
            rwkList.RequiredStartTime = masterDocListVM.RequiredStartTime;
            rwkList.Mc_Id = masterDocListVM.Mc_Id;
            rwkList.Plan_Duration = masterDocListVM.Plan_Duration;
            rwkList.Actual_Start_Time = masterDocListVM.Actual_Start_Time;
            rwkList.Actual_end_Time = masterDocListVM.Actual_end_Time;
            rwkList.Actual_Duration = masterDocListVM.Actual_Duration;
            rwkList.Closure_Comment = masterDocListVM.Closure_Comment;
            var result = await _woService.PostRwk_List(rwkList);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRwk_List(long itemMasterDocListId)
        {
            var result = await _woService.DeleteRwk_List(itemMasterDocListId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInventory_Master(Inventory_MasterVM masterDocListVM)
        {
            var findInv = await _woService.GetAllInventory_Master();
            var update = findInv.Where(i => i.Opr_No_Id == masterDocListVM.Opr_No_Id).FirstOrDefault();
            if(update != null)
            {
                update.Current_QntOnHand = masterDocListVM.Current_QntOnHand;
                update.Opr_No_Id = masterDocListVM.Opr_No_Id;
                var result = await _woService.PostInventory_Master(update);
                return Ok(result);
            }
            return Ok(masterDocListVM);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllShop_Insp_Log()
        {
            var result = await _woService.GetAllShop_Insp_Log();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostShop_Insp_Log(Shop_Insp_LogVM masterDocListVM)
        {
            var result = await _woService.PostShop_Insp_Log(masterDocListVM);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteShop_Insp_Log(long itemMasterDocListId)
        {
            var result = await _woService.DeleteShop_Insp_Log(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSubCon_List()
        {
            var result = await _woService.GetAllSubCon_List();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostSubCon_List(SubCon_ListVM masterDocListVM)
        {
            var result = await _woService.PostSubCon_List(masterDocListVM);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSubCon_List(long itemMasterDocListId)
        {
            var result = await _woService.DeleteSubCon_List(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTempSubCon_List()
        {
            var result = await _woService.GetAllTempSubCon_List();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTempSubCon_List(TempSubCon_ListVM masterDocListVM)
        {
            var subs = await _woService.GetAllTempSubCon_List();
            var sub = subs.Where(s => s.TempSubCon_ListId == masterDocListVM.TempSubCon_ListId).FirstOrDefault();
            sub.Plan_Disp_date = masterDocListVM.Plan_Disp_date;
            sub.Plan_Recpt_date = masterDocListVM.Plan_Recpt_date;
            sub.Changed = 1;
            var result = await _woService.PostTempSubCon_List(sub);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTempSubCon_List(long itemMasterDocListId)
        {
            var result = await _woService.DeleteTempSubCon_List(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOpr_List()
        {
            var result = await _woService.GetAllOpr_List();
            var prodnWos = await _woService.AllProductionPlan_Wo();
            foreach (var item in result)
            {
                var prodwo = prodnWos.Where(w => w.ProductionPlanId == item.Wo_Id).FirstOrDefault();
                var oprs =await _routingService.RoutingSteps((int)prodwo.RoutingId);
                var opr = oprs.Where(o => o.StepId == item.Opr_No).FirstOrDefault();
                if (opr != null)
                {
                    item.OprName = opr.StepNumber ?? "Unknown";
                    item.ModeName = "Test";
                    item.IniTpt = item.Initial_Opr_TPT.ToString();
                    item.RolledTpt = item.Rolledup_Opr_TPT.ToString();
                }
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostOpr_List(Opr_ListVM masterDocListVM)
        {
            var result = await _woService.PostOpr_List(masterDocListVM);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteOpr_List(long itemMasterDocListId)
        {
            var result = await _woService.DeleteOpr_List(itemMasterDocListId);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> ClearTemp()
        {
            var opr_Lists = await _woService.GetAllTempOpr_List();
            foreach (var item in opr_Lists)
            {
                var delopr = await _woService.DeleteTempOpr_List(item.TempOpr_ListId);
            }
            var tempWO_Wait_Lists = await _woService.GetAllTempWo_Wait_List();
            foreach (var item in tempWO_Wait_Lists)
            {
                var delopr = await _woService.DeleteTempWo_Wait_List(item.TempWO_Wait_ListId);
            }
            var tempSubCon_Lists = await _woService.GetAllTempSubCon_List();
            foreach (var item in tempSubCon_Lists)
            {
                var delopr = await _woService.DeleteTempSubCon_List(item.TempSubCon_ListId);
            }
            var tempMc_Wait_Lists = await _woService.GetAllTempMc_Wait_List();
            foreach (var item in tempMc_Wait_Lists)
            {
                var delopr = await _woService.DeleteTempMc_Wait_List(item.TempMc_Wait_ListId);
            }
            return Ok(opr_Lists);
        }

        [HttpPost]
        public async Task<IActionResult> SimulateWOAllocation([FromBody]List<int> selectedWOIds)
        {
            var productions = await _woService.AllProductionPlan_Wo();
            var allOprs = new List<TempOpr_ListVM>();
            var allTimeslots = await _woService.GetAllTimeslot_List();
            var allMcTimeslots = await _woService.GetAllTempMc_Timeslot_List();
            var allmac = await _machineService.GetMachinesList();
            var reworkList = await _woService.GetAllRwk_List();
            var nclogs = await _woService.GetAllNcLog();
            var recpts = await _woService.GetAllInw_Recpt_Header();
            var podetails = await _woService.GetAllPodetails();
            var procplans = await _woService.GetAllProcPlan();
            var prodnwos = productions;
            List<TempWO_Wait_ListVM> readyWOs = new List<TempWO_Wait_ListVM>();
            SimulationState.IsStopped = false;

            var pendingReworks = reworkList
                .Where(rwk => (rwk.Allocated == 'N' || rwk.Allocated == ' ' || rwk.Allocated == '\0') && rwk.Mc_Id > 0)
                .ToList();

            foreach (var rwk in pendingReworks)
            {
                await CheckPauseAsync();
                var machine = await _machineService.GetMachine(rwk.Mc_Id);
                if (machine == null) continue;

                var plant = await _plantService.GetPlantWD(machine.MachinePlantId);
                if (plant == null) continue;

                int timeslotDuration = plant.Timeslot_duration;
                rwk.Plan_Duration = "00:" + rwk.Plan_Duration;
                int durationInMinutes = (int)TimeSpan.Parse(rwk.Plan_Duration).TotalMinutes;
                int requiredSlots = (int)Math.Ceiling((double)durationInMinutes / timeslotDuration);

                var timeslots = allTimeslots
                    .Where(t => t.PlantId == machine.MachinePlantId && t.Start_time >= DateTime.Now && t.Break_Slot != 'Y')
                    .OrderBy(t => t.Start_time)
                    .ToList();
                var nclog = nclogs.Where(n => n.Insp_Outcome_Details_Id == rwk.NC_Log_Id).FirstOrDefault();
                var recpt = recpts.Where(r => r.Inw_Recpt_HeaderId == nclog.Inw_Recpt_Header_Id).FirstOrDefault();
                var podetail = podetails.Where(p => p.PoDetailsId == recpt.PoHeaderId).FirstOrDefault();
                var procplan = procplans.Where(pp => pp.ProcPlanId == podetail.ProcPlanId).FirstOrDefault();
                var prodnwo = prodnwos.Where(w => w.WoId == procplan.WorkOrderId).FirstOrDefault();
                var availableSlots = timeslots.Take(requiredSlots).ToList();
                if (availableSlots.Count < requiredSlots) continue;

                // Create Mc_Wait_List entry
                var mcWait = new TempMc_Wait_ListVM
                {
                    Wo_Id = prodnwo.ProductionPlanId,
                    Opr_No_Id = prodnwo.StartingOpNo,
                    Mc_Id = rwk.Mc_Id,
                    Wait_Seq_No = await CalculateNextTempWaitSeqNo(rwk.Mc_Id),
                    Plan_start_time_Id = availableSlots.First().Timeslot_ListId,
                    Plan_end_time_Id = availableSlots.Last().Timeslot_ListId,
                    Mc_TPT = durationInMinutes,
                    Rework_Wo = 'Y',
                    Mode = 1,
                    Plan_Qnty = (long)nclog.NC_Qnty
                };
                mcWait = await _woService.PostTempMc_Wait_List(mcWait);

                // Create Mc_Timeslot_List entry
                var newMcSlot = new TempMc_Timeslot_ListVM
                {
                    Mc_Id = rwk.Mc_Id,
                    Timeslot_List_Id = availableSlots.First().Timeslot_ListId,
                    EndTimeslot_List_Id = availableSlots.Last().Timeslot_ListId,
                    Mc_Wait_List_Id = mcWait.TempMc_Wait_ListId,
                    Allocation = 2, // Rework
                    Slot_Not_Avl = 'N',
                    Not_Avl_reason = 0
                };
                await _woService.PostTempMc_Timeslot_List(newMcSlot);

                // Update the Rework entry
                rwk.Allocated = 'Y';
                await _woService.PostRwk_List(rwk);
            }
            var nonPlanList = await _woService.GetAllNon_Plan_Wk_List(); // Allocated = 'N' and Mc_Id != null
            foreach (var np in nonPlanList.Where(n => n.Allocated == 'N' || n.Allocated == ' ' || n.Allocated == '\0' && n.Mc_Id > 0))
            {
                await CheckPauseAsync();

                var machine = await _machineService.GetMachine(np.Mc_Id);
                if (machine == null) continue;

                var plant = await _plantService.GetPlantWD(machine.MachinePlantId);
                if (plant == null) continue;

                int timeslotDuration = plant.Timeslot_duration;
                np.Plan_Duration = "00:" + np.Plan_Duration;
                int durationInMinutes = (int)TimeSpan.Parse(np.Plan_Duration).TotalMinutes;
                int requiredSlots = (int)Math.Ceiling((double)durationInMinutes / timeslotDuration);

                var timeslots = allTimeslots
                    .Where(t => t.PlantId == machine.MachinePlantId && t.Start_time >= np.Plan_start_time && t.Break_Slot != 'Y')
                    .OrderBy(t => t.Start_time)
                    .ToList();

                var availableSlots = timeslots.Take(requiredSlots).ToList();
                if (availableSlots.Count < requiredSlots) continue;

                // Create Mc_Wait_List record
                var mcWait = new TempMc_Wait_ListVM
                {
                    Wo_Id = 0,
                    Non_Plan_Wk = 'Y',
                    Non_Plan_wk_Id = np.Non_Plan_Wk_ListId,
                    Opr_No_Id = 0,
                    Mc_Id = np.Mc_Id,
                    Wait_Seq_No = await CalculateNextTempWaitSeqNo(np.Mc_Id),
                    Plan_start_time_Id = availableSlots.First().Timeslot_ListId,
                    Plan_end_time_Id = availableSlots.Last().Timeslot_ListId,
                    Mc_TPT = durationInMinutes,
                    Mode = 9, // Non-Plan mode
                    Plan_Qnty = 0
                };
                mcWait = await _woService.PostTempMc_Wait_List(mcWait);

                // Allocate in Mc_Timeslot_List
                var newMcSlot = new TempMc_Timeslot_ListVM
                {
                    Mc_Id = np.Mc_Id,
                    Timeslot_List_Id = availableSlots.First().Timeslot_ListId,
                    EndTimeslot_List_Id = availableSlots.Last().Timeslot_ListId,
                    Mc_Wait_List_Id = mcWait.TempMc_Wait_ListId,
                    Allocation = 1, // Non-plan work
                    Slot_Not_Avl = 'N',
                    Not_Avl_reason = 0
                };
                await _woService.PostTempMc_Timeslot_List(newMcSlot);

                // Mark the non-plan record as allocated
                np.Allocated = 'Y';
                await _woService.PostNon_Plan_Wk_List(np);
            }

            // Step 1: Create WO_Wait_List and Opr_List entries
            foreach (var woId in selectedWOIds)
            {
                await CheckPauseAsync();

                var item = productions.FirstOrDefault(p => p.ProductionPlanId == woId);
                if (item == null) continue;

                var addData = new TempWO_Wait_ListVM
                {
                    Wo_Id = item.ProductionPlanId,
                    Mode = 1,
                    Allow_Routing_Chg = 'Y',
                    Total_TPT = 0,
                    Rework_Wo = 'N',
                    NC_Log_Ref = 0,
                    WO_Wait_Seq_No = 0,
                    Plan_Start_Date = item.PlanStartDate,
                    Plan_End_Date = (DateTime)item.PlanCompletionDate,
                    Plan_Simul_Qnty = item.CalcWOQty
                };

                var woResult = await _woService.PostTempWo_Wait_List(addData);
                readyWOs.Add(woResult);

                var mf = await _masterService.GetManufPart((int)item.PartId);
                var route = (await _routingService.Routings(mf.ManufacturedPartNoDetailId))
                            .FirstOrDefault(r => r.RoutingId == item.RoutingId);
                if (route == null) continue;

                var routingSteps = await _routingService.RoutingSteps(route.RoutingId);

                foreach (var step in routingSteps)
                {
                    await CheckPauseAsync();

                    var tptResult = await GetTPT(step);
                    var tptStepItem = (RoutingStepVM)((OkObjectResult)tptResult).Value;
                    int setup = int.Parse(tptStepItem.SetupTime ?? "0");
                    int firstPiece = int.Parse(tptStepItem.FirstPieceTime ?? "0");
                    int cycle = int.Parse(tptStepItem.CycleTime ?? "0");
                    int quantity = item.CalcWOQty;

                    var opr = new TempOpr_ListVM
                    {
                        Wo_Id = item.ProductionPlanId,
                        Opr_No = step.StepId,
                        Mode = 1,
                        Rework_Wo = 'N',
                        NC_Log_Ref = 0,
                        Act_Qnty = 0,
                        Plan_Qnty = quantity,
                        No_of_Simult_Mcs = step.NumberOfSimMachines,
                        Initial_Opr_TPT = setup + firstPiece + cycle * (quantity - 1)
                    };
                    var oprResult = await _woService.PostTempOpr_List(opr);
                    allOprs.Add(oprResult);
                }
            }

            // Step 2: Recalculate Total TPT and Sequence WOs
            foreach (var wo in readyWOs)
            {
                var oprs = allOprs.Where(o => o.Wo_Id == wo.Wo_Id).OrderBy(o => o.Opr_No).ToList();
                double totalTPT = oprs.Sum(o => o.Initial_Opr_TPT);
                wo.Total_TPT = Convert.ToInt32(totalTPT);
                await _woService.PostTempWo_Wait_List(wo);
            }

            // Step 3: Sequence WOs
            var sequencedWOs = readyWOs.OrderBy(w => w.Plan_End_Date).ThenBy(w => w.Total_TPT).ToList();
            for (int i = 0; i < sequencedWOs.Count; i++)
            {
                sequencedWOs[i].WO_Wait_Seq_No = i ;
                await _woService.PostTempWo_Wait_List(sequencedWOs[i]);
            }

            // Step 4: Allocate timeslots
            foreach (var wo in sequencedWOs)
            {
                await CheckPauseAsync();
                var oprs = allOprs.Where(o => o.Wo_Id == wo.Wo_Id).OrderBy(o => o.Opr_No).ToList();
                var prodwo = productions.First(p => p.ProductionPlanId == wo.Wo_Id);
                var routingSteps = await _routingService.RoutingSteps((int)prodwo.RoutingId);

                foreach (var opr in oprs)
                {
                    await CheckPauseAsync();
                    var step = routingSteps.FirstOrDefault(s => s.StepId == opr.Opr_No);
                    var oprMachines = await _routingService.StepMachines((int)opr.Opr_No);
                    if (step == null) continue;

                    int noOfSimultMcs = step.NumberOfSimMachines;
                    if (step.StepLocation == "2") // SubCon
                    {
                        var subconList = await _routingService.SubCons((int)opr.Opr_No);
                        var subtransport = subconList.FirstOrDefault(s => s.PreferredSubcon == 1) ?? subconList.FirstOrDefault();

                        if (subtransport != null)
                        {
                            var subconwss = await _routingService.SubConWSS((int)opr.Opr_No, subtransport.SubConDetailsId);
                            var subconws = subconwss.FirstOrDefault();
                            // Time calculations
                            var trans = TimeSpan.Parse(subtransport.TransportTime); // "01:00:00"
                            var setupTime = TimeSpan.Parse(subconws.SetupTime); // "01:00:00"
                            var floorToFloorTime = TimeSpan.Parse(subconws.FloorToFloorTime);
                            int qty = wo.Plan_Simul_Qnty;
                            double totalMinutes = trans.TotalMinutes + setupTime.TotalMinutes + (floorToFloorTime.TotalMinutes * qty);

                            // Get plant working duration per day from any machine's plant
                            var anyMachine = await _machineService.GetMachine(allmac.FirstOrDefault(m=>m.MachineTypeId == subconws.MachineType).MachineId);
                            var plant = await _plantService.GetPlantWD(anyMachine.MachinePlantId);

                            int totalWorkingMinutes = 0;

                            if (plant.NoOfShifts >= 1)
                                totalWorkingMinutes += (int)TimeSpan.Parse(plant.FirstShiftDuration).TotalMinutes;
                            if (plant.NoOfShifts >= 2)
                                totalWorkingMinutes += (int)TimeSpan.Parse(plant.SecondShiftDuration).TotalMinutes;
                            if (plant.NoOfShifts == 3)
                                totalWorkingMinutes += (int)TimeSpan.Parse(plant.ThirdShiftDuration).TotalMinutes;

                            int slotsPerDay = totalWorkingMinutes / plant.Timeslot_duration;
                            int subconDays = (int)Math.Ceiling(totalMinutes / totalWorkingMinutes);


                            // Timeslot allocation logic
                            DateTime today = DateTime.Now.Date;
                            var futureSlots = allTimeslots
                                .Where(t => t.Start_time.Date > today)
                                .OrderBy(t => t.Start_time)
                                .ToList();

                            DateTime subconStartDate = futureSlots.First().Start_time.Date;
                            DateTime subconEndDate = subconStartDate.AddDays(subconDays - 1);

                            long? startId = futureSlots
                                .FirstOrDefault(t => t.Start_time.Date == subconStartDate)?.Timeslot_ListId;
                            long? endId = futureSlots
                                .Where(t => t.Start_time.Date == subconEndDate)
                                .LastOrDefault()?.Timeslot_ListId;
                            DateTime planDispDate;
                            if (opr.Opr_No== prodwo.StartingOpNo)
                            {
                                // SubCon is first operation
                                planDispDate = DateTime.Now.AddDays(1);
                            }
                            else
                            {
                                // SubCon comes after in-house Opr
                                var currentIndex = oprs.FindIndex(o => o.Opr_No == opr.Opr_No);
                                var prevOpr = currentIndex > 0 ? oprs[currentIndex - 1] : null; 
                                if (prevOpr != null)
                                {
                                    var endTimeslot = allTimeslots.FirstOrDefault(t => t.Timeslot_ListId == prevOpr.Shop_Plan_end_time);
                                    if (endTimeslot != null)
                                        planDispDate = endTimeslot.End_time.AddDays(1);
                                    else
                                        planDispDate = DateTime.Now.AddDays(1);
                                }
                                else
                                {
                                    planDispDate = DateTime.Now.AddDays(1);
                                }
                            }
                            var tempSubCon_List = new TempSubCon_ListVM
                            {
                                Wo_Id = wo.Wo_Id,
                                Opr_No = opr.Opr_No,
                                Supplier_Id = subtransport.SupplierId,
                                Rework_Wo = 'N',
                                Loaded = 'N',
                                Mode = 1,
                                Changed=0,
                                Plan_Disp_date = planDispDate,
                                Plan_Qnty = wo.Plan_Simul_Qnty
                            };
                            var stepConvTime = subconws.FloorToFloorTime; // e.g., "01:00:00"
                            if (TimeSpan.TryParse(stepConvTime, out TimeSpan convTime))
                            {
                                tempSubCon_List.Plan_Recpt_date = planDispDate.Add(convTime);
                            }
                            else
                            {
                                tempSubCon_List.Plan_Recpt_date = planDispDate;
                            }
                            await _woService.PostTempSubCon_List(tempSubCon_List);
                            if (startId.HasValue && endId.HasValue)
                            {
                                opr.Subcon_plan_start_time = startId.Value;
                                opr.Subcon_plan_end_time = endId.Value;
                                opr.Rolledup_Opr_TPT = opr.Initial_Opr_TPT;

                                await _woService.PostTempOpr_List(opr);
                            }
                        }
                    }
                    else
                    {
                        int noOfMcs = noOfSimultMcs;
                        int qtyPerMc = wo.Plan_Simul_Qnty / noOfMcs;
                        var machines = await _routingService.StepMachines((int)opr.Opr_No);
                        List<TempMc_Wait_ListVM> mcWaits = new List<TempMc_Wait_ListVM>();
                        var insttempMCTime = new TempMc_Timeslot_ListVM();
                        foreach (var mc in machines.Take(noOfMcs))
                        {
                            await CheckPauseAsync();
                            var setupTime = TimeSpan.Parse(mc.SetupTime); // e.g., "00:15:00"
                            var floorToFloorTime = TimeSpan.Parse(mc.FloorToFloorTime);
                            var tpt = setupTime.TotalMinutes + (floorToFloorTime.TotalMinutes * qtyPerMc); // total time in minutes
                            var getmachine = await _machineService.GetMachine(mc.MachineId);
                            var plantwd = await _plantService.GetPlantWD(getmachine.MachinePlantId);
                            int slotsRequired = (int)Math.Ceiling(tpt / plantwd.Timeslot_duration);

                            var plantSlots = allTimeslots
                                .Where(t => t.PlantId == getmachine.MachinePlantId && t.Break_Slot !='Y')
                                .OrderBy(t => t.Timeslot_ListId)
                                .ToList();
                            allMcTimeslots = await _woService.GetAllTempMc_Timeslot_List();
                            var existingMcSlots = allMcTimeslots
                                .Where(s => s.Mc_Id == mc.MachineId && s.Allocation == 2)
                                .OrderByDescending(s => s.EndTimeslot_List_Id)
                                .ToList();
                            long startFromTimeslotId = 0;
                            if (existingMcSlots.Any())
                            {
                                startFromTimeslotId = existingMcSlots.First().EndTimeslot_List_Id;
                            }
                            var availableTimeslots = plantSlots
                                .Where(t => t.Timeslot_ListId > startFromTimeslotId || t.Start_time > DateTime.Now)
                                .Take(slotsRequired)
                                .ToList();

                            //var mcTimeslots = allMcTimeslots
                            //    .Where(s => s.Mc_Id == mc.MachineId && s.Allocation == 1 && s.Slot_Not_Avl != 'Y')
                            //    .OrderBy(s => s.Timeslot_List_Id) // use Timeslot_List_Id order
                            //    .ToList();

                            //var startIdx = mcTimeslots.FindIndex(s =>
                            //{
                            //    var slotTime = allTimeslots.FirstOrDefault(t => t.Timeslot_ListId == s.Timeslot_List_Id)?.Start_time;
                            //    return slotTime.HasValue && slotTime.Value > DateTime.Now;
                            //});

                            //if (startIdx >= 0 && (startIdx + slotsRequired) <= mcTimeslots.Count)
                            //{
                            //    var availableSlots = mcTimeslots.Skip(startIdx).Take(slotsRequired).ToList();

                                var mcWait = new TempMc_Wait_ListVM
                                {
                                    Wo_Id = wo.Wo_Id,
                                    Opr_No_Id = opr.Opr_No,
                                    Mc_Id = mc.MachineId,
                                    Wait_Seq_No = await CalculateNextTempWaitSeqNo(mc.MachineId),
                                    Plan_start_time_Id = availableTimeslots.First().Timeslot_ListId,
                                    Plan_end_time_Id = availableTimeslots.Last().Timeslot_ListId,
                                    Mc_TPT = Convert.ToDecimal(tpt),
                                    Mode = 1,
                                    Plan_Qnty = qtyPerMc
                                };

                                mcWait = await _woService.PostTempMc_Wait_List(mcWait);

                                //foreach (var slot in availableSlots)
                                //{
                                    await CheckPauseAsync();
                                    var newMcSlot = new TempMc_Timeslot_ListVM
                                    {
                                        Mc_Id = mc.MachineId,
                                        Timeslot_List_Id = availableTimeslots.First().Timeslot_ListId,
                                        EndTimeslot_List_Id = availableTimeslots.Last().Timeslot_ListId, // if applicable
                                        Mc_Wait_List_Id = mcWait.TempMc_Wait_ListId, // Will set after mcWait is created
                                        Allocation = 2,
                                        Slot_Not_Avl = 'N',
                                        Not_Avl_reason = 0// set appropriately
                                    };
                                    insttempMCTime = await _woService.PostTempMc_Timeslot_List(newMcSlot);
                                //}

                                mcWaits.Add(mcWait);
                            //}
                        }

                        // Update Opr_List with aggregated info
                        if (!mcWaits.Any())
                            continue;
                        opr.Shop_Plan_start_time = mcWaits.Min(mw => mw.Plan_start_time_Id);
                        opr.Shop_Plan_end_time = mcWaits.Max(mw => mw.Plan_end_time_Id);
                        opr.Rolledup_Opr_TPT = mcWaits.Sum(mw => Convert.ToInt64(mw.Mc_TPT));
                        await _woService.PostTempOpr_List(opr);

                        // Calculate Mc_Wait_List.Next_Opr_Start_time_ID
                        foreach (var mcWait in mcWaits)
                        {
                            var startSlot = insttempMCTime;
                            var startTime = allTimeslots.FirstOrDefault(t => t.Timeslot_ListId == startSlot.Timeslot_List_Id)?.Start_time ?? DateTime.Now;

                            // Find the corresponding machine again
                            var mc = machines.First(m => m.MachineId == mcWait.Mc_Id);

                            var setupTime = TimeSpan.Parse(mc.SetupTime);
                            var partTime = TimeSpan.FromMinutes(TimeSpan.Parse(mc.FloorToFloorTime).TotalMinutes * Math.Min(qtyPerMc, 1));
                            var requiredTime = setupTime + partTime;

                            var nextSlotTime = startTime.Add(requiredTime);
                            var nextSlot = allTimeslots.FirstOrDefault(t => t.Start_time >= nextSlotTime);

                            mcWait.Next_Opr_Start_time_Id = nextSlot?.Timeslot_ListId ?? mcWait.Plan_end_time_Id;
                            await _woService.PostTempMc_Wait_List(mcWait);
                        }

                    }
                }

                // Update final WO Start & End Date
                allMcTimeslots = await _woService.GetAllTempMc_Timeslot_List();
                var startMcSlotId = oprs.First().Shop_Plan_start_time;
                var endMcSlotId = oprs.Last().Shop_Plan_end_time;
                var startMcSlot = allMcTimeslots.FirstOrDefault(ts => ts.Timeslot_List_Id == startMcSlotId);
                var endMcSlot = allMcTimeslots.FirstOrDefault(ts => ts.EndTimeslot_List_Id == endMcSlotId);
                var startTs = allTimeslots.FirstOrDefault(ts => ts.Timeslot_ListId == startMcSlot?.Timeslot_List_Id);
                var endTs = allTimeslots.FirstOrDefault(ts => ts.Timeslot_ListId == endMcSlot?.EndTimeslot_List_Id);

                if (startTs?.Start_time != null && endTs?.End_time != null)
                {
                    wo.Plan_Start_Date = startTs.Start_time;
                    wo.Plan_End_Date = endTs.End_time;
                    wo.NoOfSimulation = wo.NoOfSimulation + 1;
                    await _woService.PostTempWo_Wait_List(wo);
                }
                var oprsForWO = allOprs.Where(o => o.Wo_Id == wo.Wo_Id).OrderBy(o => o.Opr_No).ToList();
                foreach (var opr in oprsForWO)
                {
                    var oprMcWaits = await _woService.GetAllTempMc_Wait_List(); // You’ll need this helper
                    var oprMcWait = oprMcWaits.Where(m => m.Wo_Id == wo.Wo_Id && m.Opr_No_Id == opr.Opr_No).ToList();
                    foreach (var mcWait in oprMcWaits)
                    {
                        // 1️⃣ Check if it's a first operation in a shop (Mc_Seq_No = 0)
                        bool isFirstOpInShop = mcWait.Wait_Seq_No == 0;

                        if (isFirstOpInShop)
                        {
                            // Load Matl_Issue_Settings
                            var machine = await _machineService.GetMachine(mcWait.Mc_Id);
                            long deptId = machine.MachineDepartmentId;
                            var allSettings = await _woService.GetAllMatl_Issue_Settings();
                            var setting = allSettings.FirstOrDefault(s => s.Shop_Id == deptId);// map Mc_Id → Shop_Id

                            int daysCoverage = setting?.No_days_coverage ?? 0;
                            int planQty =(int) mcWait.Plan_Qnty;

                            var tsStart = allTimeslots.FirstOrDefault(t => t.Timeslot_ListId == mcWait.Plan_start_time_Id);
                            DateTime simStartTime = tsStart?.Start_time ?? DateTime.Now;

                            // Material issued FROM Stores to Shop
                            DateTime issueDate = simStartTime.Date.AddDays(-1);
                            if (issueDate < DateTime.Today)
                                issueDate = DateTime.Today;

                            int cumulativeQty = 0;
                            DateTime nextIssueDate = issueDate;

                            while (cumulativeQty < planQty)
                            {
                                int issuedQty = await CalculateIssueQty(mcWait.Mc_Id, opr.Opr_No, nextIssueDate, daysCoverage, planQty - cumulativeQty);
                                if (issuedQty <= 0) break; // Avoid infinite loop on 0 qty

                                await _woService.PostMatl_Issue_List(new Matl_Issue_ListVM
                                {
                                    Part_Ref = opr.TempOpr_ListId,
                                    Issue_Mov_date = nextIssueDate,
                                    Issue_Qnty = issuedQty,
                                    Mode = 3,
                                    Immediate_Movmt = 'N',
                                    Issue_Mov_Compl = 'N',
                                    From_Location = 0,
                                    To_Location = machine.MachineDepartmentId
                                });

                                cumulativeQty += issuedQty;
                                nextIssueDate = nextIssueDate.AddDays(daysCoverage);
                            }
                        }

                        // 2️⃣ If shop changes in next operation
                        var nextOpr = oprsForWO.FirstOrDefault(x => x.Opr_No > opr.Opr_No);
                        if (nextOpr != null)
                        {
                            var machine = await _machineService.GetMachine(mcWait.Mc_Id);
                            long deptId = machine.MachineDepartmentId;
                            var currShop = deptId;
                            var nextMcWaits = await _woService.GetAllTempMc_Wait_List();
                            var nextMcWaitss = nextMcWaits.Where(n => n.Wo_Id == wo.Wo_Id && n.Opr_No_Id == nextOpr.Opr_No).ToList();
                            var nextMc = nextMcWaits.FirstOrDefault();

                            if (nextMc != null)
                            {
                                var machineNext = await _machineService.GetMachine(nextMc.Mc_Id);
                                long deptIdNext = machineNext.MachineDepartmentId;
                                var nextShop = deptIdNext;
                                if (currShop != nextShop)
                                {
                                    var currEnd = allTimeslots.FirstOrDefault(t => t.Timeslot_ListId == mcWait.Plan_end_time_Id)?.End_time;
                                    var nextStart = allTimeslots.FirstOrDefault(t => t.Timeslot_ListId == nextMc.Plan_start_time_Id)?.Start_time;

                                    if (currEnd.HasValue && nextStart.HasValue &&
                                        currEnd.Value.Date == nextStart.Value.Date &&
                                        nextMc.Wait_Seq_No == 0)
                                    {
                                        // Direct shop-to-shop movement
                                        await _woService.PostMatl_Issue_List(new Matl_Issue_ListVM
                                        {
                                            Part_Ref = nextOpr.TempOpr_ListId,
                                            Issue_Mov_date = currEnd.Value.Date,
                                            Immediate_Movmt = 'Y',
                                            Mode = 3,
                                            Issue_Mov_Compl = 'N',
                                            From_Location = currShop,
                                            To_Location = nextShop
                                        });
                                    }
                                    else
                                    {
                                        // Delayed movement via stores
                                        var settings = await _woService.GetAllMatl_Issue_Settings();
                                        var setting = settings.Where(s => s.Shop_Id == nextMc.Mc_Id).FirstOrDefault();
                                        int daysCoverage = setting?.No_days_coverage ?? 0;
                                        DateTime issueDate = (nextStart?.Date.AddDays(-1)) ?? DateTime.Today;
                                        if (issueDate < DateTime.Today)
                                            issueDate = DateTime.Today;

                                        int issuedQty = await CalculateIssueQty(nextMc.Mc_Id, nextOpr.Opr_No, nextStart ?? DateTime.Now, daysCoverage, nextOpr.Plan_Qnty);

                                        await _woService.PostMatl_Issue_List(new Matl_Issue_ListVM
                                        {
                                            Part_Ref = nextOpr.TempOpr_ListId,
                                            Issue_Mov_date = issueDate,
                                            Issue_Qnty = opr.Plan_Qnty,
                                            Immediate_Movmt = 'N',
                                            Mode = 3,
                                            Issue_Mov_Compl = 'N',
                                            From_Location = 0,
                                            To_Location = nextShop
                                        });
                                    }
                                }
                                else
                                {
                                    var tsStart = allTimeslots.FirstOrDefault(t => t.Timeslot_ListId == mcWait.Plan_start_time_Id);
                                    DateTime simStartTime = tsStart?.Start_time ?? DateTime.Now;
                                    DateTime issueDate = simStartTime.Date.AddDays(-1);
                                    await _woService.PostMatl_Issue_List(new Matl_Issue_ListVM
                                    {
                                        Part_Ref = nextOpr.TempOpr_ListId,
                                        Issue_Mov_date = issueDate,
                                        Issue_Qnty = nextOpr.Plan_Qnty,
                                        Immediate_Movmt = 'Y',
                                        Mode = 3,
                                        Issue_Mov_Compl = 'N',
                                        From_Location = currShop,
                                        To_Location = nextShop
                                    });
                                }
                            }
                        }
                    }
                }

            }
            try
            {
                SimulationState.IsPaused = false;
                SimulationState.IsStopped = false;
                // your simulation logic here...

                return Json(new { message = "Simulation completed." });
            }
            catch (OperationCanceledException ex)
            {
                return Json(new { message = "Simulation stopped." });
            }
        }
        public async Task<int> CalculateIssueQty(long mcId, long oprNo, DateTime simStartTime, int noDaysCoverage, int planQty)
        {
            // Get the shop of the current machine
            var machine = await _machineService.GetMachine(mcId);
            if (machine == null) return 0;

            long shopId = machine.MachineDepartmentId;
            DateTime coverageEndDate = simStartTime.Date.AddDays(noDaysCoverage);

            // Get all machines in this shop
            var machinesInShop = (await _machineService.GetMachinesList())
                .Where(m => m.ShopId == shopId)
                .Select(m => m.MachineId)
                .ToList();

            // Get all machine wait list entries and their times
            var allMcWaits = await _woService.GetAllTempMc_Wait_List();
            var allTimeslots = await _woService.GetAllTimeslot_List();

            int totalQty = 0;

            foreach (var wait in allMcWaits)
            {
                if (!machinesInShop.Contains(wait.Mc_Id)) continue;
                if (wait.Opr_No_Id != oprNo) continue;

                var slotStart = allTimeslots.FirstOrDefault(t => t.Timeslot_ListId == wait.Plan_start_time_Id);
                if (slotStart == null) continue;

                if (slotStart.Start_time.Date >= simStartTime.Date && slotStart.Start_time.Date < coverageEndDate)
                {
                    totalQty += (int) wait.Plan_Qnty;
                }
            }

            // Don't exceed original planQty
            return Math.Min(totalQty, planQty);
        }

        [HttpPost]
        public IActionResult PauseSimulation()
        {
            SimulationState.IsPaused = true;
            return Ok("Simulation paused.");
        }
        [HttpPost]
        public IActionResult ResumeSimulation()
        {
            SimulationState.IsPaused = false;
            return Ok("Simulation resumed.");
        }

        [HttpPost]
        public async Task<IActionResult> StopSimulation()
        {
            SimulationState.IsStopped = true;
            SimulationState.IsPaused = false;
            var availableSlots = await _woService.GetAllTempMc_Timeslot_List();
            var allocate = availableSlots.Where(a => a.Allocation == 2).ToList();
            foreach (var slot in allocate)
            {
                slot.Allocation = 1;
                await _woService.PostTempMc_Timeslot_List(slot);
            }
            var tempWO_Wait_Lists = await _woService.GetAllTempWo_Wait_List();
            foreach (var item in tempWO_Wait_Lists)
            {
                var delopr = await _woService.DeleteTempWo_Wait_List(item.TempWO_Wait_ListId);
            }
            return Ok("Simulation Has Canceled");
        }
        private async Task CheckPauseAsync()
        {
            while (SimulationState.IsPaused)
            {
                await Task.Delay(500); 
            }

            if (SimulationState.IsStopped)
                throw new OperationCanceledException("Simulation stopped by user.");
        }

        [HttpPost]
        public async Task<IActionResult> PostReadForProdWoWaitList(List<int> selectedWOIds)
        {
            var productions = await _woService.AllProductionPlan_Wo();
            var procplan = await _woService.GetAllProcPlan();
            var postresult = new WO_Wait_ListVM(); 
            List<ProductionPlan_WoVM> result = new List<ProductionPlan_WoVM>();
            foreach (var woId in selectedWOIds)
            {
                foreach (ProductionPlan_WoVM item in productions)
                {
                    if (item.ProductionPlanId == woId)
                    {
                        result.Add(item);
                    }
                }
            }
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    WO_Wait_ListVM addData = new WO_Wait_ListVM();
                    addData.Wo_Id = item.ProductionPlanId;
                    addData.Mode = 1;
                    addData.Allow_Routing_Chg = 'Y';
                    addData.Total_TPT = 0;
                    addData.Rework_Wo = 'N';
                    addData.NC_Log_Ref = 0;
                    addData.WO_Wait_Seq_No = 0;
                    addData.Plan_Start_Date = item.PlanStartDate;
                    addData.Plan_End_Date = (DateTime)item.PlanCompletionDate;
                    addData.Plan_Simul_Qnty = item.CalcWOQty;
                    postresult = await _woService.PostWO_Wait_List(addData);
                    ManufacturedPartNoDetailVM mf = await _masterService.GetManufPart((int)item.PartId);
                    var routingList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                    var route = routingList.Where(r => r.RoutingId == item.RoutingId).FirstOrDefault();
                    if (route != null)
                    {
                        var routingSteps = await _routingService.RoutingSteps(route.RoutingId);

                        foreach (var step in routingSteps)
                        {
                            Opr_ListVM addOprData = new Opr_ListVM();
                            addOprData.Wo_Id = item.ProductionPlanId;
                            addOprData.Opr_No = step.StepId;
                            addOprData.Mode = 1;
                            addOprData.Rework_Wo = 'N';
                            addOprData.NC_Log_Ref = 0;
                            addOprData.Act_Qnty = 0;
                            addOprData.Plan_Qnty = item.CalcWOQty;
                            addOprData.No_of_Simult_Mcs = step.NumberOfSimMachines;
                            var tpt = await GetTPT(step);
                            var tptStepItem = (RoutingStepVM)((OkObjectResult)tpt).Value;

                            int setup = int.Parse(tptStepItem.SetupTime ?? "0");
                            int firstPiece = int.Parse(tptStepItem.FirstPieceTime ?? "0");
                            int cycle = int.Parse(tptStepItem.CycleTime ?? "0");
                            int quantity = item.CalcWOQty;
                            addOprData.Initial_Opr_TPT = setup + firstPiece + cycle * (quantity - 1);
                            var postOprresult = await _woService.PostOpr_List(addOprData);

                            //if (step.StepLocation == "1")
                            //{
                            //    var stepmc = await _routingService.StepMachines((int)step.StepId);
                            //    if (stepmc.Count() != 0)
                            //    {
                            //        foreach (var mc in stepmc)
                            //        {
                            //            Mc_Wait_ListVM addMcWaitData = new Mc_Wait_ListVM();
                            //            addMcWaitData.Wo_Id = item.ProductionPlanId;
                            //            addMcWaitData.Opr_No_Id = step.StepId;
                            //            addMcWaitData.Mc_Id = mc.MachineId;
                            //            addMcWaitData.Mode = 1;
                            //            addMcWaitData.Wait_Seq_No = 0;
                            //            addMcWaitData.Plan_Qnty = item.CalcWOQty;
                            //            addMcWaitData.Rework_Wo = 'N';
                            //            addMcWaitData.Non_Plan_Wk = 'N';
                            //            addMcWaitData.Mc_TPT = setup + firstPiece + cycle * (quantity - 1);
                            //            var postMcwaitresult = await _woService.PostMc_Wait_List(addMcWaitData);
                            //        }
                            //    }
                            //}
                        }
                    }
                }
            }
            return Ok(postresult);
        }
        public async Task<IActionResult> GetTPT(RoutingStepVM item)
        {
            int mcminutes = 0;
            int mcFirstminutes = 0;
            int subminutes = 0;
            int mcsetminutes = 0;
            int subsetminutes = 0;
            var stepmc = await _routingService.StepMachines((int)item.StepId);
            if (stepmc.Count() != 0)
            {
                foreach (var mc in stepmc)
                {
                    // if (mc.PreferredMachine == 1)
                    //{
                    TimeSpan time = TimeSpan.Parse(mc.FloorToFloorTime);
                    mcminutes = (int)time.TotalMinutes;
                    TimeSpan Settime = TimeSpan.Parse(mc.SetupTime);
                    mcsetminutes = (int)Settime.TotalMinutes;
                    TimeSpan firsttime = TimeSpan.Parse(mc.FirstPieceProcessingTime);
                    mcFirstminutes = (int)firsttime.TotalMinutes;
                    //}
                }
                item.CycleTime = mcminutes.ToString();
                item.SetupTime = mcsetminutes.ToString();
                item.FirstPieceTime = mcFirstminutes.ToString();
                return Ok(item);
            }
            var stepsubcon = await _routingService.SubCons((int)item.StepId);
            if (stepsubcon.Count() != 0)
            {
                foreach (var sub in stepsubcon)
                {
                    //if (sub.PreferredSubcon == 1)
                    //{
                    var subworkdetails = await _routingService.SubConWSS((int)item.StepId, sub.SubConDetailsId);
                    if (subworkdetails.Count() != 0)
                    {
                        foreach (var mc in subworkdetails)
                        {
                            TimeSpan time = TimeSpan.Parse(mc.FloorToFloorTime);
                            subminutes = (int)time.TotalMinutes;
                            TimeSpan Settime = TimeSpan.Parse(mc.SetupTime);
                            subsetminutes = (int)Settime.TotalMinutes;
                        }
                    }
                    //}
                }
                item.CycleTime = subminutes.ToString();
                item.SetupTime = subsetminutes.ToString();
                return Ok(item);
            }
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> PostReadForTempProdWoWaitList()
        {
            var productions = await _woService.AllProductionPlan_Wo();
            var procplan = await _woService.GetAllProcPlan();

            List<ProductionPlan_WoVM> result = new List<ProductionPlan_WoVM>();

            foreach (var item in productions)
            {
                if (item.PartType == 2)
                {
                    item.PlanStartDateStr = item.PlanStartDate.ToString("dd-MM-yyyy");
                }
                else
                {
                    var findpp = procplan.FirstOrDefault(p => p.WorkOrderId == item.WoId)?.CalcReceiptDate;
                    item.PlanStartDateStr = findpp?.ToString("dd-MM-yyyy");
                }

                var so = await _baService.GetOneSO(item.SalesOrderId);
                if (so != null)
                {
                    if (item.PlanCompletionDate >= so.RequiredByDate && item.CalcWOQty >= so.RequiredQuantity)
                    {
                        item.WoRelease = "Y";
                        result.Add(item);
                    }
                    else
                    {
                        item.WoRelease = "N";
                    }
                }
                else
                {
                    var wosos = await _woService.GetSoWoRel(item.WoId);
                    foreach (var woso in wosos)
                    {
                        var sos = await _baService.GetOneSO(woso.SalesOrderId);
                        if (sos != null) item.SoComplDateStr = sos.RequiredByDateStr;

                        if (sos.RequiredByDate > item.PlanCompletionDate)
                        {
                            item.WoRelease = "Y";
                            result.Add(item);
                        }
                        else
                        {
                            item.WoRelease = "N";
                        }
                    }
                }
            }

            // Prioritize: Rework > NonPlan > Production
            //var reworkWos = result.Where(r => r.Rework_Wo == 'Y').ToList();
            //var nonPlanWos = result.Where(r => r.NonPlan_Wk == 'Y').ToList();
            var prodWos = result.OrderBy(r => r.PlanCompletionDate).ToList();

            //var fullOrderedList = reworkWos.Concat(nonPlanWos).Concat(prodWos).ToList();
            var fullOrderedList = prodWos.ToList();
            int seqNo = 1;

            foreach (var item in fullOrderedList)
            {
                var addData = new TempWO_Wait_ListVM
                {
                    Wo_Id = item.ProductionPlanId,
                    Mode = 1,
                    Allow_Routing_Chg = 'Y',
                    Total_TPT = 0,
                    Rework_Wo = 'N',
                    NC_Log_Ref = 0,
                    WO_Wait_Seq_No = seqNo++,
                    Plan_Start_Date = item.PlanStartDate,
                    Plan_End_Date = (DateTime)item.PlanCompletionDate,
                    Plan_Simul_Qnty = item.CalcWOQty
                };

                await _woService.PostTempWo_Wait_List(addData);

                var mf = await _masterService.GetManufPart((int)item.PartId);
                var routingList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                var route = routingList.FirstOrDefault(r => r.RoutingId == item.RoutingId);
                if (route == null) continue;

                var routingSteps = await _routingService.RoutingSteps(route.RoutingId);

                foreach (var step in routingSteps)
                {
                    var tpt = await GetTPT(step);
                    var tptStepItem = (RoutingStepVM)((OkObjectResult)tpt).Value;

                    int setup = int.Parse(tptStepItem.SetupTime ?? "0");
                    int firstPiece = int.Parse(tptStepItem.FirstPieceTime ?? "0");
                    int cycle = int.Parse(tptStepItem.CycleTime ?? "0");
                    int quantity = item.CalcWOQty;

                    var addOprData = new TempOpr_ListVM
                    {
                        Wo_Id = item.ProductionPlanId,
                        Opr_No = step.StepId,
                        Mode = 1,
                        Rework_Wo = 'N',
                        NC_Log_Ref = 0,
                        Act_Qnty = 0,
                        Plan_Qnty = quantity,
                        No_of_Simult_Mcs = step.NumberOfSimMachines,
                        Initial_Opr_TPT = setup + firstPiece + cycle * (quantity - 1)
                    };

                    await _woService.PostTempOpr_List(addOprData);
                }
            }

            return Ok("TempWO_Wait_List and TempOpr_List posted with priority-based sequencing.");
        }
        [HttpPost]
        public async Task<IActionResult> PostTempToActiveWaitList()
        {
            var tempWOs = await _woService.GetAllTempWo_Wait_List();
            var activeWOs = await _woService.GetAllWO_Wait_List();

            // Remove duplicates (same WO_Id) already existing in active list
            var filteredTemp = tempWOs.Where(temp => !activeWOs.Any(active => active.Wo_Id == temp.Wo_Id)).ToList();

            // Prioritize: Rework → NonPlan → Production
            var reworkWos = filteredTemp.Where(w => w.Rework_Wo == 'Y').ToList();
            var nonPlanWos = filteredTemp.Where(w => w.Rework_Wo != 'Y' && w.NC_Log_Ref > 0).ToList(); // assume NonPlan if NC_Log_Ref > 0
            var prodWos = filteredTemp.Except(reworkWos).Except(nonPlanWos)
                                      .OrderBy(w => w.Plan_End_Date).ToList();

            var orderedList = reworkWos.Concat(nonPlanWos).Concat(prodWos).ToList();

            int startSeq = 0;

            foreach (var item in orderedList)
            {
                var activeWO = new WO_Wait_ListVM
                {
                    Wo_Id = item.Wo_Id,
                    Mode = item.Mode,
                    Allow_Routing_Chg = item.Allow_Routing_Chg,
                    Total_TPT = item.Total_TPT,
                    Rework_Wo = item.Rework_Wo,
                    NC_Log_Ref = item.NC_Log_Ref,
                    WO_Wait_Seq_No = startSeq++,
                    Plan_Start_Date = item.Plan_Start_Date,
                    Plan_End_Date = item.Plan_End_Date,
                    Plan_Simul_Qnty = item.Plan_Simul_Qnty
                };

                await _woService.PostWO_Wait_List(activeWO);

                // Now move the Opr steps
                var tempOprsList = await _woService.GetAllTempOpr_List();
                var tempOprs = tempOprsList.Where(o => o.Wo_Id == item.Wo_Id).ToList();
                foreach (var tempOpr in tempOprs)
                {
                    var activeOpr = new Opr_ListVM
                    {
                        Wo_Id = tempOpr.Wo_Id,
                        Opr_No = tempOpr.Opr_No,
                        Mode = tempOpr.Mode,
                        Rework_Wo = tempOpr.Rework_Wo,
                        NC_Log_Ref = tempOpr.NC_Log_Ref,
                        Act_Qnty = tempOpr.Act_Qnty,
                        Plan_Qnty = tempOpr.Plan_Qnty,
                        No_of_Simult_Mcs = tempOpr.No_of_Simult_Mcs,
                        Initial_Opr_TPT = tempOpr.Initial_Opr_TPT
                    };

                    await _woService.PostOpr_List(activeOpr);
                }
            }

            return Ok("Temp data promoted to active tables with correct WO_Wait_Seq_No based on priority.");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateWO_Wait_List(long itemMasterDocListId)
        {
            var result = await _woService.GetAllWO_Wait_List();
            var findWowait = result.Where(w => w.WO_Wait_ListId == itemMasterDocListId).FirstOrDefault();
            if(findWowait != null)
            {
                findWowait.Mode = 2;
                var postresult = await _woService.PostWO_Wait_List(findWowait);
                return Ok(postresult);
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateMc_Timeslot_List(long itemMasterDocListId)
        {
            var result = await _woService.GetAllMc_Timeslot_List();
            var findWowait = result.Where(w => w.Mc_Timeslot_List_Id == itemMasterDocListId).FirstOrDefault();
            if(findWowait != null)
            {
                findWowait.Allocation = 2;
                var postresult = await _woService.PostMc_Timeslot_List(findWowait);
                return Ok(postresult);
            }
            return Ok(result);
        } 
        [HttpGet]
        public async Task<IActionResult> UpdateProdWo(long pwoid,long routingId)
        {
            var result = await _woService.AllProductionPlan_Wo();
            var findWowait = result.Where(w => w.ProductionPlanId == pwoid).FirstOrDefault();
            if(findWowait != null)
            {
                var findrout = await _routingService.RoutingSteps((int)routingId);
                findWowait.RoutingId = routingId;
                findWowait.StartingOpNo = (int)findrout.FirstOrDefault().StepId;
                findWowait.EndingOpNo = (int)findrout.LastOrDefault().StepId;
                var procdutionpost = await _woService.UpdateProductionPlan_Wo(findWowait);
                return Ok(procdutionpost);
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReadyforProductionWos()
        {
            var productions = await _woService.AllProductionPlan_Wo();
            var masterparts = await _masterService.ItemMasterParts();
            var procplan = await _woService.GetAllProcPlan();
            var customer = await _baService.GetCustomerOrders();
            var wO_Wait_Lists = await _woService.GetAllTempWo_Wait_List();
            var activewO_Wait_Lists = await _woService.GetAllWO_Wait_List();
            var opr_Lists = await _woService.GetAllOpr_List();
            var allTimeslots = await _woService.GetAllTimeslot_List();
            var allMcTimeslots = await _woService.GetAllMc_Timeslot_List();
            List<ProductionPlan_WoVM> result = new List<ProductionPlan_WoVM>();
            foreach (ProductionPlan_WoVM item in productions)
            {
                if (item.PartType == 2)
                {
                    item.PlanStartDateStr = item.PlanStartDate.ToString("dd-MM-yyyy");
                }
                else
                {
                    var findpp = procplan.Where(p => p.WorkOrderId == item.WoId).FirstOrDefault().CalcReceiptDate.ToString("dd-MM-yyyy");
                    item.PlanStartDateStr = findpp;
                }
                item.SoComplDateStr = item.SoComplDate.Value.ToString("dd-MM-yyyy");
                foreach (ItemMasterPartVM imp in masterparts)
                {
                    if (item.PartId == imp.PartId)
                    {
                        item.PartNo = imp.PartNo;
                        item.PartDesc = imp.Description;
                        bool isParentWo = productions.Any(x => x.ParentWoId == item.WoId);

                        if (item.PartType == 1 && item.ParentWoId == 0)
                        {
                            item.PartTypeName = isParentWo ? "Parent CMP" : "CMP";
                        }
                        else if (item.PartType == 2)
                        {
                            item.PartTypeName = "Assembly";
                        }
                    }
                }
                var so = await _baService.GetOneSO(item.SalesOrderId);
                if (so != null)
                {
                    foreach (CustomerOrderVM cu in customer)
                    {
                        if (so.CustomerOrderId == cu.CustomerOrderId)
                        {
                            item.Customer = cu.CustomerName;
                        }
                    }
                    //if (item.PlanCompletionDate >= so.RequiredByDate && item.CalcWOQty >= so.RequiredQuantity)
                    //{
                    //    item.WoRelease = "Y";
                    //    result.Add(item);
                    //}
                    //else
                    //{
                    //    item.WoRelease = "N";
                    //}
                }
                else
                {
                    var wosos = await _woService.GetSoWoRel(item.WoId);
                    foreach (var woso in wosos)
                    {
                        var sos = await _baService.GetOneSO(woso.SalesOrderId);
                        if (sos != null)
                        {
                            item.SoComplDateStr = sos.RequiredByDateStr;
                        }
                        if (sos.RequiredByDate > item.PlanCompletionDate)
                        {
                            item.WoRelease = "Y";
                            result.Add(item);
                        }
                        else
                        {
                            item.WoRelease = "N";
                        }
                        foreach (CustomerOrderVM cu in customer)
                        {
                            if (sos.CustomerOrderId == cu.CustomerOrderId)
                            {
                                item.Customer = cu.CustomerName;
                            }
                        }
                    }
                }
                DateTime? earliestStartTime = null;

                foreach (var opr in opr_Lists.Where(w => w.Wo_Id == item.ProductionPlanId))
                {
                    var mcTimeslot = allMcTimeslots.FirstOrDefault(m => m.Mc_Timeslot_List_Id == opr.Shop_Plan_start_time);
                    if (mcTimeslot != null)
                    {
                        var timeslot = allTimeslots.FirstOrDefault(t => t.Timeslot_ListId == mcTimeslot.Timeslot_List_Id);
                        if (timeslot != null)
                        {
                            if (earliestStartTime == null || timeslot.Start_time < earliestStartTime)
                            {
                                earliestStartTime = timeslot.Start_time;
                            }
                        }
                    }
                }

                if (earliestStartTime != null)
                {
                    item.ActStartDateStr = earliestStartTime.Value.ToString("dd-MM-yyyy");
                }
                    item.DataChange = (item.Changed == 0) ? "N" : "Y";
                var wO_Wait_List = wO_Wait_Lists.Where(w => w.Wo_Id == item.ProductionPlanId).FirstOrDefault();
                var activewO_Wait_List = activewO_Wait_Lists.Where(w => w.Wo_Id == item.ProductionPlanId).FirstOrDefault();
                if (wO_Wait_List != null)
                {
                    item.CsStartDate = wO_Wait_List.Plan_Start_Date.ToString("dd-MM-yyyy");
                    item.CsEndDate = wO_Wait_List.Plan_End_Date.ToString("dd-MM-yyyy");
                    if(activewO_Wait_List != null)
                    {
                        item.PsStartDate = activewO_Wait_List.Plan_Start_Date.ToString("dd-MM-yyyy");
                        item.PsEndDate = activewO_Wait_List.Plan_End_Date.ToString("dd-MM-yyyy");
                    }
                    item.CriticalParts = (wO_Wait_List.Plan_End_Date > item.PlanCompletionDate) ? "Y" : "N";
                    var mf = await _masterService.GetManufPart((int)item.PartId);
                    var routingList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                    item.NoOfRoutes = routingList.Count();
                    result.Add(item);
                }
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllReadyforProductionWo()
        {
            // 1. Fetch Data in Parallel (Backend API + External Services)
            // We add bulk fetches for Sales Orders, Manufactured Parts, and Routings here.
            var productionTask = _woService.GetAllReadyforProductionWo();
            var masterPartsTask = _masterService.ItemMasterParts();
            var customerTask = _baService.GetCustomerOrders();
            var allSalesOrdersTask = _baService.AllSalesOrders();
            var allManufPartsTask = _masterService.GetAllManufacturedPartNoDetailList();
            var allRoutingsTask = _routingService.AllRoutings();

            // Await all tasks concurrently
            await Task.WhenAll(productionTask, masterPartsTask, customerTask, allSalesOrdersTask, allManufPartsTask, allRoutingsTask);

            var productions = productionTask.Result.ToList();
            var masterparts = masterPartsTask.Result.ToDictionary(p => p.PartId);
            var customers = customerTask.Result.ToList();

            // 2. Create Optimized Lookups
            // Dictionary for fast Sales Order lookup by ID
            var salesOrderDict = allSalesOrdersTask.Result.ToDictionary(s => s.SalesOrderId);

            // Dictionary for Manufactured Part details by PartId (assuming PartId is available in the VM)
            // Note: If multiple ManufParts exist for one PartId, use GroupBy. FirstOrDefault is used here for safety.
            var manufPartDict = allManufPartsTask.Result
                                    .GroupBy(m => m.PartId)
                                    .ToDictionary(g => g.Key, g => g.First());

            // Lookup for Routings grouped by ManufacturedPartId
            // This allows us to quickly get all routings for a specific part without an API call
            var routingLookup = allRoutingsTask.Result.ToLookup(r => r.ManufacturedPartId);

            // 3. Enrich Data
            foreach (var item in productions)
            {
                // A. Enrich Part Details
                if (masterparts.TryGetValue(item.PartId, out var imp))
                {
                    item.PartNo = imp.PartNo;
                    item.PartDesc = imp.Description;

                    // Determine Part Type Name
                    bool isParentWo = productions.Any(x => x.ParentWoId == item.WoId);
                    if (item.PartType == 1 && item.ParentWoId == 0)
                    {
                        item.PartTypeName = isParentWo ? "Parent CMP" : "CMP";
                    }
                    else if (item.PartType == 2)
                    {
                        item.PartTypeName = "Assembly";
                    }
                }

                // B. Enrich Customer & WO Release Status
                // OPTIMIZATION: Use dictionary lookup instead of _baService.GetOneSO(item.SalesOrderId)
                SalesOrderVM so = null;
                salesOrderDict.TryGetValue(item.SalesOrderId, out so);

                if (so != null)
                {
                    var cust = customers.FirstOrDefault(c => c.CustomerOrderId == so.CustomerOrderId);
                    if (cust != null) item.Customer = cust.CustomerName;
                }
                else
                {
                    // Indirect SO Check (via WOSO Relation)
                    // This remains inside the loop as requested ("except GetSoWoRel")
                    var wosos = await _woService.GetSoWoRel(item.WoId);

                    foreach (var woso in wosos)
                    {
                        // OPTIMIZATION: Use dictionary lookup instead of _baService.GetOneSO(woso.SalesOrderId)
                        SalesOrderVM sos = null;
                        salesOrderDict.TryGetValue(woso.SalesOrderId, out sos);

                        if (sos != null)
                        {
                            item.SoComplDateStr = sos.RequiredByDateStr;
                            item.WoRelease = (sos.RequiredByDate > item.PlanCompletionDate) ? "Y" : "N";

                            var cust = customers.FirstOrDefault(c => c.CustomerOrderId == sos.CustomerOrderId);
                            if (cust != null) item.Customer = cust.CustomerName;
                        }
                    }
                }

                // C. Enrich Routing Count
                // OPTIMIZATION: Use dictionary lookup instead of _masterService.GetManufPart
                if (manufPartDict.TryGetValue((int)item.PartId, out var mf))
                {
                    // OPTIMIZATION: Use lookup instead of _routingService.Routings(...)
                    // We count the routings associated with this ManufacturedPartNoDetailId
                    item.NoOfRoutes = routingLookup[mf.ManufacturedPartNoDetailId].Count();
                }
            }

            return Ok(productions);
        }
        [HttpGet]
        public async Task<IActionResult> CompareMcTimeSlot(long McWaitId)
        {
            // Get all necessary data
            var mcWaitList = await _woService.GetAllMc_Wait_List();
            var mcWait = mcWaitList.FirstOrDefault(m => m.Mc_Wait_ListId == McWaitId);
            if (mcWait == null)
                return NotFound("Invalid McWait ID");

            var allTimeslots = await _woService.GetAllTimeslot_List();
            var allPlannedMcTimeSlots = await _woService.GetAllMc_Timeslot_List();

            // Get planned timeslots for this McWaitId (you should ideally filter by McWaitId or WO_Id)
            var plannedTimeSlots = allPlannedMcTimeSlots
                .Where(pt => pt.Mc_Wait_List_Id == McWaitId)
                .ToList();

            if (!plannedTimeSlots.Any())
                return NotFound("No planned timeslots found for this McWait ID.");

            // Simulate actual completion for now (you can replace this with actual completed qty logic)
            var actualCompletedQty = mcWait.Plan_Qnty; // You should replace this with bookout logs
            var totalPlannedQty = mcWait.Plan_Qnty;
            var balanceQty = totalPlannedQty - actualCompletedQty;

            // Get the corresponding timeslot range from Timeslot_List
            var timeslotIds = plannedTimeSlots.Select(pt => pt.Timeslot_List_Id).ToList();
            var relevantTimeslots = allTimeslots
                .Where(t => timeslotIds.Contains(t.Timeslot_ListId))
                .OrderBy(t => t.Start_time) // assuming StartTime exists
                .ToList();

            var result = new
            {
                McWaitId,
                PlannedStart = relevantTimeslots.First().Start_time,
                PlannedEnd = relevantTimeslots.Last().End_time,
                ActualCompletedQty = actualCompletedQty,
                BalanceQty = balanceQty,
                ProjectedCompletionTime = relevantTimeslots.Last().End_time 
            };

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CopyFromTempToActiveTable()
        {
            var tempMc_Timeslots = await _woService.GetAllTempMc_Timeslot_List();
            var tempMc_Wait_Lists = await _woService.GetAllTempMc_Wait_List();
            var tempOpr_Lists = await _woService.GetAllTempOpr_List();
            var tempWO_Wait_Lists = await _woService.GetAllTempWo_Wait_List();
            var activeMc_Timeslots = await _woService.GetAllMc_Timeslot_List();
            var activeMc_Wait_Lists = await _woService.GetAllMc_Wait_List();
            var activeOpr_Lists = await _woService.GetAllOpr_List();
            var activeWO_Wait_Lists = await _woService.GetAllWO_Wait_List();

            foreach (var temp in tempMc_Timeslots)
            {
                var active = activeMc_Timeslots.FirstOrDefault(a => a.Mc_Timeslot_List_Id == temp.ActiveId);
                if (active != null)
                {
                    active.Mc_Id = temp.Mc_Id;
                    active.Allocation = temp.Allocation;
                    active.Timeslot_List_Id = temp.Timeslot_List_Id;
                    active.Slot_Not_Avl = temp.Slot_Not_Avl;
                    active.Not_Avl_reason = temp.Not_Avl_reason;
                    active.Mc_Wait_List_Id = temp.Mc_Wait_List_Id;
                }
            }
            foreach (var temp in tempMc_Wait_Lists)
            {
                var active = activeMc_Wait_Lists.FirstOrDefault(a => a.Mc_Wait_ListId == temp.ActiveId);
                if (active != null)
                {
                    active.Mc_Id = temp.Mc_Id;
                    active.Wo_Id = temp.Wo_Id;
                    active.Opr_No_Id = temp.Opr_No_Id;
                    active.Bal_Qnty = active.Plan_Qnty -temp.Plan_Qnty;
                    active.Plan_Qnty = temp.Plan_Qnty;
                    active.Plan_start_time_Id = temp.Plan_start_time_Id;
                    active.Plan_end_time_Id = temp.Plan_end_time_Id;
                    active.Next_Opr_Start_time_Id = temp.Next_Opr_Start_time_Id;
                    active.Setup_Apprvl_time = temp.Setup_Apprvl_time;
                    active.Act_End_time = temp.Act_End_time;
                    active.Mc_TPT = temp.Mc_TPT;
                    active.Setup_Start_time = temp.Setup_Start_time;
                    active.Rework_Wo = temp.Rework_Wo;
                    active.Non_Plan_Wk = temp.Non_Plan_Wk;
                    active.Non_Plan_wk_Id = temp.Non_Plan_wk_Id;
                    active.Mode = temp.Mode;
                    active.Wait_Seq_No = temp.Wait_Seq_No;
                }
            }
            foreach (var temp in tempOpr_Lists)
            {
                var active = activeOpr_Lists.FirstOrDefault(a => a.Opr_ListId == temp.ActiveId);
                if (active != null)
                {
                    active.Opr_No = temp.Opr_No;
                    active.Wo_Id = temp.Wo_Id;
                    active.Mode = temp.Mode;
                    active.Initial_Opr_TPT = temp.Initial_Opr_TPT;
                    active.Rolledup_Opr_TPT = temp.Rolledup_Opr_TPT;
                    active.Rework_Wo = temp.Rework_Wo;
                    active.NC_Log_Ref = temp.NC_Log_Ref;
                    active.Act_Qnty = temp.Act_Qnty;
                    active.Plan_Qnty = temp.Plan_Qnty;
                    active.No_of_Simult_Mcs = temp.No_of_Simult_Mcs;
                    active.Shop_Plan_start_time = temp.Shop_Plan_start_time;
                    active.Shop_Plan_end_time = temp.Shop_Plan_end_time;
                    active.Subcon_plan_start_time = temp.Subcon_plan_start_time;
                    active.Subcon_plan_end_time = temp.Subcon_plan_end_time;
                    active.Setup_Start_time = temp.Setup_Start_time;
                    active.Act_End_time = temp.Act_End_time;
                }
            }
            foreach (var temp in tempWO_Wait_Lists)
            {
                var active = activeWO_Wait_Lists.FirstOrDefault(a => a.WO_Wait_ListId == temp.ActiveId);
                if (active != null)
                {
                    active.Wo_Id = temp.Wo_Id;
                    active.Mode = temp.Mode;
                    active.Allow_Routing_Chg = temp.Allow_Routing_Chg;
                    active.Total_TPT = temp.Total_TPT;
                    active.Rework_Wo = temp.Rework_Wo;
                    active.NC_Log_Ref = temp.NC_Log_Ref;
                    active.WO_Wait_Seq_No = temp.WO_Wait_Seq_No;
                    active.Plan_Start_Date = temp.Plan_Start_Date;
                    active.Plan_End_Date = temp.Plan_End_Date;
                    active.Plan_Simul_Qnty = temp.Plan_Simul_Qnty;
                }
            }
            return Ok("Data Copied From Temp Tables To Active Tables Sucessfully.");
        }
        [HttpPost]
        public async Task<IActionResult> AllocateRwkMcTimeslots()
        {
            var rwkList = await _woService.GetAllRwk_List();
            var allTimeSlots = await _woService.GetAllTimeslot_List();
            var allMcTimeSlots = await _woService.GetAllMc_Timeslot_List();

            foreach (var rwk in rwkList.Where(r => r.Allocated == 'N' && r.Mc_Id > 0))
            {
                // Get only slots for this machine
                var machineSlots = allMcTimeSlots
                    .Where(slot => slot.Mc_Id == rwk.Mc_Id && slot.Allocation == 1)
                    .Join(allTimeSlots,
                          mc => mc.Timeslot_List_Id,
                          ts => ts.Timeslot_ListId,
                          (mc, ts) => new { Slot = mc, StartTime = ts.Start_time })
                    .OrderBy(s => s.StartTime)
                    .ToList();

                if (!machineSlots.Any())
                    continue;

                // Calculate how many slots are needed
                double planDuration = Convert.ToDouble(rwk.Plan_Duration);
                double slotDuration = (machineSlots.Count >= 2)
                    ? (machineSlots[1].StartTime - machineSlots[0].StartTime).TotalMinutes
                    : 30; 

                int neededSlots = (int)Math.Ceiling(planDuration / slotDuration);
                if (neededSlots > machineSlots.Count)
                    continue;

                var selectedSlots = machineSlots.Take(neededSlots).Select(ms => ms.Slot).ToList();

                foreach (var slot in selectedSlots)
                {
                    slot.Mc_Id = rwk.Mc_Id;
                    slot.Allocation = 3;

                    await _woService.PostMc_Timeslot_List(slot);
                }

                rwk.Allocated = 'Y';
                await _woService.PostRwk_List(rwk);
            }

            return Ok("Rework Timeslots Allocated Successfully");
        }
        [HttpPost]
        public async Task<IActionResult> AllocateNonPlanMcTimeslots()
        {
            var nonPlanList = await _woService.GetAllNon_Plan_Wk_List(); // assumes a method to get all non-plan work
            var allTimeSlots = await _woService.GetAllTimeslot_List();
            var allMcTimeSlots = await _woService.GetAllMc_Timeslot_List();

            foreach (var nonPlan in nonPlanList.Where(n => n.Allocated == 'N' && n.Mc_Id > 0))
            {
                var planStartDate = nonPlan.Plan_start_time;

                // Filter machine timeslots for that machine and on or after plan start date
                var machineSlots = allMcTimeSlots
                    .Where(slot => slot.Mc_Id == nonPlan.Mc_Id && slot.Allocation == 1)
                    .Join(allTimeSlots,
                          mc => mc.Timeslot_List_Id,
                          ts => ts.Timeslot_ListId,
                          (mc, ts) => new { Slot = mc, StartTime = ts.Start_time })
                    .Where(s => s.StartTime >= planStartDate)
                    .OrderBy(s => s.StartTime)
                    .ToList();

                if (machineSlots.Count < 1)
                    continue;

                // Estimate duration per slot
                double slotDuration = (machineSlots.Count >= 2)
                    ? (machineSlots[1].StartTime - machineSlots[0].StartTime).TotalMinutes
                    : 60; // default to 60 mins if unknown

                int neededSlots = (int)Math.Ceiling(Convert.ToDouble(nonPlan.Plan_Duration) / slotDuration);
                if (neededSlots > machineSlots.Count)
                    continue;

                var selectedSlots = machineSlots.Take(neededSlots).Select(ms => ms.Slot).ToList();

                // Update timeslot allocations
                foreach (var slot in selectedSlots)
                {
                    slot.Mc_Id = nonPlan.Mc_Id;
                    slot.Allocation = 2;

                    await _woService.PostMc_Timeslot_List(slot); // update in DB
                }

                // Optionally mark the Non-Plan work as Allocated
                nonPlan.Allocated = 'Y';
                await _woService.PostNon_Plan_Wk_List(nonPlan);
            }

            return Ok("Non-Plan Timeslots allocated successfully.");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCurrentProductionStatus()
        {
            try
            {
                var now = DateTime.Now;

                // Fetch all required data
                var allTimeslots = await _woService.GetAllTimeslot_List();
                var allMcTimeslots = await _woService.GetAllMc_Timeslot_List();
                var allOprList = await _woService.GetAllOpr_List();
                var allMcWaitList = await _woService.GetAllMc_Wait_List();
                var allShopInspLogs = await _woService.GetAllShop_Insp_Log();
                var allNcLogs = await _woService.GetAllNcLog();

                // 1. Copy Mc_Timeslot_List to TempMc_Timeslot_List from now for each Mc
                foreach (var group in allMcTimeslots.GroupBy(m => m.Mc_Id))
                {
                    var futureSlots = group
                        .Where(g => allTimeslots.First(t => t.Timeslot_ListId == g.Timeslot_List_Id).Start_time >= now)
                        .ToList();

                    foreach (var slot in futureSlots)
                    {
                        var tempSlot = new TempMc_Timeslot_ListVM
                        {
                            Mc_Id = slot.Mc_Id,
                            Timeslot_List_Id = slot.Timeslot_List_Id,
                            EndTimeslot_List_Id = slot.EndTimeslot_List_Id,
                            Slot_Not_Avl = slot.Slot_Not_Avl,
                            Not_Avl_reason = slot.Not_Avl_reason,
                            Allocation = 1, // Default
                            ActiveId = slot.Mc_Timeslot_List_Id
                        };
                        await _woService.PostTempMc_Timeslot_List(tempSlot);
                    }
                }

                // 2. Copy Opr_List entries for Opr_Nos in M/C (Mc_Wait_List.Wait_Seq_No = 0)
                var oprInMc = allMcWaitList.Where(w => w.Wait_Seq_No == 0).Select(w => w.Opr_No_Id).ToList();
                foreach (var opr in allOprList.Where(o => oprInMc.Contains(o.Opr_No)))
                {
                    var tempOpr = opr;
                    tempOpr.Shop_Plan_end_time = 0;
                    tempOpr.Subcon_plan_end_time = 0;
                    tempOpr.Act_Qnty = 0;
                    await _woService.PostOpr_List(tempOpr);
                }

                // 3. Copy Mc_Wait_List.Wait_Seq_No = 0 to Temp
                foreach (var mcw in allMcWaitList.Where(w => w.Wait_Seq_No == 0))
                {
                    var tempMcWait = new TempMc_Wait_ListVM
                    {
                        Wo_Id = mcw.Wo_Id,
                        Opr_No_Id = mcw.Opr_No_Id,
                        Mc_Id = mcw.Mc_Id,
                        Wait_Seq_No = 0,
                        Plan_start_time_Id = mcw.Plan_start_time_Id,
                        Mc_TPT = mcw.Mc_TPT,
                        Mode = mcw.Mode,
                        Plan_Qnty = mcw.Plan_Qnty,
                        Rework_Wo = mcw.Rework_Wo
                    };
                    tempMcWait.Bal_Qnty = (long)(mcw.Plan_Qnty -
                        (allNcLogs.Where(n => n.Opr_No_Id == mcw.Opr_No_Id).Sum(n => n.NC_Qnty) +
                        allShopInspLogs.Where(s => s.Input_Opr_NoId == mcw.Opr_No_Id).Sum(s => s.Qnty_OK_finished)));

                    await _woService.PostTempMc_Wait_List(tempMcWait);
                }

                // 4. Re-simulate for current balance using existing logic (reuse AllocateForReadyWOs steps)
                await AllocateForReadyWOs();

                return Ok(new { message = "Current production status checked and simulated." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error in status update", error = ex.Message });
            }
        }

        public async Task<IActionResult> AllocateForReadyWOs()
        {
            // 1. Fetch all ready WOs
            var readyWOs = await _woService.GetAllTempWo_Wait_List();
            var allOprs = await _woService.GetAllTempOpr_List();
            var allTimeslots = await _woService.GetAllTimeslot_List();
            var allMcTimeslots = await _woService.GetAllMc_Timeslot_List();
            var nclogs = await _woService.GetAllNcLog();
            var recpts = await _woService.GetAllInw_Recpt_Header();
            var podetails = await _woService.GetAllPodetails();
            var procplans = await _woService.GetAllProcPlan();
            var prodnwos = await _woService.AllProductionPlan_Wo();
            var reworkList = await _woService.GetAllRwk_List(); // Implement this method
            var pendingReworks = reworkList
                .Where(rwk => rwk.Allocated == 'N' && rwk.Mc_Id > 0)
                .ToList();

            foreach (var rwk in pendingReworks)
            {
                await CheckPauseAsync();
                var machine = await _machineService.GetMachine(rwk.Mc_Id);
                if (machine == null) continue;

                var plant = await _plantService.GetPlantWD(machine.MachinePlantId);
                if (plant == null) continue;

                int timeslotDuration = plant.Timeslot_duration;
                int durationInMinutes = (int)TimeSpan.Parse(rwk.Plan_Duration).TotalMinutes;
                int requiredSlots = (int)Math.Ceiling((double)durationInMinutes / timeslotDuration);

                var timeslots = allTimeslots
                    .Where(t => t.PlantId == machine.MachinePlantId && t.Start_time >= DateTime.Now && t.Break_Slot != 'Y')
                    .OrderBy(t => t.Start_time)
                    .ToList();
                var nclog = nclogs.Where(n => n.Insp_Outcome_Details_Id == rwk.NC_Log_Id ).FirstOrDefault();
                var recpt = recpts.Where(r => r.Inw_Recpt_HeaderId == nclog.Inw_Recpt_Header_Id).FirstOrDefault();
                var podetail = podetails.Where(p => p.PoDetailsId == recpt.PoHeaderId).FirstOrDefault();
                var procplan = procplans.Where(pp=>pp.ProcPlanId == podetail.ProcPlanId).FirstOrDefault();
                var prodnwo = prodnwos.Where(w => w.WoId == procplan.WorkOrderId).FirstOrDefault();
                var availableSlots = timeslots.Take(requiredSlots).ToList();
                if (availableSlots.Count < requiredSlots) continue;

                // Create Mc_Wait_List entry
                var mcWait = new TempMc_Wait_ListVM
                {
                    Wo_Id = prodnwo.ProductionPlanId,
                    Opr_No_Id = prodnwo.StartingOpNo,
                    Mc_Id = rwk.Mc_Id,
                    Wait_Seq_No = await CalculateNextTempWaitSeqNo(rwk.Mc_Id),
                    Plan_start_time_Id = availableSlots.First().Timeslot_ListId,
                    Plan_end_time_Id = availableSlots.Last().Timeslot_ListId,
                    Mc_TPT = durationInMinutes,
                    Rework_Wo = 'Y',
                    Mode = 1, 
                    Plan_Qnty = (long)nclog.NC_Qnty
                };
                mcWait = await _woService.PostTempMc_Wait_List(mcWait);

                // Create Mc_Timeslot_List entry
                var newMcSlot = new TempMc_Timeslot_ListVM
                {
                    Mc_Id = rwk.Mc_Id,
                    Timeslot_List_Id = availableSlots.First().Timeslot_ListId,
                    EndTimeslot_List_Id = availableSlots.Last().Timeslot_ListId,
                    Mc_Wait_List_Id = mcWait.TempMc_Wait_ListId,
                    Allocation = 2, // Rework
                    Slot_Not_Avl = 'N',
                    Not_Avl_reason = 0
                };
                await _woService.PostTempMc_Timeslot_List(newMcSlot);

                // Update the Rework entry
                rwk.Allocated = 'Y';
                await _woService.PostRwk_List(rwk);
            }
            var prodnWos = await _woService.AllProductionPlan_Wo();


            var nonPlanList = await _woService.GetAllNon_Plan_Wk_List(); // Allocated = 'N' and Mc_Id != null
            foreach (var np in nonPlanList.Where(n => n.Allocated == 'N' && n.Mc_Id > 0))
            {
                await CheckPauseAsync();

                var machine = await _machineService.GetMachine(np.Mc_Id);
                if (machine == null) continue;

                var plant = await _plantService.GetPlantWD(machine.MachinePlantId);
                if (plant == null) continue;

                int timeslotDuration = plant.Timeslot_duration;
                int durationInMinutes = (int)TimeSpan.Parse(np.Plan_Duration).TotalMinutes;
                int requiredSlots = (int)Math.Ceiling((double)durationInMinutes / timeslotDuration);

                var timeslots = allTimeslots
                    .Where(t => t.PlantId == machine.MachinePlantId && t.Start_time >= np.Plan_start_time && t.Break_Slot != 'Y')
                    .OrderBy(t => t.Start_time)
                    .ToList();

                var availableSlots = timeslots.Take(requiredSlots).ToList();
                if (availableSlots.Count < requiredSlots) continue;

                // Create Mc_Wait_List record
                var mcWait = new TempMc_Wait_ListVM
                {
                    Wo_Id = 0,
                    Opr_No_Id = 0,
                    Mc_Id = np.Mc_Id,
                    Wait_Seq_No = await CalculateNextTempWaitSeqNo(np.Mc_Id),
                    Plan_start_time_Id = availableSlots.First().Timeslot_ListId,
                    Plan_end_time_Id = availableSlots.Last().Timeslot_ListId,
                    Mc_TPT = durationInMinutes,
                    Mode = 9, // Non-Plan mode
                    Plan_Qnty = 0
                };
                mcWait = await _woService.PostTempMc_Wait_List(mcWait);

                // Allocate in Mc_Timeslot_List
                var newMcSlot = new TempMc_Timeslot_ListVM
                {
                    Mc_Id = np.Mc_Id,
                    Timeslot_List_Id = availableSlots.First().Timeslot_ListId,
                    EndTimeslot_List_Id = availableSlots.Last().Timeslot_ListId,
                    Mc_Wait_List_Id = mcWait.TempMc_Wait_ListId,
                    Allocation = 3, // Non-plan work
                    Slot_Not_Avl = 'N',
                    Not_Avl_reason = 0
                };
                await _woService.PostTempMc_Timeslot_List(newMcSlot);

                // Mark the non-plan record as allocated
                np.Allocated = 'Y';
                await _woService.PostNon_Plan_Wk_List(np);
            }

            // 2. Calculate TPT for each Opr
            foreach (var wo in readyWOs)
            {
                await CheckPauseAsync();
                var oprs = allOprs.Where(o => o.Wo_Id == wo.Wo_Id).OrderBy(o => o.Opr_No).ToList();
                double totalTPT = 0;

                foreach (var opr in oprs)
                {
                    //var routingInfo = await _routingService.GetRoutingInfo(opr.Opr_No);
                    //var routingSteps = await _routingService.RoutingSteps(opr.Opr_No);
                    //double setupTime = routingInfo.SetupTime;
                    //double floorToFloorTime = routingInfo.FloorToFloorTime;
                    //double tpt = setupTime + (floorToFloorTime * wo.Plan_Simul_Qnty);
                    opr.Initial_Opr_TPT = opr.Initial_Opr_TPT;
                    await _woService.PostTempOpr_List(opr);
                    totalTPT += opr.Initial_Opr_TPT;
                }

                wo.Total_TPT = Convert.ToInt32(totalTPT);
                await _woService.PostTempWo_Wait_List(wo);
            }

            // 3. Sequence WOs
            var sequencedWOs = readyWOs
                .OrderBy(wo => wo.Plan_End_Date)
                .ThenBy(wo => wo.Total_TPT)
                .ToList();

            int seq = 1;
            foreach (var wo in sequencedWOs)
            {
                await CheckPauseAsync();
                wo.WO_Wait_Seq_No = seq++;
                await _woService.PostTempWo_Wait_List(wo);
            }

            // 4. Allocate Timeslots per WO & Operation
            foreach (var wo in sequencedWOs)
            {
                await CheckPauseAsync();
                var oprs = allOprs.Where(o => o.Wo_Id == wo.Wo_Id).OrderBy(o => o.Opr_No).ToList();

                foreach (var opr in oprs)
                {
                    await CheckPauseAsync();
                    var oprMachines = await _routingService.StepMachines((int)opr.Opr_No);
                    var prodwo = prodnWos.Where(w => w.ProductionPlanId == wo.Wo_Id).FirstOrDefault();
                    var routingsteps = await _routingService.RoutingSteps((int)prodwo.RoutingId);
                    var routingstep = routingsteps.FirstOrDefault(r => r.StepId == opr.Opr_No);
                    int noOfSimultMcs = routingstep.NumberOfSimMachines;
                    if (routingstep.StepLocation == "2") // SubCon
                    {
                        var subconList = await _routingService.SubCons((int)opr.Opr_No);
                        var subtransport = subconList.FirstOrDefault(s => s.PreferredSubcon == 1) ?? subconList.FirstOrDefault();

                        if (subtransport != null)
                        {
                            var subconwss = await _routingService.SubConWSS((int)opr.Opr_No, subtransport.SubConDetailsId);
                            var subconws = subconwss.FirstOrDefault();
                            // Time calculations
                            var trans = TimeSpan.Parse(subtransport.TransportTime); // "01:00:00"
                            var setupTime = TimeSpan.Parse(subconws.SetupTime); // "01:00:00"
                            var floorToFloorTime = TimeSpan.Parse(subconws.FloorToFloorTime);
                            int qty = wo.Plan_Simul_Qnty;
                            double totalMinutes = trans.TotalMinutes + setupTime.TotalMinutes + (floorToFloorTime.TotalMinutes * qty);

                            // Get plant working duration per day from any machine's plant
                            var anyMachine = await _machineService.GetMachine(oprMachines.First().MachineId);
                            var plant = await _plantService.GetPlantWD(anyMachine.MachinePlantId);

                            int totalWorkingMinutes = 0;

                            if (plant.NoOfShifts >= 1)
                                totalWorkingMinutes += (int)TimeSpan.Parse(plant.FirstShiftDuration).TotalMinutes;
                            if (plant.NoOfShifts >= 2)
                                totalWorkingMinutes += (int)TimeSpan.Parse(plant.SecondShiftDuration).TotalMinutes;
                            if (plant.NoOfShifts == 3)
                                totalWorkingMinutes += (int)TimeSpan.Parse(plant.ThirdShiftDuration).TotalMinutes;

                            int slotsPerDay = totalWorkingMinutes / plant.Timeslot_duration;
                            int subconDays = (int)Math.Ceiling(totalMinutes / totalWorkingMinutes);


                            // Timeslot allocation logic
                            DateTime today = DateTime.Now.Date;
                            var futureSlots = allTimeslots
                                .Where(t => t.Start_time.Date > today)
                                .OrderBy(t => t.Start_time)
                                .ToList();

                            DateTime subconStartDate = futureSlots.First().Start_time.Date;
                            DateTime subconEndDate = subconStartDate.AddDays(subconDays - 1);

                            long? startId = futureSlots
                                .FirstOrDefault(t => t.Start_time.Date == subconStartDate)?.Timeslot_ListId;
                            long? endId = futureSlots
                                .Where(t => t.Start_time.Date == subconEndDate)
                                .LastOrDefault()?.Timeslot_ListId;

                            if (startId.HasValue && endId.HasValue)
                            {
                                opr.Subcon_plan_start_time = startId.Value;
                                opr.Subcon_plan_end_time = endId.Value;
                                opr.Rolledup_Opr_TPT = opr.Initial_Opr_TPT;

                                await _woService.PostTempOpr_List(opr);
                            }
                        }
                    }
                    else
                    {
                        int noOfMcs = noOfSimultMcs;
                        int qtyPerMc = wo.Plan_Simul_Qnty / noOfMcs;
                        var machines = await _routingService.StepMachines((int)opr.Opr_No);
                        List<Mc_Wait_ListVM> mcWaits = new List<Mc_Wait_ListVM>();

                        foreach (var mc in machines.Take(noOfMcs))
                        {
                            var setupTime = TimeSpan.Parse(mc.SetupTime); // e.g., "00:15:00"
                            var floorToFloorTime = TimeSpan.Parse(mc.FloorToFloorTime);
                            var tpt = setupTime.TotalMinutes + (floorToFloorTime.TotalMinutes * qtyPerMc); // total time in minutes
                            var getmachine =await _machineService.GetMachine(mc.MachineId);
                            var plantwd = await _plantService.GetPlantWD(getmachine.MachinePlantId);
                            int slotsRequired = (int)Math.Ceiling(tpt / plantwd.Timeslot_duration);

                            var mcTimeslots = allMcTimeslots
                                .Where(s => s.Mc_Id == mc.MachineId && s.Allocation == 1 && s.Slot_Not_Avl != 'Y')
                                .OrderBy(s => s.Timeslot_List_Id) // use Timeslot_List_Id order
                                .ToList();

                            var startIdx = mcTimeslots.FindIndex(s =>
                            {
                                var slotTime = allTimeslots.FirstOrDefault(t => t.Timeslot_ListId == s.Timeslot_List_Id)?.Start_time;
                                return slotTime.HasValue && slotTime.Value > DateTime.Now;
                            });

                            if (startIdx >= 0 && (startIdx + slotsRequired) <= mcTimeslots.Count)
                            {
                                var availableSlots = mcTimeslots.Skip(startIdx).Take(slotsRequired).ToList();

                                var mcWait = new Mc_Wait_ListVM
                                {
                                    Wo_Id = wo.Wo_Id,
                                    Opr_No_Id = opr.Opr_No,
                                    Mc_Id = mc.MachineId,
                                    Plan_start_time_Id = availableSlots.First().Mc_Timeslot_List_Id,
                                    Plan_end_time_Id = availableSlots.Last().Mc_Timeslot_List_Id,
                                    Mc_TPT = Convert.ToDecimal(tpt),
                                    Mode = 1,
                                    Plan_Qnty = qtyPerMc
                                };

                                mcWait = await _woService.PostMc_Wait_List(mcWait);

                                foreach (var slot in availableSlots)
                                {
                                    slot.Allocation = 2;
                                    slot.Mc_Wait_List_Id = mcWait.Mc_Wait_ListId;
                                    await _woService.PostMc_Timeslot_List(slot);
                                }

                                mcWaits.Add(mcWait);
                            }
                        }

                        // Update Opr_List with aggregated info
                        if (!mcWaits.Any())
                            continue;
                        opr.Shop_Plan_start_time = mcWaits.Min(mw => mw.Plan_start_time_Id);
                        opr.Shop_Plan_end_time = mcWaits.Max(mw => mw.Plan_end_time_Id);
                        opr.Rolledup_Opr_TPT = mcWaits.Sum(mw => Convert.ToInt64(mw.Mc_TPT));
                        await _woService.PostTempOpr_List(opr);

                        // Calculate Mc_Wait_List.Next_Opr_Start_time_ID
                        foreach (var mcWait in mcWaits)
                        {
                            var startSlot = allMcTimeslots.FirstOrDefault(s => s.Mc_Timeslot_List_Id == mcWait.Plan_start_time_Id);
                            var startTime = allTimeslots.FirstOrDefault(t => t.Timeslot_ListId == startSlot.Timeslot_List_Id)?.Start_time ?? DateTime.Now;

                            // Find the corresponding machine again
                            var mc = machines.First(m => m.MachineId == mcWait.Mc_Id);

                            var setupTime = TimeSpan.Parse(mc.SetupTime);
                            var partTime = TimeSpan.FromMinutes(TimeSpan.Parse(mc.FloorToFloorTime).TotalMinutes * Math.Min(qtyPerMc, 1));
                            var requiredTime = setupTime + partTime;

                            var nextSlotTime = startTime.Add(requiredTime);
                            var nextSlot = allTimeslots.FirstOrDefault(t => t.Start_time >= nextSlotTime);

                            mcWait.Next_Opr_Start_time_Id = nextSlot?.Timeslot_ListId ?? mcWait.Plan_end_time_Id;
                            await _woService.PostMc_Wait_List(mcWait);
                        }

                    }
                }
                if (!allTimeslots.Any() || !allMcTimeslots.Any())
                    continue;

                // Get the start and end Mc_Timeslot_List entries from Shop_Plan_start_time and end_time
                var startMcSlotId = oprs.First().Shop_Plan_start_time;
                var endMcSlotId = oprs.Last().Shop_Plan_end_time;

                var startMcSlot = allMcTimeslots.FirstOrDefault(ts => ts.Mc_Timeslot_List_Id == startMcSlotId);
                var endMcSlot = allMcTimeslots.FirstOrDefault(ts => ts.Mc_Timeslot_List_Id == endMcSlotId);

                // Now get the corresponding Timeslot_List entries
                var startTimeslot = allTimeslots.FirstOrDefault(ts => ts.Timeslot_ListId == startMcSlot?.Timeslot_List_Id);
                var endTimeslot = allTimeslots.FirstOrDefault(ts => ts.Timeslot_ListId == endMcSlot?.Timeslot_List_Id);

                if (startTimeslot?.Start_time != null && endTimeslot?.End_time != null)
                {
                    wo.Plan_Start_Date = startTimeslot.Start_time;
                    wo.Plan_End_Date = endTimeslot.End_time;
                    await _woService.PostTempWo_Wait_List(wo);
                }

            }
            try
            {
                SimulationState.IsPaused = false;
                SimulationState.IsStopped = false;
                // your simulation logic here...

                return Json(new { message = "Update Production Completion Status completed." });
            }
            catch (OperationCanceledException ex)
            {
                return Json(new { message = "Update Production Completion Status stopped." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> FreezeSimulation([FromBody]List<int> selectedWOIds)
        {
            foreach (var woId in selectedWOIds)
            {
                var wos = await _woService.GetAllTempWo_Wait_List();
                var tempwo = wos.Where(w => w.Wo_Id == woId).FirstOrDefault();
                var wo = new WO_Wait_ListVM()
                {
                    Wo_Id = tempwo.Wo_Id,
                    Allow_Routing_Chg = tempwo.Allow_Routing_Chg,
                    Total_TPT = tempwo.Total_TPT,
                    Rework_Wo = tempwo.Rework_Wo,
                    NC_Log_Ref = tempwo.NC_Log_Ref,
                    WO_Wait_Seq_No = tempwo.WO_Wait_Seq_No,
                    Plan_Start_Date = tempwo.Plan_Start_Date,
                    Plan_End_Date = tempwo.Plan_End_Date,
                    Plan_Simul_Qnty = tempwo.Plan_Simul_Qnty,
                    Mode = 2
                };
                await _woService.PostWO_Wait_List(wo);

                var oprs = await _woService.GetAllTempOpr_List();
                var ops = oprs.Where(o => o.Wo_Id == woId).ToList();
                foreach (var tempopr in oprs)
                {
                    var opr = new Opr_ListVM()
                    {
                        Wo_Id = tempopr.Wo_Id,
                        Opr_No = tempopr.Opr_No,
                        Initial_Opr_TPT = tempopr.Initial_Opr_TPT,
                        Rolledup_Opr_TPT = tempopr.Rolledup_Opr_TPT,
                        Rework_Wo = tempopr.Rework_Wo,
                        NC_Log_Ref = tempopr.NC_Log_Ref,
                        Act_Qnty = tempopr.Act_Qnty,
                        Plan_Qnty = tempopr.Plan_Qnty,
                        No_of_Simult_Mcs = tempopr.No_of_Simult_Mcs,
                        Shop_Plan_start_time = tempopr.Shop_Plan_start_time,
                        Shop_Plan_end_time = tempopr.Shop_Plan_end_time,
                        Subcon_plan_start_time = tempopr.Subcon_plan_start_time,
                        Subcon_plan_end_time = tempopr.Subcon_plan_end_time,
                        Setup_Start_time = tempopr.Setup_Start_time,
                        Act_End_time = tempopr.Act_End_time,
                        Mode = 2
                    };
                    await _woService.PostOpr_List(opr);
                }

                var mcWaits = await _woService.GetAllTempMc_Wait_List();
                var mcWait = mcWaits.Where(o => o.Wo_Id == woId).ToList();
                foreach (var tempmw in mcWait)
                {
                    var mw = new Mc_Wait_ListVM()
                    {
                        Wo_Id = tempmw.Wo_Id,
                        Opr_No_Id = tempmw.Opr_No_Id,
                        Mc_Id = tempmw.Mc_Id,
                        Wait_Seq_No = tempmw.Wait_Seq_No,
                        Plan_Qnty = tempmw.Plan_Qnty,
                        Rework_Wo = tempmw.Rework_Wo,
                        Non_Plan_Wk = tempmw.Non_Plan_Wk,
                        Non_Plan_wk_Id = tempmw.Non_Plan_wk_Id,
                        Plan_start_time_Id = tempmw.Plan_start_time_Id,
                        Plan_end_time_Id = tempmw.Plan_end_time_Id,
                        Next_Opr_Start_time_Id = tempmw.Next_Opr_Start_time_Id,
                        Setup_Apprvl_time = tempmw.Setup_Apprvl_time,
                        Act_End_time = tempmw.Act_End_time,
                        Mc_TPT = tempmw.Mc_TPT,
                        Setup_Start_time = tempmw.Setup_Start_time,
                        Mode = 2
                    };
                   var activemw = await _woService.PostMc_Wait_List(mw);

                    var slots = await _woService.GetAllTempMc_Timeslot_List();
                    var slotsmc = slots.Where(s => s.Mc_Wait_List_Id == tempmw.TempMc_Wait_ListId).ToList();
                    foreach (var tempslot in slotsmc)
                    {
                        var slot = new Mc_Timeslot_ListVM()
                        {
                            Timeslot_List_Id = tempslot.Timeslot_List_Id,
                            EndTimeslot_List_Id = tempslot.EndTimeslot_List_Id,
                            Mc_Id = tempslot.Mc_Id,
                            Mc_Wait_List_Id = activemw.Mc_Wait_ListId,
                            Slot_Not_Avl = tempslot.Slot_Not_Avl,
                            Not_Avl_reason = tempslot.Not_Avl_reason,
                            Allocation = 3
                        };
                        await _woService.PostMc_Timeslot_List(slot);
                    }
                }
                var tempSubCons = await _woService.GetAllTempSubCon_List();
                var tempSubConss = tempSubCons.Where(o => o.Wo_Id == woId).ToList(); foreach (var tempSubCon in tempSubConss)
                {
                    var subCon = new SubCon_ListVM()
                    {
                        Wo_Id = tempSubCon.Wo_Id,
                        Opr_No = tempSubCon.Opr_No,
                        Supplier_Id = tempSubCon.Supplier_Id,
                        Mode = tempSubCon.Mode,
                        Act_Qnty = tempSubCon.Act_Qnty,
                        Plan_Qnty = tempSubCon.Plan_Qnty,
                        Loaded = tempSubCon.Loaded,
                        Rework_Wo = tempSubCon.Rework_Wo,
                        Plan_Disp_date = tempSubCon.Plan_Disp_date,
                        Plan_Recpt_date = tempSubCon.Plan_Recpt_date,
                        Act_Disp_date = tempSubCon.Act_Disp_date,
                        Act_Recpt_date = tempSubCon.Act_Recpt_date
                    };

                    await _woService.PostSubCon_List(subCon); 
                }
            }
            return Json(new { message = "Simulation Freezed." });
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllSimOutputWo()
        {
            var productions = await _woService.AllProductionPlan_Wo();
            var masterparts = await _masterService.ItemMasterParts();
            var procplan = await _woService.GetAllProcPlan();
            var customer = await _baService.GetCustomerOrders();
            var wO_Wait_Lists = await _woService.GetAllTempWo_Wait_List();
            var opr_Lists = await _woService.GetAllOpr_List();
            var allTimeslots = await _woService.GetAllTimeslot_List();
            var activewO_Wait_Lists = await _woService.GetAllWO_Wait_List();
            var allMcTimeslots = await _woService.GetAllMc_Timeslot_List();
            var rwkList = await _woService.GetAllRwk_List();
            var ncLogs = await _woService.GetAllNcLog();
            var recpts = await _woService.GetAllInw_Recpt_Header();
            var poDetails = await _woService.GetAllPodetails();
            var customerDict = customer.ToDictionary(c => c.CustomerOrderId);
            var masterPartDict = masterparts.ToDictionary(p => p.PartId);
            var procPlanDict = procplan
    .GroupBy(p => p.WorkOrderId)
    .ToDictionary(g => g.Key, g => g.ToList());
            var timeslotDict = allTimeslots.ToDictionary(t => t.Timeslot_ListId);
            var mcTimeslotDict = allMcTimeslots.ToDictionary(m => m.Mc_Timeslot_List_Id);
            //var woWaitListDict = wO_Wait_Lists.GroupBy(o => o.Wo_Id).ToDictionary(w => w.Key, w => w);
            var oprLookup = opr_Lists.GroupBy(o => o.Wo_Id).ToDictionary(g => g.Key, g => g.ToList());

            var result = new List<ProductionPlan_WoVM>();
            var rwkWoIds = new HashSet<int>();

            foreach (var rwk in rwkList)
            {
                var ncLog = ncLogs.FirstOrDefault(n => n.Insp_Outcome_Details_Id == rwk.NC_Log_Id);
                if (ncLog == null) continue;

                var recpt = recpts.FirstOrDefault(r => r.Inw_Recpt_HeaderId == ncLog.Inw_Recpt_Header_Id);
                if (recpt == null) continue;

                var po = poDetails.FirstOrDefault(p => p.PoDetailsId == recpt.PoHeaderId);
                if (po == null) continue;

                var relatedProcPlan = procplan.FirstOrDefault(pp => pp.ProcPlanId == po.ProcPlanId);
                if (relatedProcPlan == null) continue;

                rwkWoIds.Add((int)relatedProcPlan.WorkOrderId); // This is the WoId
            }

            foreach (var item in productions)
            {
                item.PlanStartDateStr = (item.PartType == 2)
                 ? item.PlanStartDate.ToString("dd-MM-yyyy")
                 : (procPlanDict.TryGetValue(item.WoId, out var plans) && plans.Any()
                     ? plans.OrderByDescending(p => p.CalcReceiptDate).First().CalcReceiptDate.ToString("dd-MM-yyyy")
                     : string.Empty);

                item.SoComplDateStr = item.SoComplDate?.ToString("dd-MM-yyyy");

                if (masterPartDict.TryGetValue(item.PartId, out var part))
                {
                    item.PartNo = part.PartNo;
                    item.PartDesc = part.Description;
                    bool isParentWo = productions.Any(x => x.ParentWoId == item.WoId);

                    if (item.PartType == 1 && item.ParentWoId == 0)
                        item.PartTypeName = isParentWo ? "Parent CMP" : "CMP";
                    else if (item.PartType == 2)
                        item.PartTypeName = "Assembly";
                }

                var so = await _baService.GetOneSO(item.SalesOrderId); // Consider caching GetOneSO results if reused
                if (so != null)
                {
                    if (customerDict.TryGetValue(so.CustomerOrderId, out var cust))
                    {
                        item.Customer = cust.CustomerName;
                    }

                    // Release condition skipped/commented logic
                }
                else
                {
                    var wosos = await _woService.GetSoWoRel(item.WoId);
                    foreach (var woso in wosos)
                    {
                        var sos = await _baService.GetOneSO(woso.SalesOrderId); // Again, consider caching
                        if (sos != null)
                        {
                            if (customerDict.TryGetValue(sos.CustomerOrderId, out var cus))
                            {
                                item.Customer = cus.CustomerName;
                            }
                        }
                    }
                }

                // Earliest Start Time
                if (oprLookup.TryGetValue(item.ProductionPlanId, out var itemOprs))
                {
                    var startTimes = itemOprs
                        .Select(o => mcTimeslotDict.TryGetValue(o.Shop_Plan_start_time, out var mcTime)
                                        && timeslotDict.TryGetValue(mcTime.Timeslot_List_Id, out var slot)
                                        ? slot.Start_time
                                        : (DateTime?)null)
                        .Where(t => t != null)
                        .ToList();

                    if (startTimes.Any())
                    {
                        item.ActStartDateStr = startTimes.Min().Value.ToString("dd-MM-yyyy");
                    }
                }

                item.DataChange = (item.Changed == 0) ? "N" : "Y";
                var waitItem = wO_Wait_Lists.Where(w => w.Wo_Id == item.ProductionPlanId).FirstOrDefault();
                if (waitItem != null)
                {
                    item.CsStartDate = waitItem.Plan_Start_Date.ToString("dd-MM-yyyy");
                    item.CsEndDate = waitItem.Plan_End_Date.ToString("dd-MM-yyyy");
                    item.CriticalParts = (waitItem.Plan_End_Date > item.PlanCompletionDate) ? "Y" : "N";
                    if(item.ActStartDateStr == string.Empty)
                    {
                        item.ActStartDateStr = waitItem.Plan_Start_Date.ToString("dd-MM-yyyy");
                    }
                    var activewO_Wait_List = activewO_Wait_Lists.Where(w => w.Wo_Id == item.ProductionPlanId).FirstOrDefault();
                    if (activewO_Wait_List != null)
                    {
                        item.PsStartDate = activewO_Wait_List.Plan_Start_Date.ToString("dd-MM-yyyy");
                        item.PsEndDate = activewO_Wait_List.Plan_End_Date.ToString("dd-MM-yyyy");
                    }
                    var mf = await _masterService.GetManufPart((int)item.PartId);  // Consider caching this call
                    var routingList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                    var routingStep = await _routingService.RoutingSteps((int)item.RoutingId);
                    item.NoOfRoutes = routingList.Count();
                    item.RoutingName = routingList.First(r=>r.RoutingId == item.RoutingId).RoutingName;
                    item.CurOpr = routingStep.First(r=>r.StepId == item.StartingOpNo).StepNumber;
                    item.ReworkWo = rwkWoIds.Contains((int)item.WoId) ? "Y" : "N";
                    item.Holdstr = (item.Status == 8) ? "Y" : "N";
                    result.Add(item);
                }
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllChildPartAssem(long woId)
        {
            var productions = await _woService.AllProductionPlan_Wo();
            var procplan = await _woService.GetAllProcPlan();
            var findwo = productions.Where(p => p.WoId == woId && p.ParentWoId == 0).FirstOrDefault();
            var childwos = productions.Where(p => p.ParentWoId == woId).ToList();
            var childpros = procplan.Where(p => p.WorkOrderId == woId).ToList();
            var masterparts = await _masterService.ItemMasterParts();
            var pODetails = await _woService.GetAllPodetails();
            var masterPartDict = masterparts.ToDictionary(p => p.PartId);
            List<ProductionPlan_WoVM> result = new List<ProductionPlan_WoVM>();
            foreach (var item in childwos)
            {
                if (masterPartDict.TryGetValue(item.PartId, out var part))
                {
                    item.PartNo = part.PartNo;
                    bool isParentWo = productions.Any(x => x.ParentWoId == item.WoId);

                    if (item.PartType == 1 )
                        item.PartTypeName = "Child Manf Part";
                    else if (item.PartType == 2)
                        item.PartTypeName = "Sub Assy";

                    var wostatus = await _woService.GetWOStatus(item.Status);
                    item.WoStatus = wostatus.Status;
                    item.FinQnty = "0";
                    item.PoNumber = "-";
                    item.PoStatus = "-";
                    result.Add(item);
                }
            }
            foreach (var pp in childpros)
            {
                if (pp.PartType == "RawMaterial")
                {
                    continue;
                }
                var po = pODetails.Where(po => po.ProcPlanId == pp.ProcPlanId).FirstOrDefault();
                if(po!=null)
                {
                    ProductionPlan_WoVM item = new ProductionPlan_WoVM();
                    item.PoNumber = pp.Reference;
                    if (masterPartDict.TryGetValue(pp.PartId, out var part))
                    {
                        item.PartNo = part.PartNo;
                    }
                    if (findwo != null)
                    {
                        item.CalcWOQty = findwo.CalcWOQty;
                        var wostatus = await _woService.GetWOStatus(findwo.Status);
                        item.WoStatus = wostatus.Status;
                    }
                    item.PoQnty = po.PoQnty.ToString();
                    item.WONumber = "-";
                    if (po.Status == 1)
                    {
                        item.PoStatus = "Not Approved";
                    }
                    else if (po.Status == 2)
                    {
                        item.PoStatus = "PO Approved";
                    }
                    else if (po.Status == 3)
                    {
                        item.PoStatus = "Completed";
                    }
                    item.PartTypeName = pp.PartType;
                    item.FinQnty = "0";
                    result.Add(item);
                }
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateWOStatusOnBookout(long woId, int oprNo, int mcId, int balBookoutQty, DateTime bookoutTime)
        {
            var wos = await _woService.AllProductionPlan_Wo();
            var allTransLogs = await _woService.GetAllInv_Trans_Log();
            var wo = wos.Where(w => w.ProductionPlanId == woId).FirstOrDefault();
            if (wo == null) return NotFound("WO not found");

            var totalBookoutQty = allTransLogs
    .Where(t => t.Wo_Id == woId )
    .Sum(t => t.Qnty);

            bool isFinalOpr = (oprNo == wo.EndingOpNo);


            // ----- CASE 1: Last Operation + No Balance -----
            if (isFinalOpr && balBookoutQty == 0)
            {
                wo.Status = 6;
                wo.ActWOQty = (int)totalBookoutQty;
                wo.ActCompletionDate = bookoutTime;
                await _woService.UpdateProductionPlan_Wo(wo);
                return Ok("WO marked Complete");
            }

            // ----- CASE 2: Not Final Opr + No Balance (allow next step) -----
            if (!isFinalOpr && balBookoutQty == 0)
            {
                wo.Status = 5;
                wo.ActWOQty = (int)totalBookoutQty;
                wo.ActCompletionDate = bookoutTime;
                await _woService.UpdateProductionPlan_Wo(wo);
                return Ok("WO WIP - current operation completed");
            }

            // ----- CASE 3: Not Final Opr + Balance remaining (shift-end partial) -----
            if (!isFinalOpr && balBookoutQty > 0)
            {
                wo.Status = 5;
                await _woService.UpdateProductionPlan_Wo(wo);
                return Ok("WO WIP - partial bookout");
            }

            return Ok("No matching condition found");
        }


        [HttpGet]
        public async Task<IActionResult> GetAllMcWos(long mcId)
        {
            var productions = await _woService.AllProductionPlan_Wo();
            var masterparts = await _masterService.ItemMasterParts();
            var mcwaitList = await _woService.GetAllTempMc_Wait_List();
            var allTimeSlots = await _woService.GetAllTimeslot_List();
            var mcwaits = mcwaitList.Where(m => m.Mc_Id == mcId).ToList();
            var wO_Wait_Lists = await _woService.GetAllTempWo_Wait_List();
            var nclogs = await _woService.GetAllNcLog();
            var recpts = await _woService.GetAllInw_Recpt_Header();
            var podetails = await _woService.GetAllPodetails();
            var procplans = await _woService.GetAllProcPlan();
            var rwkList = await _woService.GetAllRwk_List();
            var allTimeslots = await _woService.GetAllTimeslot_List();
            foreach (var item in mcwaits)
            {
                var pwo = productions.Where(p => p.ProductionPlanId == item.Wo_Id).FirstOrDefault();
                item.WoNumber = pwo.WONumber;
                item.DataChanged = (pwo.Changed == 0) ? "N" : "Y";
                item.WoQnty = pwo.CalcWOQty.ToString();
                item.Bal_Qnty = pwo.CalcWOQty;
                var imp = masterparts.Where(im => im.PartId == pwo.PartId).FirstOrDefault();
                item.PartNo = imp.PartNo + " / "+imp.Description;
                var mf = await _masterService.GetManufPart((int)pwo.PartId);  // Consider caching this call
                var routingList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                var routingStep = await _routingService.RoutingSteps((int)pwo.RoutingId);
                item.RoutingName =routingList.First(r => r.RoutingId == pwo.RoutingId).RoutingName;
                item.OprNoName = routingStep.First(r => r.StepId == item.Opr_No_Id).StepNumber;
                var waitItem = wO_Wait_Lists.Where(w => w.Wo_Id == pwo.ProductionPlanId).FirstOrDefault();
                if(waitItem != null)
                {
                item.CsStartDate = waitItem.Plan_Start_Date.ToString("dd-MM-yyyy");
                item.CsEndDate = waitItem.Plan_End_Date.ToString("dd-MM-yyyy");
                item.ActStartDate = waitItem.Plan_Start_Date.ToString("dd-MM-yyyy");
                    item.Rework_Wo = 'N';
                }
                else
                {
                    var procplan = procplans.Where(pp => pp.WorkOrderId == pwo.WoId).FirstOrDefault();
                    var podetail = podetails.Where(p => p.PartId == procplan.PartId).FirstOrDefault();
                    var recpt = recpts.Where(r => r.PoHeaderId == podetail.PoDetailsId).FirstOrDefault();
                    var nclog = nclogs.Where(n => n.Inw_Recpt_Header_Id == recpt.Inw_Recpt_HeaderId).FirstOrDefault();
                    var rewk = rwkList.Where(w => w.NC_Log_Id == nclog.Insp_Outcome_Details_Id).FirstOrDefault();
                    var csstart = allTimeslots.Where(t => t.Timeslot_ListId == item.Plan_start_time_Id).FirstOrDefault();
                    var csend = allTimeslots.Where(t => t.Timeslot_ListId == item.Plan_end_time_Id).FirstOrDefault();
                    item.CsStartDate = csstart.Start_time.ToString("dd-MM-yyyy");
                    item.CsEndDate = csend.End_time.ToString("dd-MM-yyyy");
                    item.ActStartDate = rewk.Actual_Start_Time.ToString("dd-MM-yyyy");
                    item.Rework_Wo = 'Y';
                }
                item.MatlIssued = "";
                item.Non_Plan_Wk = 'N';
                var routingMcs = await _routingService.StepMachines((int)item.Opr_No_Id);
                var routingMc = routingMcs.FirstOrDefault(r => r.PreferredMachine==1);
                if(routingMc == null)
                {
                    routingMc = routingMcs.FirstOrDefault();
                }
                if(item.Wait_Seq_No == 0)
                {
                    if (TimeSpan.TryParse(routingMc.FloorToFloorTime, out var f2fTime))
                    {
                        double f2fHrs = f2fTime.TotalHours;
                        double totalHrs = item.Bal_Qnty * f2fHrs;
                        TimeSpan hrsBooked = TimeSpan.FromHours(totalHrs);
                        item.HrsBooked = $"{(int)hrsBooked.TotalHours:D2}:{hrsBooked.Minutes:D2}";
                    }
                    else
                    {
                        item.HrsBooked = "00:00";
                    }
                    item.WaitTime = "";
                }
                else
                {
                    if (TimeSpan.TryParse(routingMc.SetupTime, out var setupTime) &&
                        TimeSpan.TryParse(routingMc.FloorToFloorTime, out var cycleTime))
                    {
                        double setupHrs = setupTime.TotalHours;
                        double cycleHrs = cycleTime.TotalHours;
                        double totalHrs = setupHrs + (item.Bal_Qnty * cycleHrs);
                        TimeSpan hrsBooked = TimeSpan.FromHours(totalHrs);
                        item.HrsBooked = $"{(int)hrsBooked.TotalHours:D2}:{hrsBooked.Minutes:D2}";
                    }
                    else
                    {
                        item.HrsBooked = "00:00";
                    }
                    var currentStart = allTimeSlots.FirstOrDefault(t => t.Timeslot_ListId == item.Plan_start_time_Id)?.Start_time;
                    var prevTask = mcwaits
                        .Where(w => w.Mc_Id == item.Mc_Id && w.Wait_Seq_No == item.Wait_Seq_No - 1)
                        .FirstOrDefault();
                    DateTime? prevEnd = null;
                    if (prevTask != null)
                    {
                        prevEnd = allTimeSlots
                            .FirstOrDefault(t => t.Timeslot_ListId == prevTask.Plan_end_time_Id)?.End_time;
                    }
                    if (currentStart != null && prevEnd != null)
                    {
                        if (currentStart == prevEnd)
                        {
                            item.WaitTime = "0"; // Starts immediately after previous
                        }
                        else
                        {
                            item.WaitTime = currentStart.Value.ToString("dd-MM-yyyy hh:mm tt");
                        }
                    }
                    else
                    {
                        item.WaitTime = "-"; // Could not determine time
                    }

                }
            }
            return Ok(mcwaits);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllSubconWos()
        {
            var tempsubconList = await _woService.GetAllTempSubCon_List();
            var prodnwos = await _woService.AllProductionPlan_Wo();
            var compaines = await _masterService.GetCompanies();
            foreach (var item in tempsubconList)
            {
                item.Supplier = compaines.FirstOrDefault(c => c.CompanyId == item.Supplier_Id).CompanyName;
                item.PartCount = prodnwos.Count(p => p.ProductionPlanId == item.Wo_Id);
            }
            var groupedSubcons = tempsubconList
                .GroupBy(s => new { s.Supplier_Id, s.Supplier })
                .Select(g => new TempSubCon_ListVM()
                {
                    Supplier_Id = g.Key.Supplier_Id,
                    Supplier = g.Key.Supplier,
                    PartCount = g.Sum(x => x.PartCount),
                    PartMatlSent = 0,
                    PartToSent = 0,
                    DateNotLoaded = g.LastOrDefault(x => x.Loaded == 'N')?.Plan_Recpt_date.ToString("dd-MM-yyyy") ?? "-"
                })
                .ToList();
            return Ok(groupedSubcons);
        }
        [HttpGet]
        public async Task<IActionResult> GetSubConMachineHoursMatrix()
        {
            // 1. Fetch all planned subcon operations
            var subconOps = await _woService.GetAllTempSubCon_List(); // Contains Wo_Id, Opr_No, Supplier_Id, Plan_Qnty, etc.

            var compaines = await _masterService.GetCompanies();
            var mactypes = await _machineService.GetMachineTypes();
            // 2. Group by SubCon and MachineType
            var subconMatrix = new Dictionary<string, Dictionary<string, double>>(); // SubConName => (MachineType => TotalHours)

            foreach (var subconOp in subconOps)
            {
                var subconList = await _routingService.SubCons((int)subconOp.Opr_No);
                var subtransport = subconList.FirstOrDefault(s => s.PreferredSubcon == 1) ?? subconList.FirstOrDefault();
                var subconWSS = await _routingService.SubConWSS((int)subconOp.Opr_No, subtransport.SubConDetailsId);
                var subconWS = subconWSS.FirstOrDefault();
                if (subconWS == null) continue;

                var subcon = compaines.Where(c=>c.CompanyId == subconOp.Supplier_Id).FirstOrDefault(); // get name
                var subconName = subcon?.CompanyName ?? "Unknown";
                var machineType = mactypes.Where(mt=>mt.MachineTypeTypeId == subconWS.MachineType).FirstOrDefault()?.MachineTypeName;

                var floorToFloorTime = TimeSpan.TryParse(subconWS.FloorToFloorTime, out var ftf) ? ftf : TimeSpan.Zero;
                var hours = (floorToFloorTime.TotalMinutes * subconOp.Plan_Qnty) / 60.0;

                // Add to matrix
                if (!subconMatrix.ContainsKey(subconName))
                    subconMatrix[subconName] = new Dictionary<string, double>();

                if (!subconMatrix[subconName].ContainsKey(machineType))
                    subconMatrix[subconName][machineType] = 0;

                subconMatrix[subconName][machineType] += hours;
            }

            // 3. Build flat list for frontend binding
            var allMachineTypes = subconMatrix.SelectMany(x => x.Value.Keys).Distinct().ToList();
            var result = new List<Dictionary<string, object>>();

            foreach (var mType in allMachineTypes)
            {
                var row = new Dictionary<string, object>();
                row["MachineType"] = mType;
                double rowTotal = 0;

                foreach (var subcon in subconMatrix.Keys)
                {
                    double hrs = subconMatrix[subcon].ContainsKey(mType) ? subconMatrix[subcon][mType] : 0;
                    row[subcon] = Math.Round(hrs, 2);
                    rowTotal += hrs;
                }

                row["TotalHrs"] = Math.Round(rowTotal, 2);
                result.Add(row);
            }

            // 4. Add final row for Total Hrs per subcon
            var totalRow = new Dictionary<string, object>();
            totalRow["MachineType"] = "Total Hrs";
            double grandTotal = 0;

            foreach (var subcon in subconMatrix.Keys)
            {
                double colTotal = subconMatrix[subcon].Values.Sum();
                totalRow[subcon] = Math.Round(colTotal, 2);
                grandTotal += colTotal;
            }
            totalRow["TotalHrs"] = Math.Round(grandTotal, 2);
            result.Add(totalRow);

            // Return as:
            return Ok(new
            {
                Columns = new List<string> { "MachineType" }.Concat(subconMatrix.Keys).Concat(new[] { "TotalHrs" }).ToList(),
                Rows = result
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSimWosBySuppiler(long Suppid)
        {
            var subconOps = await _woService.GetAllTempSubCon_List();
            var activesubconOps = await _woService.GetAllSubCon_List();
            var subcons = subconOps.Where(s => s.Supplier_Id == Suppid).ToList();
            var productions = await _woService.AllProductionPlan_Wo();
            var wO_Wait_Lists = await _woService.GetAllTempWo_Wait_List();
            var masterparts = await _masterService.ItemMasterParts();
            foreach (var item in subcons)
            {
                var pwo = productions.Where(p => p.ProductionPlanId == item.Wo_Id).FirstOrDefault();
                item.WoNumber = pwo.WONumber;
                item.DataChanged = (item.Changed == 0) ? "N" : "Y";
                var imp = masterparts.Where(im => im.PartId == pwo.PartId).FirstOrDefault();
                item.PartNo = imp.PartNo + " / " + imp.Description;
                var mf = await _masterService.GetManufPart((int)pwo.PartId);  
                var routingList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                var routingStep = await _routingService.RoutingSteps((int)pwo.RoutingId);
                item.RoutingName = routingList.First(r => r.RoutingId == pwo.RoutingId).RoutingName;
                item.OprNoName = routingStep.First(r => r.StepId == item.Opr_No).StepNumber;
                var waitItem = wO_Wait_Lists.Where(w => w.Wo_Id == pwo.ProductionPlanId).FirstOrDefault();
                var activesubconOp = activesubconOps.Where(w => w.Wo_Id == item.Wo_Id).FirstOrDefault();
                if (activesubconOp != null)
                {
                    item.PsStartDate = activesubconOp.Plan_Disp_date.ToString("dd-MM-yyyy");
                    item.PsEndDate = activesubconOp.Plan_Recpt_date.ToString("dd-MM-yyyy");
                }
                item.CsStartDate = item.Plan_Disp_date.ToString("dd-MM-yyyy");
                item.CsEndDate = item.Plan_Recpt_date.ToString("dd-MM-yyyy");

            }
            return Ok(subcons);
        }

        [HttpPost]
        public async Task<IActionResult> ReSimulateWOAllocation([FromBody] List<int> selectedWOIds)
        {
            var productions = await _woService.AllProductionPlan_Wo();
            var allOprs = new List<TempOpr_ListVM>();
            var allTimeslots = await _woService.GetAllTimeslot_List();
            var allPrevWo = await _woService.GetAllTempWo_Wait_List();
            var allMcTimeslots = await _woService.GetAllTempMc_Timeslot_List();
            var allmac = await _machineService.GetMachinesList();
            List<TempWO_Wait_ListVM> readyWOs = new List<TempWO_Wait_ListVM>();
            SimulationState.IsStopped = false;
            // Step 1: Create WO_Wait_List and Opr_List entries
            foreach (var woId in selectedWOIds)
            {
                await CheckPauseAsync();

                var item = productions.FirstOrDefault(p => p.ProductionPlanId == woId);
                if (item == null) continue;
                var prev = allPrevWo.FirstOrDefault(p => p.Wo_Id == woId);
                var addData = new TempWO_Wait_ListVM
                {
                    TempWO_Wait_ListId = prev.TempWO_Wait_ListId,
                    Wo_Id = item.ProductionPlanId,
                    Mode = 1,
                    Allow_Routing_Chg = 'Y',
                    Total_TPT = 0,
                    Rework_Wo = 'N',
                    NC_Log_Ref = 0,
                    WO_Wait_Seq_No = 0,
                    Plan_Start_Date = item.PlanStartDate,
                    Plan_End_Date = (DateTime)item.PlanCompletionDate,
                    Plan_Simul_Qnty = item.CalcWOQty
                };

                var woResult = await _woService.PostTempWo_Wait_List(addData);
                readyWOs.Add(woResult);

                var mf = await _masterService.GetManufPart((int)item.PartId);
                var route = (await _routingService.Routings(mf.ManufacturedPartNoDetailId))
                            .FirstOrDefault(r => r.RoutingId == item.RoutingId);
                if (route == null) continue;

                var routingSteps = await _routingService.RoutingSteps(route.RoutingId);

                foreach (var step in routingSteps)
                {
                    await CheckPauseAsync();

                    var tptResult = await GetTPT(step);
                    var tptStepItem = (RoutingStepVM)((OkObjectResult)tptResult).Value;
                    int setup = int.Parse(tptStepItem.SetupTime ?? "0");
                    int firstPiece = int.Parse(tptStepItem.FirstPieceTime ?? "0");
                    int cycle = int.Parse(tptStepItem.CycleTime ?? "0");
                    int quantity = item.CalcWOQty;

                    var opr = new TempOpr_ListVM
                    {
                        Wo_Id = item.ProductionPlanId,
                        Opr_No = step.StepId,
                        Mode = 1,
                        Rework_Wo = 'N',
                        NC_Log_Ref = 0,
                        Act_Qnty = 0,
                        Plan_Qnty = quantity,
                        No_of_Simult_Mcs = step.NumberOfSimMachines,
                        Initial_Opr_TPT = setup + firstPiece + cycle * (quantity - 1)
                    };
                    var oprResult = await _woService.PostTempOpr_List(opr);
                    allOprs.Add(oprResult);
                }
            }

            // Step 2: Recalculate Total TPT and Sequence WOs
            foreach (var wo in readyWOs)
            {
                var oprs = allOprs.Where(o => o.Wo_Id == wo.Wo_Id).OrderBy(o => o.Opr_No).ToList();
                double totalTPT = oprs.Sum(o => o.Initial_Opr_TPT);
                wo.Total_TPT = Convert.ToInt32(totalTPT);
                await _woService.PostTempWo_Wait_List(wo);
            }

            // Step 3: Sequence WOs
            var sequencedWOs = readyWOs.OrderBy(w => w.Plan_End_Date).ThenBy(w => w.Total_TPT).ToList();
            for (int i = 0; i < sequencedWOs.Count; i++)
            {
                sequencedWOs[i].WO_Wait_Seq_No = i + 1;
                await _woService.PostTempWo_Wait_List(sequencedWOs[i]);
            }

            // Step 4: Allocate timeslots
            foreach (var wo in sequencedWOs)
            {
                await CheckPauseAsync();
                var oprs = allOprs.Where(o => o.Wo_Id == wo.Wo_Id).OrderBy(o => o.Opr_No).ToList();
                var prodwo = productions.First(p => p.ProductionPlanId == wo.Wo_Id);
                var routingSteps = await _routingService.RoutingSteps((int)prodwo.RoutingId);

                foreach (var opr in oprs)
                {
                    await CheckPauseAsync();
                    var step = routingSteps.FirstOrDefault(s => s.StepId == opr.Opr_No);
                    var oprMachines = await _routingService.StepMachines((int)opr.Opr_No);
                    if (step == null) continue;

                    int noOfSimultMcs = step.NumberOfSimMachines;
                    if (step.StepLocation == "2") // SubCon
                    {
                        var subconList = await _routingService.SubCons((int)opr.Opr_No);
                        var subtransport = subconList.FirstOrDefault(s => s.PreferredSubcon == 1) ?? subconList.FirstOrDefault();

                        if (subtransport != null)
                        {
                            var subconwss = await _routingService.SubConWSS((int)opr.Opr_No, subtransport.SubConDetailsId);
                            var subconws = subconwss.FirstOrDefault();
                            // Time calculations
                            var trans = TimeSpan.Parse(subtransport.TransportTime); // "01:00:00"
                            var setupTime = TimeSpan.Parse(subconws.SetupTime); // "01:00:00"
                            var floorToFloorTime = TimeSpan.Parse(subconws.FloorToFloorTime);
                            int qty = wo.Plan_Simul_Qnty;
                            double totalMinutes = trans.TotalMinutes + setupTime.TotalMinutes + (floorToFloorTime.TotalMinutes * qty);

                            // Get plant working duration per day from any machine's plant
                            var anyMachine = await _machineService.GetMachine(allmac.FirstOrDefault(m => m.MachineTypeId == subconws.MachineType).MachineId);
                            var plant = await _plantService.GetPlantWD(anyMachine.MachinePlantId);

                            int totalWorkingMinutes = 0;

                            if (plant.NoOfShifts >= 1)
                                totalWorkingMinutes += (int)TimeSpan.Parse(plant.FirstShiftDuration).TotalMinutes;
                            if (plant.NoOfShifts >= 2)
                                totalWorkingMinutes += (int)TimeSpan.Parse(plant.SecondShiftDuration).TotalMinutes;
                            if (plant.NoOfShifts == 3)
                                totalWorkingMinutes += (int)TimeSpan.Parse(plant.ThirdShiftDuration).TotalMinutes;

                            int slotsPerDay = totalWorkingMinutes / plant.Timeslot_duration;
                            int subconDays = (int)Math.Ceiling(totalMinutes / totalWorkingMinutes);


                            // Timeslot allocation logic
                            DateTime today = DateTime.Now.Date;
                            var futureSlots = allTimeslots
                                .Where(t => t.Start_time.Date > today)
                                .OrderBy(t => t.Start_time)
                                .ToList();

                            DateTime subconStartDate = futureSlots.First().Start_time.Date;
                            DateTime subconEndDate = subconStartDate.AddDays(subconDays - 1);

                            long? startId = futureSlots
                                .FirstOrDefault(t => t.Start_time.Date == subconStartDate)?.Timeslot_ListId;
                            long? endId = futureSlots
                                .Where(t => t.Start_time.Date == subconEndDate)
                                .LastOrDefault()?.Timeslot_ListId;
                            DateTime planDispDate;
                            if (opr.Opr_No == prodwo.StartingOpNo)
                            {
                                // SubCon is first operation
                                planDispDate = DateTime.Now.AddDays(1);
                            }
                            else
                            {
                                // SubCon comes after in-house Opr
                                var currentIndex = oprs.FindIndex(o => o.Opr_No == opr.Opr_No);
                                var prevOpr = currentIndex > 0 ? oprs[currentIndex - 1] : null;
                                if (prevOpr != null)
                                {
                                    var endTimeslot = allTimeslots.FirstOrDefault(t => t.Timeslot_ListId == prevOpr.Shop_Plan_end_time);
                                    if (endTimeslot != null)
                                        planDispDate = endTimeslot.End_time.AddDays(1);
                                    else
                                        planDispDate = DateTime.Now.AddDays(1);
                                }
                                else
                                {
                                    planDispDate = DateTime.Now.AddDays(1);
                                }
                            }
                            var tempSubCon_List = new TempSubCon_ListVM
                            {
                                Wo_Id = wo.Wo_Id,
                                Opr_No = opr.Opr_No,
                                Supplier_Id = subtransport.SupplierId,
                                Rework_Wo = 'N',
                                Loaded = 'N',
                                Mode = 1,
                                Changed = 0,
                                Plan_Disp_date = planDispDate,
                                Plan_Qnty = wo.Plan_Simul_Qnty
                            };
                            var stepConvTime = subconws.FloorToFloorTime; // e.g., "01:00:00"
                            if (TimeSpan.TryParse(stepConvTime, out TimeSpan convTime))
                            {
                                tempSubCon_List.Plan_Recpt_date = planDispDate.Add(convTime);
                            }
                            else
                            {
                                tempSubCon_List.Plan_Recpt_date = planDispDate;
                            }
                            await _woService.PostTempSubCon_List(tempSubCon_List);
                            if (startId.HasValue && endId.HasValue)
                            {
                                opr.Subcon_plan_start_time = startId.Value;
                                opr.Subcon_plan_end_time = endId.Value;
                                opr.Rolledup_Opr_TPT = opr.Initial_Opr_TPT;

                                await _woService.PostTempOpr_List(opr);
                            }
                        }
                    }
                    else
                    {
                        int noOfMcs = noOfSimultMcs;
                        int qtyPerMc = wo.Plan_Simul_Qnty / noOfMcs;
                        var machines = await _routingService.StepMachines((int)opr.Opr_No);
                        List<TempMc_Wait_ListVM> mcWaits = new List<TempMc_Wait_ListVM>();
                        var insttempMCTime = new TempMc_Timeslot_ListVM();
                        foreach (var mc in machines.Take(noOfMcs))
                        {
                            await CheckPauseAsync();
                            var setupTime = TimeSpan.Parse(mc.SetupTime); // e.g., "00:15:00"
                            var floorToFloorTime = TimeSpan.Parse(mc.FloorToFloorTime);
                            var tpt = setupTime.TotalMinutes + (floorToFloorTime.TotalMinutes * qtyPerMc); // total time in minutes
                            var getmachine = await _machineService.GetMachine(mc.MachineId);
                            var plantwd = await _plantService.GetPlantWD(getmachine.MachinePlantId);
                            int slotsRequired = (int)Math.Ceiling(tpt / plantwd.Timeslot_duration);

                            var plantSlots = allTimeslots
                                .Where(t => t.PlantId == getmachine.MachinePlantId && t.Break_Slot != 'Y')
                                .OrderBy(t => t.Timeslot_ListId)
                                .ToList();
                            allMcTimeslots = await _woService.GetAllTempMc_Timeslot_List();
                            var existingMcSlots = allMcTimeslots
                                .Where(s => s.Mc_Id == mc.MachineId && s.Allocation == 2)
                                .OrderByDescending(s => s.EndTimeslot_List_Id)
                                .ToList();
                            long startFromTimeslotId = 0;
                            if (existingMcSlots.Any())
                            {
                                startFromTimeslotId = existingMcSlots.First().EndTimeslot_List_Id;
                            }
                            var availableTimeslots = plantSlots
                                .Where(t => t.Timeslot_ListId > startFromTimeslotId && t.Start_time > DateTime.Now)
                                .Take(slotsRequired)
                                .ToList();

                            //var mcTimeslots = allMcTimeslots
                            //    .Where(s => s.Mc_Id == mc.MachineId && s.Allocation == 1 && s.Slot_Not_Avl != 'Y')
                            //    .OrderBy(s => s.Timeslot_List_Id) // use Timeslot_List_Id order
                            //    .ToList();

                            //var startIdx = mcTimeslots.FindIndex(s =>
                            //{
                            //    var slotTime = allTimeslots.FirstOrDefault(t => t.Timeslot_ListId == s.Timeslot_List_Id)?.Start_time;
                            //    return slotTime.HasValue && slotTime.Value > DateTime.Now;
                            //});

                            //if (startIdx >= 0 && (startIdx + slotsRequired) <= mcTimeslots.Count)
                            //{
                            //    var availableSlots = mcTimeslots.Skip(startIdx).Take(slotsRequired).ToList();

                            var mcWait = new TempMc_Wait_ListVM
                            {
                                Wo_Id = wo.Wo_Id,
                                Opr_No_Id = opr.Opr_No,
                                Mc_Id = mc.MachineId,
                                Wait_Seq_No = await CalculateNextTempWaitSeqNo(mc.MachineId),
                                Plan_start_time_Id = availableTimeslots.First().Timeslot_ListId,
                                Plan_end_time_Id = availableTimeslots.Last().Timeslot_ListId,
                                Mc_TPT = Convert.ToDecimal(tpt),
                                Mode = 1,
                                Plan_Qnty = qtyPerMc
                            };

                            mcWait = await _woService.PostTempMc_Wait_List(mcWait);

                            //foreach (var slot in availableSlots)
                            //{
                            await CheckPauseAsync();
                            var newMcSlot = new TempMc_Timeslot_ListVM
                            {
                                Mc_Id = mc.MachineId,
                                Timeslot_List_Id = availableTimeslots.First().Timeslot_ListId,
                                EndTimeslot_List_Id = availableTimeslots.Last().Timeslot_ListId, // if applicable
                                Mc_Wait_List_Id = mcWait.TempMc_Wait_ListId, // Will set after mcWait is created
                                Allocation = 2,
                                Slot_Not_Avl = 'N',
                                Not_Avl_reason = 0// set appropriately
                            };
                            insttempMCTime = await _woService.PostTempMc_Timeslot_List(newMcSlot);
                            //}

                            mcWaits.Add(mcWait);
                            //}
                        }

                        // Update Opr_List with aggregated info
                        if (!mcWaits.Any())
                            continue;
                        opr.Shop_Plan_start_time = mcWaits.Min(mw => mw.Plan_start_time_Id);
                        opr.Shop_Plan_end_time = mcWaits.Max(mw => mw.Plan_end_time_Id);
                        opr.Rolledup_Opr_TPT = mcWaits.Sum(mw => Convert.ToInt64(mw.Mc_TPT));
                        await _woService.PostTempOpr_List(opr);

                        // Calculate Mc_Wait_List.Next_Opr_Start_time_ID
                        foreach (var mcWait in mcWaits)
                        {
                            var startSlot = insttempMCTime;
                            var startTime = allTimeslots.FirstOrDefault(t => t.Timeslot_ListId == startSlot.Timeslot_List_Id)?.Start_time ?? DateTime.Now;

                            // Find the corresponding machine again
                            var mc = machines.First(m => m.MachineId == mcWait.Mc_Id);

                            var setupTime = TimeSpan.Parse(mc.SetupTime);
                            var partTime = TimeSpan.FromMinutes(TimeSpan.Parse(mc.FloorToFloorTime).TotalMinutes * Math.Min(qtyPerMc, 1));
                            var requiredTime = setupTime + partTime;

                            var nextSlotTime = startTime.Add(requiredTime);
                            var nextSlot = allTimeslots.FirstOrDefault(t => t.Start_time >= nextSlotTime);

                            mcWait.Next_Opr_Start_time_Id = nextSlot?.Timeslot_ListId ?? mcWait.Plan_end_time_Id;
                            await _woService.PostTempMc_Wait_List(mcWait);
                        }

                    }
                }

                // Update final WO Start & End Date
                allMcTimeslots = await _woService.GetAllTempMc_Timeslot_List();
                var startMcSlotId = oprs.First().Shop_Plan_start_time;
                var endMcSlotId = oprs.Last().Shop_Plan_end_time;
                var startMcSlot = allMcTimeslots.FirstOrDefault(ts => ts.Timeslot_List_Id == startMcSlotId);
                var endMcSlot = allMcTimeslots.FirstOrDefault(ts => ts.EndTimeslot_List_Id == endMcSlotId);
                var startTs = allTimeslots.FirstOrDefault(ts => ts.Timeslot_ListId == startMcSlot?.Timeslot_List_Id);
                var endTs = allTimeslots.FirstOrDefault(ts => ts.Timeslot_ListId == endMcSlot?.EndTimeslot_List_Id);

                if (startTs?.Start_time != null && endTs?.End_time != null)
                {
                    wo.Plan_Start_Date = startTs.Start_time;
                    wo.Plan_End_Date = endTs.End_time;
                    await _woService.PostTempWo_Wait_List(wo);
                }
            }
            try
            {
                SimulationState.IsPaused = false;
                SimulationState.IsStopped = false;
                // your simulation logic here...

                return Json(new { message = "Re-Simulation completed." });
            }
            catch (OperationCanceledException ex)
            {
                return Json(new { message = "Re-Simulation stopped." });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllSimulationMove()
        {
            var result = await _woService.GetAllMatl_Issue_List();
            var tempoprs = await _woService.GetAllTempOpr_List();
            var depts = await _departmentService.GetDepartments(1);
            var prodns = await _woService.AllProductionPlan_Wo();
            var masterparts = await _masterService.ItemMasterParts();
            foreach (var item in result)
            {
                var tempopr = tempoprs.Where(o => o.TempOpr_ListId == item.Part_Ref).FirstOrDefault();
                var pp = prodns.Where(p => p.ProductionPlanId == tempopr.Wo_Id).FirstOrDefault();
                var todept = depts.FirstOrDefault(d => d.DepartmentId == item.To_Location)?.Name ?? "Stores";
                var fromdept = depts.FirstOrDefault(d => d.DepartmentId == item.From_Location)?.Name ?? "Stores";
                item.To_LocationStr = todept;
                item.From_LocationStr = fromdept;
                if (todept != "Stores")
                {
                    item.Shop = todept;
                }
                else if (fromdept != "Stores")
                {
                    item.Shop = fromdept;
                }
                item.WoNumber = pp.WONumber ?? "";
                var masterpart = masterparts.Where(m => m.PartId == pp.PartId).FirstOrDefault();
                item.PartNo = masterpart.PartNo ?? "" + " / " + masterpart.Description;
                var mf = await _masterService.GetManufPart((int)pp.PartId);
                var routingList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                if (mf.ManufacturedPartType == 1)
                {
                    var routing = routingList.Where(r => r.RoutingId == pp.RoutingId).FirstOrDefault();
                    var getmkfrom = await _masterService.ItemMasterPartById((int)routing.MKPartId);
                    item.InputPartNo = getmkfrom.PartNo + " / " + getmkfrom.PartDescription;
                }
                else
                {
                    var bom = await _masterService.BOMS(mf.ManufacturedPartNoDetailId.ToString());
                    var boms = bom.FirstOrDefault();
                    if(boms != null)
                    {
                        item.InputPartNo = bom.FirstOrDefault().BOMPartNo + " / " + bom.FirstOrDefault().BOMPartDesc;
                    }
                    else
                    {
                        item.InputPartNo = string.Empty;
                    }

                }
                var routstep = await _routingService.RoutingSteps((int)pp.RoutingId);
                item.RoutingName = routingList.Where(r => r.RoutingId == pp.RoutingId).FirstOrDefault().RoutingName;
                item.OpNo = routstep.FirstOrDefault(s=>s.StepId == tempopr.Opr_No).StepNumber ?? "";
                item.BalWoQnty = pp.CalcWOQty.ToString() ?? "0";
                item.QntyAvl = 0;
                item.BookOutQnty = 0;
                item.IssueMovDtStr = item.Issue_Mov_date.ToString("dd-MM-yyyy");
            }
            // Group by PartNo, IssueMovDtStr and Qnty 
            var groupedResult = result
                .GroupBy(r => new { r.PartNo, r.IssueMovDtStr, r.Issue_Qnty, r.OpNo })
                .Select(g => g.First()).ToList();

            return Ok(groupedResult);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMcWaitSetupList()
        {
            var result = new List<TempMc_Wait_ListVM>();

            var waitList = await _woService.GetAllMc_Wait_List();
            var timeslots = await _woService.GetAllTempMc_Timeslot_List();
            var allTimeslots = await _woService.GetAllTimeslot_List();
            var parts = await _masterService.ItemMasterParts();
            var machines = await _machineService.GetMachinesList();
            var shops = await _departmentService.GetDepartments(1);
            var allWO = await _woService.AllProductionPlan_Wo();
            var translogs = await _woService.GetAllInv_Trans_Log();
            var allTimeSlots = await _woService.GetAllTimeslot_List();

            foreach (var mcWait in waitList)
            {
                var wo = allWO.FirstOrDefault(p => p.ProductionPlanId == mcWait.Wo_Id);
                var part = parts.FirstOrDefault(p => p.PartId == wo.PartId);
                var machine = machines.FirstOrDefault(m => m.MachineId == mcWait.Mc_Id);
                var shop = shops.FirstOrDefault(s => s.DepartmentId == machine.ShopId);
                var mf = await _masterService.GetManufPart((int)wo.PartId);
                var routingList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                var plantwd = await _plantService.GetPlantWD(machine.PlantId);
                var routing = await _routingService.RoutingSteps((int)wo.RoutingId);
                var routingstep = routing.FirstOrDefault(r => r.StepId == mcWait.Opr_No_Id);
                var stepMachines = await _routingService.StepMachines((int)routingstep.StepId);
                var currentStart = allTimeSlots.FirstOrDefault(t => t.Timeslot_ListId == mcWait.Plan_start_time_Id)?.Start_time;
                var shiftName = GetShiftName(currentStart.Value, plantwd);
                result.Add(new TempMc_Wait_ListVM
                {
                    ShopName = shop?.Name ?? "",
                    McName = machine?.Name ?? "",
                    WoNumber = wo?.WONumber ?? "",
                    PartNo = part?.PartNo + " / " + part?.Description,
                    RoutingName = routingList.First(r => r.RoutingId == wo.RoutingId).RoutingName ?? "",
                    OprNoName = routingstep?.StepNumber ?? "",
                    WoQnty = wo?.CalcWOQty.ToString() ?? "0",
                    Plan_Qnty = mcWait.Plan_Qnty,
                    ActiveId = mcWait.Mc_Wait_ListId,
                    ShiftName = shiftName,
                    PlanStartStr = currentStart.Value.ToString("dd-MM-yyyy hh:mm tt")
                });
            }

            return Ok(result);
        }
        string GetShiftName(DateTime currentStart, PlantWorkingDetailsVM plantwd)
        {
            var currentTime = currentStart.TimeOfDay;

            // Helper function to check shift time range with midnight handling
            bool IsInShift(TimeSpan start, TimeSpan duration)
            {
                var end = start + duration;

                if (end >= TimeSpan.FromHours(24)) // wrap around midnight
                    end -= TimeSpan.FromHours(24);

                if (start < end)
                    return currentTime >= start && currentTime < end;
                else
                    return currentTime >= start || currentTime < end;
            }

            // First Shift
            if (DateTime.TryParseExact(plantwd.FirstShiftStartTime, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out var firstStartDT) &&
                TimeSpan.TryParse(plantwd.FirstShiftDuration, out var firstDur))
            {
                if (IsInShift(firstStartDT.TimeOfDay, firstDur))
                    return "1st Shift";
            }

            // Second Shift
            if (plantwd.NoOfShifts >= 2 &&
                DateTime.TryParseExact(plantwd.SecondShiftStartTime, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out var secondStartDT) &&
                TimeSpan.TryParse(plantwd.SecondShiftDuration, out var secondDur))
            {
                if (IsInShift(secondStartDT.TimeOfDay, secondDur))
                    return "2nd Shift";
            }

            // Third Shift
            if (plantwd.NoOfShifts == 3 &&
                DateTime.TryParseExact(plantwd.ThirdShiftStartTime, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out var thirdStartDT) &&
                TimeSpan.TryParse(plantwd.ThirdShiftDuration, out var thirdDur))
            {
                if (IsInShift(thirdStartDT.TimeOfDay, thirdDur))
                    return "3rd Shift";
            }

            return "Unknown Shift";
        }


    }
}
