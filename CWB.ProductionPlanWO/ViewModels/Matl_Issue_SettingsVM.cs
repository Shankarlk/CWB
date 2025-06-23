using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class Matl_Issue_SettingsVM
    {
        public long Matl_Issue_SettingsId { get; set; }
        public long Shop_Id { get; set; }
        public DateTime LastIssueTime { get; set; }
        public long IssueDay { get; set; }
        public int No_days_coverage { get; set; }
        public long TenantId { get; set; }
    }
}
