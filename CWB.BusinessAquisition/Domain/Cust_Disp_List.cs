using CWB.CommonUtils.Common;
using System;

namespace CWB.BusinessAquisition.Domain
{
    public class Cust_Disp_List : BaseEntity
    {
        public long SO_ID { get; set; }

        public long PartId { get; set; }
        public int Disp_Qnty { get; set; }
        public char Dispatch_Complete { get; set; }
        public char Invoice_Details_Entered   { get; set; }
        public char Data_Confirmed   { get; set; }

        public long TenantId { get; set; }
    }
}
