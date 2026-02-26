using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.Routings
{
    public class RoutingLookupVM
    {
        public long ManufacturedPartId { get; set; }
        public List<RoutingSelectVM> Routings { get; set; }
    }
}
