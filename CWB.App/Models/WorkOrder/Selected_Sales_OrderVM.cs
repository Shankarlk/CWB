using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class Selected_Sales_OrderVM
    {
        public string? SoNumber { get; set; }

       public string PartNo { get; set; }
        public string RoutingName { get; set; } = "-";
        public string StepNo { get; set; } = "-";
        public string Machines { get; set; } = "-";
        public string Subcons { get; set; } = "-";

    }
}
