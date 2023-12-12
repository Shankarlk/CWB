using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Infrastructure;
using CWB.Masters.Repositories.Company;

namespace CWB.Masters.Repositories.ItemMaster
{
    public class MPBOMRepository : Repository<Domain.MPBOM>, IMPBOMRepository
    {
        public MPBOMRepository(MastersDbContext context)
        : base(context)
        { }
    }
}
