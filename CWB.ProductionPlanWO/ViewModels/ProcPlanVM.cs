using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class ProcPlanVM
    {
        public long ProcPlanId { get; set; }
        public string Reference { get; set; }
        public char TestData { get; set; }
        public int For_Ref { get; set; }
        public long PartId { get; set; }
        public long WorkOrderId { get; set; }
        public string PartType { get; set; }
        public decimal Calc_Proc_Qnty { get; set; }
        public long UOMId { get; set; }
        public long StartingOpNO { get; set; }
        public long EndingOpNO { get; set; }
        public decimal OtyOnHand { get; set; }
        public decimal AddnOtyUser { get; set; }
        public int PO_Flag { get; set; }
        public decimal Plan_Proc_Qnty { get; set; }
        public DateTime PlanReceiptDate { get; set; }
        public DateTime CalcReceiptDate { get; set; }
        public int CriticalPart { get; set; }
        public int Changed { get; set; }
        public long TenantId { get; set; }
    }
}
