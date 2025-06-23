using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class SubCon_List : BaseEntity
    {
        public long Wo_Id { get; set; }
        public long Opr_No { get; set; }
        public long Supplier_Id { get; set; }
        public long Mode { get; set; }
        public long Act_Qnty { get; set; }
        public long Plan_Qnty { get; set; }
        public char Loaded { get; set; }
        public char Rework_Wo { get; set; }
        public DateTime Plan_Disp_date { get; set; }
        public DateTime Plan_Recpt_date { get; set; }
        public DateTime Act_Disp_date { get; set; }
        public DateTime Act_Recpt_date { get; set; }
        public long TenantId { get; set; }
    }
}
