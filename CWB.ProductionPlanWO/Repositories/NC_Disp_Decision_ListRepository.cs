using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class NC_Disp_Decision_ListRepository : Repository<NC_Disp_Decision_List>, INC_Disp_Decision_ListRepository
    {
        public NC_Disp_Decision_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
