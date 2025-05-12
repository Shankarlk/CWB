using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class NC_Disp_Dec_Maker : BaseEntity
    {
        public string Level_1_dec_maker { get; set; }
        public string Level_2_dec_maker { get; set; }
        public string TenantId { get; set; }
    }
}
