using CWB.CommonUtils.Common;
using System;

namespace CWB.Gro.Domain
{
    public class Gro_Stock_Det:BaseEntity
    {
        public long Gro_Part_List_ID { get; set; }
        public string Part_Sl_No { get; set; }
        public long Sl_No_Status_ID { get; set; }
        public long TenantId { get; set; }
    }
}
