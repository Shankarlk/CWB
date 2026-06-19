using CWB.CommonUtils.Common;
using System;

namespace CWB.Gro.Domain
{
    public class Upload_Format:BaseEntity
    {
        public long CWB_Customer { get; set; }
        public string Column_Name { get; set; }
        public string Column_Position { get; set; }

        public long Field_Type { get; set; }
        public string Mapped_Field { get; set; }
        public long TenantId { get; set; }
    }
}
