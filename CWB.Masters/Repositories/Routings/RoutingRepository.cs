using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Infrastructure;
using CWB.Masters.Repositories.Routings;

namespace CWB.Masters.Repositories.Routings
{
    public class RoutingRepository : Repository<Domain.Routing>, IRoutingRepository
    {
        public RoutingRepository(MastersDbContext context)
         : base(context)
        { }
    }
}