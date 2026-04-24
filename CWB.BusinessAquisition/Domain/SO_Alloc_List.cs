using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.BusinessAquisition.Domain
{
    public class SO_Alloc_List : BaseEntity
    {
        public long SO_ID { get; set; }

        public long PartId { get; set; }
        public DateTime? Allocation_Date { get; set; }
        public int Allocated_Qnty { get; set; }

        public char Dispatch_Complete    { get; set; }
        public int Final_Dispatch_Qnty { get; set; }
        public long TenantId { get; set; }
    }
}
