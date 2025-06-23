using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class Timeslot_List:BaseEntity
    {
        public long PlantId { get; set; }
        public DateTime Start_time { get; set; }
        public DateTime End_time { get; set; }
        public char Break_Slot { get; set; }
        public long TenantId { get; set; }
    }
}
