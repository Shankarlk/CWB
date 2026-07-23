using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.ProductionSimulation
{
    public class ProductionSimulationVM
    {
        public List<SimulationHeaderVM> Headers { get; set; } = new List<SimulationHeaderVM>();

        public List<SimulationPartRowVM> Parts { get; set; } = new List<SimulationPartRowVM>();
        public int TotalHours { get; set; }
        public int TotalSlots { get; set; }
    }
}
