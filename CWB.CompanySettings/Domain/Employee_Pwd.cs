using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Domain
{
    public class Employee_Pwd : BaseEntity
    {
        public long Employee_Id { get; set; }
        public DateTime Date_Changed { get; set; }
        public long TenantId { get; set; }
    }
}
