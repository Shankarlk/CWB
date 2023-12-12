using System;

namespace CWB.Masters.ViewModels.ItemMaster
{
    public class ManufacturedPartNoDetailVM
    {
        public long? ManufacturedPartNoDetailId { get; set; }
        public long? MPMakeFromId { get; set; }
        public long? MPBOMId { get; set; }
        public long ManufacturedPartType { get; set; }
        public string CompanyName { get; set; }
        public string PartNumber { get; set; }
        public string PartDescription { get; set; }
        public long? FinishedWeight { get; set; }
        public string RevNo { get; set; }
        public DateTime? RevDate { get; set; }
        public long UOMId { get; set; }
        public long Status { get; set; }
        public string StatusChangeReason { get; set; }
        public long? PartMadeFrom { get; set; }
        public string InputPartNo { get; set; }
        public long? InputWeight { get; set; }
        public long? ScrapGenerated { get; set; }
        public long? QuantityPerInput { get; set; }
        public string YieldNotes { get; set; }
        public Boolean PreferedRawMaterial { get; set; }
        public long? Quantity { get; set; }
        public string MakeFrom { get; set; }
        public string BOM { get; set; }
        public long TenantId { get; set; }
    }
}
