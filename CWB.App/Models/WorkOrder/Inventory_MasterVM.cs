using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class Inventory_MasterVM
    {
        public long Inventory_MasterId { get; set; }
        public DateTime Dt_time { get; set; }
        public long Part_NoId { get; set; }
        public long Routing_Id { get; set; }
        public long Opr_No_Id { get; set; }
        public decimal Current_QntOnHand { get; set; }
        public long Location_Id { get; set; }
        public long Inv_Trans_Log_Id { get; set; }
        public long TenantId { get; set; }
        public long Qnty { get; set; }
        public string ReasonDesc { get; set; }
        public string PartNoStr { get; set; } = string.Empty;
        public string PartDescStr { get; set; } = string.Empty;
        public string PartTypeStr { get; set; } = string.Empty;
        public string DateStr { get; set; } = string.Empty;
        public string PartStatus { get; set; } = string.Empty;
        public string CompanyStr { get; set; } = string.Empty;
        public string RoutName { get; set; } = "-";
        public string OpName { get; set; } = string.Empty;
        public string LocationStr { get; set; } = "-";
    }
}
