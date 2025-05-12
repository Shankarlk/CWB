using CWB.App.AppUtils;
using CWB.App.Models.BusinessProcesses;
using CWB.App.Models.WorkOrder;
using CWB.CommonUtils.Common;
using CWB.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Services.ProductionPlanWo
{
    public class WOService:IWOService
    {
        private readonly ILoggerManager _logger;
        private readonly ApiUrls _apiUrls;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly long tenantId;

        public WOService(ILoggerManager logger, ApiUrls apiUrlsOptions, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _apiUrls = apiUrlsOptions;
            _httpContextAccessor = httpContextAccessor;
            tenantId = long.Parse(AppUtil.GetTenantId(_httpContextAccessor.HttpContext.User));
        }

        public async Task<IEnumerable<WOSOVM>> GetSoWoRel(long workOrderId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getsowo/{workOrderId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<IEnumerable<WOSOVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<ProductionPlan_WoVM>> AllProductionPlan_Wo()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allproductionplanwo/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<IEnumerable<ProductionPlan_WoVM>>.GetAsync(uri, headers);
        }

        public async Task<List<ProductionPlan_WoVM>> ProductionPlanWoPost(IEnumerable<ProductionPlan_WoVM> productions)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/productionplan");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            foreach (var item in productions)
            {
                item.TenantId = tenantId;
            }
            return await RestHelper<List<ProductionPlan_WoVM>>.PostAsync(uri, productions, headers);
        }

        public async Task<List<ProcPlanVM>> ProcPlanPost(IEnumerable<ProcPlanVM> procPlans)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/procplan");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            foreach (var item in procPlans)
            {
                item.TenantId = tenantId;
            }
            return await RestHelper<List<ProcPlanVM>>.PostAsync(uri, procPlans, headers);
        }

        public async Task<List<WorkOrdersVM>> UpdateMultipleWorkOrder(IEnumerable<WorkOrdersVM> workOrders)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/updatemultipleworkorder");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            foreach (var item in workOrders)
            {
                item.TenantId = tenantId;
            }
            return await RestHelper<List<WorkOrdersVM>>.PostAsync(uri, workOrders, headers);
        }
        public async Task<List<BOMListVM>> BomListPost(IEnumerable<BOMListVM> bomlist)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/bomlistwo");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            foreach (var item in bomlist)
            {
                item.TenantId = tenantId;
            }
            return await RestHelper<List<BOMListVM>>.PostAsync(uri, bomlist, headers);
        }
        public async Task<List<ProcPlanPartPurChaseRelVM>> ProcPurchasePost(IEnumerable<ProcPlanPartPurChaseRelVM> bomlist)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postprocpurchase");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<ProcPlanPartPurChaseRelVM>>.PostAsync(uri, bomlist, headers);
        }

        public async Task<IEnumerable<ProcPlanVM>> GetAllProcPlan()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allprocplan/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<IEnumerable<ProcPlanVM>>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<BOMListVM>> GetAllBomlist()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allbomlist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<IEnumerable<BOMListVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<WoSubConSupplierVM>> GetAllSubCOnSupp()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allwosubcon/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<IEnumerable<WoSubConSupplierVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<PODetailsVM>> GetAllPodetails()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allpodetails/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<IEnumerable<PODetailsVM>>.GetAsync(uri, headers);
        }

        public async Task<WOStatusVM> GetWOStatus(long Id)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getwostatus/{Id}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<WOStatusVM>.GetAsync(uri, headers);
        }

        public async Task<List<ChildWoRelVM>> PostChildWoRel(IEnumerable<ChildWoRelVM> childWoRels)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/childworel");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            foreach (var item in childWoRels)
            {
                item.TenantId = tenantId;
            }
            return await RestHelper<List<ChildWoRelVM>>.PostAsync(uri, childWoRels, headers);
        }
        public async Task<bool> DeleteSubCon(long doctypeId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/delwosubcon/{doctypeId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<bool> DeleteWo(long doctypeId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletewo/{doctypeId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<WoSubConSupplierVM> PostSubConSupplier(WoSubConSupplierVM childWoRels)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postwosubcon");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            childWoRels.TenantId = tenantId;
            return await RestHelper<WoSubConSupplierVM>.PostAsync(uri, childWoRels, headers);
        }
        public async Task<List<McTimeListVM>> PostMcTimeList(IEnumerable<McTimeListVM> mcTimeListVMs)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postmctimelist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            foreach (var item in mcTimeListVMs)
            {
                item.TenantId = tenantId;
            }
            return await RestHelper<List<McTimeListVM>>.PostAsync(uri, mcTimeListVMs, headers);
        }
        public async Task<IEnumerable<McTimeListVM>> GetAllMcTimeList()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allmctimelist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<IEnumerable<McTimeListVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<WorkOrdersVM>> AllParentChildWos(long parentWoId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allparentchildwos/{parentWoId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<WorkOrdersVM>>.GetAsync(uri, headers);
        }
        public async Task<List<PODetailsVM>> PODetails(IEnumerable<PODetailsVM> pODetails)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/multiplepodetails");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            foreach (var item in pODetails)
            {
                item.TenantId = tenantId;
            }
            return await RestHelper<List<PODetailsVM>>.PostAsync(uri, pODetails, headers);
        }
        public async Task<List<POHeaderVM>> POHeader(IEnumerable<POHeaderVM> pOHeaderVMs)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/multiplepoheaders");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            foreach (var item in pOHeaderVMs)
            {
                item.TenantId = tenantId;
            }
            return await RestHelper<List<POHeaderVM>>.PostAsync(uri, pOHeaderVMs, headers);
        }
        public async Task<Inw_Recpt_HeaderVM> PostInw_Recpt_Header(Inw_Recpt_HeaderVM childWoRels)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postinwrecptheader");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            childWoRels.TenantId = tenantId;
            return await RestHelper<Inw_Recpt_HeaderVM>.PostAsync(uri, childWoRels, headers);
        }
        public async Task<Inw_Recpt_DetailsVM> PostInw_Recpt_Details(Inw_Recpt_DetailsVM childWoRels)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postinwrecptdetails");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            childWoRels.TenantId = tenantId;
            return await RestHelper<Inw_Recpt_DetailsVM>.PostAsync(uri, childWoRels, headers);
        }
        public async Task<Insp_Outcome_DetailsVM> PostNcLog(Insp_Outcome_DetailsVM childWoRels)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postinspoutcomedetails");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            childWoRels.TenantId = tenantId;
            return await RestHelper<Insp_Outcome_DetailsVM>.PostAsync(uri, childWoRels, headers);
        }
        public async Task<Inw_Recpt_Part_NoVM> PostInw_Recpt_Part_No(Inw_Recpt_Part_NoVM childWoRels)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postinwrecptpartno");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            childWoRels.TenantId = tenantId;
            return await RestHelper<Inw_Recpt_Part_NoVM>.PostAsync(uri, childWoRels, headers);
        }
        public async Task<Insp_Outcome_DetailsVM> PostInsp_Outcome_Details(Insp_Outcome_DetailsVM childWoRels)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postinspoutcomedetails");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            childWoRels.TenantId = tenantId;
            return await RestHelper<Insp_Outcome_DetailsVM>.PostAsync(uri, childWoRels, headers);
        }
        public async Task<IEnumerable<InwardDocTypeVM>> GetAllInWardDocList()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allinwarddoc/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<InwardDocTypeVM>>.GetAsync(uri, headers);
        }
        public async Task<InwardDocTypeVM> PostInWardDocList(InwardDocTypeVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postinwarddoc");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<InwardDocTypeVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteInWardDocList(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deleteinwarddoc/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<InspectDocTypeVM>> GetAllInspectDocList()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allinspectdoc/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<InspectDocTypeVM>>.GetAsync(uri, headers);
        }
        public async Task<InspectDocTypeVM> PostInspectDocList(InspectDocTypeVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postinspectdoc");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<InspectDocTypeVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteInspectDocList(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deleteinspectdoc/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<LineInspectDocTypeVM>> GetAllLineDocList()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/alllineinspdoc/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<LineInspectDocTypeVM>>.GetAsync(uri, headers);
        }
        public async Task<LineInspectDocTypeVM> PostLineDocList(LineInspectDocTypeVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postlineinspdoc");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<LineInspectDocTypeVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteLineDocList(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletelineinspectdoc/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<FinalInspectDocTypeVM>> GetAllFinalDocList()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allfinalinspdoc/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<FinalInspectDocTypeVM>>.GetAsync(uri, headers);
        }
        public async Task<FinalInspectDocTypeVM> PostFinalDocList(FinalInspectDocTypeVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postfinalinspdoc");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<FinalInspectDocTypeVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteFinalDocList(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletefinalinspectdoc/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Cust_NC_Decs_MatrixVM>> GetAllCust_NC_Decs_Matrix()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getcustncdec/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Cust_NC_Decs_MatrixVM>>.GetAsync(uri, headers);
        }
        public async Task<Cust_NC_Decs_MatrixVM> PostCust_NC_Decs_Matrix(Cust_NC_Decs_MatrixVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postcustncdec");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Cust_NC_Decs_MatrixVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteCust_NC_Decs_Matrix(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletecustncdec/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Cust_NC_Decs_Matrix_OptVM>> GetAllCust_NC_Decs_Matrix_Opt()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getcustncdecopt/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Cust_NC_Decs_Matrix_OptVM>>.GetAsync(uri, headers);
        }
        public async Task<Cust_NC_Decs_Matrix_OptVM> PostCust_NC_Decs_Matrix_Opt(Cust_NC_Decs_Matrix_OptVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postcustncdecopt");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Cust_NC_Decs_Matrix_OptVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteCust_NC_Decs_Matrix_Opt(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletecustncdecopt/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<bool> DeleteNC_Wk_List_Tmpl_Head(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletencwklsthead/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<bool> DeleteNC_Wk_List_Tmpl_Det(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletencwklisttmpldet/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<RcCaDocTypeVM>> GetAllRcaCaDocList()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allrccadoc/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<RcCaDocTypeVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<OperationSettingsVM>> GetAllOperationSettings()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getoperationsettingss");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<OperationSettingsVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<NC_Wk_List_Tmpl_DetVM>> GetAllNC_Wk_List_Tmpl_Det()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getncwklisttmpldet/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<NC_Wk_List_Tmpl_DetVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<NC_Wk_List_Tmpl_HeadVM>> GetAllNC_Wk_List_Tmpl_Head()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getncwklsthead/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<NC_Wk_List_Tmpl_HeadVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Inventory_MasterVM>> GetAllInventory_Master()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allinvmastery/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Inventory_MasterVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Inv_Trans_LogVM>> GetAllInv_Trans_Log()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allinvtranslog/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Inv_Trans_LogVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Cont_RCA_CA_LogVM>> GetAllCont_RCA_CA_log()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getcontrcacalog/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Cont_RCA_CA_LogVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<NC_Decision_LogVM>> GetAllNC_Decision_Log()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getncdeclog/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<NC_Decision_LogVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<NC_Disp_Decision_ListVM>> GetAllNC_Disp_Decision_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getncdisplist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<NC_Disp_Decision_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<NC_Disp_Decs_Appl_ListVM>> GetAllNC_Disp_Decs_Appl_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getncdispappllist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<NC_Disp_Decs_Appl_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Inward_Condn_listVM>> GetAllInward_Condn_list()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allinwardcondlist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Inward_Condn_listVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Inw_Recpt_HeaderVM>> GetAllInw_Recpt_Header()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allinwrecptheader/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Inw_Recpt_HeaderVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Inw_Recpt_DetailsVM>> GetAllInw_Recpt_Details()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allinwrecptdetails/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Inw_Recpt_DetailsVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Insp_Outcome_DetailsVM>> GetAllNcLog()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allinspoutcomedetails/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Insp_Outcome_DetailsVM>>.GetAsync(uri, headers);
        }
        public async Task<RcCaDocTypeVM> PostRcaCaDocList(RcCaDocTypeVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postrccadoc");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<RcCaDocTypeVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<NC_Wk_List_Tmpl_DetVM> PostNC_Wk_List_Tmpl_Det(NC_Wk_List_Tmpl_DetVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postncwklisttmpldet");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<NC_Wk_List_Tmpl_DetVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<NC_Disp_Decs_Appl_ListVM> PostNC_Disp_Decs_Appl_List(NC_Disp_Decs_Appl_ListVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postncdispappllist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<NC_Disp_Decs_Appl_ListVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<NC_Decision_LogVM> PostNC_Decision_Log(NC_Decision_LogVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postncdeclog");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<NC_Decision_LogVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<Inventory_MasterVM> PostInventory_Master(Inventory_MasterVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postinvenmaster");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Inventory_MasterVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<Inv_Trans_LogVM> PostInv_Trans_Log(Inv_Trans_LogVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postinvtranslog");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.PersonId = tenantId;
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Inv_Trans_LogVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<Cont_RCA_CA_LogVM> PostCont_RCA_CA_Log(Cont_RCA_CA_LogVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postcontrcacalog");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Cont_RCA_CA_LogVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<NC_Wk_List_Tmpl_HeadVM> PostNC_Wk_List_Tmpl_Head(NC_Wk_List_Tmpl_HeadVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postncwklsthead");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<NC_Wk_List_Tmpl_HeadVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<OperationSettingsVM> PostOperationsSettings(OperationSettingsVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postoperationsetting");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = 0;
            return await RestHelper<OperationSettingsVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteRcaCaDocList(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deleterccadoc/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<bool> DeleteNC_Disp_Decs_Appl_List(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletencdispappllist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
    }
}
