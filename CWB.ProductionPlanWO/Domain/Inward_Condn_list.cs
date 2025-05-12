using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class Inward_Condn_list:BaseEntity
    {
        public string Inward_Condn_desc { get; set; }
        public string Applicability { get; set; }
        public long TenantId { get; set; }
    }
}
