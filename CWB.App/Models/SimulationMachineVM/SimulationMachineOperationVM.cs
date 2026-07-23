using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.SimulationMachineVM
{
    public class SimulationMachineOperationVM
    {
        public string PartNum { get; set; }

        public string OperationName { get; set; }

        public double Left { get; set; }

        public double Width { get; set; }

        public string Status { get; set; }
    }
}
