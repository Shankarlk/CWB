using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;

namespace CWB.Masters.Domain
{
    public class BoughtOutFinishDetail : BaseEntity
    {
        public long BoughtOutFinishMadeType { get; set; }
        public string PartNo { get; set; }
        public string SupplierPartNo { get; set; }
        public string PartDescription { get; set; }
        public string AdditionalInformation { get; set; }
        public long Status { get; set; }
        public string StatusChangeReason { get; set; }
        public string RevNo { get; set; }
        public DateTime RevDate { get; set; }
        public string PurchaseDetail { get; set; }
        public string Supplier { get; set; }
        public string PurSupplierPartNo { get; set; }
        public long LeadTimeInDays { get; set; }
        public long MinOrderQuantity { get; set; }
        public decimal Price { get; set; }
        public string ShareOfBusiness { get; set; }
        public string PurAdditionalInformation { get; set; }
        public long TenantId { get; set; }


    }
}
