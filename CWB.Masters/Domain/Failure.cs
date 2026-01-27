using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Masters.Domain
{
    public class Failure : BaseEntity
    {
        public string Message { get; set; }
        public long TenantId { get; set; }
    }
}
