using CWB.CommonUtils.Common;
using System;

namespace CWB.BusinessAquisition.Domain
{
    public class Invoice_Disp_List : BaseEntity
    {
        public long Dispatch_Detail_Id { get; set; }
        public long Cust_Disp_List_Id { get; set; }
        public long TenantId { get; set; }
    }
}
