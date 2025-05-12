using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class Cont_RCA_CA_Log:BaseEntity
    {
        public long NcLogId { get; set; }
        public string Containment_Action { get; set; }
        public string Senior_Feedback { get; set; }
        public long Cont_RCA_CA_Status_Id { get; set; }
        public long TenantId { get; set; }
    }
}
