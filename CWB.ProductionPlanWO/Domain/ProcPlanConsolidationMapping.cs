using CWB.CommonUtils.Common;
using System;

namespace CWB.ProductionPlanWO.Domain
{
    public class ProcPlanConsolidationMapping : BaseEntity
    {
        public long ConsolidatedProcPlanId { get; set; }
        public long ProcPlanId { get; set; }
        public long WorkOrderId { get; set; }
        public long TenantId { get; set; }
    }
}
