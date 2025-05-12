using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Insp_Outcome_DetailsRepository : Repository<Insp_Outcome_Details>, IInsp_Outcome_DetailsRepository
    {
        public Insp_Outcome_DetailsRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
