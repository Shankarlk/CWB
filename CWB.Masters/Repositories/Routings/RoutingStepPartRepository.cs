using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Infrastructure;

namespace CWB.Masters.Repositories.Routings
{
    public class RoutingStepPartRepository : Repository<Domain.RoutingStepPart>, IRoutingStepPartRepository
    {
        public RoutingStepPartRepository(MastersDbContext context)
         : base(context)
        { }
    }
}