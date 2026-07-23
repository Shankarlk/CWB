using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.ProductionSimulation
{
    public class SimulationShiftVM
    {
        public string ShiftName { get; set; }

        public DateTime ShiftStart { get; set; }

        public DateTime ShiftEnd { get; set; }

        public List<int> Hours { get; set; } = new List<int>();
    }
}
