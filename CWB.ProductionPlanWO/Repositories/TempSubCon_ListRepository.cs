using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class TempSubCon_ListRepository : Repository<TempSubCon_List>, ITempSubCon_ListRepository
    {
        public TempSubCon_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
