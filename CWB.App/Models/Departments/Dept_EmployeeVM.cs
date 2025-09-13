using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.Departments
{
    public class Dept_EmployeeVM
    {
        public long Dept_EmployeeId { get; set; }
        public long Dept_Posn { get; set; }
        public long Employee_Id { get; set; }
        public char Active { get; set; }
        public DateTime Add_date { get; set; }
        public DateTime Deact_date { get; set; }
        public long TenantId { get; set; }
        public string Add_dateStr { get; set; } = string.Empty;
        public string Deact_dateStr { get; set; } = string.Empty;
        public string Level1 { get; set; } = string.Empty;
        public string Level2 { get; set; } = string.Empty;
        public string Level3 { get; set; } = string.Empty;
        public string Level4 { get; set; } = string.Empty;
        public string Level5 { get; set; } = string.Empty;
    }
}
