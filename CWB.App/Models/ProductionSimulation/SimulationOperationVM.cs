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
    }
}
