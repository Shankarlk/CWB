using CWB.CommonUtils.Common;
using System;

namespace CWB.Masters.Domain
{
    public class MPBOM : BaseEntity
    {
        public string PartNumber { get; set; }
        public string PartDesc { get; set; }
        public long? Quantity { get; set; }       
        public long TenantId { get; set; }
    }
}
