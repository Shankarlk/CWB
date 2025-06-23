using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class Mc_Wait_List:BaseEntity
    {
        public long Wo_Id { get; set; }
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
        public long StopingId { get; set; }
        public long Setup_start_time_entry { get; set; }
        public char Setup_FTR { get; set; }
        public string Setup_comments { get; set; }
        public string Setup_Appvl_Doc_Ref { get; set; }
        public long QntyOffered { get; set; }
        public long Accepted { get; set; }
        public long NonConQnty { get; set; }
        public long TenantId { get; set; }
    }
}
