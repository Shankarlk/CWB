using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class ProcPlanPartPurChaseRelVM
    {
        public long ProcPlanPartPurChaseRelId { get; set; }
        public long ProcPlanId { get; set; }
        public long PartPurchaseId { get; set; }
        public string LeadTime { get; set; }
        public int Active { get; set; }
    }
}
