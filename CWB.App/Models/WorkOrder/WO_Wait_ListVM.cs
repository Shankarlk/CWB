using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class WO_Wait_ListVM
    {
        public long WO_Wait_ListId { get; set; }
        public long Wo_Id { get; set; }
        public long Mode { get; set; }
        public char Allow_Routing_Chg { get; set; }
        public int Total_TPT { get; set; }
        public char Rework_Wo { get; set; }
        public long NC_Log_Ref { get; set; }
        public long WO_Wait_Seq_No { get; set; }
        public DateTime Plan_Start_Date { get; set; }
        public DateTime Plan_End_Date { get; set; }
        public int Plan_Simul_Qnty { get; set; }
        public long StopingId { get; set; }
        public long TenantId { get; set; }
        public string WoNumber { get; set; }
        public string Part { get; set; }
        public string PartType { get; set; }
        public string Customer { get; set; }
        public string ModeName { get; set; }
        public string Tpt { get; set; }
        public string PlanEndStr { get; set; }
        public string PlanStartStr { get; set; }
    }
}
