using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class Cust_NC_Decs_Matrix:BaseEntity
    {
        public long Cust_Request_Id { get; set; }
        public string Cust_Feedback_Id { get; set; }
        public long TenantId { get; set; }
    }
}
