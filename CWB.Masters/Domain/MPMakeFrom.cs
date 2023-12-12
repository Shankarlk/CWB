using CWB.CommonUtils.Common;
using System;

namespace CWB.Masters.Domain
{
    public class MPMakeFrom : BaseEntity
    {
        public long? PartMadeFrom { get; set; }
        public long? InputWeight { get; set; }
        public string InputPartNo { get; set; }
        public long? Scrapgenerated { get; set; }
        public long? QuantityPerInput { get; set; }
        public string YieldNotes { get; set; }
        public Boolean PreferedRawMaterial { get; set; }
        public long TenantId { get; set; }


    }
}
