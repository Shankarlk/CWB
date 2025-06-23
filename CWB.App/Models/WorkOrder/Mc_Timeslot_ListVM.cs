using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class Mc_Timeslot_ListVM
    {
        public long Mc_Timeslot_List_Id { get; set; }
        public long Timeslot_List_Id { get; set; }
        public long EndTimeslot_List_Id { get; set; }
        public long Mc_Id { get; set; }
        public long Mc_Wait_List_Id { get; set; }
        public long Allocation { get; set; }
        public char Slot_Not_Avl { get; set; }
        public long Not_Avl_reason { get; set; }
        public long TenantId { get; set; }
        public DateTime GetStartTime { get; set; }
        public DateTime GetEndTime { get; set; }
        public long ShopId { get; set; }
        public string ShopName { get; set; }
        public string EndTime { get; set; }
        public string StartTime { get; set; }
        public string NonAvlReas { get; set; }
        public string McName { get; set; }
    }
}
