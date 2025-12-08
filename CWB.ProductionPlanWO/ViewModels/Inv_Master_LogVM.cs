using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class Inv_Master_LogVM
    {
        public long Inv_Master_LogId { get; set; }
        public long Inv_mast_ID { get; set; }
        public DateTime Dt_time { get; set; }
        public long Part_No { get; set; }
        public long Changed_by { get; set; }
        public string Field_Changed { get; set; }
        public string Old_Value { get; set; }
        public string New_Value { get; set; }
        public string Reason_Desc { get; set; }
        public long Financial_Impact { get; set; }
        public long TenantId { get; set; }
    }
}
