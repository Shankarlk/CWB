using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class ConsolidatedWoMapping:BaseEntity
    {
        public long CombinedWoId { get; set; }
        public long WoId { get; set; }
        public long ParentWoId { get; set; }
        public long TenantId { get; set; }
    }
}
