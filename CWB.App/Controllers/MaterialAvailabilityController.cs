using CWB.App.Models.ItemMaster;
using CWB.App.Models.WorkOrder;
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
    public class MaterialAvailabilityController : Controller
    {
        private readonly ILogger<MaterialAvailabilityController> _logger;
        private readonly IWOService _woService;
        private readonly IRoutingService _routingService;
        private readonly IDepartmentService _departmentService;
        private readonly IMastersServices _mastersService;
        public MaterialAvailabilityController(ILogger<MaterialAvailabilityController> logger, IMastersServices mastersService, IWOService woService, IRoutingService routingService, IDepartmentService departmentService)
        {
            _logger = logger;
            _mastersService = mastersService;
            _routingService = routingService;
            _woService = woService;
            _departmentService = departmentService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StockAvl()
        {
            return View();
        }
        public IActionResult MatlAvl()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> MasterParts()
        {
            var mfpdList = await _mastersService.ItemMasterParts();
            var result = new List<ItemMasterPartVM>();
            foreach (var item in mfpdList)
            {

                if (item.MasterPartType == "ManufacturedPart")
                {
                    item.MasterDisplay = "Child Manufactured Part";
                }
                if (item.MasterPartType == "Assembly")
                {
                    item.MasterDisplay = "Assembly";
                }
                if (item.MasterPartType == "BOF")
                {
                    if (item.BoughtOutFinishMadeType == 1)
                    {
                        item.MasterDisplay = "Standard BOF";
                    }
                    else if (item.BoughtOutFinishMadeType == 2)
                    {
                        item.MasterDisplay = "Catalog BOF";
                    }
                    else
                    {
                        item.MasterDisplay = "Purchased Made to Print BOF";
                    }
                }
                if (item.MasterPartType == "RawMaterial")
                {
                    var rm = await _mastersService.GetRMPart((int)item.PartId);
                    if (rm.RawMaterialMadeType == 1)
                    {
                        item.MasterDisplay = "Own Purchased RM";
                    }
                    else
                    {
                        item.MasterDisplay = "Customer Supplied RM";
                    }
                }
                result.Add(item);
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> StockAvlOfParts(int partid, string parttype)
        {
            var mfpdList = await _mastersService.ItemMasterParts();
            var result = new List<Inv_Trans_LogVM>();
            var transactionss = await _woService.GetAllInv_Trans_Log();
            var nclogs = await _woService.GetAllNcLog();
            var companies = await _mastersService.GetCompanies();
            var ncdecisions = await _woService.GetAllNC_Decision_Log();
            var rwk_Lists = await _woService.GetAllRwk_List();
            var transactions = transactionss.Where(t => t.Input_Routing_Id == partid || t.Output_Part_No == partid).ToList();
            foreach (var item in transactions)
            {
                item.OkQnty = item.Qnty.ToString("0");
                var mfpdLis = (item.Input_Part_NoId != 0)
                ? mfpdList.Where(i => i.PartId == item.Input_Part_NoId).LastOrDefault()
                : mfpdList.Where(i => i.PartId == item.Output_Part_No).LastOrDefault();
                if (mfpdList != null)
                {
                    item.PartNo = mfpdLis.PartNo;
                    item.Description = mfpdLis.Description;
                    if (parttype == "Child Manufactured Part")
                    {

                        ManufacturedPartNoDetailVM mf = await _mastersService.GetManufPart((int)mfpdLis.PartId);
                        var resultList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                        var routingId = (item.Output_Routing_Id != 0)
    ? item.Output_Routing_Id
    : item.Input_Routing_Id;

                        // RoutingName
                        item.RoutingName = resultList
                            .FirstOrDefault(r => r.RoutingId == routingId)
                            ?.RoutingName; // safe null check

                        // OprNo
                        var step = await _routingService.RoutingSteps((int)routingId);
                        item.OprNo = step.FirstOrDefault()?.StepNumber;
                    }
                }
                var findpartId = (item.Input_Part_NoId != 0)
                ? item.Input_Part_NoId
                : item.Output_Part_No;
                var nclog = nclogs.LastOrDefault(i => i.Inw_Recpt_Part_No_Id == findpartId);
                if (nclog != null)
                {
                    item.NcAvl = "Y";
                    if (parttype == "Child Manufactured Part" || parttype == "Assembly")
                    {
                        var rwk_List = rwk_Lists.Where(r => r.NC_Log_Id == nclog.Insp_Outcome_Details_Id).FirstOrDefault();
                        if(rwk_List != null)
                        {
                            item.RwkQnty = nclog.NC_Qnty.ToString("0");
                        }
                    }
                }
                else
                {
                    item.NcAvl = "N";
                }
                var supplier = companies
                .FirstOrDefault(c => c.CompanyId ==
                    ((item.To_Location_Id != 0)
                        ? item.To_Location_Id
                        : item.From_Location_Id));
                if (supplier != null)
                {
                    item.CompanyName = supplier.CompanyName;
                }
                if (parttype != "Child Manufactured Part" || parttype != "Assembly" || parttype != "Customer Supplied RM")
                {
                    item.WfInwdInsp = "N";
                }
                else
                {
                    item.WfInwdInsp = "Y";
                }
                result.Add(item);
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNcLog()
        {
            var result = await _woService.GetAllNcLog();
            List<Insp_Outcome_DetailsVM> listnc = new List<Insp_Outcome_DetailsVM>();
            var masterparts = await _mastersService.ItemMasterParts();
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
                foreach (var inward in inw)
                {
                    if (inward.Inw_Recpt_DetailsId == item.Inw_Recpt_Header_Id && inward.Inward_Condition == 2)
                    {
                    }
                }
                listnc.Add(item);
            }
            return Ok(listnc);
        }

        [HttpGet]
        public async Task<IActionResult> MatlAvlOfParts(string location)
        {
            var mfpdList = await _mastersService.ItemMasterParts();
            var result = new List<Inv_Trans_LogVM>();
            var transactionss = await _woService.GetAllInv_Trans_Log();
            var nclogs = await _woService.GetAllNcLog();
            var companies = await _mastersService.GetCompanies();
            var ncdecisions = await _woService.GetAllNC_Decision_Log();
            var rwk_Lists = await _woService.GetAllRwk_List();
            //var transactions = transactionss.Where(t => t.Input_Routing_Id == partid || t.Output_Part_No == partid).ToList();
            foreach (var item in transactionss)
            {
                item.OkQnty = item.Qnty.ToString("0");
                var mfpdLis = (item.Input_Part_NoId != 0)
                ? mfpdList.Where(i => i.PartId == item.Input_Part_NoId).LastOrDefault()
                : mfpdList.Where(i => i.PartId == item.Output_Part_No).LastOrDefault();
                if (mfpdList != null)
                {
                    item.PartNo = mfpdLis.PartNo;
                    item.Description = mfpdLis.Description;
                    item.MasterPartType = mfpdLis.MasterPartType;

                        ManufacturedPartNoDetailVM mf = await _mastersService.GetManufPart((int)mfpdLis.PartId);
                        var resultList = await _routingService.Routings(mf.ManufacturedPartNoDetailId);
                        var routingId = (item.Output_Routing_Id != 0)
    ? item.Output_Routing_Id
    : item.Input_Routing_Id;

                        // RoutingName
                        item.RoutingName = resultList
                            .FirstOrDefault(r => r.RoutingId == routingId)
                            ?.RoutingName; // safe null check

                        // OprNo
                        var step = await _routingService.RoutingSteps((int)routingId);
                        item.OprNo = step.FirstOrDefault()?.StepNumber;
                    var st = step.FirstOrDefault();
                    //if (st != null)
                    //{
                    //    if(st.StepLocation == "2")
                    //    {

                    //        var subconList = await _routingService.SubCons((int)st.StepId);
                    //        var subtransport = subconList.FirstOrDefault(s => s.PreferredSubcon == 1) ?? subconList.FirstOrDefault();
                    //        if (subtransport != null)
                    //        {
                    //            item.From_Location_Id == SupplierId
                    //        }
                    //}
                   
                }
                var findpartId = (item.Input_Part_NoId != 0)
                ? item.Input_Part_NoId
                : item.Output_Part_No;
                var nclog = nclogs.LastOrDefault(i => i.Inw_Recpt_Part_No_Id == findpartId);
                if (nclog != null)
                {
                    item.NcAvl = "Y";
                    var rwk_List = rwk_Lists.Where(r => r.NC_Log_Id == nclog.Insp_Outcome_Details_Id).FirstOrDefault();
                        if (rwk_List != null)
                        {
                            item.RwkQnty = nclog.NC_Qnty.ToString("0");
                        }
                }
                else
                {
                    item.NcAvl = "N";
                }
                item.WfInwdInsp = "N";
                var supplier = companies
                .FirstOrDefault(c => c.CompanyId ==
                    ((item.To_Location_Id != 0)
                        ? item.To_Location_Id
                        : item.From_Location_Id));
                if (supplier != null)
                {
                    item.CompanyName = supplier.CompanyName;
                }
                result.Add(item);
            }
            return Ok(result);
        }
    }
}
