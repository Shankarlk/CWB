using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class Inw_Recpt_HeaderVM
    {
        public long Inw_Recpt_HeaderId { get; set; }
        public long PoHeaderId { get; set; }
        public string Supplier_Dc_Ref { get; set; }
        public DateTime Supplier_Dc_Date { get; set; }
        public string Supplier_Inv_ref { get; set; }
        public DateTime Supplier_Inv_date { get; set; }
        public DateTime Inw_Date_time { get; set; }
        public long TenantId { get; set; }
    }
}
