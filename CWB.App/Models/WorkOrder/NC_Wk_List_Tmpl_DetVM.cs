using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class NC_Wk_List_Tmpl_DetVM
    {
        public long NC_Wk_List_Tmpl_DetId { get; set; }
        public long NC_Wk_List_Tmpl_Appl_Id { get; set; }
        public string NC_Wk_Step_Desc { get; set; }
        public long Resp_Dept { get; set; }
        public long UI_ID { get; set; }
        public long Seq_no { get; set; }
        public long TenantId { get; set; }
        public string UI_Name { get; set; }
        public string Resp_DeptName { get; set; }
    }
}
