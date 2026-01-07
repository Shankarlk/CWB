namespace CWB.App.Models.Machine
{
    public class MachineListVM
    {
        public long MachineId { get; set; }
        public long SectionId { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string SlNo { get; set; }
        public long PlantId { get; set; }
        public long MachineOperationListId { get; set; }
        public string Plant { get; set; }
        public long ShopId { get; set; }
        public long MachineTypeId { get; set; }
        public string Shop { get; set; }
        public string SectionName { get; set; }
        public string MachineType { get; set; }
        public string NextOprTime { get; set; }
    }
}
