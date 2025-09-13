using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.CompanySettings.ViewModels.Location
{
    public class Employee_PwdVM
    {
        public long Employee_PwdId { get; set; }
        public long Employee_Id { get; set; }
        public DateTime Date_Changed { get; set; }
        public long TenantId { get; set; }
    }
}
