using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class NC_Disp_Decs_Appl_ListVM
    {
        public long NC_Disp_Decs_Appl_ListId { get; set; }
        public long NC_Disp_Decision_Id { get; set; }
        public char Level_2_Dec_Reqd { get; set; }
        public char Rm { get; set; }
        public char Bof { get; set; }
        public char SubCon { get; set; }
        public char Cmp { get; set; }
        public char Assy { get; set; }
        public long TenantId { get; set; }
        public string NC_Disp_Decision_Name { get; set; } = string.Empty;
    }
}
