using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Domain.Routings;
using CWB.Masters.Infrastructure;

namespace CWB.Masters.Repositories.Routings
{
    public class RoutingStepPartRepository : Repository<RoutingStepPart>, IRoutingStepPartRepository
    {
        public RoutingStepPartRepository(MastersDbContext context)
         : base(context)
        { }
    }
}