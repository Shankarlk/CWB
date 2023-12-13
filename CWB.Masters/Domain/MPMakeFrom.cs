using CWB.CommonUtils.Common;
using System;

namespace CWB.Masters.Domain
{
    /** PartId bigint not null,
  `PartDescription` varchar(4000) COLLATE utf8mb4_cs_0900_ai_ci DEFAULT NULL,
  `PartMadeFrom` bigint NOT NULL,
  `InputWeight` bigint NOT NULL,
  `Scrapgenerated` bigint NOT NULL,
  `QuantityPerInput` bigint NOT NULL,
  `YieldNotes` varchar(450) NOT NULL,
  `PreferedRawMaterial` tinyint NOT NULL,
   ManufPartId bigint not null,
     */
    public class MPMakeFrom : BaseEntity
    {
        //DbPart-Start
        public int PartId { get; set; }
        public long PartMadeFrom { get; set; }
        public long InputWeight { get; set; }
        public long ScrapGenerated { get; set; }
        public long QuantityPerInput { get; set; }
        public string? YieldNotes { get; set; }
        public Boolean PreferedRawMaterial { get; set; }
        public int ManufPartId { get; set; }
        //DbPart-End

        public string? PartDescription { get; set; }
        public long TenantId { get; set; }
    }
}
