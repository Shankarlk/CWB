using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class DispatchDetailsVM
    {
        public long DispatchDetailsId { get; set; }
        public long SaleOrderId { get; set; }
        public long CustomerId { get; set; }
        public long PoNoId { get; set; }
        public long PartNoId { get; set; }
        public long NoOfParts { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string DispatchDetail { get; set; }
        public long TenantId { get; set; }
    }
}
