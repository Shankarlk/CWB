using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class SubCon_ListRepository : Repository<SubCon_List>, ISubCon_ListRepository
    {
        public SubCon_ListRepository(WODbContext context)
       : base(context)
        {

        }
    }
}
