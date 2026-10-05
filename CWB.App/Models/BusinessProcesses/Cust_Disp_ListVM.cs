using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.BusinessProcesses
{
    public class Cust_Disp_ListVM
    {
        public long Cust_Disp_List_ID { get; set; }

        public long SO_ID { get; set; }
        public DateTime? CreationDate { get; set; }
        public long PartId { get; set; }
        public int Disp_Qnty { get; set; }
        public char Dispatch_Complete { get; set; }
        public char Invoice_Details_Entered { get; set; }
        public char Data_Confirmed { get; set; }

        public long TenantId { get; set; }
        //below are derived feilds not connected to table 

        public string Customer { get; set; }
        public string? PartNo { get; set; } = string.Empty;
        public string? PartDesc { get; set; } = string.Empty;
        public string? PoNumber { get; set; }
        public int RequiredQuantity { get; set; }
        public long BalanceSOQty { get; set; }
    }
}
