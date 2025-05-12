using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class NC_Work_ListRepository : Repository<NC_Work_List>, INC_Work_ListRepository
    {
        public NC_Work_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
