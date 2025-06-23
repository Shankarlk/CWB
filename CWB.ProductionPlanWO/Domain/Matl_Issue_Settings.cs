using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class Matl_Issue_Settings : BaseEntity
    {
        public long Shop_Id { get; set; }
        public DateTime LastIssueTime { get; set; }
        public long IssueDay { get; set; }
        public int No_days_coverage { get; set; }
        public long TenantId { get; set; }
    }
}
