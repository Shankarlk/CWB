using System;

namespace CWB.App.Models.ItemMaster
{
    public class MPBomVM
    {
        //DbPart--Start
        public int BOMPartId { get; set; }
        public decimal Quantity { get; set; }
        public long BOMManufPartId { get; set; }
        public string? BOMPartDesc {get; set; }
        //DbPart--End
        public long MPBOMId { get; set; }
        public string BOMPartNo { get; set; }

        public string UOM { get; set; } = string.Empty;

        public long TenantId { get; set; }

        public long? PartId { get; set; }
        public string PartNo { get; set; } = "";
        public string Description { get; set; } = "";
        public string MasterPartType { get; set; } = "";
    }
}

/*
public class PartUOMVM
{
    //DbPart--Start
    public int PartId { get; set; }
    public string UOMName { get; set; }
}
*/