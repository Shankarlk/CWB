using CWB.CommonUtils.Common;

namespace CWB.Masters.Domain
{
    public class Division : BaseEntity
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public long CompanyId { get; set; }
        public Company Company { get; set; }
        public long TenantId { get; set; }
    }
}
