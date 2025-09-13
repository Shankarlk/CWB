using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.EmployeeMaster
{
    public class Role_UI_ListVM
    {
        public long Role_Ui_ListId { get; set; }
        public long RoleId { get; set; }
        public long Ui_Id { get; set; }
        public long PermissionId { get; set; }
        public char Active { get; set; }
        public DateTime Add_date { get; set; }
        public DateTime Deact_date { get; set; }
        public long EmployeeId { get; set; }
        public string Comment { get; set; }
        public long TenantId { get; set; }
        public long DepartmentId { get; set; }
        public string RoleName { get; set; }= string.Empty;
        public string WorkDone { get; set; }= string.Empty;
        public string FromDept { get; set; }= string.Empty;
        public string UiLevel { get; set; }= string.Empty;
        public string Approval_Allowed { get; set; }= string.Empty;
        public string View_Allowed { get; set; }= string.Empty;
        public string Add_Edit_Allowed { get; set; }= string.Empty;
        public string Delete_Allowed { get; set; } = string.Empty;
        public string Menu1 { get; set; } = string.Empty;
        public string Menu2 { get; set; } = string.Empty;
        public string Menu3 { get; set; } = string.Empty;
        public string Menu4 { get; set; } = string.Empty;
        public string Menu5 { get; set; } = string.Empty;
        public string Permission { get; set; } = string.Empty;
        public string Add_dateStr { get; set; } = string.Empty;
        public string Deact_dateStr { get; set; } = string.Empty;
    }
}
