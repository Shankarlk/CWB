using CWB.CommonUtils.Common;
using System;


namespace CWB.Gro.Domain
{
    public class Indent_Part_Sl_No:BaseEntity
    {
        public long Gro_Disp_Det_ID { get; set; }
        public long Gro_Stock_Det_ID { get; set; }
        public long TenantId { get; set; }
    }
}
