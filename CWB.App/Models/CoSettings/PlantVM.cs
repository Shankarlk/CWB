namespace CWB.App.Models.CoSettings
{
    public class PlantVM
    {
        public long PlantId { get; set; }
        public string Name { get; set; }
        public bool IsMainPlant { get; set; }
        public bool IsProductDesigned { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; } = string.Empty;
        public long TenantId { get; set; }
    }
}
