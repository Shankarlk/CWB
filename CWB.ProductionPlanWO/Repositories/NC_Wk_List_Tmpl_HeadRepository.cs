using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class NC_Wk_List_Tmpl_HeadRepository : Repository<NC_Wk_List_Tmpl_Head>, INC_Wk_List_Tmpl_HeadRepository
    {
        public NC_Wk_List_Tmpl_HeadRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
