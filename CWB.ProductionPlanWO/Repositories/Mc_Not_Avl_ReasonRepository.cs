using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Mc_Not_Avl_ReasonRepository : Repository<Mc_Not_Avl_Reason>, IMc_Not_Avl_ReasonRepository
    {
        public Mc_Not_Avl_ReasonRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
