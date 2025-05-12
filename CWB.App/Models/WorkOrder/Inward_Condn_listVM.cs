using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class Inward_Condn_listVM
    {
        public long Inward_Condn_listId { get; set; }
        public string Inward_Condn_desc { get; set; }
        public string Applicability { get; set; }
        public long TenantId { get; set; }
    }
}
