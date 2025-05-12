using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class NC_Wk_List_HeaderVM
    {
        public long NC_Wk_List_HeaderId { get; set; }
        public long NC_Log_Id { get; set; }
        public long NC_Wk_List_Header_Status { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public long TenantId { get; set; }
    }
}
