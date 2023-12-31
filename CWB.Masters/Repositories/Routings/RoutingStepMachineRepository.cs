using CWB.CommonUtils.Common.Repositories;
using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Infrastructure;

namespace CWB.Masters.Repositories.Routings
{
    public class RoutingStepMachineRepository : Repository<Domain.RoutingStepMachine>, IRoutingStepMachineRepository
    {
        public RoutingStepMachineRepository(MastersDbContext context)
         : base(context)
        { }
    }
}