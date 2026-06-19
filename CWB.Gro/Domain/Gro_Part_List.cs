using CWB.CommonUtils.Common;
using System;


namespace CWB.Gro.Domain
{
    public class Gro_Part_List : BaseEntity
    {
        public string Gro_Part_No { get; set; }
        public long Part_No { get; set; }
        public decimal MRP { set; get; }
        public DateTime? Data_Update { set; get; }
        public long Update_By { set; get; }
        public int Part_Status { set; get; }

        public decimal OurPrice { set; get; }

        public int GSTRate { get; set; }
        public string HSNCode { get; set; }

        public long TenantId { get; set; }
    }
}
