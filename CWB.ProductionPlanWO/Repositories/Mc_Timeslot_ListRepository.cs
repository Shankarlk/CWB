using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Mc_Timeslot_ListRepository : Repository<Mc_Timeslot_List>, IMc_Timeslot_ListRepository
    {
        public Mc_Timeslot_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
