using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class Mc_Not_Avl_Reason : BaseEntity
    {
        public string Reason_Desc { get; set; }
        public long TenantId { get; set; }
    }   
}
