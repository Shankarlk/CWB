using System;

namespace CWB.App.Models.ItemMaster
{
    public class MPMakeFromVM
    {
        //DbPart-Start
        public int MPPartId { get; set; }
        public long MPPartMadeFrom { get; set; }
        public long InputWeight { get; set; }
        public long ScrapGenerated { get; set; }
        public long QuantityPerInput { get; set; }
        public string? YieldNotes { get; set; }
        public Boolean PreferedRawMaterial { get; set; }
        public int ManufPartId { get; set; }
        //DbPart-End

        public long? MPMakeFromId { get; set; }
        public string MFDescription { get; set; }
        public string InputPartNo { get; set; } = null!;
        public long TenantId { get; set; }
    }
}
