using CWB.CommonUtils.Common;
using System;

namespace CWB.Masters.Domain
{
    public class RawMaterialDetail : BaseEntity
    {
        public long RawMaterialMadeType { get; set; }
        public long RawMaterialTypeId { get; set; }
        public long InHousePartNo { get; set; }
        public string PartDescription { get; set; }
        public long BaseRawMaterialId { get; set; }
        public long RawMaterialWeight { get; set; }
        public string RawMaterialNotes { get; set; }
        public long Status { get; set; }
        public string StatusChangeReason { get; set; }
        public string RevNo { get; set; }
        public DateTime RevDate { get; set; }
        public long Standard { get; set; }
        public string MaterialSpec { get; set; }
        public long PurchaseDetailId { get; set; }
        public long TenantId { get; set; }
    }
}
