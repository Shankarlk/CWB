using CWB.CommonUtils.Common;
using CWB.Masters.Domain.ItemMaster;

namespace CWB.Masters.Domain
{
    public class RoutingStepPart : BaseEntity
    {
        public long TenantId { get; set; }
        public long RoutingStepId { get; set; }
        public RoutingStep RoutingStep { get; set; }
        public long ManufacturedPartBOMId { get; set; }
        public ManufacturedPartBOM ManufacturedPartBOM { get; set; }
        public int QuantityAssembly { get; set; }
    }
}
