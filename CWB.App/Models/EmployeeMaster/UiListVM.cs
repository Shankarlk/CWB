using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.EmployeeMaster
{
    public class UiListVM
    {
        public long UiListId { get; set; }
        public long MenuLevelId { get; set; }
        public char TopLevelId { get; set; }
        public string UI_Type { get; set; }
        public string UI_Name_Label { get; set; }
        public long UI_Part_linked_to { get; set; }
        public char Approval_Allowed { get; set; }
        public char View_Allowed { get; set; }
        public char Add_Edit_Allowed { get; set; }
        public char Delete_Allowed { get; set; }
        public long TenantId { get; set; }
        public string Menu1 { get; set; } = string.Empty;
        public string Menu2 { get; set; } = string.Empty;
        public string Menu3 { get; set; } = string.Empty;
        public string Menu4 { get; set; } = string.Empty;
        public string Menu5 { get; set; } = string.Empty;
        public List<UiListVM> Children { get; set; } = new List<UiListVM>();
    }
}
