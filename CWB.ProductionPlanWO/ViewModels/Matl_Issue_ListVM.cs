using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class Matl_Issue_ListVM
    {
        public long Matl_Issue_ListId { get; set; }
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
