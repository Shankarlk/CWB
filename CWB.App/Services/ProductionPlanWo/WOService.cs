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
        public async Task<IEnumerable<ProductionPlan_WoVM>> AllProductionWoReadForProd()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allproductionworeadforprod/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<IEnumerable<ProductionPlan_WoVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<ProductionPlan_WoVM>> GetAllReadyforProductionWo()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getallreadyforproductionwo/{tenantId}");
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





        public async Task<List<Input_Resrv_ListVM>> PostInputReservelist(IEnumerable<Input_Resrv_ListVM> allocations)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postinputreservelist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            foreach (var item in allocations)
            {
                item.TenantId = tenantId;
            }
            return await RestHelper<List<Input_Resrv_ListVM>>.PostAsync(uri, allocations, headers);
        }
        public async Task<List<ProductionPlan_WoVM>> ProductionPlanWoPostFreeze(IEnumerable<ProductionPlan_WoVM> productions)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/productionplanfreeeze");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            foreach (var item in productions)
            {
                item.TenantId = tenantId;
            }
            return await RestHelper<List<ProductionPlan_WoVM>>.PostAsync(uri, productions, headers);
        }


        public async Task<List<Input_Resrv_ListVM>> GetallInputreservelistbypartid(long partId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allinputreservelistwithpartid/{tenantId}/{partId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Input_Resrv_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<List<ProductionPlan_WoVM>> ProductionPlanWoPostConsolidation(IEnumerable<ProductionPlan_WoVM> productions)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postproductionplanconsolidation");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            foreach (var item in productions)
            {
                item.TenantId = tenantId;
            }
            return await RestHelper<List<ProductionPlan_WoVM>>.PostAsync(uri, productions, headers);
        }
        public async Task<List<ProductionPlan_WoVM>> UpdateProduction_WoForReference(IEnumerable<ProductionPlan_WoVM> productions)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/updateproductionwoforreference");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            foreach (var item in productions)
            {
                item.TenantId = tenantId;
            }
            return await RestHelper<List<ProductionPlan_WoVM>>.PostAsync(uri, productions, headers);
        }

        public async Task<List<ConsolidatedWoMappingVM>> PostConsolidatedWO(IEnumerable<ConsolidatedWoMappingVM> productions)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postconsolidationwo");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            foreach (var item in productions)
            {
                item.TenantId = tenantId;
            }
            return await RestHelper<List<ConsolidatedWoMappingVM>>.PostAsync(uri, productions, headers);
        }
        public async Task<IEnumerable<ConsolidatedWoMappingVM>> Getallconsolidationproductionwo()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getallconsolidationwo/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<IEnumerable<ConsolidatedWoMappingVM>>.GetAsync(uri, headers);
        }

        public async Task<ProductionPlan_WoVM> UpdateProductionPlan_Wo(ProductionPlan_WoVM productions)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/updateproductionplan");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            productions.TenantId = tenantId;
            return await RestHelper<ProductionPlan_WoVM>.PostAsync(uri, productions, headers);
        }
        public async Task<ProductionPlan_WoVM> UpdateProductionPlan_WoCritcalPart(ProductionPlan_WoVM productions)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/updateproductionplancriticalpart");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            productions.TenantId = tenantId;
            return await RestHelper<ProductionPlan_WoVM>.PostAsync(uri, productions, headers);
        }
        public async Task<ProductionPlan_WoVM> UpdateHoldProductionPlan_Wo(ProductionPlan_WoVM productions)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/updateholdproductionplan");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            productions.TenantId = tenantId;
            return await RestHelper<ProductionPlan_WoVM>.PostAsync(uri, productions, headers);
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
        public async Task<List<ProcPlanVM>> ProcPlanPostPOFlag(IEnumerable<ProcPlanVM> procPlans)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/procplanpoflag");
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
        public async Task<Inv_Trans_ListVM> GetInv_trans_Desc(long Id)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getinventorytransdescname/{Id}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<Inv_Trans_ListVM>.GetAsync(uri, headers);
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
        public async Task<List<PODetailsVM>> UpdateInspection(IEnumerable<PODetailsVM> pODetails)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/updateinspection");
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
        public async Task<SetupVariationReasonVM> PostSetupVariationReason(SetupVariationReasonVM childWoRels)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postsetupvariation");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            childWoRels.TenantId = tenantId;
            return await RestHelper<SetupVariationReasonVM>.PostAsync(uri, childWoRels, headers);
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
        public async Task<IEnumerable<Inventory_MasterVM>> GetAllInventory_MasterBypartid(long locationId, long oprnoId, long routingId,long partid)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allinvmasterybypartid/{locationId}/{oprnoId}/{routingId}/{partid}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Inventory_MasterVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Inventory_MasterVM>> GetAllInventory_MasterBypartidWithFlag(string flag,long locationId, long oprnoId, long routingId, long partid)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allinvmasterybypartidwithflag/{flag}/{locationId}/{oprnoId}/{routingId}/{partid}/{tenantId}");
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
        public async Task<IEnumerable<Inv_Master_LogVM>> GetAllInv_Master_Log()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getinvmasterlog/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Inv_Master_LogVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Inv_Mismatch_ListVM>> GetAllInv_Mismatch_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getinvmismatchlist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Inv_Mismatch_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<bool> DeleteInvMismatch(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deleteinvmismatchlist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
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
        public async Task<IEnumerable<SetupVariationReasonVM>> GetAllSetupVariationReason()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allsetupvariation");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<SetupVariationReasonVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Cust_NC_DecisionVM>> GetAllCust_NC_Decision()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/allcustncdecision");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Cust_NC_DecisionVM>>.GetAsync(uri, headers);
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
        public async Task<Inv_Mismatch_ListVM> PostInv_Mismatch_List(Inv_Mismatch_ListVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postinvmismatchlist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.Reported_By = tenantId;
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Inv_Mismatch_ListVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<Inv_Master_LogVM> PostInv_Master_Log(Inv_Master_LogVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postinvmasterlog");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Inv_Master_LogVM>.PostAsync(uri, purchaseDetailVM, headers);
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


        public async Task<IEnumerable<WO_Wait_ListVM>> GetAllWO_Wait_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getwowaitlist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<WO_Wait_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<WO_Wait_ListVM> PostWO_Wait_List(WO_Wait_ListVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postwowaitlist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<WO_Wait_ListVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteWO_Wait_List(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletewowaitlist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<bool> DeleteInv_Master_Log(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deleteinvmasterlog/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<bool> DeleteInv_Mismatch_List(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deleteinvmismatchlist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<TempWO_Wait_ListVM>> GetAllTempWo_Wait_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/gettempwowaitlist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<TempWO_Wait_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<TempWO_Wait_ListVM> PostTempWo_Wait_List(TempWO_Wait_ListVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/posttempwowaitlist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<TempWO_Wait_ListVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteTempWo_Wait_List(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletetempwowaitlist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Mc_Not_Avl_ReasonVM>> GetAllMc_not_avl_reason()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getmcnotavlreason/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Mc_Not_Avl_ReasonVM>>.GetAsync(uri, headers);
        }
        public async Task<Mc_Not_Avl_ReasonVM> PostMc_not_avl_reason(Mc_Not_Avl_ReasonVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postmcnotavlreason");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Mc_Not_Avl_ReasonVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<IEnumerable<Timeslot_SettingVM>> GetAllTimeslot_Setting()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/gettimeslotsetting/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Timeslot_SettingVM>>.GetAsync(uri, headers);
        }
        public async Task<Timeslot_SettingVM> PostTimeslot_Setting(Timeslot_SettingVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/posttimeslotsetting");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Timeslot_SettingVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteTimeslot_Setting(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletetimeslotsetting/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Timeslot_ListVM>> GetAllTimeslot_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/gettimeslotlist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Timeslot_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<Timeslot_ListVM> PostTimeslot_List(Timeslot_ListVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/posttimeslotlist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Timeslot_ListVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteTimeslot_List(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletetimeslotlist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Mc_Timeslot_ListVM>> GetAllMc_Timeslot_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getmctimeslotlist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Mc_Timeslot_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<Mc_Timeslot_ListVM> PostMc_Timeslot_List(Mc_Timeslot_ListVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postmctimeslotlist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Mc_Timeslot_ListVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteMc_Timeslot_List(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletemctimeslotlist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<TempMc_Timeslot_ListVM>> GetAllTempMc_Timeslot_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/gettempmctimeslotlist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<TempMc_Timeslot_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<TempMc_Timeslot_ListVM> PostTempMc_Timeslot_List(TempMc_Timeslot_ListVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/posttempmctimeslotlist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<TempMc_Timeslot_ListVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteTempMc_Timeslot_List(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletetempmctimeslotlist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Mc_Wait_ListVM>> GetAllMc_Wait_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getmcwaitlist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Mc_Wait_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<TempMc_Wait_ListVM>> GetAllSetUpApprolList()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getallsetupapprovallist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<TempMc_Wait_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<TempMc_Wait_ListVM>> GetAllSetUpCnfList()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getallsetupcnflist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<TempMc_Wait_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<TempMc_Wait_ListVM>> GetAllBookOutList()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getallbookoutlist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<TempMc_Wait_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<Mc_Wait_ListVM> PostMc_Wait_List(Mc_Wait_ListVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postmcwaitlist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Mc_Wait_ListVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteMc_Wait_List(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletemcwaitlist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Matl_Issue_SettingsVM>> GetAllMatl_Issue_Settings()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getmatlissuesetting/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Matl_Issue_SettingsVM>>.GetAsync(uri, headers);
        }
        public async Task<Matl_Issue_SettingsVM> PostMatl_Issue_Settings(Matl_Issue_SettingsVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postmatlissuesetting");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Matl_Issue_SettingsVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteMatl_Issue_Settings(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletematlissuesetting/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<DispatchDetailsVM>> GetAllDispatchDetails()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getdispatchdetails/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<DispatchDetailsVM>>.GetAsync(uri, headers);
        }
        public async Task<DispatchDetailsVM> PostDispatchDetails(DispatchDetailsVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postdispatchdetails");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<DispatchDetailsVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteDispatchDetails(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletedispatchdetails/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<DispatchQntyVM>> GetAllDispatchQnty()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getdispatchqnty/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<DispatchQntyVM>>.GetAsync(uri, headers);
        }
        public async Task<DispatchQntyVM> PostDispatchQnty(DispatchQntyVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postdispatchqnty");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<DispatchQntyVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteDispatchQnty(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletedispatchqnty/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Matl_Issue_ListVM>> GetAllMatl_Issue_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getmatlissuelist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Matl_Issue_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Matl_Issue_ListVM>> GetAllMatlIssueListForShop()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getallmatlissuelistforshop/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Matl_Issue_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<Matl_Issue_ListVM> PostMatl_Issue_List(Matl_Issue_ListVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postmatlissuelist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Matl_Issue_ListVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteMatl_Issue_List(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletematlissuelist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<TempMc_Wait_ListVM>> GetAllTempMc_Wait_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/gettempmcwaitlist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<TempMc_Wait_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<TempMc_Wait_ListVM> PostTempMc_Wait_List(TempMc_Wait_ListVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/posttempmcwaitlist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<TempMc_Wait_ListVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteTempMc_Wait_List(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletetempmcwaitlist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Non_Plan_Wk_type_ListVM>> GetAllNon_Plan_Wk_type_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getnonplanwktypelist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Non_Plan_Wk_type_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Mode_ListVM>> GetAllMode_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getmodelist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Mode_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Non_Plan_Wk_ListVM>> GetAllNon_Plan_Wk_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getnonplanwklist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Non_Plan_Wk_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<Non_Plan_Wk_ListVM> PostNon_Plan_Wk_List(Non_Plan_Wk_ListVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postnonplanwklist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Non_Plan_Wk_ListVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteNon_Plan_Wk_List(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletenonplanwklist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Rwk_ListVM>> GetAllRwk_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getrwklist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Rwk_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<Rwk_ListVM> PostRwk_List(Rwk_ListVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postrwklist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Rwk_ListVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteRwk_List(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deleterwklist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Shop_Insp_LogVM>> GetAllShop_Insp_Log()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getshopinsplog/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Shop_Insp_LogVM>>.GetAsync(uri, headers);
        }
        public async Task<Shop_Insp_LogVM> PostShop_Insp_Log(Shop_Insp_LogVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postshopinsplog");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            purchaseDetailVM.Inspected_by = tenantId;
            return await RestHelper<Shop_Insp_LogVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteShop_Insp_Log(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deleteshopinsplog/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<SubCon_ListVM>> GetAllSubCon_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getsubconlist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<SubCon_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<SubCon_ListVM> PostSubCon_List(SubCon_ListVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postsubconlist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<SubCon_ListVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteSubCon_List(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletesubconlist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<TempSubCon_ListVM>> GetAllTempSubCon_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/gettempsubconlist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<TempSubCon_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<TempSubCon_ListVM> PostTempSubCon_List(TempSubCon_ListVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/posttempsubconlist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<TempSubCon_ListVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteTempSubCon_List(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletetempsubconlist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Opr_ListVM>> GetAllOpr_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getoprwklist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Opr_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Mc_Wait_ListVM>> GetAllMc_Wait_ListByMcWait(long woid)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/getmcwaitlistbywoid/{woid}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Mc_Wait_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<Opr_ListVM> PostOpr_List(Opr_ListVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/postoprwklist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<Opr_ListVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteOpr_List(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deleteoprwklist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<TempOpr_ListVM>> GetAllTempOpr_List()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/gettempoprwklist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<TempOpr_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<TempOpr_ListVM>> GetAllTempOpr_Listwithroutingidandwoid(long routingId,long woId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/gettempoprwklistwithroutingidandwoid/{routingId}/{woId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<TempOpr_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<TempOpr_ListVM> PostTempOpr_List(TempOpr_ListVM purchaseDetailVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/posttempoprwklist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            purchaseDetailVM.TenantId = tenantId;
            return await RestHelper<TempOpr_ListVM>.PostAsync(uri, purchaseDetailVM, headers);
        }
        public async Task<bool> DeleteTempOpr_List(long itemMasterDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbpwo/deletetempoprwklist/{itemMasterDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
    }
}
