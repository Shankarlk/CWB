﻿using System;

namespace CWB.Masters.ViewModels.ItemMaster
{
    public class RawMaterialDetailVM
    {
        //DbPart - Start
        public long PartId { get; set; }
        public long UOMId { get; set; }
        public long SupplierId { get; set; }
        public long RawMaterialMadeType { get; set; }
        public long RawMaterialMadeSubType { get; set; }
        public long RawMaterialTypeId { get; set; }
        public long BaseRawMaterialId { get; set; }
        public string RawMaterialWeight { get; set; }
        public string RawMaterialNotes { get; set; } = string.Empty;
        public long Standard { get; set; }
        public long MaterialSpecId { get; set; }
        public string ReorderLevel { get; set; }
        public string ReorderQnty { get; set; }
        //DbPart - End


        public string PartNo { get; set; }
        public string PartDescription { get; set; }
        public string Status { get; set; }
        public string StatusChangeReason { get; set; }
        public string RevNo { get; set; }
        public DateTime? RevDate { get; set; }
        public string MasterPartType { get; set; }
        public long? RawMaterialDetailId { get; set; }
        public string AdditionalInfo { get; set; } = string.Empty;
        public string Supplier { get; set; }
        public string RawMaterialType { get; set; } = string.Empty;
        public string BaseRawMaterial { get; set; } = string.Empty;

        //     public long? PurchaseDetailId { get; set; }
        public long TenantId { get; set; }
    }
}
