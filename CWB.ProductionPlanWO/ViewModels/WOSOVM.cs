﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.ViewModels
{
    public class WOSOVM
    {
        public int WOSOId { get; set; }
        public long WorkOrderId { get; set; }
        public long SalesOrderId { get; set; }
    }
}
