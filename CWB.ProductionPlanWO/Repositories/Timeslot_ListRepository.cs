using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Timeslot_ListRepository : Repository<Timeslot_List>, ITimeslot_ListRepository
    {
        public Timeslot_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
