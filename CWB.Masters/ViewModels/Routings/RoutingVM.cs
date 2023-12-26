namespace CWB.Masters.ViewModels.Routing
{
    public class RoutingVM
    {
        public int RoutingId {  get; set; } 
        public string RoutingName { get; set; }
        public long ManufacturedPartId { get; set; }
        public int OrigRoutingId {  get; set; }
        public string Status { get; set; } = "Active";
        public string? CreationDate { get; set; }
    }
}
