using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class Matl_Issue_ListRepository : Repository<Matl_Issue_List>, IMatl_Issue_ListRepository
    {
        public Matl_Issue_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
