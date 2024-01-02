using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Infrastructure;



namespace CWB.Masters.Repositories.Routings
{
    public class RoutingStepSupplierRepository : Repository<Domain.RoutingStepSupplier>, IRoutingStepSupplierRepository
    {
        public RoutingStepSupplierRepository(MastersDbContext context)
         : base(context)
        { }
    }
}