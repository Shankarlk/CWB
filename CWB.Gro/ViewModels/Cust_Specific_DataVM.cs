using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Gro.ViewModels
{
    public class Cust_Specific_DataVM
    {
        public long Cust_Specific_DataId { set; get; }
        public string File_Location { get; set; }
        public long Last_Upload_Row_No { get; set; }

        public DateTime? Last_Upload_date { get; set; }

        public long CWB_Customer { get; set; }
        public long UI_ID { get; set; }
        public string Upload_Mapped_Table { get; set; }
        public string Disp_Head_Map_Table { get; set; }
        public string Disp_Det_Map_Table { get; set; }
        public long TenantId { get; set; }
    }
}
