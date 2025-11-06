using CWB.CommonUtils.Common;
using CWB.Masters.MastersUtils.ItemMaster;

namespace CWB.Masters.Domain.ItemMaster
{
    public class PartsStatus : BaseEntity
    {
        public string Status { get; set; }
        public long TenantId { get; set; }

    }
}
