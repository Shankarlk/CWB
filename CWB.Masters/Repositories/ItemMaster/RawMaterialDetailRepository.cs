using CWB.CommonUtils.Common.Repositories;
using CWB.Masters.Infrastructure;
using CWB.Masters.Repositories.Company;

namespace CWB.Masters.Repositories.ItemMaster
{
    public class RawMaterialDetailRepository : Repository<Domain.RawMaterialDetail>, IRawMaterialDetailRepository
    {
        public RawMaterialDetailRepository(MastersDbContext context)
        : base(context)
        { }
    }
}
