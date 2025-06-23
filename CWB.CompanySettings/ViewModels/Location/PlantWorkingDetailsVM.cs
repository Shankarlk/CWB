using CWB.CommonUtils.Common;

namespace CWB.CompanySettings.ViewModels.Location
{
    public class PlantWorkingDetailsVM
    {
        public long WDId { get; set; }
        public long PlantId { get; set; }
        public string WeeklyOff1 { get; set; }
        public string WeeklyOff2 { get; set; }
        public int NoOfShifts { get; set; }
        public string FirstShiftStartTime { get; set; }
        public string SecondShiftStartTime { get; set; }
        public string ThirdShiftStartTime { get; set; }
        public string FirstShiftDuration { get; set; }
        public string SecondShiftDuration { get; set; }
        public string ThirdShiftDuration { get; set; }
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
