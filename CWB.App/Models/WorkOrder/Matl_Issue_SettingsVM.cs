using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class Matl_Issue_SettingsVM
    {
        public long Matl_Issue_SettingsId { get; set; }
        public long Shop_Id { get; set; }
        public int No_days_coverage { get; set; }
        public DateTime LastIssueTime { get; set; }
        public long IssueDay { get; set; }
        public long TenantId { get; set; }
        public string LastIssueTimeStr { get; set; }
        public string Shop { get; set; }
        public string NoOfShifts { get; set; }
        public string IssueDayStr { get; set; }
    }
}
