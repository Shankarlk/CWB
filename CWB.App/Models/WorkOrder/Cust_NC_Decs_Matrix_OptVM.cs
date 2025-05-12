using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.WorkOrder
{
    public class Cust_NC_Decs_Matrix_OptVM
    {
        public long Cust_NC_Decs_Matrix_OptId { get; set; }
        public long Cust_NC_Decs_Matrix_Id { get; set; }
        public string NC_Disp_Decision_Id { get; set; }
        public long TenantId { get; set; }
        public string NcDecision { get; set; } = string.Empty;
    }
}
