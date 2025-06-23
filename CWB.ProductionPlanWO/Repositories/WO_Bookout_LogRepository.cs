using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class WO_Bookout_LogRepository : Repository<WO_Bookout_Log>, IWO_Bookout_LogRepository
    {
        public WO_Bookout_LogRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
