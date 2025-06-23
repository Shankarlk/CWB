using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class TempMc_Wait_ListRepository : Repository<TempMc_Wait_List>, ITempMc_Wait_ListRepository
    {
        public TempMc_Wait_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
