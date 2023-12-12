using System;

namespace CWB.Masters.ViewModels.ItemMaster
{
    public class RawMaterialDetailVM
    {
        //DbPart - Start
        public long PartId { get; set; }
        public long SupplierId { get; set; }
        public long RawMaterialMadeType { get; set; }
        public int RawMaterialMadeSubType { get; set; }
        public long RawMaterialTypeId { get; set; }
        public long BaseRawMaterialId { get; set; }
        public string RawMaterialWeight { get; set; }
        public string? RawMaterialNotes { get; set; }
        public long Standard { get; set; }
        public long MaterialSpecId { get; set; }
        //DbPart - End

        
        public string PartNo { get; set; }
        public string PartDescription { get; set; }
        public string Status { get; set; }
        public string StatusChangeReason { get; set; }
        public string RevNo { get; set; }
        public DateTime? RevDate { get; set; }
        public string MasterPartType { get; set; }
        public long? RawMaterialDetailId { get; set; }
        public string? AdditionalInfo { get; set; }
        public string Supplier { get; set; }
       
        //     public long? PurchaseDetailId { get; set; }
        public long TenantId { get; set; }
    }
}
