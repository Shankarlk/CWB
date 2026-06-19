using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.Gro
{
    public class Courier_ListVM
    {
        public long courier_List_ID { get; set; }
        public string Courier_Name { get; set; }
        public string Contact_Person { get; set; }
        public string Contact_Phone { get; set; }
        public long TenantId { get; set; }
    }
}
