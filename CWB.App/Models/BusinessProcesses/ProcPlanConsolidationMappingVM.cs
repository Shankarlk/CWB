using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.BusinessProcesses
{
    public class ProcPlanConsolidationMappingVM
    {
        public long ConsolidationId { get; set; }

        public long ConsolidatedProcPlanId { get; set; }
        public long ProcPlanId { get; set; }
        public long WorkOrderId { get; set; }
        public long TenantId { get; set; }
    }
}
