using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class Insp_Outcome_Details : BaseEntity
    {
        public long Insp_Outcome_Details_Id { get; set; }
        public long Inw_Recpt_Header_Id { get; set; }
        public long Inw_Recpt_Part_No_Id { get; set; }
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
        public long TenantId { get; set; }
    }
}
