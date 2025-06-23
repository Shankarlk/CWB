using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class Timeslot_ListVM
    {
        public long Timeslot_ListId { get; set; }
        public long PlantId { get; set; }
        public DateTime Start_time { get; set; }
        public DateTime End_time { get; set; }
        public char Break_Slot { get; set; }
        public long TenantId { get; set; }
    }
}
