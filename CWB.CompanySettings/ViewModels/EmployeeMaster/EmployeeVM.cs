using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.CompanySettings.ViewModels.EmployeeMaster
{
    public class EmployeeVM
    {
        public long Employee_ID { get; set; }
        public string Employee_name { get; set; }
        public long Designation_Id { get; set; }
        public string Employee_No { get; set; }
        public DateTime Date_Of_Joining { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Residential_Address { get; set; }
        public string Emerg_Contact_Name { get; set; }
        public string Emerg_Contact_No { get; set; }
        public string HeadOfDepartment { get; set; }
        public string RoleIds { get; set; }
        public long RoleReportTo { get; set; }
        public long ChangedPassword { get; set; }
        public long Plant_Id { get; set; }
        public long Home_Dept_Id { get; set; }
        public char Employee_Resigned { get; set; }
        public DateTime? Date_Of_Resigning { get; set; }
        public long TenantId { get; set; }
    }
}
