using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Inv_Mismatch_ListRepository : Repository<Inv_Mismatch_List>, IInv_Mismatch_ListRepository
    {
        public Inv_Mismatch_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
