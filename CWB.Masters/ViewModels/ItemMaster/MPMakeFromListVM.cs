using System;

namespace CWB.Masters.ViewModels.ItemMaster
{
    public class MPMakeFromListVM
    {
        public long? MPMakeFromId { get; set; }
        public long? PartMadeFrom { get; set; }
        public long? InputWeight { get; set; }
        public string InputPartNo { get; set; }
        public long? Scrapgenerated { get; set; }
        public long? QuantityPerInput { get; set; }
        public string YieldNotes { get; set; }
        public Boolean PreferedRawMaterial { get; set; }
    }
}
