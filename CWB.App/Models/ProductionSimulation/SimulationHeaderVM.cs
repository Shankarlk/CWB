using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.ProductionSimulation
{
    public class SimulationHeaderVM
    {
        public DateTime Date { get; set; }

        public List<SimulationShiftVM> Shifts { get; set; } = new List<SimulationShiftVM>();
    }
}
