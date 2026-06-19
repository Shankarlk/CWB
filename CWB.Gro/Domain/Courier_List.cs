using CWB.CommonUtils.Common;
using System;

namespace CWB.Gro.Domain
{
    public class Courier_List : BaseEntity
    {
        public string Courier_Name { get; set; }
        public string Contact_Person { get; set; }
        public string Contact_Phone { get; set; }
        public long TenantId { get; set; }


    }
}
