using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Inv_Master_LogRepository : Repository<Inv_Master_Log>, IInv_Master_LogRepository
    {
        public Inv_Master_LogRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
