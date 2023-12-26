namespace CWB.Masters.ViewModels.Routing
{
    public class RoutingStepPartVM
    {
        public long StepPartId {  get; set; }
        public long RoutingStepId {  get; set; }
        public long ManufacturedPartId { get; set; }
        public long BOMId {  get; set; }
        public int QuantityAssembly { get; set; }
    }
}
