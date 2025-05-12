using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class Inw_Recpt_DetailsVM
    {
        public long Inw_Recpt_DetailsId { get; set; }
        public long Inw_Recpt_Header_Id { get; set; }
        public long Inw_Recpt_Part_No_Id { get; set; }
        public decimal Our_count { get; set; }
        public decimal Vendor_DC_count { get; set; }
        public long Inward_Condition { get; set; }
        public string Comment { get; set; }
        public long TenantId { get; set; }
    }
}
