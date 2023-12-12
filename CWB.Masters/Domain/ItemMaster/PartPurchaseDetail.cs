using CWB.CommonUtils.Common;

namespace CWB.Masters.Domain.ItemMaster
{
    public class PartPurchaseDetail : BaseEntity
    {
        public long SupplierId { get; set; }
        public Company Company { get; set; }
        public string SupplierPartNo { get; set; }
        public int LeadTimeInDays { get; set; }
        public int MinimumOrderQuantity { get; set; }
        public string Price { get; set; }
        public string ShareOfBusiness { get; set; }
        public string AdditionalInformation { get; set; }
        public long MasterPartId { get; set; }
        public MasterPart MasterPart { get; set; }
        public long TenantId { get; set; }

    }
}
