using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Rwk_ListRepository : Repository<Rwk_List>, IRwk_ListRepository
    {
        public Rwk_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
