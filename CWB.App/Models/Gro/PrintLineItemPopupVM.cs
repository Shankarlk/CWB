using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.Gro
{
    public class PrintLineItemPopupVM
    {
        public string SelectedIndents { get; set; }

        public List<PrintLineItemResultVM> Items { get; set; }
    }
}
