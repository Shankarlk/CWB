using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class Cust_NC_Decs_MatrixVM
    {
        public long Cust_NC_Decs_MatrixId { get; set; }
        public long Cust_Request_Id { get; set; }
        public string Cust_Feedback_Id { get; set; }
        public long TenantId { get; set; }
    }
}
