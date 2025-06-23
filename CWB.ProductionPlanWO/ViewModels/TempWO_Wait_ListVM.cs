using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class TempWO_Wait_ListVM
    {
        public long TempWO_Wait_ListId { get; set; }
        public long ActiveId { get; set; }
        public long Wo_Id { get; set; }
        public long Mode { get; set; }
        public char Allow_Routing_Chg { get; set; }
        public int Total_TPT { get; set; }
        public char Rework_Wo { get; set; }
        public long NC_Log_Ref { get; set; }
        public long WO_Wait_Seq_No { get; set; }
        public DateTime Plan_Start_Date { get; set; }
        public DateTime Plan_End_Date { get; set; }
        public long StopingId { get; set; }
        public int Plan_Simul_Qnty { get; set; }
        public int NoOfSimulation { get; set; }
        public long TenantId { get; set; }
    }
}
