using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class DispatchQntyVM
    {
        public long DispatchQntyId { get; set; }
        public long SaleOrderId { get; set; }
        public long CustomerId { get; set; }
        public long PoNoId { get; set; }
        public long PartNoId { get; set; }
        public long InventoryMasterId { get; set; }
        public long TotalSoQnty { get; set; }
        public long SuggestedQnty { get; set; }
        public long FinalDispQnty { get; set; }
        public long TenantId { get; set; }
    }
}
