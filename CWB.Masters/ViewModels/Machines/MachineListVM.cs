namespace CWB.Masters.ViewModels.Machines
{
    public class MachineListVM
    {
        public long MachineId { get; set; }
        public long SectionId { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string SlNo { get; set; }
        public long PlantId { get; set; }
        public long MachineTypeId { get; set; }
        public long ShopId { get; set; }
    }
}
