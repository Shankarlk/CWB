using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.BusinessProcesses
{
    public class SO_Alloc_ListVM
    {
        DateTime? requiredByDate = null;
        public long SO_Alloc_List_Id { get; set; }
        public long SO_ID { get; set; }

        public long PartId { get; set; }
        public DateTime? Allocation_Date { get; set; }
        public int Allocated_Qnty { get; set; }

        public char Dispatch_Complete { get; set; }
        public int Final_Dispatch_Qnty { get; set; }
        public string Customer { get; set; }
        public string? PartNo { get; set; } = string.Empty;
        public string? PartDesc { get; set; } = string.Empty;
        public string? SONumber { get; set; }
        public string? PoNumber { get; set; }
        public DateTime? RequiredByDate
        {
            get { return requiredByDate; }
            set { requiredByDate = value; }
        }

        public String RequiredByDateStr
        {
            get
            {
                if (requiredByDate == null)
                {
                    return "";
                }
                return requiredByDate.Value.ToString("dd-MM-yyyy");
            }
            set { }
        }
        public int RequiredQuantity { get; set; }
        public long TenantId { get; set; }
    }
}
