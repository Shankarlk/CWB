using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class FinalInspectDocTypeVM
    {
        public long FinalInspectDocTypeId { get; set; }
        public long DocumentTypeId { get; set; }
        public char Mandatory { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long TenantId { get; set; }
        public string DocumentTypeName { get; set; } = string.Empty;
    }
}
