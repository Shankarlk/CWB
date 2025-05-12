using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class NC_Disp_Decs_Appl_ListRepository : Repository<NC_Disp_Decs_Appl_List>, INC_Disp_Decs_Appl_ListRepository
    {
        public NC_Disp_Decs_Appl_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
