using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class TempMc_Wait_ListVM 
    {
        public long TempMc_Wait_ListId { get; set; }
        public long ActiveId { get; set; }
        public long Wo_Id { get; set; }
        public long StopingId { get; set; }
        public long Opr_No_Id { get; set; }
        public long Mc_Id { get; set; }
        public long Mode { get; set; }
        public long Wait_Seq_No { get; set; }
        public long Act_Qnty { get; set; }
        public long Plan_Qnty { get; set; }
        public long Bal_Qnty { get; set; }
        public char Rework_Wo { get; set; }
        public char Non_Plan_Wk { get; set; }
        public long Non_Plan_wk_Id { get; set; }
        public long Plan_start_time_Id { get; set; }
        public long Plan_end_time_Id { get; set; }
        public long Next_Opr_Start_time_Id { get; set; }
        public DateTime? Setup_Start_time { get; set; }
        public DateTime? Setup_Apprvl_time { get; set; }
        public DateTime? Act_End_time { get; set; }
        public decimal Mc_TPT { get; set; }
        public long TenantId { get; set; }

        public string ShopName { get; set; }
        public string McName { get; set; }
        public string WoNumber { get; set; }
        public string PartNo { get; set; }
        public string RoutingName { get; set; }
        public string OprNoName { get; set; }
        public string WoQnty { get; set; }
        public string MatlIssued { get; set; }
        public string PlanStartStr { get; set; }
        public string SetUpTimeStr { get; set; }
        public string MatlReceptTime { get; set; }
        public string PlannedSetupTime { get; set; }
        public long QntyOffered { get; internal set; }
        public long Accepted { get; internal set; }
        public long NonConQnty { get; internal set; }
        public string UomName { get; internal set; }
    }
}
