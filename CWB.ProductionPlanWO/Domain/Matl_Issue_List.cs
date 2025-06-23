using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class Matl_Issue_List :BaseEntity
    {
        public long Part_Ref { get; set; }
        public long Issue_Qnty { get; set; }
        public DateTime Issue_Mov_date { get; set; }
        public long Mode { get; set; }
        public char Immediate_Movmt { get; set; }
        public char Issue_Mov_Compl { get; set; }
        public long From_Location { get; set; }
        public long To_Location { get; set; }
        public long TenantId { get; set; }
    }
}
