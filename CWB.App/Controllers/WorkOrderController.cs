using CWB.App.AppUtils;
using CWB.App.Models.BusinessProcesses;
using CWB.App.Models.Contacts;
using CWB.App.Models.DocumentManagement;
using CWB.App.Models.ItemMaster;
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
        public IActionResult SalesOrderList()
        {
            return View();
        }

        [Route("~/W@A!E0#% U%1X#Q ")]
        public IActionResult WorkOrderList()
        {
            return View();
        } 
        //[Route("~/W@A!E0#% U%1X#Q ")]
        public IActionResult POLineList()
        {
            return View();
        }
        public IActionResult ApprovPoDetails()
        {
            return View();
        }
        [Route("~/W@IA!E0P#% U%1X#QC")]
        public IActionResult WipControl()
        {
            return View();
        }
        public IActionResult InwardPo()
        {
            return View();
        }
        public IActionResult Inspection()
        {
            return View();
        }
        public IActionResult OperationsSettings()
        {
            return View();
        }
        public IActionResult NcAwaitingDecision()
        {
            return View();
        }
        public IActionResult MaterialMovement()
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
        public async Task<IActionResult> RoutingSteps(int routingId)
        {
            var result = await _routingService.RoutingSteps(routingId);
            return Ok(result);
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
                            var mp = await _masterService.ItemMasterPartById(mf.PartId);
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
        public async Task<IActionResult> AllProductionWo()
        {
            var productions = await _woService.AllProductionPlan_Wo();
            var masterparts = await _masterService.ItemMasterParts();
            var procplan = await _woService.GetAllProcPlan();
            foreach (ProductionPlan_WoVM item in productions)
            {
                if(item.PartType == 2)
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

            await Task.WhenAll(resultListTask, workOrdersTask);

            var resultList = resultListTask.Result;
            var workOrders = workOrdersTask.Result;

            // Prepare a dictionary for fast lookup of Work Orders by ID
            var workOrdersDict = workOrders.ToDictionary(wo => wo.WOID, wo => wo.WONumber);

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
        public async Task<IActionResult> GetAllPodetails()
        {
            var procdutionpost = await _woService.GetAllPodetails();
            var resultList = await _woService.GetAllProcPlan();
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
            }
            return Ok(procdutionpost);
        }

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
            var procdutionpost = await _woService.GetAllPodetails();
            var resultList = await _woService.GetAllProcPlan();
            var result = await _woService.GetAllInw_Recpt_Header();
            List<PODetailsVM> woSubs = new List<PODetailsVM>();
            var companies = await _masterService.GetCompanies();
            var masterparts = await _masterService.ItemMasterParts();
            var uoms = await _masterService.GetUOMs();
            var wos = await _woService.AllProductionPlan_Wo();
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
                if (item.Status == 1)
                {
                    item.StatusStr = "Not Approved";
                }
                else if (item.Status == 2)
                {
                    item.StatusStr = "PO Approved";
                }
                else
                {
                    item.StatusStr = "Complete";
                }
                item.PoType = "Prodn";
                foreach (ItemMasterPartVM imp in masterparts)
                {
                    if (item.PartId == imp.PartId)
                    {
                        item.PartNo = imp.PartNo + " / " + imp.Description;
                        if(imp.MasterPartType == "ManufacturedPart")
                        {
                            item.PartType = "SubCon";
                        }
                        else
                        {
                            item.PartType = imp.MasterPartType;
                        }
                        var mp = await _masterService.PartPurchasesFor((int)item.PartId);
                        //item.ProcPrice = mp.FirstOrDefault().Price;
                    }
                }
                if (uomDict.TryGetValue(1, out var uomName))
                {
                    item.Unit = uomName;
                }
                foreach (var proc in resultList)
                {
                    if (item.ProcPlanId == proc.ProcPlanId)
                    {
                        if (item.PoQnty < proc.Plan_Proc_Qnty)
                        {
                            item.QntyRed = "Y";
                        }
                        if (item.PlanPoReceiptDate < proc.PlanReceiptDate)
                        {
                            item.DateRed = "Y";
                        }
                        item.WoId = wos.SingleOrDefault(s => s.ProductionPlanId == proc.WorkOrderId)?.WoId;
                    }
                }
                foreach (var inw in result)
                {
                    if(inw.PoHeaderId == item.PoDetailsId)
                    {
                        item.InwHeaderId = inw.Inw_Recpt_HeaderId;
                        item.InwDate = inw.Inw_Date_time.ToString("dd/MM/yyyy");
                        woSubs.Add(item);
                    }
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
                        listnc.Add(item);
                    }
                }
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
        public async Task<IActionResult> PostNcLog([FromBody] Insp_Outcome_DetailsVM procPlanVMs)
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
    }
}
