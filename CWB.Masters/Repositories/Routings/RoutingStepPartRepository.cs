using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Infrastructure;
using CWB.Masters.Repositories.Routing;

namespace CWB.Masters.Repositories.Company
{
    public class RoutingStepPartRepository : Repository<Domain.RoutingStepPart>, IRoutingStepPartRepository
    {
        public RoutingStepPartRepository(MastersDbContext context)
         : base(context)
        { }
    }
}