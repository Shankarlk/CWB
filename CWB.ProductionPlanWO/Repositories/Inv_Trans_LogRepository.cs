using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Inv_Trans_LogRepository : Repository<Inv_Trans_Log>, IInv_Trans_LogRepository
    {
        public Inv_Trans_LogRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
