using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Gro.ViewModels
{
    public class Gro_Stock_DetVM
    {
        public long Gro_Stock_DetId { get; set; }
        public long Gro_Part_List_ID { get; set; }
        public string Part_Sl_No { get; set; }
        public long Sl_No_Status_ID { get; set; }
        public long TenantId { get; set; }
    }
}
