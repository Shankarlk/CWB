using CWB.App.Models.ProductionSimulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.SimulationMachineVM
{
    public class SimulationMachineVM
    {
        public List<SimulationHeaderVM> Headers { get; set; } = new List<SimulationHeaderVM>();

        public List<SimulationMachineRowVM> Machines { get; set; } = new List<SimulationMachineRowVM>();
    }
}
