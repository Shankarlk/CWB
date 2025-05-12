using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class Inw_Recpt_Part_NoVM
    {
        public long Inw_Recpt_Part_No_Id { get; set; }
        public long PO_Details_Id { get; set; }
        public char Release_Insp { get; set; }
        public char Insp_Complete { get; set; }
        public long TenantId { get; set; }
    }
}
