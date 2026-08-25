using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.ProductionSimulation
{
    public class SimulationOperationVM
    {
        public int StepNo { get; set; }

        public string OperationName { get; set; }
        public string PartNum { get; set; }
        public string MachineName { get; set; }

        public string Department { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Status { get; set; }

        public string Color { get; set; }

        public double Left { get; set; }

        public double Width { get; set; }
        public int StartIndex { get; set; }

        public int SlotCount { get; set; }

        public string Tooltip { get; set; }

        public string Supplier { get; set; }

        public string DisplayLabel { get; set; }

        public string BarType { get; set; }

        public string TooltipType { get; set; }
        public string AssySoldToCustomer { get; set; }
        public string PartSoldToCustomer { get; set; }
        public string ShopMachine { get; set; }
        public string PartNoDesc { get; set; }
        public string RoutingOprNo { get; set; }
        public string WoNo { get; set; }
        public string BatchQnty { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string DurationHours { get; set; }
        public string FloorToFloorTime { get; set; }
        public string SetupTime { get; set; }
        public bool IsDelayed { get; set; }

        public string DelayOutline { get; set; }

        public string CustomerLabel { get; set; }
        public string CustomerValue { get; set; }
    }
}
