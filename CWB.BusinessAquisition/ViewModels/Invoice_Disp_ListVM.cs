using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.BusinessAquisition.ViewModels
{
    public class Invoice_Disp_ListVM
    {
        public long Invoice_Disp_List_Id { get; set;}
        public long Dispatch_Detail_Id { get; set; }
        public long Cust_Disp_List_Id { get; set; }
        public long TenantId { get; set; }
    }
}
