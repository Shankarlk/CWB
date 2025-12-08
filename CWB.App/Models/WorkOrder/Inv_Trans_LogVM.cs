using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class Inv_Trans_LogVM
    {
        public long Inv_Trans_LogId { get; set; }
        public DateTime Dt_time { get; set; }
        public long PersonId { get; set; }
        public long Input_Part_NoId { get; set; }
        public long Input_Routing_Id { get; set; }
        public long Input_Opr_No { get; set; }
        public long Output_Part_No { get; set; }
        public long Output_Routing_Id { get; set; }
        public long Output_Opr_No { get; set; }
        public long Wo_Id { get; set; }
        public long PO_No_Id { get; set; }
        public long Transaction_Id { get; set; }
        public decimal Qnty { get; set; }
        public long From_Location_Id { get; set; }
        public long To_Location_Id { get; set; }
        public long Part_Status { get; set; }
        public char Ready_for_issue { get; set; }
        public char Movement_Started { get; set; }
        public char Movement_Compl { get; set; }
        public decimal Qnty_Mismatch { get; set; }
        public long Qnty_mismatch_status { get; set; }
        public string Qnty_Mismatch_Comment { get; set; }
        public string Qnty_Mismatch_Resolution { get; set; }
        public long NC_Log_Id { get; set; }
        public long Our_DC_Ref { get; set; }
        public long Our_RGP_Ref { get; set; }
        public long TenantId { get; set; }
        public string PartNo { get; set; } = string.Empty;
        public long QntyStr { get; set; }
        public string Dt_timeStr { get; set; } = string.Empty;
        public string WoNumber { get; set; } = string.Empty;
        public string InputPartNo { get; set; } = string.Empty;
        public string OutPutPartNo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MasterPartType { get; set; } = string.Empty;
        public string ToSender { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FromSender { get; set; } = string.Empty;
        public string PartStatus { get; set; } = string.Empty;
        public string UpdateResp { get; set; } = string.Empty;
        public string MisMatchStatus { get; set; } = string.Empty;
        public string OprNo { get; set; } = string.Empty;
        public string OutputOprNo { get; set; } = string.Empty;
        public string InputOprNo { get; set; } = string.Empty;
        public string RoutingName { get; set; } = string.Empty;
        public string OutPutRoutingName { get; set; } = string.Empty;
        public string InPutRoutingName { get; set; } = string.Empty;
        public string OkQnty { get; set; } = string.Empty;
        public string NcAvl { get; set; } = string.Empty;
        public string RwkQnty { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string WfInwdInsp { get; set; } = string.Empty;
        public string TransactionName { get; set; } = string.Empty;
    }
}
