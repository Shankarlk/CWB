using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class TempWO_Wait_ListRepository : Repository<TempWO_Wait_List>, ITempWO_Wait_ListRepository
    {
        public TempWO_Wait_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
