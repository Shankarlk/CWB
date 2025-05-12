using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class NC_Wk_List_Tmpl_HeadVM
    {
        public long NC_Wk_List_Tmpl_HeadId { get; set; }
        public long NC_Disp_Decision_Id { get; set; }
        public long No_of_steps { get; set; }
        public long TenantId { get; set; }
        public string NC_Disp_Decision_Name { get; set; }
    }
}
