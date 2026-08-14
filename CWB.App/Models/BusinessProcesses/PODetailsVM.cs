using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.BusinessProcesses
{
    public class PODetailsVM
    {
        public long PoDetailsId { get; set; }
        public string POReference { get; set; }
        public long ProcPlanId { get; set; }
        public char AddHocPO { get; set; }
        public long PartId { get; set; }
        public int PoQnty { get; set; }
        public DateTime PoDate { get; set; }
        public long CompanyId { get; set; }
        public DateTime PlanPoReceiptDate { get; set; }
        public char PoSent { get; set; }
        public char Mismatch_Resolved { get; set; }
        public int PoQntyRecd { get; set; }
        public int Status { get; set; }
        public long TenantId { get; set; }
        public string DocStatus { get; set; } = string.Empty;
        public string NoOfLine { get; set; } = string.Empty;
        public string NoOfOpenLine { get; set; } = string.Empty;
        public string NoOfPastLine { get; set; } = string.Empty;
        public string DateStr { get; set; } = string.Empty;
        public string DateRed { get; set; } = string.Empty;
        public string QntyRed { get; set; } = string.Empty;
        public string ProcPrice { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;
        public string PoDateStr { get; set; } = string.Empty;
        public string StatusStr { get; set; } = string.Empty;
        public string PoType { get; set; } = string.Empty;
        public string PartType { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string PartNo { get; set; } = string.Empty;
        public string InwDate { get; set; } = string.Empty;
        public long? WoId { get; set; } = 0;
        public long InwHeaderId { get; set; } = 0;
        public string CombinedIds { get; set; } = string.Empty;
        public char Inspection { get; set; }
        public string ProcPlanIds { get; set; }
    }
}
