using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class Insp_Outcome_DetailsVM
    {
        public long Insp_Outcome_Details_Id { get; set; }
        public long Inw_Recpt_Header_Id { get; set; }
        public long Inw_Recpt_Part_No_Id { get; set; }
        public string Inw_Recpt_Part_No_Name { get; set; } = string.Empty;
        public string PartType { get; set; } = string.Empty;
        public string Lvldesc { get; set; } = string.Empty;
        public string LvlAppro { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public string NcDateStr { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string WfCustFeedBack { get; set; } = string.Empty;
        public string RcaStatus { get; set; } = string.Empty;
        public long Inw_Insp_Log_Id { get; set; }
        public long Shop_Insp_log_Id { get; set; }
        public long Final_Insp_log_Id { get; set; }
        public string NC_Tracking_No { get; set; }
        public string Balloon_No_Dir { get; set; }
        public string Balloon_No { get; set; }
        public string Feature_Descrip { get; set; }
        public string NC_Descrip { get; set; }
        public decimal NC_Qnty { get; set; }
        public char Decl_by_Supplier { get; set; }
        public string Storage_Location { get; set; }
        public string Label_Type { get; set; }
        public long NC_Log_status_Id { get; set; }
        public DateTime NcDate { get; set; }
        public long TenantId { get; set; }
        public long NoOfDays { get; set; }
    }
}
