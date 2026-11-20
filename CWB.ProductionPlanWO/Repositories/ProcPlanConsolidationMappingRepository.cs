using CWB.CommonUtils.Common.Repositories;
using CWB.ProductionPlanWO.Domain;
using CWB.ProductionPlanWO.Infrastructure;

namespace CWB.ProductionPlanWO.Repositories
{
    public class ProcPlanConsolidationMappingRepository : Repository<ProcPlanConsolidationMapping>, IProcPlanConsolidationMappingRepository
    {
        public ProcPlanConsolidationMappingRepository(WODbContext context)
      : base(context)
        { }
    }
}
