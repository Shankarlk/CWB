using CWB.CommonUtils.Common;
using System;

namespace CWB.App.Models.ItemMaster
{
    public class PartPurchaseDetailsVM
    {
        //DbPart-Start
        public int PPartId { get; set; }
        public int PSupplierId { get; set; }
        public string PSupplierPartNo { get; set; }
        public int LeadTimeInDays { get; set; }
        public int MinimumOrderQuantity { get; set; }
        public string Price { get; set; }
        public string ShareOfBusiness { get; set; }
        public string PAdditionalInfo { get; set; }
        public int PreferredSupplier { get; set; }
        //....
        public int BOFId { get; set; }
        //OR
        public int RMId { get; set; }
        //....
        public long PartPurchaseId { get; set; }
        //DbPart-End
        public string PMasterPartType {  get; set; }
        public string PSupplier { get; set; }
        public string MasterDisplay { get; set; }
        public string Description { get; set; }
        public string PartNo { get; set; }

        public long? TenantId { get; set; }

        public long CompanyId { get; set; }
        public long DivisionId { get; set; }
        public string CompanyType { get; set; }
        public string CompanyName { get; set; }
        public string DivisionName { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public string PlantName { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string Country { get; set; }
        public string GstNo { get; set; }
        public string PanNo { get; set; }
    }
}
