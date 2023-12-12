using System;

namespace CWB.App.Models.ItemMaster
{
    public class MPBomVM
    {
        //DbPart--Start
        public int BOMPartId { get; set; }
        public long Quantity { get; set; }
        public long BOMManufPartId { get; set; }
        public string? BOMPartDesc {get; set; }
        //DbPart--End
        public long MPBOMId { get; set; }
        public string BOMPartNo { get; set; }

        public long TenantId { get; set; }
    }
}
