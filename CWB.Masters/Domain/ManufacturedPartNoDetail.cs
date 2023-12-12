using CWB.CommonUtils.Common;
using System;

namespace CWB.Masters.Domain
{
    public class ManufacturedPartNoDetail : BaseEntity
    {
        public long? ManufacturedPartType { get; set; }
        public string CompanyName { get; set; }
        public string PartNumber { get; set; }
        public string PartDescription { get; set; }
        public long? FinishedWeight { get; set; }
        public string RevNo { get; set; }
        public DateTime? RevDate { get; set; }
        public long UOMId { get; set; }
        public long Status { get; set; }
        public string StatusChangeReason { get; set; }
        public string MakeFrom { get; set; }
        public string BOM { get; set; }
        public long TenantId { get; set; }


    }
}
