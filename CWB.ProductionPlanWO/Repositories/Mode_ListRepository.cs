using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Mode_ListRepository : Repository<Mode_List>, IMode_ListRepository
    {
        public Mode_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
