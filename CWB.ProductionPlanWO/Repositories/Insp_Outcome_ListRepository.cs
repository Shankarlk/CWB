using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Insp_Outcome_ListRepository : Repository<Insp_Outcome_List>, IInsp_Outcome_ListRepository
    {
        public Insp_Outcome_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
