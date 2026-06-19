using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Gro.ViewModels
{
    public class Gro_Stock_ListVM
    {
        public long Gro_Stock_ListId { get; set; }
        public long Gro_Part_List_ID { get; set; }
        public int Qnty_on_Hand { get; set; }
        public long Last_Sl_No { get; set; }
        public DateTime? Qnty_Correction_Date { get; set; }

        public long Correction_User { get; set; }
        public long TenantId { get; set; }
    }
}
