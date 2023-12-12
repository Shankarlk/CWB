using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;

namespace CWB.App.Models.ItemMaster
{
    public class BoughtOutFinishDetailVM
    {
        public long? BoughtOutFinishDetailId { get; set; }
        public long BoughtOutFinishMadeType { get; set; }
        public string PartNo { get; set; } = null!;
        public string SupplierPartNo { get; set; } = null!;
        public string PartDescription { get; set; }
        public string AdditionalInformation { get; set; } = null!;
        public long? Status { get; set; }
        public string StatusChangeReason { get; set; } = null!;
        public string RevNo { get; set; } = null!;
        public DateTime? RevDate { get; set; }
        public string PurchaseDetail { get; set; } = null!;
        public string Supplier { get; set; }
        public string PurSupplierPartNo { get; set; } = null!;
        public long? LeadTimeInDays { get; set; }
        public long? MinOrderQuantity { get; set; }
        public decimal? Price { get; set; }
        public string ShareOfBusiness { get; set; } = null!;
        public string PurAdditionalInformation { get; set; } = null!;
        public long TenantId { get; set; }


    }
}
