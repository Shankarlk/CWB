using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Non_Plan_Wk_ListRepository : Repository<Non_Plan_Wk_List>, INon_Plan_Wk_ListRepository
    {
        public Non_Plan_Wk_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
