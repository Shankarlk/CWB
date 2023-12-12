using System;

namespace CWB.App.Models.ItemMaster
{
    public class MPMakeFromListVM
    {
        public long? MPMakeFromId { get; set; }
        public long? PartMadeFrom { get; set; }
        public string InputPartNo { get; set; } = null!;
        public long? InputWeight { get; set; }
        public long? ScrapGenerated { get; set; }
        public long? QuantityPerInput { get; set; }
        public string YieldNotes { get; set; } = null!;
        public Boolean PreferedRawMaterial { get; set; }
        public long TenantId { get; set; }
    }
}
