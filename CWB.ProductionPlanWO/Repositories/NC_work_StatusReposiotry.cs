using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class NC_work_StatusReposiotry : Repository<NC_work_Status>, INC_work_StatusRepository
    {
        public NC_work_StatusReposiotry(WODbContext context)
       : base(context)
        {

        }
    }
}
