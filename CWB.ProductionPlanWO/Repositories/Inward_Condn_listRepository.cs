using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Inward_Condn_listRepository : Repository<Inward_Condn_list>, IInward_Condn_listRepository
    {
        public Inward_Condn_listRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
