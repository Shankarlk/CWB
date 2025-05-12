using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class Cont_RCA_CA_LogVM
    {
        public long Cont_RCA_CA_LogId { get; set; }
        public long NcLogId { get; set; }
        public string Containment_Action { get; set; }
        public string Senior_Feedback { get; set; }
        public long Cont_RCA_CA_Status_Id { get; set; }
        public long TenantId { get; set; }
    }
}
