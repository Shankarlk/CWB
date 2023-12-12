using System.ComponentModel;

namespace CWB.Masters.MastersUtils
{
    public enum ManufacturedPartState
    {
        [Description("Active")]
        Active,
        [Description("Inactive")]
        Inactive,
        [Description("Hold")]
        Hold
    }
}
