using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class SetupVariationReasonVM
    {
        public long SetupVariationReasonId { get; set; }
        public string SetupType { get; set; }
        public string Reason { get; set; }
        public long TenantId { get; set; }
    }
}
