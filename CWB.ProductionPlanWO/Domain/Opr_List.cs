using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class Opr_List:BaseEntity
    {
        public long Wo_Id { get; set; }
        public long Opr_No { get; set; }
        public long RoutingId { get; set; }
        public long RoutingStepSequence { get; set; }
        public long RoutingStepLocation { get; set; }
        public long Mode { get; set; }
        public long Initial_Opr_TPT { get; set; }
        public long Rolledup_Opr_TPT { get; set; }
        public char Rework_Wo { get; set; }
        public long NC_Log_Ref { get; set; }
        public int Act_Qnty { get; set; }
        public int Plan_Qnty { get; set; }
        public long No_of_Simult_Mcs { get; set; }
        public long Shop_Plan_start_time { get; set; }
        public long Shop_Plan_end_time { get; set; }
        public long Subcon_plan_start_time { get; set; }
        public long Subcon_plan_end_time { get; set; }
        public DateTime Setup_Start_time { get; set; }
        public DateTime Act_End_time { get; set; }
        public long StopingId { get; set; }
        public long TenantId { get; set; }
    }
}
