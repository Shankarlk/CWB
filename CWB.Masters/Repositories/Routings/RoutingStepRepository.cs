using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Domain.Routings;
using CWB.Masters.Infrastructure;

namespace CWB.Masters.Repositories.Routings
{
    public class RoutingStepRepository : Repository<RoutingStep>, IRoutingStepRepository
    {
        public RoutingStepRepository(MastersDbContext context)
         : base(context)
        { }
    }
}