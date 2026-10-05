using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.BusinessProcesses
{
    public class CustDispInvoiceLineVM
    {
        public long Cust_Disp_List_ID { get; set; }

        public string PoNumber { get; set; }

        public string PartNo { get; set; }

        public string PartDesc { get; set; }

        public int Disp_Qnty { get; set; }
        
        public string Customer { get; set; }
    }
}
