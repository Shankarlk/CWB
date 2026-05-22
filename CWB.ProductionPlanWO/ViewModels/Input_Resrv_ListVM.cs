using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class Input_Resrv_ListVM
    {
        public long Input_Resrv_List_ID { get; set; }
        public long PO_NO_ID { get; set; }
        public long WO_Id { get; set; }
        public long PartId { get; set; }
        public decimal Plan_Alloc_Qnty { get; set; }
        public char Allocation_done { get; set; }
        public decimal Bal_to_Issue { get; set; }

        public decimal Qnty_Recd { get; set; }
        public long TenantId { get; set; }
    }
}
