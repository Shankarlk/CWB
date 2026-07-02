using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.Gro
{
    public class InvoicePrintVM
    {
        public Gro_Disp_HeaderVM Header { get; set; }

        public List<InvoiceDetailVM> Details { get; set; } = new List<InvoiceDetailVM>();

        public decimal TaxableAmount { get; set; }

        public decimal GSTAmount { get; set; }

        public decimal GrandTotal { get; set; }
        public string AmountInWords { get; set; }
        public string TaxAmountInWords { get; set; }
    }
}
