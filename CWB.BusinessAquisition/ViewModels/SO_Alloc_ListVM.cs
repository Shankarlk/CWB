using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.BusinessAquisition.ViewModels
{
    public class SO_Alloc_ListVM
    {
        public long SO_Alloc_List_Id { get; set; }
        public long SO_ID { get; set; }

        public long PartId { get; set; }
        public DateTime? Allocation_Date { get; set; }
        public int Allocated_Qnty { get; set; }

        public char Dispatch_Complete { get; set; }
        public int Final_Dispatch_Qnty { get; set; }
        public long TenantId { get; set; }
    }
}
