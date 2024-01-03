namespace CWB.Masters.ViewModels.Routings
{
    public class RoutingVM
    {
        public int RoutingId {  get; set; } 
        public string RoutingName { get; set; }
        public long ManufacturedPartId { get; set; }
        public int OrigRoutingId {  get; set; }
        public int PreferredRouting { get; set; }
        public string Status { get; set; } = "Active";
        public string? CreationDate { get; set; }
    }
}
