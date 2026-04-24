using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.BusinessProcesses
{
    public class ConsolidatedWoMappingVM
    {
        public long ConsolidatedWoId { get; set; }
        public long CombinedWoId { get; set; }
        public long WoId { get; set; }
        public long ParentWoId { get; set; }
        public long TenantId { get; set; }
    }
}
