using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.Gro
{
    public class DeleteInvoiceLineVM
    {
        public string GroPartNo { get; set; }

        public int QntyDispatched { get; set; }

        public decimal OurPrice { get; set; }

        public decimal InvoiceValue { get; set; }
    }
}
