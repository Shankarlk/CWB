using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Inw_Recpt_HeaderRepository : Repository<Inw_Recpt_Header>, IInw_Recpt_HeaderRepository
    {
        public Inw_Recpt_HeaderRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
