using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class ProcPlanPartPurChaseRel : BaseEntity
    {
        public long ProcPlanId { get; set; }
        public long PartPurchaseId { get; set; }
        public string LeadTime { get; set; }
        public int Active { get; set; }
    }
}
