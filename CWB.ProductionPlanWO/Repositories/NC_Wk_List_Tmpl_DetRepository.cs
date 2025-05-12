using System;
using System.Collections.Generic;
using System.Linq;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class NC_Wk_List_Tmpl_DetRepository : Repository<NC_Wk_List_Tmpl_Det>, INC_Wk_List_Tmpl_DetRepository
    {
        public NC_Wk_List_Tmpl_DetRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
