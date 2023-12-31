using System;

namespace CWB.Masters.ViewModels.Routing
{
    public class RoutingStepMachineVM
    {
        public long RoutingStepMachineId { get; set; }
        public long TenantId { get; set; }
        public long MachineId { get; set; }
        public long RoutingStepId { get; set; }
        public TimeSpan SetupTime { get; set; }
        public TimeSpan FloorToFloorTime { get; set; }
        public TimeSpan FirstPieceProcessingTime { get; set; }
        public int NoOfPartsPerLoading { get; set; }
    }
}
