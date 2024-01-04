namespace CWB.App.Models.Routing
{
    public class RoutingVM
    {
        public int RoutingId { get; set; }
        public string RoutingName { get; set; }
        public long ManufacturedPartId { get; set; }
        public int OrigRoutingId { get; set; } = 0;
        public int PreferredRouting { get; set; } = 0;
        public int TenantId { get; set; }
        public string Status { get; set; }
        public string CreationDate { get; set; }

        //pass values to go to the next page
        public string NRCompanyName {  get; set; }
        public string NRPartNo { get; set;}
        public string NRPartDescription { get; set; }
    }
}
