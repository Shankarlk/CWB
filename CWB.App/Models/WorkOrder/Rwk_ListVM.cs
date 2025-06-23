using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class Rwk_ListVM
    {
        public long Rwk_ListId { get; set; }
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
        public long PartType { get; set; }
        public string WoNumber { get; set; }
        public string PartNo { get; set; }
        public string Planndt { get; set; }
        public string RecdComplDt { get; set; }
        public string MachineName { get; set; }
        public string ShopName { get; set; }
        public string InProcess { get; set; }
        public string RoutingName { get; set; }
        public int RwkQnty { get; set; }
    }
}
