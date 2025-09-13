using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Domain
{
    public class Dept_Role_List : BaseEntity
    {
        public long Dept_Struct_Id { get; set; }
        public long Role_Access_Id { get; set; }
        public long TenantId { get; set; }
    }
}
