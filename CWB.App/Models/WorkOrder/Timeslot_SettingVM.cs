using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class Timeslot_SettingVM
    {
        public long Timeslot_SettingId { get; set; }
        public int Timeslot_duration { get; set; }
        public int No_of_span_days { get; set; }
        public int Retention_Days { get; set; }
        public string First_Shift_Break_start_time { get; set; }
        public string First_Shift_Break_duration { get; set; }
        public string Sec_Shift_Break_start_time { get; set; }
        public string Sec_Shift_Break_duration { get; set; }
        public string Third_Shift_Break_start_time { get; set; }
        public string Third_Shift_Break_duration { get; set; }
        public char Change_flag { get; set; }
        public long TenantId { get; set; }
    }
}
