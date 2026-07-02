using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.Gro
{
    public class InvoiceDetailVM
    {
        public int SlNo { get; set; }

        public long PartId { get; set; }

        public string PartNo { get; set; }

        public string Description { get; set; }

        public string HSNCode { get; set; }

        public string Unit { get; set; }

        public decimal Qty { get; set; }

        public decimal Rate { get; set; }

        public decimal GSTRate { get; set; }

        public decimal TaxableAmount { get; set; }

        public decimal GSTAmount { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
