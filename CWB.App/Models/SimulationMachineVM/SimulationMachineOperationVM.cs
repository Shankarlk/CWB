using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.SimulationMachineVM
{
    public class SimulationMachineOperationVM
    {
        public string MachineName { get; set; }
        public string Department { get; set; }

        public string PartNum { get; set; }

        public string OperationName { get; set; }

        public double Left { get; set; }

        public double Width { get; set; }

        public string Status { get; set; }

        public string Color { get; set; }

        public string ShopMachine { get; set; }
        public char PartSoldToCustomer { get; set; }
        public string PartNoDesc { get; set; }
        public string RoutingOprNo { get; set; }
        public string WoNo { get; set; }
        public string BatchQnty { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string DurationHours { get; set; }
        public string FloorToFloorTime { get; set; }
        public string SetupTime { get; set; }

        public string BarType { get; set; }
        public string TooltipType { get; set; }
        public string InitiatedBy { get; set; }
    }
}
