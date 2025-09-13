using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.Departments
{
    public class SectionsVM
    {
        public long SectionsId { get; set; }
        public string Name { get; set; }
        public string DeptName { get; set; }
        public long TenantId { get; set; }
        public long ShopDepartmentId { get; set; }
    }
}
