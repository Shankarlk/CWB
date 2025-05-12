using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class NC_Wk_List_Tmpl_Head:BaseEntity
    {
        public long NC_Disp_Decision_Id { get; set; }
        public long No_of_steps { get; set; }
        public long TenantId { get; set; }
    }
}
