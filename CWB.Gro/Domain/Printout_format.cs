using CWB.CommonUtils.Common;
using System;

namespace CWB.Gro.Domain
{
    public class Printout_format : BaseEntity
    {
        public long CWB_Customer { get; set; }
        public string Template_Location { get; set; }
        public long Purpose { get; set; }
        public long  TenantId { get; set; }
    }
}
