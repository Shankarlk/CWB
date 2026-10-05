using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.BusinessProcesses
{
    public class CustomerDispatchInvoiceVM
    {
        public string InvoiceNo { get; set; }

        public DateTime InvoiceDate { get; set; }

        public string DispatchDetails { get; set; }

        public string Customer { get; set; }

        public int NoOfParts { get; set; }

        public List<long> CustDispListIds { get; set; }
    }
}
