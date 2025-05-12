using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class OperationSettings:BaseEntity
    {
        public string UiName { get; set; }
        public char EnableDisable { get; set; }
        public long TenantId { get; set; }
    }
}
