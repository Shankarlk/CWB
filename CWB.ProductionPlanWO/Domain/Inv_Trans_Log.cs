using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class Inv_Trans_Log:BaseEntity
    {
        public DateTime Dt_time { get; set; }
        public long PersonId { get; set; }
        public long Input_Part_NoId { get; set; }
        public long Input_Routing_Id { get; set; }
        public long Input_Opr_No { get; set; }
        public long Output_Part_No { get; set; }
        public long Output_Routing_Id { get; set; }
        public long Output_Opr_No { get; set; }
        public long Wo_Id { get; set; }
        public long PO_No_Id { get; set; }
        public long Transaction_Id { get; set; }
        public decimal Qnty { get; set; }
        public long From_Location_Id { get; set; }
        public long To_Location_Id { get; set; }
        public long Part_Status { get; set; }
        public char Movement_Compl { get; set; }
        public decimal Qnty_Mismatch { get; set; }
        public long Qnty_mismatch_status { get; set; }
        public string Qnty_Mismatch_Comment { get; set; }
        public string Qnty_Mismatch_Resolution { get; set; }
        public long NC_Log_Id { get; set; }
        public long Our_DC_Ref { get; set; }
        public long Our_RGP_Ref { get; set; }
        public long TenantId { get; set; }
    }
}
