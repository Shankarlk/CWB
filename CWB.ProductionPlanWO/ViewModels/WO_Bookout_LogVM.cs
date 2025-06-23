using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class WO_Bookout_LogVM
    {
        public long WO_Bookout_LogId { get; set; }
        public long Wo_Id { get; set; }
        public long Opr_No { get; set; }    
        public long Event_date_time { get; set; }
        public int Bookout_Qnty { get; set; }
        public long TenantId { get; set; }
    }
}
