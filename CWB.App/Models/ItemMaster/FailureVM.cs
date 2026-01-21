using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.ItemMaster
{
    public class FailureVM
    {
        public long FailureId { get; set; }
        public string Message { get; set; }
        public long TenantId { get; set; }
    }
}
