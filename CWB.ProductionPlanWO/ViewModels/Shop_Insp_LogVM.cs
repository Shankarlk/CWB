using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class Shop_Insp_LogVM
    {
        public long Shop_Insp_LogId { get; set; }
        public long Mc_Reference { get; set; }
        public long Operator_Id { get; set; }
        public long Inspected_by { get; set; }
        public DateTime Inspected_on { get; set; }
        public int Qnty_OK_finished { get; set; }
        public int Qnty_ok_input { get; set; }
        public long Input_Part_No { get; set; }
        public long Input_Routing_Id { get; set; }
        public long Input_Opr_NoId { get; set; }
        public string Label_Type { get; set; }
        public long TenantId { get; set; }
    }
}
