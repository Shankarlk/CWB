using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class RcCaDocTypeVM
    {
        public long RcCaDocTypeId { get; set; }
        public long DocumentTypeId { get; set; }
        public char Mandatory { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long TenantId { get; set; }
    }
}
