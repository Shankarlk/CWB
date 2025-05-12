using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class NC_Wk_List_HeaderRepository : Repository<NC_Wk_List_Header>, INC_Wk_List_HeaderRepository
    {
        public NC_Wk_List_HeaderRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
