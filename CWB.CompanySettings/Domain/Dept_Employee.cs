using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Domain
{
    public class Dept_Employee : BaseEntity
    {
        public long Dept_Posn { get; set; }
        public long Employee_Id { get; set; }
        public char Active { get; set; }
        public DateTime Add_date { get; set; }
        public DateTime Deact_date { get; set; }
        public long TenantId { get; set; }
    }
}
