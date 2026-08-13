using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.Gro
{
    public class PrintLineItemResultVM
    {
        public string PartNo { get; set; }

        public int TotalOrderQty { get; set; }

        public int QtyAvailable { get; set; }

        public int BalanceToPack { get; set; }
    }
}
