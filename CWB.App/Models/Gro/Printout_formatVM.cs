using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.Gro
{
    public class Printout_formatVM
    {
        public long Printout_Format_ID { get; set; }
        public long CWB_Customer { get; set; }
        public string Template_Location { get; set; }
        public long Purpose { get; set; }
        public long TenantId { get; set; }
    }
}
