using CWB.CommonUtils.Common;
using System;


namespace CWB.Gro.Domain
{
    public class Gro_Stock_List:BaseEntity
    {
        public long Gro_Part_List_ID { get; set; }
        public int Qnty_on_Hand { get; set; }
        public long Last_Sl_No { get; set; }
        public DateTime? Qnty_Correction_Date { get; set; }

        public long Correction_User { get; set; }
        public long TenantId { get; set; }




    }
}
