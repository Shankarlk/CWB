using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class Cust_NC_Decs_Matrix_Opt: BaseEntity
    {
        public long Cust_NC_Decs_Matrix_Id { get; set; }
        public string NC_Disp_Decision_Id { get; set; }
        public long TenantId { get; set; }
    }
}
