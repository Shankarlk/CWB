using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.BusinessProcesses
{
    public class ProductionPlan_WoVM
    {
        DateTime? planDate = null;
        //DateTime? woDateStr = null;
        public long ProductionPlanId { get; set; }
        public string? PPNumber { get; set; }
        public long WoId { get; set; }
        public long ParentWoId { get; set; }
        public string? WONumber { get; set; }
        public DateTime? WODate { get; set; }
        public long SalesOrderId { get; set; }
        public long PartId { get; set; }
        public int PartType { get; set; }
        public char Parentlevel { get; set; }
        public long ManufRMLinkId { get; set; }
        public char BuildToStock { get; set; }
        public char TestData { get; set; }
        public int CalcWOQty { get; set; }
        public int QtyOnHand { get; set; }
        public int AddnOtyUser { get; set; }
        public int Status { get; set; }
        public int PlanWOQnty { get; set; }
        public DateTime? PlanCompletionDate
        {
            get { return planDate; }
            set { planDate = value; }
        }
        public String PlanCompletionDateStr
        {
            get
            {
                if (planDate == null)
                {
                    return "";
                }
                return planDate.Value.ToString("dd-MM-yyyy");
            }
        }
        public long RoutingId { get; set; }
        public int StartingOpNo { get; set; }
        public int EndingOpNo { get; set; }
        public int CriticalPart { get; set; }
        public string ReloadOption { get; set; }
        public int Active { get; set; }
        public int Urgent { get; set; }
        public char For_Ref { get; set; }
        public int ManufDaysAvailable { get; set; }
        public int ManufDaysRequired { get; set; }
        public int Changed { get; set; }
        public DateTime? SoComplDate { get; set; }
        public DateTime PlanStartDate { get; set; }
        public DateTime ActStartDate { get; set; }
        public DateTime ActCompletionDate { get; set; }
        public int ActWOQty { get; set; }
        public int Matl { get; set; }
        public int WIP { get; set; }
        public bool Hold { get; set; }
        public bool Done { get; set; }
        public string? PartNo { get; set; } = string.Empty;
        public string? PartDesc { get; set; } = string.Empty;
        public string? SoComplDateStr { get; set; } = string.Empty;
        public string? PlanStartDateStr { get; set; } = string.Empty;
        public string? WoRelease { get; set; } = string.Empty;
        public string? Customer { get; set; } = string.Empty;
        public string? PartTypeName { get; set; } = string.Empty;
        public string? PoQnty { get; set; } = string.Empty;
        public string? FinQnty { get; set; } = string.Empty;
        public string? PoNumber { get; set; } = string.Empty;
        public string? PsStartDate { get; set; } = string.Empty;
        public string? PsEndDate { get; set; } = string.Empty;
        public string? CsStartDate { get; set; } = string.Empty;
        public string? CsEndDate { get; set; } = string.Empty;
        public string? ActStartDateStr { get; set; } = string.Empty;
        public string? DataChange { get; set; } = string.Empty;
        public string? Wipp { get; set; } = string.Empty;
        public string? CriticalParts { get; set; } = string.Empty;
        public string? RoutingName { get; set; } = string.Empty;
        public string? CurOpr { get; set; } = string.Empty;
        public string? ReworkWo { get; set; } = string.Empty;
        public string? WoStatus { get; set; } = string.Empty;
        public string? PoStatus { get; set; } = string.Empty;
        public string? ReadyForProd { get; set; } = string.Empty;
        public int NoOfRoutes { get; set; }
        public int NoOfOpenNc { get; set; }
        public int NoOfDocWf { get; set; }
        public string Comment { get; set; }
        public long TenantId { get; set; }
    }
}
