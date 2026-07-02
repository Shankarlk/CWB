using CWB.CommonUtils.Common;
using System;

namespace CWB.Gro.Domain
{
    public class Gro_Indent_DispHead:BaseEntity
    {
        public string Gro_Indent { get; set; }
        public long Gro_Disp_Header_ID { get; set; }
        public long TenantId { get; set; }
    }
}
