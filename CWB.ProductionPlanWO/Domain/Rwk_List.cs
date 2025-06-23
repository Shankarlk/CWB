using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class Rwk_List:BaseEntity
    {
        public long NC_Log_Id { get; set; }
        public long Mc_Id { get; set; }
        public char Allocated { get; set; }
        public string Plan_Duration { get; set; }
        public DateTime RequiredStartTime { get; set; }
        public DateTime Actual_Start_Time { get; set; }
        public DateTime Actual_end_Time { get; set; }
        public string Actual_Duration { get; set; }
        public string Closure_Comment { get; set; }
        public long TenantId { get; set; }
    }
}
