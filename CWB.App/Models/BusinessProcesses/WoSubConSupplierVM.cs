using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.BusinessProcesses
{
    public class WoSubConSupplierVM
    {
        public long WoSubConSupplierId { get; set; }
        public long WoId { get; set; }
        public long ProcPlanId { get; set; }
        public long SupplierId { get; set; }
        public string Qnty { get; set; }
        public string DeliveryDate { get; set; }
        public string ProcPriceQnty { get; set; }
        public string ProcPrice { get; set; }
        public string AddnInfo { get; set; }
        public DateTime RecieptDate { get; set; }
        public long TenantId { get; set; }
        public string Company { get; set; } = string.Empty;
        public string AgreedDate { get; set; } = string.Empty;
    }
}
