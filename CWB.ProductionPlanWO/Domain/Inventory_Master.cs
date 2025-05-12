using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class Inventory_Master:BaseEntity
    {
        public DateTime Dt_time { get; set; }
        public long Part_NoId { get; set; }
        public long Routing_Id { get; set; }
        public long Opr_No_Id { get; set; }
        public decimal Current_QntOnHand { get; set; }
        public long Location_Id { get; set; }
        public long Inv_Trans_Log_Id { get; set; }
        public long TenantId { get; set; }
    }
}
