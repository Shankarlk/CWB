﻿using CWB.App.Models.BusinessProcesses;
using CWB.App.Models.ItemMaster;
using CWB.App.Models.Plants;
using CWB.App.Models.WorkOrder;
using CWB.App.Services.BusinessProcesses;
using CWB.App.Services.CompanySettings;
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
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class WorkOrderController : Controller
    {
        private readonly ILogger<WorkOrderController> _logger;
        private readonly IWOService _woService;
        private readonly IBAService _baService;
        private readonly IRoutingService _routingService;
        private readonly IMastersServices _masterService;
        private readonly IPlantService _plantService;
        private readonly IMachineService _machineService;
        private readonly IDepartmentService _departmentService;
        private readonly IOperationService _operationService;
        public WorkOrderController(ILogger<WorkOrderController> logger, IWOService wOService, 
            IBAService baService, IRoutingService routingService, IMastersServices masterServices, IPlantService plantService
            , IMachineService machineService, IDepartmentService departmentService, IOperationService operationService)
        {
            _logger = logger;
            _woService = wOService;
            _baService = baService;
            _routingService = routingService;
            _masterService = masterServices;
            _plantService = plantService;
            _machineService = machineService;
            _departmentService = departmentService;
            _operationService = operationService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("~/C!O$N%1#23t! ")]
        public IActionResult SoToWo()
        {
            return View();
        }

        [Route("~/D#1@122P I%5$3T ")]
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
            return Ok(resultList);
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
                setSO.Add(await _baService.GetOneSO(item.SalesOrderId));

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
            var workOrders = await _baService.AllWorkOrders();
            List<ProductionPlan_WoVM> productions = new List<ProductionPlan_WoVM>();
            foreach (var item in workOrders)
            {
                if (item.Active != 2)
                {
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
                        PlanCompletionDate = item.PlanCompletionDate,
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
            if (procdutionpost.Any())
            {
                List<ProcPlanVM> listprocplan = new List<ProcPlanVM>();
                List<BOMListVM> listbom = new List<BOMListVM>();
                List<ProductionPlan_WoVM> childwos = new List<ProductionPlan_WoVM>();
                List<ChildWoRelVM> childWoRels = new List<ChildWoRelVM>();
                List<McTimeListVM> mcTimeListVMs = new List<McTimeListVM>();
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
                                    Qnty = Convert.ToInt32(mpmakefrom.QuantityPerInput),
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
                                           int quantity;
                                           return int.TryParse(x.QuantityPerInput, out quantity) ? quantity : 0;
                                       })
                                   })
                                    .ToList();
                            foreach (var grouped in groupedResults)
                            {
                                var mfpdList = await _masterService.PartPurchasesFor(grouped.PartId);
                                var ptype = await _masterService.GetRMPart(grouped.PartId);
                                totalLeadTime = mfpdList.Sum(x => x.LeadTimeInDays);
                                DateTime nextworkdingdate = (DateTime)item.PlanCompletionDate;
                                nextworkdingdate = nextworkdingdate.AddDays(totalLeadTime);
                                if (groupedResults != null)
                                {
                                    ProcPlanVM ppdata = new ProcPlanVM
                                    {
                                        PartId = grouped.PartId,
                                        PartType = ptype.MasterPartType,
                                        Calc_Proc_Qnty = grouped.TotalQuantity * item.CalcWOQty,
                                        CalcReceiptDate = nextworkdingdate,
                                        WorkOrderId = item.WoId
                                    };
                                    listprocplan.Add(ppdata);
                                    BOMListVM bomdata = new BOMListVM
                                    {
                                        ParentWoId = item.WoId,
                                        Child_Part_No_ID = grouped.PartId,
                                        Child_Part_No_Type = ptype.MasterPartType.ToString(),
                                        Calc_Qnty = (int)grouped.TotalQuantity * item.CalcWOQty,
                                        Plan_Qnty = item.CalcWOQty,
                                        //Plan_Start_Dt = planstartdt,
                                        Plan_Compl_Dt = item.PlanCompletionDate.GetValueOrDefault(),
                                        CalcReceiptDate = nextworkdingdate,
                                        //Manf_Days_Avl = manfDays,
                                        ProcPlanId = item.ProductionPlanId,
                                        //SaNestLevel = Sa_Nest_level
                                    };
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
                                var workdetails = await _plantService.GetPlantWD(13);
                                var holidaylist = await _plantService.GetHolidays(13);
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
                                                            TotalProcessingTime = g.Sum(sd => TimeSpan.Parse(sd.FirstPieceProcessingTime).TotalMinutes)
                                                        });
                                        foreach (var min in processingTimeSum)
                                        {
                                            minutes = (int)min.TotalProcessingTime;
                                        }
                                    }
                                }
                                int assyTime = (minutes * item.CalcWOQty) / workdetails.NoOfShifts;
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
                                                    Qnty = Convert.ToInt32(mpmakefrom.QuantityPerInput),
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
                                                        int quantity;
                                                        return int.TryParse(x.QuantityPerInput, out quantity) ? quantity : 0;
                                                    })
                                                })
                                                .ToList();
                                            foreach (var grouped in groupedResults)
                                            {
                                                var submfpdList = await _masterService.PartPurchasesFor(grouped.PartId);
                                                var subptype = await _masterService.GetRMPart(grouped.PartId);
                                                totalLeadTime = submfpdList.Sum(x => x.LeadTimeInDays);
                                                DateTime subnextworkdingdate = planstartdt;
                                                subnextworkdingdate = subnextworkdingdate.AddDays(totalLeadTime);
                                                if (groupedResults != null)
                                                {
                                                    ProcPlanVM subppdata = new ProcPlanVM
                                                    {
                                                        PartId = grouped.PartId,
                                                        PartType = subptype.MasterPartType,
                                                        Calc_Proc_Qnty = grouped.TotalQuantity * item.CalcWOQty,
                                                        CalcReceiptDate = subnextworkdingdate,
                                                        WorkOrderId = item.WoId
                                                    };
                                                    listprocplan.Add(subppdata);

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
                                            Parentlevel = item.Parentlevel,
                                            BuildToStock = item.BuildToStock,
                                            TestData = item.TestData,
                                            CalcWOQty = bomdata.Calc_Qnty,
                                            PlanCompletionDate = planstartdt,
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
                                        var bofptype = await _masterService.GetRMPart(bomgrp.PartId);
                                        totalLeadTime = bofpdList.Sum(x => x.LeadTimeInDays);
                                        DateTime bofnextworkdingdate = item.PlanCompletionDate.GetValueOrDefault();
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
                                            CalcReceiptDate = bofnextworkdingdate,
                                            WorkOrderId = item.WoId
                                        };
                                        listprocplan.Add(ppdata);
                                       
                                        break;
                                    case MasterPartType.RawMaterial:
                                        var mfpdList = await _masterService.PartPurchasesFor(bomgrp.PartId);
                                        var ptype = await _masterService.GetRMPart(bomgrp.PartId);
                                        totalLeadTime = mfpdList.Sum(x => x.LeadTimeInDays);
                                        DateTime nextworkdingdate = (DateTime)item.PlanCompletionDate;
                                        nextworkdingdate = nextworkdingdate.AddDays(totalLeadTime);

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
                                        listbom.Add(rmbomdata);
                                        
                                        break;
                                    default:
                                        break;
                                }
                            }

                        }

                    }

                    //McTimeList---
                    var routingstep = await _routingService.RoutingSteps((int)item.RoutingId);
                    var oneroutingstep = routingstep.FirstOrDefault();
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
                        int Totalplantime = Convert.ToInt32(TimeSpan.Parse(onestepmachine.SetupTime).TotalMinutes) + ((Convert.ToInt32(TimeSpan.Parse(onestepmachine.FirstPieceProcessingTime).TotalMinutes) + department.NoOfShifts) * (item.CalcWOQty - 1)) / onestepmachine.NoOfPartsPerLoading;
                        int TotalplantimeInHoursRounded = (int)Math.Round(Totalplantime / 60.0);
                        McTimeListVM mcTimeList = new McTimeListVM()
                        {
                            WoId = item.WoId,
                            Routing_StepId = oneroutingstep.StepId,
                            CompanyId = Convert.ToInt64(oneroutingstep.StepLocation),
                            MachineId = onestepmachine.MachineId,
                            MachineTypeId = machine.MachineMachineTypeId,
                            PlanQnty = item.CalcWOQty,
                            TotalPlanTime = TotalplantimeInHoursRounded,
                            McPlanStartTime = item.PlanStartDate,
                            McPlanEndTime = (DateTime)item.PlanCompletionDate,
                        };
                        mcTimeListVMs.Add(mcTimeList);
                    }
                }
                if (listprocplan.Any())
                {
                    var result = await _woService.ProcPlanPost(listprocplan);

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
                                        Qnty = Convert.ToInt32(mpmakefrom.QuantityPerInput),
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
                                               int quantity;
                                               return int.TryParse(x.QuantityPerInput, out quantity) ? quantity : 0;
                                           })
                                       })
                                        .ToList();
                                foreach (var grouped in groupedResults)
                                {
                                    var mfpdList = await _masterService.PartPurchasesFor(grouped.PartId);
                                    var ptype = await _masterService.GetRMPart(grouped.PartId);
                                    subtotalLeadTime = mfpdList.Sum(x => x.LeadTimeInDays);
                                    DateTime nextworkdingdate = (DateTime)item.PlanCompletionDate;
                                    nextworkdingdate = nextworkdingdate.AddDays(subtotalLeadTime);
                                    if (groupedResults != null)
                                    {
                                        ProcPlanVM ppdata = new ProcPlanVM
                                        {
                                            PartId = grouped.PartId,
                                            PartType = ptype.MasterPartType,
                                            Calc_Proc_Qnty = grouped.TotalQuantity * item.CalcWOQty,
                                            CalcReceiptDate = nextworkdingdate,
                                            WorkOrderId = item.WoId
                                        };
                                        sublistprocplan.Add(ppdata);

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
                                    var workdetails = await _plantService.GetPlantWD(13);
                                    var holidaylist = await _plantService.GetHolidays(13);
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
                                                                TotalProcessingTime = g.Sum(sd => TimeSpan.Parse(sd.FirstPieceProcessingTime).TotalMinutes)
                                                            });
                                            foreach (var min in processingTimeSum)
                                            {
                                                minutes = (int)min.TotalProcessingTime;
                                            }
                                        }
                                    }
                                    int assyTime = (minutes * item.CalcWOQty) / workdetails.NoOfShifts;
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
                                                        Qnty = Convert.ToInt32(mpmakefrom.QuantityPerInput),
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
                                                   int quantity;
                                                   return int.TryParse(x.QuantityPerInput, out quantity) ? quantity : 0;
                                               })
                                           })
                                            .ToList();
                                                foreach (var grouped in groupedResults)
                                                {
                                                    var submfpdList = await _masterService.PartPurchasesFor(grouped.PartId);
                                                    var subptype = await _masterService.GetRMPart(grouped.PartId);
                                                    subtotalLeadTime = submfpdList.Sum(x => x.LeadTimeInDays);
                                                    DateTime subnextworkdingdate = (DateTime)item.PlanCompletionDate;
                                                    subnextworkdingdate = subnextworkdingdate.AddDays(subtotalLeadTime);
                                                    if (groupedResults != null)
                                                    {
                                                        ProcPlanVM subppdata = new ProcPlanVM
                                                        {
                                                            PartId = grouped.PartId,
                                                            PartType = subptype.MasterPartType,
                                                            Calc_Proc_Qnty = grouped.TotalQuantity * item.CalcWOQty,
                                                            CalcReceiptDate = subnextworkdingdate,
                                                            WorkOrderId = item.WoId
                                                        };
                                                        sublistprocplan.Add(subppdata);

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
                                                Parentlevel = item.Parentlevel,
                                                BuildToStock = item.BuildToStock,
                                                TestData = item.TestData,
                                                CalcWOQty = bomdata.Calc_Qnty,
                                                PlanCompletionDate = planstartdt,
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
                                            var bofptype = await _masterService.GetRMPart(bomgrp.PartId);
                                            subtotalLeadTime = bofpdList.Sum(x => x.LeadTimeInDays);
                                            DateTime bofnextworkdingdate = (DateTime)item.PlanCompletionDate;
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
                        var routingstep = await _routingService.RoutingSteps((int)item.RoutingId);
                        var oneroutingstep = routingstep.FirstOrDefault();
                        var stepmachine = await _routingService.StepMachines((int)oneroutingstep.StepId);
                        var onestepmachine = stepmachine.FirstOrDefault();
                        if (onestepmachine != null)
                        {
                            var machine = await _machineService.GetMachine((int)onestepmachine?.MachineId);
                           
                            var departments = await _departmentService.GetDepartments(1);
                            var department = departments.FirstOrDefault(d => d.DepartmentId == machine.MachineDepartmentId);
                            int Totalplantime = Convert.ToInt32(TimeSpan.Parse(onestepmachine.SetupTime).TotalMinutes) + ((Convert.ToInt32(TimeSpan.Parse(onestepmachine.FirstPieceProcessingTime).TotalMinutes) + department.NoOfShifts) * (item.CalcWOQty - 1)) / onestepmachine.NoOfPartsPerLoading;
                            int TotalplantimeInHoursRounded = (int)Math.Round(Totalplantime / 60.0);
                            McTimeListVM mcTimeList = new McTimeListVM()
                            {
                                WoId = item.WoId,
                                Routing_StepId = oneroutingstep.StepId,
                                CompanyId = Convert.ToInt64(oneroutingstep.StepLocation),
                                MachineId = onestepmachine.MachineId,
                                MachineTypeId = machine.MachineMachineTypeId,
                                PlanQnty = item.CalcWOQty,
                                TotalPlanTime = TotalplantimeInHoursRounded,
                                McPlanStartTime = item.PlanStartDate,
                                McPlanEndTime = (DateTime)item.PlanCompletionDate,
                            };
                            submcTimeListVMs.Add(mcTimeList);
                        }
                    }
                    if (sublistprocplan.Any())
                    {
                        var result = await _woService.ProcPlanPost(sublistprocplan);

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
                }

            }

            //return RedirectToAction("DetailedProcPlan");
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> AllProductionWo()
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
            return Ok(productions);
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
            foreach (ProcPlanVM item in resultList)
            {
                var mp = await _masterService.ItemMasterPartById((int)item.PartId);
                var mfpdList = await _masterService.PartPurchasesFor((int)item.PartId);
                foreach (var purs in mfpdList)
                {
                    if (item.PartId == purs.PPartId)
                    {
                        item.Supplier = purs.PSupplier;
                        item.LeadTimeInDays = purs.LeadTimeInDays.ToString();
                        item.Moq = purs.MinimumOrderQuantity;
                    }
                }
                item.PartNo = mp.PartNo;
                item.PartDesc = mp.PartDescription;
            }
            return Ok(resultList);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBomlist()
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
                var machine = await _machineService.GetMachine((int)mctime.MachineId);
                var machineTypes = await _machineService.GetMachineTypes();
                foreach (ProductionPlan_WoVM item in productions)
                {
                    if (item.WoId == mctime.WoId && item.ParentWoId ==0)
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
            return Ok(mctimelist);
        }

        //[HttpGet]
        //public async Task<IActionResult> McTimeListSummary()
        //{

        //    return Ok();
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
            var workdetails = await _plantService.GetPlantWD(13);
            var holidaylist = await _plantService.GetHolidays(13);
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

            var workdetails = await _plantService.GetPlantWD(13);
            var holidaylist = await _plantService.GetHolidays(13);
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
    }
}
