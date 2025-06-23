using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class TempOpr_ListRepository : Repository<TempOpr_List>, ITempOpr_ListRepository
    {
        public TempOpr_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
