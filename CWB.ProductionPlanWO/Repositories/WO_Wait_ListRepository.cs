using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class WO_Wait_ListRepository : Repository<WO_Wait_List>, IWO_Wait_ListRepository
    {
        public WO_Wait_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
