using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class POLogRepository : Repository<POLog>, IPOLogRepository
    {
        public POLogRepository(WODbContext context)
         : base(context)
        { }
    }
}