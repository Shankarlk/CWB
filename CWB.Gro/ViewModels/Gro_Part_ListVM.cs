using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Gro.ViewModels
{
    public class Gro_Part_ListVM
    {
        public long Gro_Part_ListId { get; set; }
        public string Gro_Part_No { get; set; }
        public long Part_No { get; set; }
        public decimal MRP { set; get; }
        public DateTime? Data_Update { set; get; }
        public long Update_By { set; get; }
        public int Part_Status { set; get; }
        public decimal OurPrice { set; get; }

        public int GSTRate { get; set; }
        public string HSNCode { get; set; }
        public string OurPartDescription { get; set; } = string.Empty;
        public long TenantId { get; set; }
    }
}
