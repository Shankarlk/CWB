using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Infrastructure;
using CWB.Masters.Repositories.Routing;

namespace CWB.Masters.Repositories.Company
{
    public class RoutingRepository : Repository<Domain.Routing>, IRoutingRepository
    {
        public RoutingRepository(MastersDbContext context)
         : base(context)
        { }
    }
}