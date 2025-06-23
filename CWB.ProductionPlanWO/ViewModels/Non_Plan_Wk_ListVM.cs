using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class Non_Plan_Wk_ListVM
    {
        public long Non_Plan_Wk_ListId { get; set; }
        public long Mc_Id { get; set; }
        public long NP_Work_Type { get; set; }
        public char Allocated { get; set; }
        public DateTime Plan_start_time { get; set; }
        public string Plan_Duration { get; set; }
        public long Non_Plan_Wk_Type { get; set; }
        public DateTime Actual_Start_Time { get; set; }
        public DateTime Actual_end_Time { get; set; }
        public DateTime PlannedDate { get; set; }
        public string Actual_Duration { get; set; }
        public string Work_Description { get; set; }
        public string Closure_Comment { get; set; }
        public long TenantId { get; set; }
    }
}
