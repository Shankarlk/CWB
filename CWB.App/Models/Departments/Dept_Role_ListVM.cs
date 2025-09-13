using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.Departments
{
    public class Dept_Role_ListVM
    {
        public long Dept_Role_ListId { get; set; }
        public long Dept_Struct_Id { get; set; }
        public long Role_Access_Id { get; set; }
        public long TenantId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
