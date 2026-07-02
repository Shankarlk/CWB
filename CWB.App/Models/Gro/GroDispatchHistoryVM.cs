using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.Gro
{
    public class GroDispatchHistoryVM
    {
        public Gro_Disp_HeaderVM Header { get; set; }

        public List<Gro_Disp_DetVM> Details { get; set; }
    }
}
