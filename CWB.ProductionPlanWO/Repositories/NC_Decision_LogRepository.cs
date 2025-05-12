using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class NC_Decision_LogRepository : Repository<NC_Decision_Log>, INC_Decision_LogRepository
    {
        public NC_Decision_LogRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
