using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class NC_Wk_List_Tmpl_Det:BaseEntity
    {
        public long NC_Wk_List_Tmpl_Appl_Id { get; set; }
        public string NC_Wk_Step_Desc { get; set; }
        public long Resp_Dept { get; set; }
        public long UI_ID { get; set; }
        public long Seq_no { get; set; }
        public long TenantId { get; set; }
    }
}
