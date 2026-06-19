using CWB.CommonUtils.Common;
using System;

namespace CWB.Gro.Domain
{
    public class TK_DC_Inv_Contrl : BaseEntity
    {
        public long TK_DC_Last_No { get; set; }
        public long TK_Inv_Last_No { get; set; }

        public char DC_Enable { get; set; }
        public char Inv_Print_Enable { get; set; }
        public char Inv_Push_Enable { get; set; }
        public long TenantId { get; set; }
    }
}
