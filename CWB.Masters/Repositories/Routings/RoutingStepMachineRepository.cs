using CWB.CommonUtils.Common.Repositories;
using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Domain.Routings;
using CWB.Masters.Infrastructure;

namespace CWB.Masters.Repositories.Routings
{
    public class RoutingStepMachineRepository : Repository<RoutingStepMachine>, IRoutingStepMachineRepository
    {
        public RoutingStepMachineRepository(MastersDbContext context)
         : base(context)
        { }
    }
}