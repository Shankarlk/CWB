using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class OperationSettingsVM
    {
        public long OperationSettingsId { get; set; }
        public string UiName { get; set; }
        public char EnableDisable { get; set; }
        public long TenantId { get; set; }
    }
}
