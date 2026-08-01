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
        public long WO_Id { get; set; }
        public long Input_PartId { get; set; }
        public decimal Issue_Qnty { get; set; }
        public DateTime Issue_Mov_date { get; set; }
        public long Mode { get; set; }
        public char Immediate_Movmt { get; set; }
        public char Issue_Mov_Compl { get; set; }
        public long From_Location { get; set; }
        public long To_Location { get; set; }
        public long TenantId { get; set; }
        public long QntyAvl { get; set; }
        public long BookOutQnty { get; set; }
        public string BalWoQnty { get; set; }
        public string To_LocationStr { get; set; }
        public string WoNumber { get; set; }
        public string Shop { get; set; }
        public string QntyRecdCnf { get; set; }
        public string IssueMovDtStr { get; set; }
        public string PartNo { get; set; }
        public string InputPartNo { get; set; }
        public string RoutingName { get; set; }
        public string OpNo { get; set; }
        public string From_LocationStr { get; set; }
        public long PartId { get; set; }
        public long RoutingId { get; set; }
        public long OprId { get; set; }

        public string From_Loc_Flag { get; set; }
        public string To_Loc_Flag { get; set; }
    }
}
