using System;

namespace CWB.Masters.ViewModels.ItemMaster
{
    public class ItemMasterPartVM
    {
        public long? PartId { get; set; }
        public string MasterPartType { get; set; }
        public int CompanyId { get; set; }
        public string Company { get; set; }
        public string PartNo { get; set; }
        public string Description { get; set; }
        public int BoughtOutFinishMadeType { get; set; }
        public int RawMaterialMadeSubType { get; set; }
        public int RawMaterialTypeId { get; set; }
        public int BaseRawMaterialId { get; set; }
        public string BOFSupplierPartNo { get; set; }
        public string RMSupplier { get; set; }
        public string Supplier { get; set; }
        public string SupplierPartNo { get; set; }
        public string Status { get; set; }
        public int RMId { get; set; }
        public int BOFId { get; set; }

        public string Notes { get; set; }
        public int Type { get; set; }
        public long TenantId { get; set; }
        public char Inv_Trans { get; set; }
        public char Linked_to_BOM { get; set; }
        public string FinalPart { get; set; }
        public string MandocAvl { get; set; }
        public string DocStatus { get; set; }
        public string RmAvl { get; set; }
        public string SupplierAvl { get; set; }
        public string BomAvl { get; set; }
        public string MasterDisplay { get; set; }
        public string ListAssembly { get; set; }
    }
}
