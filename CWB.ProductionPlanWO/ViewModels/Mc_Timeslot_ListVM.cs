using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
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
    }
}
