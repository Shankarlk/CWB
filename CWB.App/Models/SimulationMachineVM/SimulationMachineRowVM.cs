using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.SimulationMachineVM
{
    public class SimulationMachineRowVM
    {
        public long MachineId { get; set; }

        public string MachineNo { get; set; }

        public List<SimulationMachineOperationVM> Operations { get; set; } = new List<SimulationMachineOperationVM>();
    }
}
