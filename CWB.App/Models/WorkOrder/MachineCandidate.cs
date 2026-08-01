using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.App.Models.Routing;
namespace CWB.App.Models.WorkOrder
{
    public class MachineCandidate
    {
        public RoutingStepMachineVM Machine { get; set; }

        public List<Timeslot_ListVM> AvailableTimeslots { get; set; }

        public double TPT { get; set; }

        public int QtyPerMc { get; set; }
    }
}
