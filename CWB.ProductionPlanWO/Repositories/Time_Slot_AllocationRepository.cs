using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Time_Slot_AllocationRepository : Repository<Time_Slot_Allocation>, ITime_Slot_AllocationRepository
    {
        public Time_Slot_AllocationRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
