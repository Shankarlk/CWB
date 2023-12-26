using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Infrastructure;
using CWB.Masters.Repositories.Routing;

namespace CWB.Masters.Repositories.Company
{
    public class RoutingStepRepository : Repository<Domain.RoutingStep>, IRoutingStepRepository
    {
        public RoutingStepRepository(MastersDbContext context)
         : base(context)
        { }
    }
}