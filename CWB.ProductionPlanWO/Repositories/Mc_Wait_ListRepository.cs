using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Mc_Wait_ListRepository : Repository<Mc_Wait_List>, IMc_Wait_ListRepository
    {
        public Mc_Wait_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
