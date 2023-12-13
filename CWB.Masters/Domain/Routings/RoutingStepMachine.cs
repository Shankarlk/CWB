using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;

namespace CWB.Masters.Domain
{
    public class RoutingStepMachine : BaseEntity
    {
        public long TenantId { get; set; }
        public long MachineId { get; set; }
        public Machine Machine { get; set; }
        public long RoutingStepId { get; set; }
        public RoutingStep RoutingStep { get; set; }
        public TimeSpan SetupTime { get; set; }
        public TimeSpan FloorToFloorTime { get; set; }
        public TimeSpan FirstPieceProcessingTime { get; set; }
        public int NoOfPartsPerLoading { get; set; }
        public ICollection<RoutingStepMachineReferenceDocument> RoutingStepMachineReferenceDocuments { get; set; }
    }
}
