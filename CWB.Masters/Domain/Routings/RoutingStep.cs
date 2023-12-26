using CWB.CommonUtils.Common;
using CWB.Masters.MastersUtils.Routing;
using System.Collections.Generic;

namespace CWB.Masters.Domain
{
    public class RoutingStep : BaseEntity
    {
        public long RoutingId { get; set; }
        
        public string StepNumber { get; set; }
        public string StepDescription { get; set; }
        public RoutingStepOperation RoutingStepOperation { get; set; } //Operation List
        public RoutingStepLocation RoutingStepLocation { get; set; }
        public RoutingStepSequence RoutingStepSequence { get; set; }
        public string Status { get; set; }
        //public int SequenceType { get; set; }
        public long TenantId { get; set; }
        public Routing Routing { get; set; }
        public ICollection<RoutingStepDocument> RoutingStepDocuments { get; set; }
        public ICollection<RoutingStepMachine> RoutingStepMachines { get; set; }
        public ICollection<RoutingStepPart> RoutingStepParts { get; set; }

    }
}
