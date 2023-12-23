namespace CWB.App.Models.Routing
{
    public class RoutingStepPartVM
    {
        public long BOMId { get; set; }
        public long ManufacturedPartId { get; set; }
        public string PartDescription { get; set; }
        public long RoutingStepPartId { get; set; }
        public long RoutingStepId { get; set; }
        public long PartId { get; set; }
        public int QuantityAssembly { get; set; }
        public string PartNo {  get; set; }

        public int TenantId { get; set; }


    }
}
