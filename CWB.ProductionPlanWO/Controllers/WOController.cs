using CWB.CommonUtils.Common;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using CWB.ProductionPlanWO.Services;
using CWB.ProductionPlanWO.Utils;
using CWB.ProductionPlanWO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Controllers
{
    [ApiController]
    [Authorize]
    public class WOController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IWOService _woSerivce;
        public WOController(ILoggerManager logger, IWOService woSerivce)
        {
            _logger = logger;
            _woSerivce = woSerivce;
        }
        [HttpGet]
        [Route(ApiRoutes.WO.HelloWorld)]
        [Produces(AppContentTypes.ContentType, Type = typeof(string))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<string> HelloWorld()
        {
            var pologs = _woSerivce.HelloWorld();
            return pologs;
        }


        [HttpGet]
        [Route(ApiRoutes.WO.GetWoPOLogs)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<POLogVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetWoPOLogs(long tenantId, long customerOrderId)
        {
            var pologs = await _woSerivce.GetWoPOLogs(tenantId, customerOrderId);
            return Ok(pologs);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetPOLogs)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<POLogVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetPOLogs(long tenantId, long poid)
        {
            var pologs = await _woSerivce.GetPOLogs(tenantId, poid);
            return Ok(pologs);
        }

        [HttpPost]
        [Route(ApiRoutes.WO.PostWorkOrder)]
        [Produces(AppContentTypes.ContentType,Type=typeof(WorkOrdersVM))]
        public async Task<IActionResult> PostWorkOrder([FromBody]WorkOrdersVM workOrdersVM)
        {
            var result = await _woSerivce.WorkOrder(workOrdersVM);
            return Ok(result);
        }

        [HttpPost]
        [Route(ApiRoutes.WO.PostMultipleWorkOrder)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WorkOrdersVM))]
        public async Task<IActionResult> PostMultipleWorkOrder([FromBody] List<WorkOrdersVM> workOrdersVM)
        {
            var result = await _woSerivce.MultipleWorkOrder(workOrdersVM);
            return Ok(result);
        }

        [HttpPost]
        [Route(ApiRoutes.WO.PostUpdateMultipleWorkOrder)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WorkOrdersVM))]
        public async Task<IActionResult> PostUpdateMultipleWorkOrder([FromBody] List<WorkOrdersVM> workOrdersVM)
        {
            var result = await _woSerivce.UpdateMultipleWorkOrder(workOrdersVM);
            return Ok(result);
        }

        [HttpPost]
        [Route(ApiRoutes.WO.PostWOSORel)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WOSOVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostWOSORel([FromBody] List<WOSOVM> wOSOVMs)
        {
            var woso = await _woSerivce.PostWOSO(wOSOVMs);
            return Ok(woso);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostProcPurchase)]
        [Produces(AppContentTypes.ContentType, Type = typeof(ProcPlanPartPurChaseRelVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostProcPurchase([FromBody] List<ProcPlanPartPurChaseRelVM> wOSOVMs)
        {
            var woso = await _woSerivce.PostProcPurchase(wOSOVMs);
            return Ok(woso);
        }

        [HttpPost]
        [Route(ApiRoutes.WO.PostBOMTemp)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WOSOVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> BOMPost([FromBody] List<BOMTempVM> bomtemp)
        {
            var woso = await _woSerivce.BOMTempPost(bomtemp);
            return Ok(woso);
        }


        [HttpGet]
        [Route(ApiRoutes.WO.AllWorkOrders)]
        [Produces(AppContentTypes.ContentType,Type=typeof(List<WorkOrdersVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> AllWorkOrders(long tenantId)
        {
            var allwo = await _woSerivce.AllWorkOrders(tenantId);
            return Ok(allwo);
        }

        [HttpGet]
        [Route(ApiRoutes.WO.AllParentChildWos)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<WorkOrdersVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> AllParentWo(long parentWoId, long tenantId)
        {
            var allwo = await _woSerivce.AllParentChildWo(parentWoId, tenantId);
            return Ok(allwo);
        }

        [HttpGet]
        [Route(ApiRoutes.WO.GetSingleWorkOrder)]
        [Produces(AppContentTypes.ContentType,Type=typeof(WorkOrdersVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetSingleWO(long Id,long tenantId)
        {
            WorkOrdersVM singleWO = await _woSerivce.GetSingleWO(Id, tenantId);
            return Ok(singleWO);
        }

        [HttpGet]
        [Route(ApiRoutes.WO.GetSoWo)]
        [Produces(AppContentTypes.ContentType,Type =typeof(List<WOSOVM>))]
        [Authorize(Roles=Roles.ADMIN)]
        public async Task<IActionResult> GetSoWo(long workOrderId)
        {
            var so = await _woSerivce.GetSoWo(workOrderId);
            return Ok(so);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetProCPurchase)]
        [Produces(AppContentTypes.ContentType,Type =typeof(List<ProcPlanPartPurChaseRelVM>))]
        [Authorize(Roles=Roles.ADMIN)]
        public async Task<IActionResult> GetProCPurchase(long procPlanId)
        {
            var so = await _woSerivce.GetProcPurchase(procPlanId);
            return Ok(so);
        }


        [HttpPost]
        [Route(ApiRoutes.WO.PostProcPlan)]
        [Produces(AppContentTypes.ContentType, Type = typeof(ProcPlanVM))] //changed
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostProcPlan([FromBody] List<ProcPlanVM> procplan)
        {
            var proc = await _woSerivce.PostProcPlan(procplan);
            return Ok(proc);
        }

        [HttpPost]
        [Route(ApiRoutes.WO.PostBomList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(BOMListVM))]   //changed
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostBomList([FromBody] List<BOMListVM> bOMListVMs)
        {
            var proc = await _woSerivce.PostBomList(bOMListVMs);
            return Ok(proc);
        }

        [HttpPost]
        [Route(ApiRoutes.WO.PostProductionPlan_Wo)]
        [Produces(AppContentTypes.ContentType, Type =typeof(ProductionPlan_WOVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostProductionPlan_Wo([FromBody] List<ProductionPlan_WOVM> productions)
        {
            var productionPlan = await _woSerivce.PostProductionPlan_Wo(productions);
            return Ok(productionPlan);
        }

        [HttpGet]
        [Route(ApiRoutes.WO.AllProductionPlanWo)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<ProductionPlan_WOVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> AllProductionWos(long tenantId)
        {
            var allwo = await _woSerivce.AllProductionWo(tenantId);
            return Ok(allwo);
        }

        [HttpGet]
        [Route(ApiRoutes.WO.AllProcPlan)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<ProcPlanVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllProcPlan(long tenantId)
        {
            var allprocplan = await _woSerivce.AllProcPlan(tenantId);
            return Ok(allprocplan);
        }

        [HttpGet]
        [Route(ApiRoutes.WO.AllBomList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<BOMListVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllBomList(long tenantId)
        {
            var allprocplan = await _woSerivce.AllBomList(tenantId);
            return Ok(allprocplan);
        }

        [HttpGet]
        [Route(ApiRoutes.WO.GetWoStatus)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WOStatusVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetWoStatus(long Id)
        {
            var allprocplan = await _woSerivce.GetWOStatus(Id);
            return Ok(allprocplan);
        }

        [HttpPost]
        [Route(ApiRoutes.WO.PostChildWoRel)]
        [Produces(AppContentTypes.ContentType, Type = typeof(ChildWoRelVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostChildWoRel([FromBody] List<ChildWoRelVM> childWoRels)
        {
            var woso = await _woSerivce.PostChildWoRel(childWoRels);
            return Ok(woso);
        }

        [HttpPost]
        [Route(ApiRoutes.WO.PostMcTimeList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<McTimeListVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostMcTimeList([FromBody] List<McTimeListVM> mcTimeListVMs)
        {
            var mctime = await _woSerivce.PostMcTimeList(mcTimeListVMs);
            return Ok(mctime);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllMctimeList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<McTimeListVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllMcTimeList(long tenantId)
        {
            var mctime = await _woSerivce.GetAllMcTimeListVMs(tenantId);
            return Ok(mctime);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllPodetails)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<PODetailsVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllPodetails(long tenantId)
        {
            var mctime = await _woSerivce.GetAllPodetails(tenantId);
            return Ok(mctime);
        }


        [HttpPost]
        [Route(ApiRoutes.WO.PostMultiplePODetails)]
        [Produces(AppContentTypes.ContentType, Type = typeof(PODetailsVM))]
        public async Task<IActionResult> PostMultiplePODetails([FromBody] List<PODetailsVM> pODetailsVMs)
        {
            var result = await _woSerivce.MultiplePODetails(pODetailsVMs);
            return Ok(result);
        }

        [HttpPost]
        [Route(ApiRoutes.WO.PostMultiplePOHeaders)]
        [Produces(AppContentTypes.ContentType, Type = typeof(POHeaderVM))]
        public async Task<IActionResult> PostMultiplePOHeaders([FromBody] List<POHeaderVM> poHeaders)
        {
            var result = await _woSerivce.MultiplePOHeaders(poHeaders);
            return Ok(result);
        }


        [HttpGet]
        [Route(ApiRoutes.WO.GetPoStatus)]
        [Produces(AppContentTypes.ContentType, Type = typeof(POStatusVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetPoStatus(long Id)
        {
            var pOStatus = await _woSerivce.GetPOStatus(Id);
            return Ok(pOStatus);
        }


        [HttpGet]
        [Route(ApiRoutes.WO.AllSubCon)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<WoSubConSupplierVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllWoSubCOn(long tenantId)
        {
            var allwo = await _woSerivce.GetAllWoSubCon(tenantId);
            return Ok(allwo);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostWoSubCon)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostWoSubCon([FromBody] WoSubConSupplierVM workOrdersVM)
        {
            var result = await _woSerivce.PostWoSubCon(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteSubCon)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteSubCon(long Id)
        {
            var result = await _woSerivce.DeleteSubCon(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteWo)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteWo(long Id)
        {
            var result = await _woSerivce.DeleteWo(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostInsp_Outcome_Details)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Insp_Outcome_DetailsVM))]
        public async Task<IActionResult> PostInsp_Outcome_Details([FromBody] Insp_Outcome_DetailsVM workOrdersVM)
        {
            var result = await _woSerivce.PostInsp_Outcome_Details(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostInsp_Outcome_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Insp_Outcome_ListVM))]
        public async Task<IActionResult> PostInsp_Outcome_List([FromBody] Insp_Outcome_ListVM workOrdersVM)
        {
            var result = await _woSerivce.PostInsp_Outcome_List(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostInventory_Master)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Inventory_MasterVM))]
        public async Task<IActionResult> PostInventory_Master([FromBody] Inventory_MasterVM workOrdersVM)
        {
            var result = await _woSerivce.PostInventory_Master(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostInv_Trans_Log)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Inv_Trans_LogVM))]
        public async Task<IActionResult> PostInv_Trans_Log([FromBody] Inv_Trans_LogVM workOrdersVM)
        {
            var result = await _woSerivce.PostInv_Trans_Log(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostInw_Recpt_Details)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Inw_Recpt_DetailsVM))]
        public async Task<IActionResult> PostInw_Recpt_Details([FromBody] Inw_Recpt_DetailsVM workOrdersVM)
        {
            var result = await _woSerivce.PostInw_Recpt_Details(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostInw_Recpt_Header)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Inw_Recpt_HeaderVM))]
        public async Task<IActionResult> PostInw_Recpt_Header([FromBody] Inw_Recpt_HeaderVM workOrdersVM)
        {
            var result = await _woSerivce.PostInw_Recpt_Header(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostInw_Recpt_Part_No)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Inw_Recpt_Part_NoVM))]
        public async Task<IActionResult> PostInw_Recpt_Part_No([FromBody] Inw_Recpt_Part_NoVM workOrdersVM)
        {
            var result = await _woSerivce.PostInw_Recpt_Part_No(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostInward_Condn_list)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostInward_Condn_list([FromBody] Inward_Condn_listVM workOrdersVM)
        {
            var result = await _woSerivce.PostInward_Condn_list(workOrdersVM);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.WO.GetAllInward_Condn_list)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Inward_Condn_listVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllInward_Condn_list()
        {
            var allwo = await _woSerivce.GetAllInward_Condn_list();
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllInsp_Outcome_Details)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Inward_Condn_listVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllInsp_Outcome_Details(long tenantId)
        {
            var allwo = await _woSerivce.GetAllInsp_Outcome_Details(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllInsp_Outcome_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Inward_Condn_listVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllInsp_Outcome_List()
        {
            var allwo = await _woSerivce.GetAllInsp_Outcome_List();
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllInventory_Master)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Inventory_MasterVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllInventory_Master(long tenantId)
        {
            var allwo = await _woSerivce.GetAllInventory_Master(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllInvTransLog)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Inventory_MasterVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllInvTransLog(long tenantId)
        {
            var allwo = await _woSerivce.GetAllInvTransLog(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllInw_Recpt_Details)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Inventory_MasterVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllInw_Recpt_Details(long tenantId)
        {
            var allwo = await _woSerivce.GetAllInw_Recpt_Details(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAlInw_Recpt_Header)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Inventory_MasterVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAlInw_Recpt_Header(long tenantId)
        {
            var allwo = await _woSerivce.GetAlInw_Recpt_Header(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAlInw_Recpt_Part_No)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Inventory_MasterVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAlInw_Recpt_Part_No(long tenantId)
        {
            var allwo = await _woSerivce.GetAlInw_Recpt_Part_No(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteInsp_OutcomeDetails)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteInsp_OutcomeDetails(long Id)
        {
            var result = await _woSerivce.DeleteInsp_OutcomeDetails(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteInventory_Master)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteInventory_Master(long Id)
        {
            var result = await _woSerivce.DeleteInventory_Master(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteInv_Trans_Log)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteInv_Trans_Log(long Id)
        {
            var result = await _woSerivce.DeleteInv_Trans_Log(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteInw_Recpt_Details)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteInw_Recpt_Details(long Id)
        {
            var result = await _woSerivce.DeleteInw_Recpt_Details(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteInw_Recpt_Header)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteInw_Recpt_Header(long Id)
        {
            var result = await _woSerivce.DeleteInw_Recpt_Header(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteInw_Recpt_Part_No)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteInw_Recpt_Part_No(long Id)
        {
            var result = await _woSerivce.DeleteInw_Recpt_Part_No(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteInwardDoc)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteInwardDoc(long Id,long tenantId)
        {
            var result = await _woSerivce.DeleteInwardDoc(Id, tenantId);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteRcCaDoc)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteRcCaDoc(long Id,long tenantId)
        {
            var result = await _woSerivce.DeleteRcCaDoc(Id, tenantId);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteInspectDoc)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteInspectDoc(long Id,long tenantId)
        {
            var result = await _woSerivce.DeleteInspectDoc(Id, tenantId);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteFinalInspectDoc)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteFinalInspectDoc(long Id,long tenantId)
        {
            var result = await _woSerivce.DeleteFinalInspectDoc(Id, tenantId);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeletelineinspectDoc)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteLineInspectDoc(long Id, long tenantId)
        {
            var result = await _woSerivce.DeleteLineInspectDoc(Id, tenantId);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.WO.GetAllInwardDocList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Inventory_MasterVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllInwardDocList(long tenantId)
        {
            var allwo = await _woSerivce.GetAllInwardDocList(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllFinalInspectDocList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Inventory_MasterVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllFinalInspectDocList(long tenantId)
        {
            var allwo = await _woSerivce.GetAllFinalInspectDocList(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllLineInspectDocList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Inventory_MasterVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllLineInspectDocList(long tenantId)
        {
            var allwo = await _woSerivce.GetAllLineInspectDocList(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllInspectDocList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Inventory_MasterVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllInspectDocList(long tenantId)
        {
            var allwo = await _woSerivce.GetAllInspectDocList(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllRcCaDocList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Inventory_MasterVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllRcCaDocList(long tenantId)
        {
            var allwo = await _woSerivce.GetAllRcCaDocList(tenantId);
            return Ok(allwo);
        }

        [HttpPost]
        [Route(ApiRoutes.WO.PostInwardDocList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostInwardDocList([FromBody] InwardDocTypeVM workOrdersVM)
        {
            var result = await _woSerivce.PostInwardDocList(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostInspectDocList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostInspectDocList([FromBody] InspectDocTypeVM workOrdersVM)
        {
            var result = await _woSerivce.PostInspectDocList(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostRcCaDocList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostRcCaDocList([FromBody] RcCaDocTypeVM workOrdersVM)
        {
            var result = await _woSerivce.PostRcCaDocList(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostLineInspectDocList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostLineInspectDocList([FromBody] LineInspectDocTypeVM workOrdersVM)
        {
            var result = await _woSerivce.PostLineInspectDocList(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostFinalInspectDocList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostFinalInspectDocList([FromBody] FinalInspectDocTypeVM workOrdersVM)
        {
            var result = await _woSerivce.PostFinalInspectDocList(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllNcLogStatusList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Inventory_MasterVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllNcLogStatusList()
        {
            var allwo = await _woSerivce.GetAllNcLogStatusList();
            return Ok(allwo);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostOperationSettings)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostOperationSettings([FromBody] OperationSettingsVM workOrdersVM)
        {
            var result = await _woSerivce.PostOperationSettings(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllOperationSettings)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<OperationSettingsVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllOperationSettings()
        {
            var allwo = await _woSerivce.GetAllOperationSettings();
            return Ok(allwo);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostCont_RCA_CA_Log)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostCont_RCA_CA_Log([FromBody] Cont_RCA_CA_LogVM workOrdersVM)
        {
            var result = await _woSerivce.PostCont_RCA_CA_Log(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllCont_RCA_CA_Log)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Cont_RCA_CA_LogVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllCont_RCA_CA_Log(long tenantId)
        {
            var allwo = await _woSerivce.GetAllCont_RCA_CA_Log(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteCont_RCA_CA_Log)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteCont_RCA_CA_Log(long Id, long tenantId)
        {
            var result = await _woSerivce.DeleteCont_RCA_CA_Log(Id, tenantId);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostNC_Decision_Log)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostNC_Decision_Log([FromBody] NC_Decision_LogVM workOrdersVM)
        {
            var result = await _woSerivce.PostNC_Decision_Log(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllNC_Decision_Log)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<NC_Decision_LogVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllNC_Decision_Log(long tenantId)
        {
            var allwo = await _woSerivce.GetAllNC_Decision_Log(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteNC_Decision_Log)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteNC_Decision_Log(long Id, long tenantId)
        {
            var result = await _woSerivce.DeleteNC_Decision_Log(Id, tenantId);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostNC_Wk_List_Tmpl_Det)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostNC_Wk_List_Tmpl_Det([FromBody] NC_Wk_List_Tmpl_DetVM workOrdersVM)
        {
            var result = await _woSerivce.PostNC_Wk_List_Tmpl_Det(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllNC_Wk_List_Tmpl_Det)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<NC_Wk_List_Tmpl_DetVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllNC_Wk_List_Tmpl_Det(long tenantId)
        {
            var allwo = await _woSerivce.GetAllNC_Wk_List_Tmpl_Det(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteNC_Wk_List_Tmpl_Det)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteNC_Wk_List_Tmpl_Det(long Id, long tenantId)
        {
            var result = await _woSerivce.DeleteNC_Wk_List_Tmpl_Det(Id, tenantId);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostNC_Disp_Decs_Appl_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostNC_Disp_Decs_Appl_List([FromBody] NC_Disp_Decs_Appl_ListVM workOrdersVM)
        {
            var result = await _woSerivce.PostNC_Disp_Decs_Appl_List(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllNC_Disp_Decs_Appl_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<NC_Disp_Decs_Appl_ListVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllNC_Disp_Decs_Appl_List(long tenantId)
        {
            var allwo = await _woSerivce.GetAllNC_Disp_Decs_Appl_List(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteNC_Disp_Decs_Appl_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteNC_Disp_Decs_Appl_List(long Id, long tenantId)
        {
            var result = await _woSerivce.DeleteNC_Disp_Decs_Appl_List(Id, tenantId);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostNC_Wk_List_Tmpl_Head)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostNC_Wk_List_Tmpl_Head([FromBody] NC_Wk_List_Tmpl_HeadVM workOrdersVM)
        {
            var result = await _woSerivce.PostNC_Wk_List_Tmpl_Head(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllNC_Wk_List_Tmpl_Head)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<NC_Wk_List_Tmpl_HeadVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllNC_Wk_List_Tmpl_Head(long tenantId)
        {
            var allwo = await _woSerivce.GetAllNC_Wk_List_Tmpl_Head(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteNC_Wk_List_Tmpl_Head)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteNC_Wk_List_Tmpl_Head(long Id, long tenantId)
        {
            var result = await _woSerivce.DeleteNC_Wk_List_Tmpl_Head(Id, tenantId);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostNC_Work_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostNC_Work_List([FromBody] NC_Work_ListVM workOrdersVM)
        {
            var result = await _woSerivce.PostNC_Work_List(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllNC_Work_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<NC_Work_ListVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllNC_Work_List(long tenantId)
        {
            var allwo = await _woSerivce.GetAllNC_Work_List(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteNC_Work_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteNC_Work_List(long Id, long tenantId)
        {
            var result = await _woSerivce.DeleteNC_Work_List(Id, tenantId);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostNC_Wk_List_Header)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostNC_Wk_List_Header([FromBody] NC_Wk_List_HeaderVM workOrdersVM)
        {
            var result = await _woSerivce.PostNC_Wk_List_Header(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllNC_Wk_List_Header)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<NC_Wk_List_HeaderVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllNC_Wk_List_Header(long tenantId)
        {
            var allwo = await _woSerivce.GetAllNC_Wk_List_Header(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteNC_Wk_List_Header)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteNC_Wk_List_Header(long Id, long tenantId)
        {
            var result = await _woSerivce.DeleteNC_Wk_List_Header(Id, tenantId);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostCust_NC_Decs_Matrix_Opt)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostCust_NC_Decs_Matrix_Opt([FromBody] Cust_NC_Decs_Matrix_OptVM workOrdersVM)
        {
            var result = await _woSerivce.PostCust_NC_Decs_Matrix_Opt(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllCust_NC_Decs_Matrix_Opt)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Cust_NC_Decs_Matrix_OptVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllCust_NC_Decs_Matrix_Opt(long tenantId)
        {
            var allwo = await _woSerivce.GetAllCust_NC_Decs_Matrix_Opt(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteCust_NC_Decs_Matrix_Opt)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteCust_NC_Decs_Matrix_Opt(long Id, long tenantId)
        {
            var result = await _woSerivce.DeleteCust_NC_Decs_Matrix_Opt(Id, tenantId);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostCust_NC_Decs_Matrix)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostCust_NC_Decs_Matrix([FromBody] Cust_NC_Decs_MatrixVM workOrdersVM)
        {
            var result = await _woSerivce.PostCust_NC_Decs_Matrix(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllCust_NC_Decs_Matrix)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Cust_NC_Decs_MatrixVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllCust_NC_Decs_Matrix(long tenantId)
        {
            var allwo = await _woSerivce.GetAllCust_NC_Decs_Matrix(tenantId);
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteCust_NC_Decs_Matrix)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteCust_NC_Decs_Matrix(long Id, long tenantId)
        {
            var result = await _woSerivce.DeleteCust_NC_Decs_Matrix(Id, tenantId);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostCont_RCA_CA_Status_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostCont_RCA_CA_Status_List([FromBody] Cont_RCA_CA_Status_ListVM workOrdersVM)
        {
            var result = await _woSerivce.PostCont_RCA_CA_Status_List(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllCont_RCA_CA_Status_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Cont_RCA_CA_Status_ListVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllCont_RCA_CA_Status_List()
        {
            var allwo = await _woSerivce.GetAllCont_RCA_CA_Status_List();
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteCont_RCA_CA_Status_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteCont_RCA_CA_Status_List(long Id)
        {
            var result = await _woSerivce.DeleteCont_RCA_CA_Status_List(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostNC_Disp_Decision_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostNC_Disp_Decision_List([FromBody] NC_Disp_Decision_ListVM workOrdersVM)
        {
            var result = await _woSerivce.PostNC_Disp_Decision_List(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllNC_Disp_Decision_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<NC_Disp_Decision_ListVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllNC_Disp_Decision_List()
        {
            var allwo = await _woSerivce.GetAllNC_Disp_Decision_List();
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteNC_Disp_Decision_List)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteNC_Disp_Decision_List(long Id)
        {
            var result = await _woSerivce.DeleteNC_Disp_Decision_List(Id);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.WO.PostNC_work_Status)]
        [Produces(AppContentTypes.ContentType, Type = typeof(WoSubConSupplierVM))]
        public async Task<IActionResult> PostNC_work_Status([FromBody] NC_work_StatusVM workOrdersVM)
        {
            var result = await _woSerivce.PostNC_work_Status(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.GetAllNC_work_Status)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<NC_work_StatusVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetAllNC_work_Status()
        {
            var allwo = await _woSerivce.GetAllNC_work_Status();
            return Ok(allwo);
        }
        [HttpGet]
        [Route(ApiRoutes.WO.DeleteNC_work_Status)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteNC_work_Status(long Id)
        {
            var result = await _woSerivce.DeleteNC_work_Status(Id);
            return Ok(result);
        }
    }
}
