using CWB.CommonUtils.Common;
using System;

namespace CWB.Gro.Domain
{
    public class Gro_Disp_Det : BaseEntity
    {
        public long Gro_Disp_Header_ID { get; set; }
        public long Gro_data_ID { get; set; }
        public long Gro_Part_No { get; set; }
        public long int_Part_No { get; set; }
        public int Qnty_Dispatched { get; set; }

        public char Qnty_Recd { get; set; }
        public string Indent { get; set; }
       
        public char Label_print { get; set; }
        

        public long TenantId { get; set; }
    }
}
