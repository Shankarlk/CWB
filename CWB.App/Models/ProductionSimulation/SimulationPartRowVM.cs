using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.ProductionSimulation
{
    public class SimulationPartRowVM
    {
        public long WoId { get; set; }

        public string PartNo { get; set; }

        public string Description { get; set; }

        public bool IsUrgent { get; set; }

        public List<SimulationOperationVM> Operations { get; set; } = new List<SimulationOperationVM>();
    }
}
