using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class DispatchQntyRepository : Repository<DispatchQnty>, IDispatchQntyRepository
    {
        public DispatchQntyRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
