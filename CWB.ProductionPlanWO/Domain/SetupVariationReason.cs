using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class SetupVariationReason:BaseEntity
    {
        public string SetupType { get; set; }
        public string Reason { get; set; }
        public long TenantId { get; set; }
    }
}
