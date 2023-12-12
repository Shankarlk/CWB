using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Infrastructure;
using CWB.Masters.Repositories.Company;

namespace CWB.Masters.Repositories.ItemMaster
{
    public class MPMakeFromRepository : Repository<Domain.MPMakeFrom>, IMPMakeFromRepository
    {
        public MPMakeFromRepository(MastersDbContext context)
        : base(context)
        { }
    }
}
