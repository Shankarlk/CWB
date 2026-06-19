using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Gro.ViewModels
{
    public class TK_DC_Inv_ContrlVM
    {
        public long TK_DC_Inv_ContrlId { get; set; }
        public long TK_DC_Last_No { get; set; }
        public long TK_Inv_Last_No { get; set; }

        public char DC_Enable { get; set; }
        public char Inv_Print_Enable { get; set; }
        public char Inv_Push_Enable { get; set; }
        public long TenantId { get; set; }
    }
}
