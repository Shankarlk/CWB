using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class Insp_Outcome_List:BaseEntity
    {
        public string Insp_Outcome_desc { get; set; }
        public string Applicability { get; set; }
        public long TenantId { get; set; }
    }
}
