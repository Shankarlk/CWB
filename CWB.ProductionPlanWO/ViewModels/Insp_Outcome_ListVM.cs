using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class Insp_Outcome_ListVM
    {
        public long Insp_Outcome_ListId { get; set; }
        public string Insp_Outcome_desc { get; set; }
        public string Applicability { get; set; }
        public long TenantId { get; set; }
    }
}
