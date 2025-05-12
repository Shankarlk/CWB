using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class NC_Work_ListVM
    {
        public long NC_Work_ListId { get; set; }
        public long NC_Work_List_Header_Id { get; set; }
        public string Step_Desc { get; set; }
        public long Resp_Dept { get; set; }
        public long Seq_No { get; set; }
        public long NC_Work_status_Id { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public long Resp_Person { get; set; }
        public long TenantId { get; set; }
    }
}
