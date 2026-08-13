using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class Mc_Wait_ListVM 
    {
        public long Mc_Wait_ListId { get; set; }
        public long Wo_Id { get; set; }
        public long Opr_No_Id { get; set; }
        public long StopingId { get; set; }
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
        public long QntyOffered { get; set; }
        public long Accepted { get; set; }
        public long NonConQnty { get; set; }
        public string McName { get; set; }
        public string McTypeName { get; set; }
        public string ModeName { get; set; }
        public string Allocation { get; set; }
        public string PlanStartStr { get; set; }
        public long Setup_start_time_entry { get; set; }
        public char Setup_FTR { get; set; }
        public string Setup_comments { get; set; }
        public string Setup_Appvl_Doc_Ref { get; set; }
        public long OperatorId { get; set; }
        public long ReasonfornotachievingFTRId { get; set; }
        public long ReasonforAddnSetupTimeId { get; set; }
        public string SetupTimeTaken { get; set; }
        public string AddntimeforSetup { get; set; }
        public string PlanEndStr { get; set; }
        //additonal view feilds
        public string ShopName { get; set; }
        public string SectionName { get; set; }
        public long PartInQueue { get; set; }
        public string HrsBooked { get; set; }
        public string McNotAvlHrs { get; set; }
        public string FreeHrs { get; set; }
        public string SimulationDurationHrs { get; set; }
        public string PercentHrsUsed { get; set; }
        public string DateMcNotLoaded { get; set; }
        public string PartNo { get; set; }
        public string ReworkHrs { get; set; }
        public int IsReworkCount { get; set; }
        public int IsNonPlanCount { get; set; }
        public string NonPlanHrs { get; set; }
        public string WoNumber { get; set; }
        public string DataChanged { get; set; }
        public string WoQnty { get; set; }
        public string RoutingName { get; set; }
        public string OprNoName { get; set; }
        public string CsStartDate { get; set; }
        public string ActStartDate { get; set; }
        public string MatlIssued { get; set; }
        public string CsEndDate { get; set; }
        public string WaitTime { get; set; }
    }
}
