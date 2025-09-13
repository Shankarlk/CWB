using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Domain
{
    public class Employee_UI_List : BaseEntity
    {
        public long Ui_Id { get; set; }
        public long Access_Level { get; set; }
        public long Employee_Id { get; set; }
        public char Active { get; set; }
        public DateTime Add_date { get; set; }
        public DateTime Deact_date { get; set; }
        public long TenantId { get; set; }
    }
}
