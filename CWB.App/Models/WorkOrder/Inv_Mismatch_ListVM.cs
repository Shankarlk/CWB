using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class Inv_Mismatch_ListVM
    {
        public long Inv_Mismatch_ListId { get; set; }
        public long Calling_UI_ID { get; set; }
        public long Reported_By { get; set; }
        public DateTime Report_date { get; set; }
        public long Record_No { get; set; }
        public long Part_No { get; set; }
        public long Routing_ID { get; set; }
        public long Opr_No { get; set; }
        public long Location_ID { get; set; }
        public long PO_Ref { get; set; }
        public long Mismatch_Qnty { get; set; }
        public string Mismatch_Comments { get; set; }
        public string Mismatch_Status { get; set; }
        public char Resolved { get; set; }
        public long Resolved_by { get; set; }
        public DateTime Resolution_date { get; set; }
        public string Resolution_Comments { get; set; }
        public long TenantId { get; set; }
        public string PartNoStr { get; set; }
        public string Reported_ByName { get; set; }
        public string TransactionDone { get; set; }
        public string Report_dateStr { get; set; }
        public string LocationStr { get; set; }
        public string RoutName { get; set; }
        public string OpNoName { get; set; }
    }
}
