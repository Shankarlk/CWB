using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Infrastructure;

namespace CWB.Masters.Repositories.Routings
{
    public class RoutingStepRepository : Repository<Domain.RoutingStep>, IRoutingStepRepository
    {
        public RoutingStepRepository(MastersDbContext context)
         : base(context)
        { }
    }
}