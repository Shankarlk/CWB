using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Utils
{
    public class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class WO
        {
            public const string PostWorkOrder = Base + "/workorder";
            public const string PostWoSubCon = Base + "/postwosubcon";
            public const string PostInsp_Outcome_Details = Base + "/postinspoutcomedetails";
            public const string PostInsp_Outcome_List = Base + "/postinspoutcomelist";
            public const string PostInventory_Master = Base + "/postinvenmaster";
            public const string PostInv_Trans_Log = Base + "/postinvtranslog";
            public const string PostInw_Recpt_Details = Base + "/postinwrecptdetails";
            public const string PostInw_Recpt_Header = Base + "/postinwrecptheader";
            public const string PostInw_Recpt_Part_No = Base + "/postinwrecptpartno";
            public const string PostInward_Condn_list = Base + "/postinwcondnlist";
            public const string PostMultipleWorkOrder = Base + "/multipleworkorder";
            public const string PostUpdateMultipleWorkOrder = Base + "/updatemultipleworkorder";
            public const string HelloWorld = Base + "/helloworld";
            public const string GetPOLogs = Base + "/getpologs/{tenantId}/{poid}";
            public const string GetWoPOLogs = Base + "/getwopologs/{tenantId}/{customerOrderId}";
            public const string AllWorkOrders = Base + "/allworkorders/{tenantId}";
            public const string AllSubCon = Base + "/allwosubcon/{tenantId}";
            public const string GetAllInsp_Outcome_Details = Base + "/allinspoutcomedetails/{tenantId}";
            public const string GetAllInventory_Master = Base + "/allinvmastery/{tenantId}";
            public const string GetAllInvTransLog = Base + "/allinvtranslog/{tenantId}";
            public const string GetAllInw_Recpt_Details = Base + "/allinwrecptdetails/{tenantId}";
            public const string GetAlInw_Recpt_Header = Base + "/allinwrecptheader/{tenantId}";
            public const string GetAlInw_Recpt_Part_No = Base + "/allinwrecptpartno/{tenantId}";
            public const string GetAllInward_Condn_list = Base + "/allinwardcondlist";
            public const string GetAllInsp_Outcome_List = Base + "/allinspoutcomelist";
            public const string AllParentChildWos = Base + "/allparentchildwos/{parentWoId}/{tenantId}";
            public const string GetSingleWorkOrder = Base + "/getsingleworkorder/{Id}/{tenantId}";
            public const string GetSoWo = Base + "/getsowo/{workOrderId}";
            public const string GetProCPurchase = Base + "/getprocpurchase/{procPlanId}";
            public const string PostWOSORel = Base + "/wosorel";
            public const string PostProcPurchase = Base + "/postprocpurchase";
            public const string PostBOMTemp = Base + "/bomtemp";
            public const string PostProcPlan = Base + "/procplan";
            public const string PostBomList = Base + "/bomlistwo";
            public const string AllProcPlan = Base + "/allprocplan/{tenantId}";
            public const string AllBomList = Base + "/allbomlist/{tenantId}";
            public const string PostProductionPlan_Wo = Base + "/productionplan";
            public const string AllProductionPlanWo = Base + "/allproductionplanwo/{tenantId}";
            public const string DeleteSubCon = Base + "/delwosubcon/{Id}";
            public const string DeleteWo = Base + "/deletewo/{Id}";
            public const string GetWoStatus = Base + "/getwostatus/{Id}";
            public const string PostChildWoRel = Base + "/childworel";
            public const string PostMcTimeList = Base + "/postmctimelist";
            public const string GetAllMctimeList = Base + "/allmctimelist/{tenantId}";
            public const string GetAllPodetails = Base + "/allpodetails/{tenantId}";
            public const string GetPoStatus = Base + "/getpostatus/{Id}";
            public const string PostMultiplePODetails = Base + "/multiplepodetails";
            public const string PostMultiplePOHeaders = Base + "/multiplepoheaders";
            public const string DeleteInsp_OutcomeDetails = Base + "/deleteinspoutcomedetails/{Id}";
            public const string DeleteInventory_Master = Base + "/deleteinvmaster/{Id}";
            public const string DeleteInv_Trans_Log = Base + "/deleteinvtranslog/{Id}";
            public const string DeleteInw_Recpt_Details = Base + "/deleteinwrecptdetails/{Id}";
            public const string DeleteInw_Recpt_Header = Base + "/deleteinwrecptheader/{Id}";
            public const string DeleteInw_Recpt_Part_No = Base + "/deleteinwrecptpartno/{Id}";
            public const string DeleteInwardDoc = Base + "/deleteinwarddoc/{Id}/{tenantId}";
            public const string DeleteRcCaDoc = Base + "/deleterccadoc/{Id}/{tenantId}";
            public const string DeleteInspectDoc = Base + "/deleteinspectdoc/{Id}/{tenantId}";
            public const string DeletelineinspectDoc = Base + "/deletelineinspectdoc/{Id}/{tenantId}";
            public const string DeleteCont_RCA_CA_Log = Base + "/deletecontrcacalog/{Id}/{tenantId}";
            public const string DeleteFinalInspectDoc = Base + "/deletefinalinspectdoc/{Id}/{tenantId}";
            public const string GetAllInwardDocList = Base + "/allinwarddoc/{tenantId}";
            public const string GetAllRcCaDocList = Base + "/allrccadoc/{tenantId}";
            public const string GetAllInspectDocList = Base + "/allinspectdoc/{tenantId}";
            public const string GetAllNcLogStatusList = Base + "/allnclogstatus";
            public const string GetAllFinalInspectDocList = Base + "/allfinalinspdoc/{tenantId}";
            public const string GetAllLineInspectDocList = Base + "/alllineinspdoc/{tenantId}";
            public const string PostInwardDocList = Base + "/postinwarddoc";
            public const string PostInspectDocList = Base + "/postinspectdoc";
            public const string PostRcCaDocList = Base + "/postrccadoc";
            public const string PostLineInspectDocList = Base + "/postlineinspdoc";
            public const string PostFinalInspectDocList = Base + "/postfinalinspdoc";
            public const string PostOperationSettings = Base + "/postoperationsetting";
            public const string PostCont_RCA_CA_Log = Base + "/postcontrcacalog";
            public const string GetAllOperationSettings = Base + "/getoperationsettingss";
            public const string GetAllCont_RCA_CA_Log = Base + "/getcontrcacalog/{tenantId}";
            public const string GetAllNC_Decision_Log = Base + "/getncdeclog/{tenantId}";
            public const string PostNC_Decision_Log = Base + "/postncdeclog";
            public const string DeleteNC_Decision_Log = Base + "/deletencdeclog/{Id}/{tenantId}";
            public const string GetAllNC_Wk_List_Tmpl_Det = Base + "/getncwklisttmpldet/{tenantId}";
            public const string PostNC_Wk_List_Tmpl_Det = Base + "/postncwklisttmpldet";
            public const string DeleteNC_Wk_List_Tmpl_Det = Base + "/deletencwklisttmpldet/{Id}/{tenantId}";
            public const string GetAllNC_Disp_Decs_Appl_List = Base + "/getncdispappllist/{tenantId}";
            public const string PostNC_Disp_Decs_Appl_List = Base + "/postncdispappllist";
            public const string DeleteNC_Disp_Decs_Appl_List = Base + "/deletencdispappllist/{Id}/{tenantId}";
            public const string GetAllNC_Wk_List_Tmpl_Head = Base + "/getncwklsthead/{tenantId}";
            public const string PostNC_Wk_List_Tmpl_Head = Base + "/postncwklsthead";
            public const string DeleteNC_Wk_List_Tmpl_Head = Base + "/deletencwklsthead/{Id}/{tenantId}";
            public const string GetAllNC_Work_List = Base + "/getncwklist/{tenantId}";
            public const string PostNC_Work_List = Base + "/postncwklist";
            public const string DeleteNC_Work_List = Base + "/deletencwklist/{Id}/{tenantId}";
            public const string GetAllNC_Wk_List_Header = Base + "/getncwklistheader/{tenantId}";
            public const string PostNC_Wk_List_Header = Base + "/postncwklistheader";
            public const string DeleteNC_Wk_List_Header = Base + "/deletencwklistheader/{Id}/{tenantId}";
            public const string GetAllCust_NC_Decs_Matrix_Opt = Base + "/getcustncdecopt/{tenantId}";
            public const string PostCust_NC_Decs_Matrix_Opt = Base + "/postcustncdecopt";
            public const string DeleteCust_NC_Decs_Matrix_Opt = Base + "/deletecustncdecopt/{Id}/{tenantId}";
            public const string GetAllCust_NC_Decs_Matrix = Base + "/getcustncdec/{tenantId}";
            public const string PostCust_NC_Decs_Matrix = Base + "/postcustncdec";
            public const string DeleteCust_NC_Decs_Matrix = Base + "/deletecustncdec/{Id}/{tenantId}";
            public const string GetAllCont_RCA_CA_Status_List = Base + "/getcontcastatuts";
            public const string PostCont_RCA_CA_Status_List = Base + "/postcontrcastatus";
            public const string DeleteCont_RCA_CA_Status_List = Base + "/deletecontrcastatus/{Id}";
            public const string GetAllNC_Disp_Decision_List = Base + "/getncdisplist";
            public const string PostNC_Disp_Decision_List = Base + "/postncdisplist";
            public const string DeleteNC_Disp_Decision_List = Base + "/deletencdisplist/{Id}";
            public const string GetAllNC_work_Status = Base + "/getncwkstatus";
            public const string PostNC_work_Status = Base + "/postncwkstatus";
            public const string DeleteNC_work_Status = Base + "/deletencwkstatus/{Id}";
        }
    }
}
