using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class NC_Decision_Log:BaseEntity
    {
        public long NcLogId { get; set; }
        public char RedoCA { get; set; }
        public char Cust_Decision_Reqd { get; set; }
        public long Cust_Dec_Request { get; set; }
        public DateTime Cust_Request_date { get; set; }
        public DateTime Cust_feedback_date { get; set; }
        public long Cust_feedback_Id { get; set; }
        public string Cust_Feedback_Desc { get; set; }
        public long NC_Disp_Deci_Lvl1_Id { get; set; }
        public long NC_Disp_Deci_Lvl2_Id { get; set; }
        public string NC_Disp_Inst_Lvl1 { get; set; }
        public string NC_Disp_Inst_Lvl2 { get; set; }
        public long NC_Disp_Decision_Id { get; set; }
        public string NC_Disp_Instruction { get; set; }
        public long TenantId { get; set; }
    }
}
