using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Cont_RCA_CA_LogRepository : Repository<Cont_RCA_CA_Log>, ICont_RCA_CA_LogRepository
    {
        public Cont_RCA_CA_LogRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
