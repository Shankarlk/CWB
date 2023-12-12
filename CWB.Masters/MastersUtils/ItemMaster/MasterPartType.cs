using System.ComponentModel;

namespace CWB.Masters.MastersUtils.ItemMaster
{
    public enum MasterPartType
    {
        [Description("Bought Out Finish")]
        BOF,
        [Description("Raw Material")]
        RawMaterial,
        [Description("Manufactured Part")]
        ManufacturedPart
    }
}
