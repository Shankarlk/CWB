using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class TempMc_Timeslot_ListRepository : Repository<TempMc_Timeslot_List>, ITempMc_Timeslot_ListRepository
    {
        public TempMc_Timeslot_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
