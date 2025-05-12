using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Inw_Recpt_DetailsRepository : Repository<Inw_Recpt_Details>, IInw_Recpt_DetailsRepository
    {
        public Inw_Recpt_DetailsRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
