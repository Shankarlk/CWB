using CWB.App.Models.BusinessProcesses;
using CWB.App.Models.WorkOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Services.ProductionPlanWo
{
    public interface IWOService
    {
        Task<IEnumerable<WOSOVM>> GetSoWoRel(long workOrderId);
        Task<List<ProductionPlan_WoVM>> ProductionPlanWoPost(IEnumerable<ProductionPlan_WoVM> productions);
        Task<IEnumerable<ProductionPlan_WoVM>> AllProductionPlan_Wo();
        Task<List<ProcPlanVM>> ProcPlanPost(IEnumerable<ProcPlanVM> procPlans);
        Task<List<WorkOrdersVM>> UpdateMultipleWorkOrder(IEnumerable<WorkOrdersVM> workOrders);
        Task<List<BOMListVM>> BomListPost(IEnumerable<BOMListVM> bomlist);
        Task<List<ProcPlanPartPurChaseRelVM>> ProcPurchasePost(IEnumerable<ProcPlanPartPurChaseRelVM> bomlist);
        Task<IEnumerable<ProcPlanVM>> GetAllProcPlan();
        Task<IEnumerable<BOMListVM>> GetAllBomlist();
        Task<IEnumerable<WoSubConSupplierVM>> GetAllSubCOnSupp();
        Task<IEnumerable<PODetailsVM>> GetAllPodetails();
        Task<WOStatusVM> GetWOStatus(long Id);
        Task<List<ChildWoRelVM>> PostChildWoRel(IEnumerable<ChildWoRelVM> childWoRels);
        Task<List<McTimeListVM>> PostMcTimeList(IEnumerable<McTimeListVM> mcTimeListVMs);
        Task<IEnumerable<McTimeListVM>> GetAllMcTimeList();
        Task<IEnumerable<WorkOrdersVM>> AllParentChildWos(long parentWoId);
        Task<WoSubConSupplierVM> PostSubConSupplier(WoSubConSupplierVM childWoRels);

        Task<bool> DeleteSubCon(long doctypeId);
        Task<bool> DeleteWo(long doctypeId);
        Task<List<PODetailsVM>> PODetails(IEnumerable<PODetailsVM> pODetails);
        Task<List<POHeaderVM>> POHeader(IEnumerable<POHeaderVM> pOHeaderVMs);
        Task<Inw_Recpt_HeaderVM> PostInw_Recpt_Header(Inw_Recpt_HeaderVM childWoRels);
        Task<Inw_Recpt_DetailsVM> PostInw_Recpt_Details(Inw_Recpt_DetailsVM childWoRels);
        Task<Insp_Outcome_DetailsVM> PostNcLog(Insp_Outcome_DetailsVM childWoRels);

        Task<IEnumerable<InwardDocTypeVM>> GetAllInWardDocList();
        Task<Inw_Recpt_Part_NoVM> PostInw_Recpt_Part_No(Inw_Recpt_Part_NoVM inwardDocTypeVM);
        Task<InwardDocTypeVM> PostInWardDocList(InwardDocTypeVM inwardDocTypeVM);
        Task<bool> DeleteInWardDocList(long itemMasterDocListId);
        Task<IEnumerable<InspectDocTypeVM>> GetAllInspectDocList();
        Task<InspectDocTypeVM> PostInspectDocList(InspectDocTypeVM InspectDocTypeVM);
        Task<bool> DeleteInspectDocList(long itemMasterDocListId);
        Task<IEnumerable<LineInspectDocTypeVM>> GetAllLineDocList();
        Task<LineInspectDocTypeVM> PostLineDocList(LineInspectDocTypeVM LineDocTypeVM);
        Task<bool> DeleteLineDocList(long itemMasterDocListId);
        Task<IEnumerable<FinalInspectDocTypeVM>> GetAllFinalDocList();
        Task<FinalInspectDocTypeVM> PostFinalDocList(FinalInspectDocTypeVM FinalDocTypeVM);
        Task<bool> DeleteFinalDocList(long itemMasterDocListId);
        Task<IEnumerable<Cust_NC_Decs_MatrixVM>> GetAllCust_NC_Decs_Matrix();
        Task<Cust_NC_Decs_MatrixVM> PostCust_NC_Decs_Matrix(Cust_NC_Decs_MatrixVM FinalDocTypeVM);
        Task<bool> DeleteCust_NC_Decs_Matrix(long itemMasterDocListId);
        Task<IEnumerable<Cust_NC_Decs_Matrix_OptVM>> GetAllCust_NC_Decs_Matrix_Opt();
        Task<Cust_NC_Decs_Matrix_OptVM> PostCust_NC_Decs_Matrix_Opt(Cust_NC_Decs_Matrix_OptVM FinalDocTypeVM);
        Task<bool> DeleteCust_NC_Decs_Matrix_Opt(long itemMasterDocListId);
        Task<bool> DeleteNC_Wk_List_Tmpl_Head(long itemMasterDocListId);
        Task<bool> DeleteNC_Wk_List_Tmpl_Det(long itemMasterDocListId);
        Task<IEnumerable<RcCaDocTypeVM>> GetAllRcaCaDocList();
        Task<IEnumerable<OperationSettingsVM>> GetAllOperationSettings();
        Task<IEnumerable<NC_Wk_List_Tmpl_DetVM>> GetAllNC_Wk_List_Tmpl_Det();
        Task<IEnumerable<NC_Wk_List_Tmpl_HeadVM>> GetAllNC_Wk_List_Tmpl_Head();
        Task<IEnumerable<Cont_RCA_CA_LogVM>> GetAllCont_RCA_CA_log();
        Task<IEnumerable<Inv_Trans_LogVM>> GetAllInv_Trans_Log();
        Task<IEnumerable<Inventory_MasterVM>> GetAllInventory_Master();
        Task<IEnumerable<NC_Decision_LogVM>> GetAllNC_Decision_Log();
        Task<IEnumerable<NC_Disp_Decision_ListVM>> GetAllNC_Disp_Decision_List();
        Task<IEnumerable<NC_Disp_Decs_Appl_ListVM>> GetAllNC_Disp_Decs_Appl_List();
        Task<IEnumerable<Inw_Recpt_HeaderVM>> GetAllInw_Recpt_Header();
        Task<IEnumerable<Inward_Condn_listVM>> GetAllInward_Condn_list();
        Task<IEnumerable<Inw_Recpt_DetailsVM>> GetAllInw_Recpt_Details();
        Task<IEnumerable<Insp_Outcome_DetailsVM>> GetAllNcLog();
        Task<RcCaDocTypeVM> PostRcaCaDocList(RcCaDocTypeVM RcaCaDocTypeVM);
        Task<NC_Wk_List_Tmpl_DetVM> PostNC_Wk_List_Tmpl_Det(NC_Wk_List_Tmpl_DetVM RcaCaDocTypeVM);
        Task<NC_Disp_Decs_Appl_ListVM> PostNC_Disp_Decs_Appl_List(NC_Disp_Decs_Appl_ListVM RcaCaDocTypeVM);
        Task<NC_Wk_List_Tmpl_HeadVM> PostNC_Wk_List_Tmpl_Head(NC_Wk_List_Tmpl_HeadVM RcaCaDocTypeVM);
        Task<Cont_RCA_CA_LogVM> PostCont_RCA_CA_Log(Cont_RCA_CA_LogVM RcaCaDocTypeVM);
        Task<Inventory_MasterVM> PostInventory_Master(Inventory_MasterVM RcaCaDocTypeVM);
        Task<Inv_Trans_LogVM> PostInv_Trans_Log(Inv_Trans_LogVM RcaCaDocTypeVM);
        Task<NC_Decision_LogVM> PostNC_Decision_Log(NC_Decision_LogVM RcaCaDocTypeVM);
        Task<OperationSettingsVM> PostOperationsSettings(OperationSettingsVM RcaCaDocTypeVM);
        Task<bool> DeleteRcaCaDocList(long itemMasterDocListId);
        Task<bool> DeleteNC_Disp_Decs_Appl_List(long itemMasterDocListId);

    }
}
